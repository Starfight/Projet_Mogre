﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace BaseMogre
{
    class Cube : Objet
    {
        #region Constantes/Variables statiques
        /// <summary>
        /// Nom du node par défaut
        /// </summary>
        private const String NAMENODE = "Node_";

        /// <summary>
        /// Compteur de cube
        /// </summary>
        private static int _COUNT = 0;
        #endregion

        #region Attributs
        /// <summary>
        /// Type de cube
        /// </summary>
        private TypeCube _type;
      
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
        /// Nom du cube
        /// </summary>
        protected String _nomEntity;

        /// <summary>
        /// position du cube
        /// </summary>
        protected Vector3 _position;
        #endregion

        #region Getters et Setters
        /// <summary>
        /// Nom de l'entité
        /// </summary>
        public String NomEntity
        {
            get { return _nomEntity; }
        }

        /// <summary>
        /// Position du cube
        /// </summary>
        public Vector3 Position
        {
            get { return _node.Position; }
            set { _node.Position = value; }
        }

        /// <summary>
        /// Orientation du cube
        /// </summary>
        public Quaternion Orientation
        {
            get { return _node.Orientation; }
        }
        #endregion

        #region Constructeur
        public Cube(ref SceneManager scm, Vector3 position, TypeCube type)
        {
            //Création de l'Entity et du Scenenode à la position
            _COUNT++;
            _type = type;
            _position = position;
            _nomEntity = "Cube" + _COUNT;
            _entity = scm.CreateEntity(_nomEntity, "cube.mesh");
            _node = scm.RootSceneNode.CreateChildSceneNode(NAMENODE + _COUNT, position);
            _node.SetPosition(position.x,position.y,position.z);
            _node.Scale(new Vector3((float)0.3));
            _node.AttachObject(_entity);

            //Mise en place de la texture
            if (type == TypeCube.Bois)
            {
                _entity.SetMaterialName("Texture/CubeBois");
            }
            else if (type == TypeCube.Pierre)
            {
                _entity.SetMaterialName("Texture/CubePierre");
            }

            //Enregistrement du Scenemanager
            _scm = scm;

        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// Rotation du cube
        /// </summary>
        /// <param name="quat"></param>
        public void Rotate(Quaternion quat)
        {
            _node.Rotate(quat);
        }

        /// <summary>
        /// Transalation du cube
        /// </summary>
        /// <param name="vec"></param>
        public void Translate(Vector3 vec)
        {
            _node.Translate(vec);
        }
        #endregion
    }


    public enum TypeCube
    {
        Bois,
        Pierre
    }
}
