using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace BaseMogre
{
    class Tour:Batiment
    {
        #region Constantes/Variables statiques
        /// <summary>
        /// Compteur de maison
        /// </summary>
        private static int _COUNT = 0;

        /// <summary>
        /// Nom de base
        /// </summary>
        private static String NAMEDEFAULT = "tour";

        private static String NAMEMESHTOUR = "tour.mesh";
        #endregion

        #region Getters et Setters

        #endregion

        public Tour(ref SceneManager scm,Vector3 position)
            : base(ref scm, position, NAMEDEFAULT + _COUNT, NAMEMESHTOUR,100,50,50)
        {
            _COUNT++;
        }
    }
}
