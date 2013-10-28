﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace BaseMogre
{
    abstract class Personnage
    {
        #region Constantes
        /// <summary>
        /// Nom du node par défaut
        /// </summary>
        private const String NAMENODE = "Node_";
        #endregion

        #region Variables
        /// <summary>
        /// Points de vie 
        /// </summary>
        protected int _pointsDeVie;

        /// <summary>
        /// Référence du scenemanager
        /// </summary>
        protected SceneManager _scm;

        /// <summary>
        /// Entity représentant le personnage
        /// </summary>
        protected Entity _entity;

        /// <summary>
        /// Scenenode du personnage
        /// </summary>
        protected SceneNode _node;

        /// <summary>
        /// Nom du personnage
        /// </summary>
        protected String _nomEntity;

        /// <summary>
        /// Liste des objets possédés
        /// </summary>
        private List<Objet> _objetsPerso;

        
        #endregion

        #region Constructeur
        public Personnage(ref SceneManager scm, Vector3 position, String nomPersonnage, String nomMesh)
        {
            //Création de l'Entity et du Scenenode à la position
            _entity = scm.CreateEntity(nomPersonnage, nomMesh);
            _node = scm.RootSceneNode.CreateChildSceneNode(NAMENODE + nomPersonnage, position);
            _node.AttachObject(_entity);

            //Enregistrement du Scenemanager
            _scm = scm;

            //Enregistrement du nom du personnage
            _nomEntity = nomPersonnage;
        }
        #endregion

        #region Getters et Setters
        public String NomEntity
        {
            get { return _nomEntity; }
        }
        public Vector3 Position
        {
            get { return _entity.BoundingBox.Center; }
        }
        public List<Objet> ObjetsPerso
        {
            get { return _objetsPerso; }
            set { _objetsPerso = value; }
        }
        #endregion

        #region methodes
        /// <summary>
        /// méthodes de recherche dans l'inventaire
        /// </summary>
        /// <param name="t">type d'objet recherché</param>
        /// <returns>si ou ou non il posséde un objet</returns>
        public bool possedeObjet(Type t)
        {
            foreach(Objet obj in this._objetsPerso)
            {
                if(t == obj.GetType())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Permet de récuperer un objet dans l'inventaire
        /// </summary>
        /// <param name="t">type de l'objet voulu</param>
        /// <returns>null ou l'objet voulu</returns>
        public Objet getObjet(Type t)
        {
            Objet O = null;
            foreach (Objet obj in this._objetsPerso)
            {
                if (t == obj.GetType())
                {
                    O= obj;
                    break;
                }
            }
            if (O != null)
                this._objetsPerso.Remove(O);
            return O;
        }

        /// <summary>
        /// ajout d'un objet à l'inventaire
        /// </summary>
        /// <param name="o">objet à ajouter</param>
        public void addObjet(Objet o)
        {
            if (o != null)
                this._objetsPerso.Add(o);
        }
        #endregion

    }
}
