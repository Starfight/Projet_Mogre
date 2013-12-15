﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MogreFramework;
using Mogre;
using MOIS;

namespace BaseMogre
{
    public partial class OgreForm : Form
    {
        #region Variables globales
        Root mRoot;
        RenderWindow mWindow;
        SceneManager mgr;
        Camera cam;
        //déplacement camera (touches)
        const float TRANSLATE = 200; 
        Mogre.Vector3 mTranslation = Mogre.Vector3.ZERO;
        //changement angle camera (souris)
        const float ROTATE = 0.2f;
        Point mLastPosition;
        bool mouseMove=false;
        FormParametresEnv fParam;
        bool attachedCam = false;
        int indiceObjetSuivi=-1;
        bool pause = false;
        #endregion

        #region Constructeur
        public OgreForm()
        {
            InitializeComponent();
            this.Size = new Size(800, 600);
            Disposed += new EventHandler(OgreForm_Disposed);
            Resize += new EventHandler(OgreForm_Resize);
        }
        #endregion

        public void Go()
        {
            Show();
            while (mRoot != null && mRoot.RenderOneFrame())
            {
                Application.DoEvents();
            }
        }

        public void Init()
        {
            // Create root object
            mRoot = new Root();

            // Define Resources
            LoadConfig();

            // Setup RenderSystem
            SetupRenderSystem();

            // Create Render Window
            mRoot.Initialise(false, "Main Ogre Window");
            NameValuePairList misc = new NameValuePairList();
            misc["externalWindowHandle"] = Handle.ToString();
            mWindow = mRoot.CreateRenderWindow("Main RenderWindow", 800, 600, false, misc);

            // Init resources
            TextureManager.Singleton.DefaultNumMipmaps = 5;
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();

            // Create a Simple Scene
            mgr = mRoot.CreateSceneManager(SceneType.ST_EXTERIOR_CLOSE);
            //plan
            Map.CreationPlan(ref mgr);

            //camera
            cam = mgr.CreateCamera("Camera");
            cam.AutoAspectRatio = true;
            mWindow.AddViewport(cam);

            //camera position
            cam.Position = new Mogre.Vector3(0, 200, -200);

            //Attache les handler
            CreateInputHandler();
            
            //Test
            /*SceneNode myNode = mgr.RootSceneNode.CreateChildSceneNode();
            BillboardSet mySet = mgr.CreateBillboardSet("mySet");
            Billboard myBillboard = mySet.CreateBillboard(new Mogre.Vector3(50, 0, 20));
            myBillboard.SetDimensions(100, 100);
            mySet.SetMaterialName("Texture/Coeur");
            myNode.AttachObject(mySet);
            myNode.Position = new Mogre.Vector3(0, 100, 0);*/

            //Initialisation des événements de la souris
            this.MouseMove += new MouseEventHandler(OgreForm_MouseMove);
            this.MouseDown += new MouseEventHandler(OgreForm_MouseDown);
            this.MouseUp += new MouseEventHandler(OgreForm_MouseUp);
            
        }

        #region événement pour la rotation de la caméra
        void OgreForm_MouseUp(object sender, MouseEventArgs e)
        {
            mouseMove = false;
        }

