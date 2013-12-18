using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using System.Threading;

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
        /// Random pour la naissance d'un ogre batisseur
        /// </summary>
        private static Random RND = new Random();
        #endregion

        #region variables
        /// <summary>
        /// Mutex de protection pour le dépot d'un cube
        /// </summary>
        private Mutex _depotCube;

        /// <summary>
        /// position du future cube
        /// </summary>
        private PositionCubes _positionFuture;

        /// <summary>
        /// entité représentant la maison (le pied)
        /// </summary>
        private Entity _entityMaison;

        /// <summary>
        /// Node du pied de la maison
        /// </summary>
        private SceneNode _nodeBaseMaison;
        #endregion

        #region Constructeur
        public Maison(ref SceneManager scm,Vector3 position)
            : base(ref scm, position, NAMEDEFAULT + _COUNT, 20,11,12)
        {
            //création de l'entity et du node de la maison
            _entityMaison = scm.CreateEntity("base_maison" + _COUNT, "cube.mesh");
            _nodeBaseMaison = scm.RootSceneNode.CreateChildSceneNode("base_maison_node" + _COUNT, position);

            //réglage de la position et de l'échelle
            _nodeBaseMaison.SetPosition(position.x, position.y - 12, position.z);
            _nodeBaseMaison.Scale(new Vector3(1, 0.05f, 1));

            //laison entity /node
            _nodeBaseMaison.AttachObject(_entityMaison);

            //application du matiérail à la base de la maison
            _entityMaison.SetMaterialName("Texture/BaseMaison");
            
            _COUNT++;
            
            //définition de l'emplacement du prochain cube
            _positionFuture = new PositionCubes(this.Position.x+30, 0, this.Position.z-30);
            
            _depotCube = new Mutex();

            //écriture dans les logs qu'un maison est crée
            Log.writeNewLine("Maison commencée en (" + this.Position.x + "," + this.Position.y + "," + this.Position.z + ")");
        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// Implémentation de l'interface IDisposable pour l'arrêt des threads
        /// </summary>
        public override void Dispose()
        {
            _nodeBaseMaison.DetachObject(_entityMaison);
            _scm.DestroyEntity(_entityMaison);
            _scm.DestroySceneNode(_nodeBaseMaison);
            
            base.Dispose();
        }

        /// <summary>
        /// Ajout d'un cube dans la maison
        /// </summary>
        /// <param name="C">Cube à ajouter</param>
        /// <returns>réussite ou échec de l'ajout du cube</returns>
        public bool ajoutDeBloc(Cube C)
        {
            //vérouillage du mutex
            _depotCube.WaitOne();

            //test de la possibilité d'ajout du cube à la maison
            bool possible = this.ajoutCube(C);
            if (possible)
            {
                //positionnement du cube
                C.Position = _positionFuture.PositionToVector();

                //rotation du cub
                Vector3 src = C.Orientation * Vector3.UNIT_Z;
                Quaternion quat = src.GetRotationTo(Vector3.UNIT_Z);
                C.Rotate(quat);

                //définiton de la position suivante
                SetNextCubePosition();

                //déverouillage du mutex
                _depotCube.ReleaseMutex();
                return true;
            }
            //déverouillage du mutex
            _depotCube.ReleaseMutex();
            return false;
        }

        /// <summary>
        /// méthode de random pour savoir quel type d'ogre va naitre 1/5 pour que ca soit un batisseur
        /// </summary>
        /// <returns>le type d'ogre à créer</returns>
        public Type NaissanceOgre()
        {
            if (RND.Next(5) == 0)
            {
                return typeof(OgreBatisseur);
            }
            else
            {
                return typeof(OgreOuvrier);
            }
        }
        #endregion

        #region Méthodes privées
        /// <summary>
        /// méthode servant à définir l'emplacement du prochain cube
        /// </summary>
        private void SetNextCubePosition()
        {
            switch (_nbCubeBoisNecessaire + _nbCubePierreNecessaire)
            {
                case 22:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 21:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 20:
                    _positionFuture.ChangeValeurs(-Cube._SIZE, 0, 0);
                    break;
                case 19:
                    _positionFuture.ChangeValeurs(-Cube._SIZE, 0, 0);
                    break;
                case 18:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 17:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 16:
                    _positionFuture.ChangeValeurs(0, Cube._SIZE, 0);
                    break;
                case 15:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 14:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 13:
                    _positionFuture.ChangeValeurs(Cube._SIZE, 0, 0);
                    break;
                case 12:
                    _positionFuture.ChangeValeurs(Cube._SIZE, 0, 0);
                    break;
                case 11:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 10:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 9:
                    _positionFuture.ChangeValeurs(0, Cube._SIZE, 0);
                    break;
                case 8:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 7:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 6:
                    _positionFuture.ChangeValeurs(-Cube._SIZE, 0, 0);
                    break;
                case 5:
                    _positionFuture.ChangeValeurs(-Cube._SIZE, 0, 0);
                    break;
                case 4:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 3:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 2:
                    _positionFuture.ChangeValeurs(Cube._SIZE, 0, 0);
                    break;
                case 1:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    Log.writeNewLine("Maison finie en ("+this.Position.x+","+this.Position.y+","+this.Position.z+")");
                    break;
            }
        }
        #endregion
    }
}
