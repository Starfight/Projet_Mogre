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
    }
}