        void OgreForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mouseMove = true;
                mLastPosition = new Point(e.X,e.Y);
            }
        }

        void OgreForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseMove)
            {
                cam.Yaw(new Degree((mLastPosition.X - e.X) * ROTATE));
                cam.Pitch(new Degree((mLastPosition.Y - e.Y) * ROTATE));
                mLastPosition = new Point(e.X, e.Y);
            }
        }
        #endregion

        #region méthode d'initialisation
        private void LoadConfig()
        {
            ConfigFile cf = new ConfigFile();
            cf.Load("resources.cfg", "\t:=", true);
            ConfigFile.SectionIterator seci = cf.GetSectionIterator();
            String secName, typeName, archName;

            while (seci.MoveNext())
            {
                secName = seci.CurrentKey;
                ConfigFile.SettingsMultiMap settings = seci.Current;
                foreach (KeyValuePair<string, string> pair in settings)
                {
                    typeName = pair.Key;
                    archName = pair.Value;
                    ResourceGroupManager.Singleton.AddResourceLocation(archName, typeName, secName);
                }
            }
        }

        private void SetupRenderSystem()
        {
            RenderSystem rs = mRoot.GetRenderSystemByName("Direct3D9 Rendering Subsystem");
            mRoot.RenderSystem = rs;
            rs.SetConfigOption("Full Screen", "No");
            rs.SetConfigOption("Video Mode", "800 x 600 @ 32-bit colour");
        }
        #endregion

        #region événement form
        void OgreForm_Disposed(object sender, EventArgs e)
        {
            mRoot.Dispose();
            mRoot = null;
            //Fini le thread de l'environnement
            if (Environnement.getInstance() != null)
                Environnement.getInstance().Dispose();
        }

        void OgreForm_Resize(object sender, EventArgs e)
        {
            mWindow.WindowMovedOrResized();
        }
        #endregion

        protected void CreateInputHandler()
        {
            this.mRoot.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);
            this.KeyDown += new KeyEventHandler(KeyDownHandler);
            this.KeyUp += new KeyEventHandler(KeyUpHandler);
        }

        #region gestion camera clavier
        void KeyUpHandler(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Z:
                case Keys.S:
                    attachedCam = false;
                    mTranslation.z = 0;
                    break;
                case Keys.Q:
                case Keys.D:
                    attachedCam = false;
                    mTranslation.x = 0;
                    break;
                case Keys.N:
                    attachedCam = true;
                    indiceObjetSuivi++;
                    break;
                case Keys.B:
                    attachedCam = false;
                    break;

            }
        }

        //gestion camera via clavier 
        void KeyDownHandler(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Z:
                    mTranslation.z = -TRANSLATE;
                    break;
                case Keys.S:
                    mTranslation.z = TRANSLATE;
                    break;
                case Keys.Q:
                    mTranslation.x = -TRANSLATE;
                    break;
                case Keys.D:
                    mTranslation.x = TRANSLATE;
                    break;
            }
        }

        bool FrameStarted(FrameEvent evt)
        {
            cam.Position += cam.Orientation * mTranslation * evt.timeSinceLastFrame;
            if (attachedCam)
            {
                bool end = Environnement.getInstance().attachedCamera(indiceObjetSuivi);
                if (end)
                {
                    indiceObjetSuivi = 0;
                }
            }
            return true;
        }
        #endregion

        #region Gestion Interface
        //Bouton quitter
        private void bQuitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        //bouton nouveau
        private void bNewEnv_Click(object sender, EventArgs e)
        {
            if (fParam != null)
                fParam.Close();

            fParam = new FormParametresEnv(ref mgr, ref cam);
            fParam.Show();
        }

        //Bouton accelerer
        private void bAccelerer_Click(object sender, EventArgs e)
        {
            Variables.VITESSEOGRE *= 2;
            Variables.VITESSEROBOT *= 2;
        }

        //bouton ralentir
        private void bRalentir_Click(object sender, EventArgs e)
        {
            Variables.VITESSEOGRE /= 2;
            Variables.VITESSEROBOT /= 2;
        }

        //bouton play/pause
        private void bPausePlay_Click(object sender, EventArgs e)
        {
            if (pause)
            {
                Variables.VITESSEOGRE = Variables.VITESSEDEFAULT;
                Variables.VITESSEROBOT = Variables.VITESSEDEFAULT;
                bPausePlay.BackgroundImage = new Bitmap("../../Media/boutons/icon-ios7-pause-64.png");
                pause = false;
            }
            else
            {
                Variables.VITESSEOGRE = 0;
                Variables.VITESSEROBOT = 0;
                bPausePlay.BackgroundImage = new Bitmap("../../Media/boutons/icon-ios7-play-64.png");
                pause = true;
            }
        }
        #endregion
    }
}
