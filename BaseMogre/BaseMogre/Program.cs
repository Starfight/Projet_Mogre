using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Mogre;
using MogreFramework;

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
            OgreForm form = new OgreForm();
            form.Init();
            form.Go();
        }
    }
}
