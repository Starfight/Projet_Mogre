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
        public const float ALIGNEMENTTERRAIN = -15;

        private static int MUSHCOUNT = 0;
        private static int TREECOUNT = 0;

        private static Random rnd = new Random(); 

        public static void CreationPlan(ref SceneManager scm)
        {
            scm.AmbientLight = ColourValue.White;
            scm.SetSkyDome(true, "Examples/CloudySky", 5, 8);

            Plane plane = new Plane(Mogre.Vector3.UNIT_Y, ALIGNEMENTTERRAIN);
            MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, 5000, 5000, 20, 20, true, 1, 150, 150, Mogre.Vector3.UNIT_Z);
            Entity groundEnt = scm.CreateEntity("GroundEntity", "ground");
            scm.RootSceneNode.CreateChildSceneNode().AttachObject(groundEnt);
            groundEnt.SetMaterialName("Texture/Grass");

            //repartition circulaire
            for (int i = 0; i <= 360; i +=2)
            {
                int id = rnd.Next(2) + 1;
                int rayon = rnd.Next(1500, 2500);
                float x = (float)System.Math.Cos(System.Math.PI * i / 180) * rayon;
                float z = (float)System.Math.Sin(System.Math.PI * i / 180) * rayon;
                createTree(ref scm, id, new Vector3(x, ALIGNEMENTTERRAIN, z));
            }

            //Disposition "aléatoire" pour faire plus fun
            for (int i = 0; i <= 200; i ++)
            {
                int id = rnd.Next(3) + 1;
                float x = rnd.Next(-2500, 2500);
                float z = rnd.Next(-2500, 2500);
                createShrooms(ref scm, id, new Vector3(x, ALIGNEMENTTERRAIN, z));
            }
        }

        /// <summary>
        /// Crée un champignon
        /// </summary>
        /// <param name="index"></param>
        /// <param name="position"></param>
        public static void createShrooms(ref SceneManager scm, int index, Vector3 position)
        {
            //Création de l'Entity et du Scenenode à la position
            Entity entity = scm.CreateEntity("Mushroom" + MUSHCOUNT, "shroom1_"+index+".mesh");
            SceneNode node = scm.RootSceneNode.CreateChildSceneNode("Node_Mushroom" + MUSHCOUNT, position);
            node.AttachObject(entity);
            node.Scale(new Vector3(2, 2, 2));//réduction de la taille des champignons

            MUSHCOUNT++;
        }

        private static void createTree(ref SceneManager scm, int index, Vector3 position)
        {
            //Création de l'Entity et du Scenenode à la position
            Entity entity = scm.CreateEntity("Tree" + TREECOUNT, "tree" + index + ".mesh");
            SceneNode node = scm.RootSceneNode.CreateChildSceneNode("Node_Tree" + TREECOUNT, position);
            node.AttachObject(entity);
            int scaleTree = rnd.Next(20, 31);
            node.Scale(new Vector3(scaleTree, scaleTree, scaleTree));

            TREECOUNT++;
        }
    }
}
