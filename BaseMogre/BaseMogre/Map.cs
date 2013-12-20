using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MogreFramework;

namespace BaseMogre
{
    static class Map
    {
        #region variables statiques
        /// <summary>
        /// hauteur du terrain
        /// </summary>
        public const float ALIGNEMENTTERRAIN = -15;

        /// <summary>
        /// nombre de champignons
        /// </summary>
        private static int MUSHCOUNT = 0;

        /// <summary>
        /// nombre d'arbres
        /// </summary>
        private static int TREECOUNT = 0;

        /// <summary>
        /// Random pour la création des aléatoires
        /// </summary>
        private static Random rnd = new Random();
        #endregion

        #region méthode principale
        /// <summary>
        /// méthode de définition du monde
        /// </summary>
        /// <param name="scm">scne manager auqel le monde doit être lié</param>
        public static void CreationPlan(ref SceneManager scm)
        {
            //définition ambiantLight, skydome et gestion des ombres
            scm.AmbientLight = ColourValue.White;
            scm.SetSkyDome(true, "Examples/CloudySky", 5, 8);
            scm.ShadowTechnique = ShadowTechnique.SHADOWTYPE_TEXTURE_MODULATIVE;

            //création du sol
            Plane plane = new Plane(Mogre.Vector3.UNIT_Y, ALIGNEMENTTERRAIN);
            MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, 5000, 5000, 20, 20, true, 1, 150, 150, Mogre.Vector3.UNIT_Z);
            Entity groundEnt = scm.CreateEntity("GroundEntity", "ground");
            scm.RootSceneNode.CreateChildSceneNode().AttachObject(groundEnt);
            
            //ajout de la texture au sol
            groundEnt.SetMaterialName("Texture/Grass");
            groundEnt.CastShadows = false;

            //repartition circulaire des arbres
            for (int i = 0; i <= 360; i +=2)
            {
                //choix aléatoire du modèle d'arbre
                int id = rnd.Next(2) + 1;
                int rayon = rnd.Next(1500, 2500);

                //création des coordonnées aléatoires
                float x = (float)System.Math.Cos(System.Math.PI * i / 180) * rayon;
                float z = (float)System.Math.Sin(System.Math.PI * i / 180) * rayon;

                createTree(ref scm, id, new Vector3(x, ALIGNEMENTTERRAIN, z));
            }

            //Disposition aléatoire des champignons
            for (int i = 0; i <= 200; i ++)
            {
                //choix aléatoire du modèle de champignon
                int id = rnd.Next(3) + 1;

                //création des coordonnées aléatoires
                float x = rnd.Next(-2500, 2500);
                float z = rnd.Next(-2500, 2500);

                createShrooms(ref scm, id, new Vector3(x, ALIGNEMENTTERRAIN, z));
            }
        }
        #endregion

        #region méthodes d'ajout de décors
        /// <summary>
        /// création d'un champignon sur le monde
        /// </summary>
        /// <param name="scm">scéne manager auquel le champignon sera attaché</param>
        /// <param name="index">numéro du champignon</param>
        /// <param name="position">position du champignon dans le monde</param>
        public static void createShrooms(ref SceneManager scm, int index, Vector3 position)
        {
            //Création de l'Entity et du Scenenode à la position
            Entity entity = scm.CreateEntity("Mushroom" + MUSHCOUNT, "shroom1_"+index+".mesh");
            SceneNode node = scm.RootSceneNode.CreateChildSceneNode("Node_Mushroom" + MUSHCOUNT, position);
            node.AttachObject(entity);

            //gestion de la taille des champignons
            node.Scale(new Vector3(2, 2, 2));
            MUSHCOUNT++;
        }

        /// <summary>
        /// création d'un arbre sur le monde
        /// </summary>
        /// <param name="scm">scéne manager auquel l'arbre doit être lié</param>
        /// <param name="index">numéro de l'arbre</param>
        /// <param name="position">position de l'arbre dans le monde</param>
        private static void createTree(ref SceneManager scm, int index, Vector3 position)
        {
            //Création de l'Entity et du Scenenode à la position
            Entity entity = scm.CreateEntity("Tree" + TREECOUNT, "tree" + index + ".mesh");
            SceneNode node = scm.RootSceneNode.CreateChildSceneNode("Node_Tree" + TREECOUNT, position);
            node.AttachObject(entity);

            //création d'une taille aléatoire
            int scaleTree = rnd.Next(20, 31);
            node.Scale(new Vector3(scaleTree, scaleTree, scaleTree));

            TREECOUNT++;
        }
        #endregion
    }
}
