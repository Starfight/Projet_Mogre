using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace BaseMogre
{
    class OgreBatisseur:Ogres
    {
        #region constantes
        /// <summary>
        /// Caractéristique des PV
        /// </summary>
        private const int PVMAX = 20;

        /// <summary>
        /// attaque des ogres batisseurs
        /// </summary>
        private const int ATK = 3;

        /// <summary>
        /// défense des ogres batisseurs
        /// </summary>
        private const int DEF = 15;

        /// <summary>
        /// Chance de pouvoir construire une tour (1 sur cette valeur)
        /// </summary>
        private const int CHANCEPOURUNETOUR = 10;
        #endregion

        #region variable
        TourInfo _tourCible;
        #endregion

        #region constructeurs
        public OgreBatisseur(ref SceneManager scm, Vector3 position)
            : base(ref scm, position, ATK, DEF, PVMAX)
        {
            _tourCible.Reset();
        }
        #endregion

        #region méthodes privées
        private void SetTour(TourInfo T)
        {
            _tourCible = T;
        }

        protected override void Decision()
        {
            //TODO
        }
        #endregion
    }
}
