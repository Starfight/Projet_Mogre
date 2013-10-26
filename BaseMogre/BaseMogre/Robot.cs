using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MogreFramework;

namespace BaseMogre
{
    class Robot
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
        /// Nom du node par défaut
        /// </summary>
        private const String NAMENODE = "Node_";

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
        SceneManager _scm;

        /// <summary>
        /// Entity représentant le robot
        /// </summary>
        Entity _robot;

        /// <summary>
        /// Scenenode du robot
        /// </summary>
        SceneNode _nodeRobot;

        String _nomEntity;
        int _pointsDeVie;
        #endregion

        #region Constructeur
        /// <summary>
        /// Création du robot
        /// </summary>
        /// <param name="scm">Scenemanager d'intégration du robot</param>
        /// <param name="position">Position de départ</param>
        public Robot(ref SceneManager scm, Vector3 position)
        {
            //Attribution du nom automatique
            _COUNT++;
            _nomEntity = NAMEDEFAULT + _COUNT;

            //Création de l'Entity et du Scenenode à la position
            _robot = scm.CreateEntity(_nomEntity, NAMEMESHROBOT);
            _nodeRobot = scm.RootSceneNode.CreateChildSceneNode(NAMENODE + _nomEntity, position);
            _nodeRobot.AttachObject(_robot);

            //Enregistrement du Scenemanager
            _scm = scm;

            //Initialisation des caractéristiques
            _pointsDeVie = PVMAX;
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
