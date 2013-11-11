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
        //moves
        AnimationState mAnimationState = null; //The AnimationState the moving object
        float mDistance = 0.0f;              //The distance the object has left to travel
        Mogre.Vector3 mDirection = Mogre.Vector3.ZERO;   // The direction the object is moving
        Mogre.Vector3 mDestination = Mogre.Vector3.ZERO; // The destination the object is moving towards
        LinkedList<Mogre.Vector3> mWalkList = null; // A doubly linked containing the waypoints
        float mWalkSpeed = 50.0f;  // The speed at which the object is moving
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


            //objet
            /*
            Entity ent = Robot.CreationRobot(ref mgr, "Robot");
            OgreBatisseur OB = new OgreBatisseur(ref mgr, new Mogre.Vector3(0.0f, 0.0f, 0.25f));
            */
            //walklist
            /*
            mWalkList = new LinkedList<Mogre.Vector3>();
            mWalkList.AddLast(new Mogre.Vector3(100.0f, 0.0f, 50.0f));
            mWalkList.AddLast(new Mogre.Vector3(50.0f, 0.0f, -200.0f));
            mWalkList.AddLast(new Mogre.Vector3(0.0f, 0.0f, 25.0f));
            */
            //camera position
            cam.Position = new Mogre.Vector3(0, 200, -400);
            //cam.LookAt(ent.BoundingBox.Center);

            CreateInputHandler();

            //Création de l'environnement
            Environnement.createEnvironnement(ref mgr, 15, 15);
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

        #region deplacement
        protected bool nextLocation()
        {
            if (mWalkList.Count == 0)
                return false;
            return true;
        }
        /*
        bool moveRobot(FrameEvent evt)
        {

            Entity ent = mgr.GetEntity("Robot");
            SceneNode node = mgr.GetSceneNode("Node_Robot");

            float move = mWalkSpeed * (evt.timeSinceLastFrame);
            mDistance -= move;

            //Knot arrival check
            if (mDistance <= 0.0f)
            {
                if (!TurnNextLocation())
                {
                    mAnimationState = ent.GetAnimationState("Idle");
                    return true;
                }
            }
            else
            {
                //movement code goes here
                node.Translate(mDirection * move);
            }

            //Update the Animation State.
            mAnimationState.AddTime(evt.timeSinceLastFrame * mWalkSpeed / 20);

            return true;

        }

        bool TurnNextLocation()
        {
            Entity ent = mgr.GetEntity("Robot");
            SceneNode node = mgr.GetSceneNode("Node_Robot");

            if (nextLocation())
            {
                //Start the walk animation
                mAnimationState = ent.GetAnimationState("Walk");
                mAnimationState.Loop = true;
                mAnimationState.Enabled = true;

                LinkedListNode<Mogre.Vector3> tmp;  //temporary listNode
                mDestination = mWalkList.First.Value; //get the next destination.
                tmp = mWalkList.First; //save the node that held it
                mWalkList.RemoveFirst(); //remove that node from the front of the list
                mWalkList.AddLast(tmp);  //add it to the back of the list.

                //update the direction and the distance
                mDirection = mDestination - node.Position;
                mDistance = mDirection.Normalise();

                Mogre.Vector3 src = node.Orientation * Mogre.Vector3.UNIT_X;


                if ((1.0f + src.DotProduct(mDirection)) < 0.0001f)
                {
                    node.Yaw(new Angle(180.0f));
                }
                else
                {
                    Quaternion quat = src.GetRotationTo(mDirection);
                    node.Rotate(quat);
                }

                return true;

            }

            return false;
        }
         * */
        #endregion

        protected void CreateInputHandler()
        {
            //this.mRoot.FrameStarted += new FrameListener.FrameStartedHandler(moveRobot);
            this.mRoot.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);
            this.KeyDown += new KeyEventHandler(KeyDownHandler);
            this.KeyUp += new KeyEventHandler(KeyUpHandler);
        }

        //gestion camera
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

        #region Frame Handlers
        bool FrameStarted(FrameEvent evt)
        {
            cam.Position += cam.Orientation * mTranslation * evt.timeSinceLastFrame;

            return true;
        }
        #endregion
    }
}
