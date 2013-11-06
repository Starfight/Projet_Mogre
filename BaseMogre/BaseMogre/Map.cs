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
        public static void CreationPlan(ref SceneManager scm)
        {
            scm.AmbientLight = ColourValue.White;
            scm.SetSkyDome(true, "Examples/CloudySky", 5, 8);

            Plane plane = new Plane(Mogre.Vector3.UNIT_Y, -15);
            MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, 5000, 5000, 20, 20, true, 1, 150, 150, Mogre.Vector3.UNIT_Z);
            Entity groundEnt = scm.CreateEntity("GroundEntity", "ground");
            scm.RootSceneNode.CreateChildSceneNode().AttachObject(groundEnt);
            groundEnt.SetMaterialName("Texture/Grass");
        }


    }
}
