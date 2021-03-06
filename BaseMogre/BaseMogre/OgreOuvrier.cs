﻿using System;
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

        /// <summary>
        /// ataque des robots
        /// </summary>
        private const int ATK = 5;

        /// <summary>
        /// défense des robots
        /// </summary>
        private const int DEF = 5;
        #endregion

        #region Variables
        /// <summary>
        /// Maison en cours de construction
        /// </summary>
        private MaisonInfo _currentMaison;
        #endregion

        #region constructeurs
        public OgreOuvrier(ref SceneManager scm, Vector3 position)
            : base(ref scm, position, ATK, DEF,PVMAX)
        {            
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
                    bool ok = ramassecube(Environnement.getInstance().getCube(kq.Nom));
                    //S'il connait une maison, change la destination
                    if ((!_currentMaison.isEmpty()) && ok)
                    {
                        Destination = _currentMaison.position;
                    }
                }
                //Evite la collision
                else if (((kq.Classe == Classe.Cube) && (_cube != null))|| //si un l'ogre possède déjà un cube
                        (kq.Classe == Classe.Ogre) ||                      //si l'ogre rencontre un autre ogre
                        (kq.Classe == Classe.Robot))                       //si l'ogre rencontre un robot     
                {
                    if (kq.Classe == Classe.Robot) //combat
                    {
                        int atk;
                        if (int.TryParse(kq.Parametre, out atk))
                        {
                            this.Combat(atk);
                        }
                    }
                    else if ((kq.Classe == Classe.Ogre)&&(!_currentMaison.isEmpty())) //transmet une info sur la maison en cours
                    {
                        KnowledgeQuery kqToSend = new KnowledgeQuery(kq.Nom, Classe.Maison, _currentMaison.nom, _currentMaison.position, "info");
                        Environnement.getInstance().send(kqToSend);
                    }
                    EviteCollision(kq.Position);
                }
                //Rencontre d'une maison
                else if (kq.Classe == Classe.Maison) 
                {
                    
                    if ((kq.Parametre == "False")|| //Si la maison n'est pas complète
                        (kq.Parametre == "info")) //Si c'est une info
                    {
                        _currentMaison = new MaisonInfo(kq.Nom, kq.Position);
                        if (kq.Parametre == "False")
                        {
                            if (_cube != null)
                            {
                                //Essaie de donner le cube à la maison
                                if (Environnement.getInstance().giveCube(_cube, _currentMaison.nom))
                                {
                                    _cube = null;
                                }
                                else //Si le cube n'est pas accepté
                                {
                                    _currentMaison.Reset();
                                }
                            }
                        }
                    }
                    else if ((kq.Parametre == "True")&&(kq.Nom==_currentMaison.nom)) //Si la maison est complète
                    {
                        _currentMaison.Reset();
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
                    else
                    {
                        //Donne une nouvelle destination aléatoire
                        Destination = Environnement.getRandomDestination(Position);
                    }
                }
                //Si il a repéré une maison et qu'il a un cube 
                else if ((!_currentMaison.isEmpty()) && (_cube != null))
                {
                    Destination = _currentMaison.position;

                    //Solution paliative pour éviter que les ogres se retrouvent coincé dans une maison
                    if ((Destination - Position).SquaredLength < 100)
                    {
                        EviteCollision(_currentMaison.position);
                    }
                }
                //Sinon
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
