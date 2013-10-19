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

            Plane plane = new Plane(Mogre.Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, 1500, 1500, 20, 20, true, 1, 5, 5, Mogre.Vector3.UNIT_Z);
            Entity groundEnt = scm.CreateEntity("GroundEntity", "ground");
            scm.RootSceneNode.CreateChildSceneNode().AttachObject(groundEnt);
            groundEnt.SetMaterialName("Examples/Rockwall");
        }


    }
}
