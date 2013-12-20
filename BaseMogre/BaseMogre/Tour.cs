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

        /// <summary>
        /// Nombre réel de cube nécessaire
        /// </summary>
        private static int NOMBREDECUBETOTAL = 20;
        #endregion

        #region variables
        /// <summary>
        /// compteur de cube servant au switch de construction
        /// </summary>
        private int _nombreCube;
        #endregion

        #region constructeur
        public Tour(ref SceneManager scm,Vector3 position)
            : base(ref scm, position, NAMEDEFAULT)
        {
            _nombreCube = 1;
            Log.writeNewLine("Tour commencée en (" + this.Position.x + "," + this.Position.y + "," + this.Position.z + ")");
        }
        #endregion

        #region méthodes public
        /// <summary>
        /// redéfinition de l'ajout de cube car la tour est insensible au type de cube qu'on lui donne.
        /// </summary>
        /// <param name="C">Cube à ajouter</param>
        /// <returns>si le cube à été ajouté ou non</returns>
        protected override bool ajoutCube(Cube C)
        {
            if (!this.isFinish())
            {
                _listeDesCubes.Add(C);
                C.Deplacable = false;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Ajout d'un cube dans la tour
        /// </summary>
        /// <param name="C">Cube à ajouter</param>
        /// <returns>réussite ou échec de l'ajout du cube</returns>
        public override bool ajoutDeBloc(Cube C)
        {
            if (base.ajoutDeBloc(C))
            {
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
        /// redéfinition du isFinish pour la tour qui ne prend pas en compte le type de cube.
        /// </summary>
        /// <returns></returns>
        public override bool isFinish()
        {
            if (_listeDesCubes.Count == NOMBREDECUBETOTAL)
                return true;
            else
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
                case 2:
                case 1:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 7:
                    _positionFuture.ChangeValeurs(Cube._SIZE, 0, 0);
                    break;
                case 6:
                case 5:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 4:
                case 3:
                    _positionFuture.ChangeValeurs(-Cube._SIZE, 0, 0);
                    break;
            }
        }
        #endregion
    }
}
