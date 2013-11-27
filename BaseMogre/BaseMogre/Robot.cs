using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MogreFramework;

namespace BaseMogre
{
    class Robot : Personnage
    {
        #region Constantes/Variables statiques
        /// <summary>
        /// Nom du mesh
        /// </summary>
        private const String NAMEMESHROBOT = "robot.mesh";

        /// <summary>
        /// Nom par défaut
        /// </summary>
        private const String NAMEDEFAULT = "robot";

        /// <summary>
        /// Caractéristique des PV
        /// </summary>
        private const int PVMAX = 20;

        /// <summary>
        /// Compteur de robot
        /// </summary>
        private static int _COUNT = 0;

        /// <summary>
        /// Vitesse des robots
        /// </summary>
        private const float VITESSE = 50;
        #endregion

        #region Variables
        /// <summary>
        /// Animation du robot lors de la marche
        /// </summary>
        private AnimationState _robotAnim;
        #endregion

        #region Constructeur
        /// <summary>
        /// Création du robot
        /// </summary>
        /// <param name="scm">Scenemanager d'intégration du robot</param>
        /// <param name="position">Position de départ</param>
        public Robot(ref SceneManager scm, Vector3 position)
            : base(ref scm, position, NAMEDEFAULT + _COUNT, NAMEMESHROBOT)
        {
            //Compteur de robots
            _COUNT++;            

            //Initialisation de l'animation
            _robotAnim = _entity.GetAnimationState("Idle");
            _robotAnim.Loop = true;
            _robotAnim.Enabled = true;

            //Initialisation des caractéristiques
            _pointsDeVie = PVMAX;
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
                Vector3 src = _node.Orientation * Vector3.UNIT_X;
                Quaternion quat = src.GetRotationTo(_vDirection);
                _node.Rotate(quat);

                //Remise à zero du booleen
                _DestinationChanged = false;

                //Démarrage de l'animation
                _robotAnim = _entity.GetAnimationState("Walk");
                _robotAnim.Loop = true;
                _robotAnim.Enabled = true;
            }

            //Mise à jour de la position
            if (_distance > 10)
            {
                //Position
                float move = VITESSE * (fEvt.timeSinceLastFrame);
                _node.Translate(_vDirection * move);
                _distance -= move;

                //Animation
                _robotAnim.AddTime(fEvt.timeSinceLastFrame * VITESSE / 20);
            }
            else if (!_needToDecide)
            {
                //Stoppe l'animation
                _robotAnim = _entity.GetAnimationState("Idle");

                //Indique qu'il faut prendre une décision
                _needToDecide = true;
            }

            return true;
        }

        /// <summary>
        /// Prise de décision du robot
        /// </summary>
        protected override void Decision()
        {
            //S'il y a des messages à traiter
            if (_listComInput.Count > 0)
            {
                KnowledgeQuery kq = _listComInput.Dequeue();

                //Evite la collision avec un autre robot ou un cube
                if ((kq.Classe == Classe.Robot)||(kq.Classe == Classe.Cube))
                {
                    EviteCollision(kq.Position);
                }
            }
            else
            {
                Destination = Environnement.getRandomHorizontalVecteur();
            }
        }
        #endregion

        #region Méthodes publiques
        public override Classe getClasse()
        {
            return Classe.Robot;
        }
        #endregion
    }
}
