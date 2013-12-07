using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace BaseMogre
{
    class Batiment
    {
        #region Constantes/Variables statiques
        /// <summary>
        /// Nom du node par défaut
        /// </summary>
        private const String NAMENODE = "Node_";
        #endregion

        #region Attributs
        /// <summary>
        /// Nombre de cubes pierre necessaire pour finir
        /// </summary>
        protected int _nbCubePierreNecessaire;

        /// <summary>
        /// Nombre de cubes bois necessaire pour finir
        /// </summary>
        protected int _nbCubeBoisNecessaire;

        /// <summary>
        /// points de vie du batiment
        /// </summary>
        protected int _pointsDeVie;

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
        /// points de vie du batiment
        /// </summary>
        public int PointsDeVie
        {
            get { return _pointsDeVie; }
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

        /// <summary>
        /// Contient tous les cubes utilisés ( comem ca on pourra faire des trucs rigolos avec si le batiment casse :D
        /// </summary>
        public List<Cube> _listeDesCubes;
        #endregion

        public Batiment(ref SceneManager scm, Vector3 position, String nomBatiment,int PdV,int nbCubeBois, int NbCubePierre)
        {
            //Création de l'Entity et du Scenenode à la position
            _node = scm.RootSceneNode.CreateChildSceneNode(NAMENODE + nomBatiment, position);
            
            //Enregistrement du Scenemanager
            _scm = scm;

            //Enregistrement du nom du personnage
            _nomEntity = nomBatiment;

            //Enregistrement des variables
            _pointsDeVie = PdV;
            _position = position;
            _nbCubeBoisNecessaire = nbCubeBois;
            _nbCubePierreNecessaire = NbCubePierre;
            _listeDesCubes = new List<Cube>();
        }

        /// <summary>
        /// Ajoute un cube et le retire de ce qu'il reste à ajouter
        /// </summary>
        /// <param name="C">cube à ajouter</param>
        /// <returns>bool si c'est bon false si on a pas besoin du cube</returns>
        protected bool ajoutCube(Cube C)//peut etre passer la référence ici, je sais pas trop
        {
            if (C.Type == TypeCube.Bois && _nbCubeBoisNecessaire > 0)
            {
                _nbCubeBoisNecessaire--;
                _listeDesCubes.Add(C);
                C.Deplacable = false;
                return true;
            }
            if (C.Type == TypeCube.Pierre && _nbCubePierreNecessaire > 0)
            {
                _nbCubePierreNecessaire--;
                _listeDesCubes.Add(C);
                C.Deplacable = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Si la maison est finie
        /// </summary>
        /// <returns></returns>
        public bool isFinish()
        {
            if (_nbCubeBoisNecessaire + _nbCubePierreNecessaire == 0)
                return true;
            else
                return false;
        }
    }
}
