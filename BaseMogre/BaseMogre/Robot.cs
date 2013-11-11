using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MogreFramework;

namespace BaseMogre
{
    class Robot : Personnage
    {
        #region Constantes/Variables statiques
        /// <summary>
        /// Nom du mesh
        /// </summary>
        private const String NAMEMESHROBOT = "robot.mesh";

        /// <summary>
        /// Nom par défaut
        /// </summary>
        private const String NAMEDEFAULT = "robot";

        /// <summary>
        /// Caractéristique des PV
        /// </summary>
        private const int PVMAX = 20;

        /// <summary>
        /// Compteur de robot
        /// </summary>
        private static int _COUNT = 0;
        #endregion

        #region Variables
        
        #endregion

        #region Constructeur
        /// <summary>
        /// Création du robot
        /// </summary>
        /// <param name="scm">Scenemanager d'intégration du robot</param>
        /// <param name="position">Position de départ</param>
        public Robot(ref SceneManager scm, Vector3 position)
            : base(ref scm, position, NAMEDEFAULT + _COUNT, NAMEMESHROBOT)
        {
            //Compteur de robots
            _COUNT++;            

            //Initialisation des caractéristiques
            _pointsDeVie = PVMAX;
        }
        #endregion

        #region Méthodes interface Knowledge Query
        public new Result send(KnowledgeQuery iKQ)
        {
            return Result.OK;
        }
        #endregion

        #region Méthodes privées
        protected override void  Start()
        {
 	        //TODO
            throw new NotImplementedException();
        }

        protected override bool Update(FrameEvent fEvt)
        {
            //TODO
            return true;
        }
        #endregion

        #region old to delete
        public static Entity CreationRobot(ref SceneManager scm,string nomEntite)
        {
            Entity ent = scm.CreateEntity(nomEntite, "robot.mesh");
            SceneNode node = scm.RootSceneNode.CreateChildSceneNode("Node_"+nomEntite, new Mogre.Vector3(0.0f, 0.0f, 0.25f));
            node.AttachObject(ent);
            return ent;
        }

        private static void AnimationMarche(ref SceneNode node,ref SceneManager scm)
        {
            //animation robot
            Entity ent = scm.CreateEntity("Knot1", "knot.mesh");
            node = scm.RootSceneNode.CreateChildSceneNode("Knot1Node", new Mogre.Vector3(0.0f, -10.0f, 25.0f));
            node.AttachObject(ent);
            node.Scale(0.1f, 0.1f, 0.1f);
            //
            ent = scm.CreateEntity("Knot2", "knot.mesh");
            node = scm.RootSceneNode.CreateChildSceneNode("Knot2Node", new Mogre.Vector3(550.0f, -10.0f, 50.0f));
            node.AttachObject(ent);
            node.Scale(0.1f, 0.1f, 0.1f);
            //
            ent = scm.CreateEntity("Knot3", "knot.mesh");
            node = scm.RootSceneNode.CreateChildSceneNode("Knot3Node", new Mogre.Vector3(-100.0f, -10.0f, -200.0f));
            node.AttachObject(ent);
            node.Scale(0.1f, 0.1f, 0.1f);
        }
        #endregion
    }
}
