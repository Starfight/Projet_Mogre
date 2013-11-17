using System;
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
        //camera move
        const float TRANSLATE = 200;
        const float ROTATE = 0.01f;
        Mogre.Vector3 mTranslation = Mogre.Vector3.ZERO;

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

            //Création de l'environnement
            Environnement.createEnvironnement(ref mgr, 15, 15, 5);
            Cube C = new Cube(ref mgr, new Mogre.Vector3(100, 0, 0), TypeCube.Bois);
        }

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

        void OgreForm_Disposed(object sender, EventArgs e)
        {
            mRoot.Dispose();
            mRoot = null;
            //Fini le thread de l'environnement
            Environnement.getInstance().Dispose();
        }

        void OgreForm_Resize(object sender, EventArgs e)
        {
            mWindow.WindowMovedOrResized();
        }

        protected void CreateInputHandler()
        {
            this.mRoot.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);
            this.KeyDown += new KeyEventHandler(KeyDownHandler);
            this.KeyUp += new KeyEventHandler(KeyUpHandler);
        }

        #region gestion camera
        void KeyUpHandler(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                //case Keys.Up:
                case Keys.Z:
                //case Keys.Down:
                case Keys.S:
                    mTranslation.z = 0;
                    break;

                case Keys.Left:
                case Keys.Q:
                case Keys.Right:
                case Keys.D:
                    mTranslation.x = 0;
                    break;

                case Keys.Up:
                //case Keys.Q:
                case Keys.Down:
                    //case Keys.E:
                    mTranslation.y = 0;
                    mTranslation.z = 0;
                    break;
            }
        }

        //gestion camera 
        void KeyDownHandler(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                //case Keys.Up:
                case Keys.Z:
                    mTranslation.z = -TRANSLATE;
                    break;

                //case Keys.Down:
                case Keys.S:
                    mTranslation.z = TRANSLATE;
                    break;

                case Keys.Left:
                    //case Keys.Q:
                    mTranslation.x = -TRANSLATE;
                    break;

                case Keys.Right:
                    //case Keys.D:
                    mTranslation.x = TRANSLATE;
                    break;

                case Keys.Up:
                    //case Keys.Q:
                    mTranslation.y = TRANSLATE;
                    mTranslation.z = TRANSLATE;
                    break;

                case Keys.Down:
                    //case Keys.E:
                    mTranslation.y = -TRANSLATE;
                    mTranslation.z = -TRANSLATE;
                    break;

                case Keys.Q:
                    cam.Yaw(ROTATE);
                    break;

                case Keys.D:
                    cam.Yaw(-ROTATE);
                    break;
            }
        }

        bool FrameStarted(FrameEvent evt)
        {
            cam.Position += cam.Orientation * mTranslation * evt.timeSinceLastFrame;

            return true;
        }
        #endregion
    }
}
