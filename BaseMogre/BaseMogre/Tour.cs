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
        /// Nom de base
        /// </summary>
        private static String NAMEDEFAULT = "tour";
        #endregion

        #region variables
        private PositionCubes _positionFuture;
        private int _nombreCube;
        #endregion

        #region Getters et Setters

        #endregion

        public Tour(ref SceneManager scm,Vector3 position)
            : base(ref scm, position, NAMEDEFAULT , 100,50,50)
        {
            _nombreCube = 1;
        }

        public bool ajoutDeBloc(Cube C)
        {
            bool possible = this.ajoutCube(C);
            if (possible)
            {
                C.Position = _positionFuture.PositionToVector();

                Vector3 src = C.Orientation * Vector3.UNIT_Z;
                Quaternion quat = src.GetRotationTo(Vector3.UNIT_Z);
                C.Rotate(quat);

                if (_nombreCube == 9)
                {
                    _nombreCube = 0;
                    _positionFuture.ChangeValeurs(Cube._SIZE, Cube._SIZE, -Cube._SIZE);
                }
                else
                {
                    SetNextCubePosition();
                }
                _nombreCube++;
                return true;
            }
            return false;
        }

        public TourInfo getInfo()
        {
            TourInfo t = new TourInfo(this.NomEntity, this.Position);
            return t;
        }

        private void SetNextCubePosition()
        {
            switch (_nombreCube)
            {
                case 8:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 7:
                    _positionFuture.ChangeValeurs(Cube._SIZE,0,0);
                    break;
                case 6:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 5:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 4:
                    _positionFuture.ChangeValeurs(-Cube._SIZE,0, 0);
                    break;
                case 3:
                    _positionFuture.ChangeValeurs(-Cube._SIZE,0, 0);
                    break;
                case 2:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 1:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
            }
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
