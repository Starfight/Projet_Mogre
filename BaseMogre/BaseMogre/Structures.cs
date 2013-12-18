using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace BaseMogre
{
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

    /// <summary>
    /// structure permettant une manipulation rapide de la position d'un cube
    /// </summary>
    public struct PositionCubes
    {
        float _x;
        float _y;
        float _z;
        public PositionCubes(float x, float y, float z)
        {
            this._x = x;
            this._y = y;
            this._z = z;
        }
        public void ChangeValeurs(float x, float y, float z)
        {
            this._x += x;
            this._y += y;
            this._z += z;
        }
        /// <summary>
        /// conversion des coordonnées en vecteur
        /// </summary>
        /// <returns>vecteur correspondant au coordonnées</returns>
        public Vector3 PositionToVector()
        {
            return new Vector3(_x, _y, _z);
        }
    }

    /// <summary>
    /// Structure informelle pour les tour
    /// </summary>
    public struct TourInfo
    {
        public String nom;
        public Vector3 position;

        public TourInfo(String iNom, Vector3 iPosition)
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
