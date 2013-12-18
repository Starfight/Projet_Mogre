using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Mogre;
using MogreFramework;
using System.IO;

namespace BaseMogre
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //on nettoie les fichiers de log
            File.WriteAllText("Ogre.log", "", System.Text.Encoding.UTF8);
            //File.WriteAllText("resultats.log", "", System.Text.Encoding.UTF8);

            //on lance la fenetre de l'application
            OgreForm form = new OgreForm();
            form.Init();
            form.Go();
        }
    }
}
