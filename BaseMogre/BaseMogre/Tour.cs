using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using System.Threading;

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
        /// <summary>
        /// Future positin d'un cube
        /// </summary>
        private PositionCubes _positionFuture;

        /// <summary>
        /// compteur de cube servant au switch de construction
        /// </summary>
        private int _nombreCube;

        /// <summary>
        /// Mutex pour la protection contre le dépot de 2 cubes simultanés
        /// </summary>
        private Mutex _depotCube;
        #endregion

        #region constructeur
        public Tour(ref SceneManager scm,Vector3 position)
            : base(ref scm, position, NAMEDEFAULT , 100,50,50)
        {
            _nombreCube = 1;
            _positionFuture = new PositionCubes(this.Position.x + 30, 0, this.Position.z - 30);
            _depotCube = new Mutex();
            Log.writeNewLine("Tour commencée en (" + this.Position.x + "," + this.Position.y + "," + this.Position.z + ")");
        }
        #endregion

        #region méthodes public
        /// <summary>
        /// Ajout d'un cube dans la tour
        /// </summary>
        /// <param name="C">Cube à ajouter</param>
        /// <returns>réussite ou échec de l'ajout du cube</returns>
        public bool ajoutDeBloc(Cube C)
        {
            //verrouillage du mutex
            _depotCube.WaitOne();

            //test de la possibilité d'ajout du cube à la tour
            bool possible = this.ajoutCube(C);
            
            if (possible)
            {
                //positionnement du cube
                C.Position = _positionFuture.PositionToVector();

                //orientation du cube
                Vector3 src = C.Orientation * Vector3.UNIT_Z;
                Quaternion quat = src.GetRotationTo(Vector3.UNIT_Z);
                C.Rotate(quat);

                //définition de la position suivante
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

                //déverouillage du mutex
                _depotCube.ReleaseMutex();
                return true;
            }
            //déverouillage du mutex
            _depotCube.ReleaseMutex();
            return false;
        }

        /// <summary>
        /// Informations sur la tour à destination d'un ogre
        /// </summary>
        /// <returns>informations accessibles à l'ogre</returns>
        public TourInfo getInfo()
        {
            TourInfo t = new TourInfo(this.NomEntity, this.Position);
            return t; 
        }
        #endregion

        #region méthodes privées
        /// <summary>
        /// Méthode de définition de la future position d'un cube
        /// </summary>
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
        #endregion
    }
}
