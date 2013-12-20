using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using System.Threading;

namespace BaseMogre
{
    class Batiment: IDisposable
    {
        #region Constantes/Variables statiques
        /// <summary>
        /// Nom du node par défaut
        /// </summary>
        private const String NAMENODE = "Node_";
        #endregion

        #region Attributs
        /// <summary>
        /// Mutex de protection pour le dépot d'un cube
        /// </summary>
        protected Mutex _depotCube;

        /// <summary>
        /// position du future cube
        /// </summary>
        protected PositionCubes _positionFuture;

        /// <summary>
        /// Nombre de cubes pierre necessaire pour finir
        /// </summary>
        protected int _nbCubePierreNecessaire;

        /// <summary>
        /// Nombre de cubes bois necessaire pour finir
        /// </summary>
        protected int _nbCubeBoisNecessaire;

        /// <summary>
        /// Référence du scenemanager
        /// </summary>
        protected SceneManager _scm;

        /// <summary>
        /// Scenenode du batiment
        /// </summary>
        protected SceneNode _node;

        /// <summary>
        /// Nom du batiment
        /// </summary>
        protected String _nomEntity;

        /// <summary>
        /// position du batiment
        /// </summary>
        protected Vector3 _position;

        /// <summary>
        /// Contient tous les cubes utilisés
        /// </summary>
        public List<Cube> _listeDesCubes;
        #endregion

        #region Getter et Setter
        /// <summary>
        /// Position du batiment
        /// </summary>
        public Vector3 Position
        {
            get { return _position; }
        }

        /// <summary>
        /// Nb de cubes de bois necessaire pour finir
        /// </summary>
        public int NbCubeBoisNecessaire
        {
            get { return _nbCubeBoisNecessaire; }
        }

        /// <summary>
        /// Nb de cube de pierre necessaire pour finir
        /// </summary>
        public int NbCubePierreNecessaire
        {
            get { return _nbCubePierreNecessaire; }
        }

        /// <summary>
        /// Nom de l'entité
        /// </summary>
        public String NomEntity
        {
            get { return _nomEntity; }
        }

        /// <summary>
        /// Orientation de la maison
        /// </summary>
        public Quaternion Orientation
        {
            get { return _node.Orientation; }
        }
        #endregion

        #region constructeur
        public Batiment(ref SceneManager scm, Vector3 position, String nomBatiment,int nbCubeBois=0, int NbCubePierre=0)
        {
            //Création de l'Entity et du Scenenode à la position
            _node = scm.RootSceneNode.CreateChildSceneNode(NAMENODE + nomBatiment, position);

            //Enregistrement du Scenemanager
            _scm = scm;

            //Enregistrement du nom du personnage
            _nomEntity = nomBatiment;

            //Enregistrement des variables
            _position = position;
            _nbCubeBoisNecessaire = nbCubeBois;
            _nbCubePierreNecessaire = NbCubePierre;
            _listeDesCubes = new List<Cube>();

            //préparation du mutex
            _depotCube = new Mutex();

            //définition de la position du cube
            _positionFuture = new PositionCubes(this.Position.x + 30, 0, this.Position.z - 30);
        }
        #endregion

        #region méthodes protected
        /// <summary>
        /// Ajoute un cube et le retire de ce qu'il reste à ajouter
        /// </summary>
        /// <param name="C">cube à ajouter</param>
        /// <returns>bool si c'est bon false si on a pas besoin du cube</returns>
        protected virtual bool ajoutCube(Cube C)//peut etre passer la référence ici, je sais pas trop
        {
            bool ok = false;
            if (C.Type == TypeCube.Bois && _nbCubeBoisNecessaire > 0)
            {
                _nbCubeBoisNecessaire--;
                _listeDesCubes.Add(C);
                C.Deplacable = false;
                ok = true;
            }
            if (C.Type == TypeCube.Pierre && _nbCubePierreNecessaire > 0)
            {
                _nbCubePierreNecessaire--;
                _listeDesCubes.Add(C);
                C.Deplacable = false;
                ok = true;
            }
            if ((ok)&&(isFinish()))
            {
                Log.writeNewLine("Bâtiment " + NomEntity + " terminé.");
            }

            return ok;
        }
        #endregion

        #region méthodes public
        /// <summary>
        /// Si la maison est finie
        /// </summary>
        /// <returns></returns>
        public virtual bool isFinish()
        {
            if (_nbCubeBoisNecessaire + _nbCubePierreNecessaire == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Implémentation de l'interface IDisposable pour les batiment
        /// </summary>
        public virtual void Dispose()
        {
            //Suppression des cubes
            while(_listeDesCubes.Count>0)
            {
                Cube c = _listeDesCubes.First();
                _listeDesCubes.Remove(c);
                c.Dispose();
            }
            _scm.DestroySceneNode(_node);
        }

        public virtual bool ajoutDeBloc(Cube C)
        {
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
                return true;
            }
            return false;
        }
        #endregion
    }
}
