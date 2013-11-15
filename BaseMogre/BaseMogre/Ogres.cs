﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace BaseMogre
{
    abstract class Ogres : Personnage
    {
        #region Constantes/Variables statiques
        /// <summary>
        /// Nom du mesh
        /// </summary>
        private const String NAMEMESHROBOT = "ogrehead.mesh";

        /// <summary>
        /// Nom par défaut
        /// </summary>
        private const String NAMEDEFAULT = "ogres";

        /// <summary>
        /// Vitesse des ogres
        /// </summary>
        protected const float VITESSE = 50;

        /// <summary>
        /// Compteur d'ogre
        /// </summary>
        private static int _COUNT = 0;
        #endregion

        #region Variables
        /// <summary>
        /// Cube détenu par l'ogre
        /// </summary>
        protected Cube _cube;

        /// <summary>
        /// puissance d'attaque de l'ogre
        /// </summary>
        private int _atk;

        /// <summary>
        /// défense de l'ogre
        /// </summary>
        private int _def;
        #endregion

        #region Constructeur
        public Ogres()
        {
            this._cube = null;
        }
        public Ogres(ref SceneManager scm,Vector3 position,int atk, int def)
            : base(ref scm, position, NAMEDEFAULT + _COUNT, NAMEMESHROBOT)
        {
            _COUNT++;
            this._cube = null;
            this._atk = atk;
            this._def = def;
        }
        #endregion

        #region Méthodes privées
        protected override bool Update(FrameEvent fEvt)
        {
            //Au changement de direction
            if (_DestinationChanged)
            {
                //Modification de la direction et de la distance
                _vDirection = Destination - Position;
                _distance = _vDirection.Length;
                _vDirection.Normalise();

                //Rotation
                Vector3 src = _node.Orientation * Vector3.UNIT_Z;
                Quaternion quat = src.GetRotationTo(_vDirection);
                _node.Rotate(quat);

                //Remise à zero du booleen
                _DestinationChanged = false;
            }

            //Mise à jour de la position
            if (_distance > 10)
            {
                float move = VITESSE * (fEvt.timeSinceLastFrame);
                _node.Translate(_vDirection * move);
                _distance -= move;
            }
            else if (!_needToDecide)
            {
                //Indique qu'il faut prendre une décision
                _needToDecide = true;
            }
            
            return true;
        }
        #endregion

        #region méthodes
        /// <summary>
        /// méthode permettant de récupérer le cube de l'ogre et le supprimme (de l'ogre)
        /// </summary>
        /// <returns>cube possédé par l'ogre</returns>
        public Cube utiliseCube()
        {
            Cube c = this._cube;
            this._cube = null;
            return c;
        }
        /// <summary>
        /// méthode de ramassage d'un cube par l'ogre
        /// </summary>
        /// <param name="c">cube que l'on veux ramasser</param>
        /// <returns>true si il a été ramassé, false si l'inventaire est plein</returns>
        public bool ramassecube(Cube c)
        {
            if (this._cube != null)
            {
                this._cube = c;
                return true;
            }
            return false;
        }
        /// <summary>
        /// méthode permettant de tester le cube que posséde l'ogre
        /// </summary>
        /// <param name="typeCubeVoulu">type de cube cherché</param>
        /// <returns>true si l'ogre posséde un cube du bon type / false sinon</returns>
        public bool requeteCube(Type typeCubeVoulu)
        {
            if (this._cube != null && this._cube.GetType() == typeCubeVoulu)
                return true;
            return false;
        }
        #endregion
    }
}
