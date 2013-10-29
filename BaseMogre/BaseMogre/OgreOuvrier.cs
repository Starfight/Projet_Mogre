﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace BaseMogre
{
    class OgreOuvrier:Ogres
    {
        #region constantes
        /// <summary>
        /// Caractéristique des PV
        /// </summary>
        private const int PVMAX = 20;
        #endregion

        #region constructeurs
        public OgreOuvrier(ref SceneManager scm, Vector3 position)
            : base(ref scm, position, 5, 5)
        {
            this._pointsDeVie = PVMAX;
        }
        #endregion

        #region méthodes
        #endregion
    }
}
