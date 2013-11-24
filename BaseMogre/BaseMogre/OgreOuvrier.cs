using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using System.Threading;
using System.Windows.Forms;

namespace BaseMogre
{
    class OgreOuvrier:Ogres
    {
        #region constantes
        /// <summary>
        /// Caractéristique des PV
        /// </summary>
        private const int PVMAX = 20;

        /// <summary>
        /// Chance de pouvoir construire une maison (1 sur cette valeur)
        /// </summary>
        private const int CHANCEPOURUNEMAISON = 10;
        #endregion

        #region Variables
        /// <summary>
        /// Maison en cours de construction
        /// </summary>
        private MaisonInfo _currentMaison;
        #endregion

        #region constructeurs
        public OgreOuvrier(ref SceneManager scm, Vector3 position)
            : base(ref scm, position, 5, 5)
        {
            this._pointsDeVie = PVMAX;
            
            //init
            _currentMaison.Reset();
        }
        #endregion

        #region méthodes privées
        /// <summary>
        /// Prise de décision
        /// </summary>
        protected override void Decision()
        {
            //S'il y a des messages à traiter
            if (_listComInput.Count > 0)
            {
                KnowledgeQuery kq = _listComInput.Dequeue();

                //Ramassage de cube
                if ((kq.Classe == Classe.Cube)&&(_cube==null))
                {
                    ramassecube(Environnement.getInstance().getCube(kq.Nom));
                }
                //Evite la collision
                else if (((kq.Classe == Classe.Cube) && (_cube != null))|| //si un l'ogre possède déjà un cube
                        (kq.Classe == Classe.Ogre))                        //si l'ogre rencontre un autre ogre 
                {
                    EviteCollision(kq.Position);
                }
                //Rencontre d'une maison
                else if (kq.Classe == Classe.Maison) 
                {
                    _currentMaison = new MaisonInfo(kq.Nom, kq.Position);
                    if (_cube!=null)
                    {
                        //Essaie de donner le cube à la maison
                        if (Environnement.getInstance().giveCube(_cube, _currentMaison.nom))
                        {
                            _cube = null;
                        }
                        else
                        {
                            _currentMaison.Reset();
                        }
                    }
                    EviteCollision(kq.Position);
                }
            }
            else
            {
                //Si il n'a pas repéré de maison et qu'il a un cube
                if ((_currentMaison.isEmpty()) && (_cube != null))
                {
                    //Donne une chance de crée une maison
                    if (Environnement.getInstance().getUneChanceSur(CHANCEPOURUNEMAISON))
                    {
                        KnowledgeQuery kq = new KnowledgeQuery(NomEntity, Classe.Maison, "", Position);
                        Environnement.getInstance().send(kq);
                    }
                }
                //Si il a repéré une maison et qu'il a un cube 
                else if ((!_currentMaison.isEmpty()) && (_cube != null))
                {
                    Destination = _currentMaison.position;
                }
                //Sinon
                else
                {
                    //Donne une nouvelle destination aléatoire
                    Destination = Environnement.getRandomHorizontalVecteur();
                }
            }
        }

        /// <summary>
        /// Envoi un message
        /// </summary>
        /// <param name="iKQ"></param>
        private void envoyer(KnowledgeQuery iKQ)
        {
            Result res;
            do
            {
                res = Environnement.getInstance().send(iKQ);
            } while (res == Result.FAIL);
        }
        #endregion
    }
}
