using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace BaseMogre
{
    class Maison:Batiment
    {
        #region Constantes/Variables statiques
        /// <summary>
        /// Compteur de maison
        /// </summary>
        private static int _COUNT = 0;

        /// <summary>
        /// Nom de base
        /// </summary>
        private static String NAMEDEFAULT = "maison";

        /// <summary>
        /// Mesh de la maison
        /// </summary>
        private static String NAMEMESHMAISON = "cube.mesh";//on a pas encore de mesh pour la maison...
        #endregion

        public Maison(ref SceneManager scm,Vector3 position)
            : base(ref scm, position, NAMEDEFAULT + _COUNT, NAMEMESHMAISON,20,5,5)
        {
            _COUNT++;
        }
    }

    /// <summary>
    /// Structure informelle pour les maisons
    /// </summary>
    public struct MaisonInfo
    {
        public String nom;
        public Vector3 position;

        public MaisonInfo(String iNom, Vector3 iPosition)
        {
            nom = iNom;
            position = iPosition;
        }
        public bool isEmpty()
        {
            if ((nom == null) && (position == Vector3.ZERO))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Reset()
        {
            nom = null;
            position = new Vector3(0, 0, 0);
        }
    }
}
