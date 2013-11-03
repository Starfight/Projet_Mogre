using System;
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

        #region méthodes privées
        protected override void Start()
        {
            //boucle principale
            while (!_stop)
            {
                //envoi un message pour savoir où construire/chercher une maison

                //va à l'emplacement si maison en construction

                //demande le prochain cube pour la maison

                //va à la position des entrepôts 

                //demande un cube

                //retourne à la maison

                //demande de donner le cube
            }
        }
        #endregion
    }
}
