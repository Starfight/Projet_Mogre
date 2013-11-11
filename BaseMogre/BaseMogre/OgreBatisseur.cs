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
        #endregion

        #region constructeurs
        public OgreBatisseur(ref SceneManager scm, Vector3 position)
            : base(ref scm, position, 5, 5)
        {
            this._pointsDeVie = PVMAX;
        }
        #endregion

        #region méthodes privées
        protected override void Start()
        {
            //TODO
            //throw new NotImplementedException();
        }

        protected override bool Update(FrameEvent fEvt)
        {
            //TODO
            return true;
        }
        #endregion
    }
}
