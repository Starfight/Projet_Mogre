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
            : base(ref scm, position, NAMEDEFAULT + _COUNT,11,12)
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
        public override bool ajoutDeBloc(Cube C)
        {
            if(base.ajoutDeBloc(C))
            {
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
        public Type NaissancePerso()
        {
            int chance = RND.Next(5);
            if (chance == 0)
            {
                return typeof(OgreBatisseur);
            }
            else if (chance == 1)
            {
                return typeof(Robot);
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
                case 21:
                case 15:
                case 14:
                case 8:
                case 7:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 20:
                case 19:
                case 6:
                case 5:
                    _positionFuture.ChangeValeurs(-Cube._SIZE, 0, 0);
                    break;
                case 18:
                case 17:
                case 11:
                case 10:
                case 4:
                case 3:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 16:
                case 9:
                    _positionFuture.ChangeValeurs(0, Cube._SIZE, 0);
                    break;
                case 13:
                case 12:
                case 2:
                    _positionFuture.ChangeValeurs(Cube._SIZE, 0, 0);
                    break;
                case 1:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    Log.writeNewLine("Maison finie en (" + this.Position.x + "," + this.Position.y + "," + this.Position.z + ")");
                    break;
            }
        }
        #endregion
    }
}
