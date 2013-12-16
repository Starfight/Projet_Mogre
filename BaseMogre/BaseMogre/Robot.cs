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
        /// ataque des robots
        /// </summary>
        private const int ATK = 10;

        /// <summary>
        /// défense des robots
        /// </summary>
        private const int DEF = 10;
        
        #endregion

        #region Variables
        /// <summary>
        /// Animation du robot lors de la marche
        /// </summary>
        private AnimationState _robotAnim;
        #endregion

        #region Propriété
        public override Vector3 Position
        {
            get
            {
                Vector3 p = base.Position;
                p.y = 0;
                return p;
            }
        }
        #endregion

        #region Constructeur
        /// <summary>
        /// Création du robot
        /// </summary>
        /// <param name="scm">Scenemanager d'intégration du robot</param>
        /// <param name="position">Position de départ</param>
        public Robot(ref SceneManager scm, Vector3 position)
            : base(ref scm, position, NAMEDEFAULT + _COUNT, NAMEMESHROBOT,ATK,DEF,PVMAX)
        {
            Light L = new Light("pointLight");
            L.DiffuseColour = ColourValue.Red;
            L.SpecularColour = ColourValue.Red;
            L.Position = position;
            //Compteur de robots
            _COUNT++;            
            this._node.AttachObject(L);
            //Initialisation des caractéristiques
            _pointsDeVie = PVMAX;
            
        }
        #endregion

        #region Méthodes privées
        protected override bool Update(FrameEvent fEvt)
        {
            if (_combat)
            {
                if (!_robotAnim.HasEnded) //Animation
                {
                    _robotAnim.AddTime(fEvt.timeSinceLastFrame * Variables.VITESSEROBOT / 50);
                }
                else //fin du combat
                {
                    _combat = false;

                    //Redémarrage de l'animation
                    _robotAnim.Enabled = false;
                    _robotAnim = _entity.GetAnimationState("Walk");
                    _robotAnim.Loop = true;
                    _robotAnim.Enabled = true;
                }
            }
            else
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
                    float move = Variables.VITESSEROBOT * (fEvt.timeSinceLastFrame);
                    _node.Translate(_vDirection * move);
                    _distance -= move;

                    //Animation
                    _robotAnim.AddTime(fEvt.timeSinceLastFrame * Variables.VITESSEROBOT / 30);
                }
                else if (!_needToDecide)
                {
                    //Stoppe l'animation
                    _robotAnim.Enabled = false;
                    _robotAnim = _entity.GetAnimationState("Idle");

                    //Indique qu'il faut prendre une décision
                    _needToDecide = true;
                }
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

                //Hors combat
                if (!_combat)
                {
                    //Evite la collision avec un autre robot ou un cube
                    if ((kq.Classe == Classe.Robot) || (kq.Classe == Classe.Cube) || (kq.Classe == Classe.Maison))
                    {
                        EviteCollision(kq.Position);
                    }
                    else if (kq.Classe == Classe.Ogre)
                    {
                        //Stoppe le robot
                        _combat = true;
                        
                        //Met en animation de combat
                        _robotAnim.Enabled = false;
                        _robotAnim = _entity.GetAnimationState("Shoot");
                        _robotAnim.TimePosition = 0;
                        _robotAnim.Loop = false;
                        _robotAnim.Enabled = true;
                        
                        //Attaque
                        int atk;
                        if (int.TryParse(kq.Parametre, out atk))
                        {
                            this.Combat(atk);
                            Log.writeNewLine("contact " + this._nomEntity + " vs " + kq.Classe.ToString() + " " + this._pointsDeVie + " pv restants au robot");
                        }
                    }
                }
            }
            else
            {
                //Donne une chance de partir loin
                if (Environnement.getInstance().getUneChanceSur(CHANCEPOURPARTIRLOIN))
                {
                    Destination = Environnement.getRandomHorizontalVecteur();
                }
                else
                {
                    //Donne une nouvelle destination aléatoire
                    Destination = Environnement.getRandomDestination(Position);
                }
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
