using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace BaseMogre
{
    class OgreBatisseur:Ogres
    {
        #region constantes
        /// <summary>
        /// Caractéristique des PV
        /// </summary>
        private const int PVMAX = 20;

        /// <summary>
        /// attaque des ogres batisseurs
        /// </summary>
        private const int ATK = 3;

        /// <summary>
        /// défense des ogres batisseurs
        /// </summary>
        private const int DEF = 5;

        /// <summary>
        /// Chance de pouvoir construire une tour (1 sur cette valeur)
        /// </summary>
        private const int CHANCEPOURUNETOUR = 10;
        #endregion

        #region variable
        /// <summary>
        /// Informations sur la tour cible
        /// </summary>
        private TourInfo _tourCible;
        #endregion

        #region constructeurs
        public OgreBatisseur(ref SceneManager scm, Vector3 position, TourInfo tourcible = new TourInfo())
            : base(ref scm, position, ATK, DEF, PVMAX)
        {
            //Plus gros pour les différentier
            _node.Scale(new Vector3(1.5f,1.5f,1.5f));

            //Attribution de la tour si existante
            if (_tourCible.isEmpty())
                _tourCible.Reset();
            else
                _tourCible = tourcible;
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
                if ((kq.Classe == Classe.Cube) && (_cube == null))
                {
                    bool ok = ramassecube(Environnement.getInstance().getCube(kq.Nom));
                    //S'il connait une maison, change la destination
                    if ((!_tourCible.isEmpty()) && ok)
                    {
                        Destination = _tourCible.position;
                    }
                }
                //Evite la collision
                else if (((kq.Classe == Classe.Cube) && (_cube != null)) || //si un l'ogre possède déjà un cube
                        (kq.Classe == Classe.Ogre) ||                      //si l'ogre rencontre un autre ogre
                        (kq.Classe == Classe.Robot))                       //si l'ogre rencontre un robot     
                {
                    if (kq.Classe == Classe.Robot)
                    {
                        int atk;
                        if (int.TryParse(kq.Parametre, out atk))
                        {
                            this.Combat(atk);
                            //Log.writeNewLine("contact " + this._nomEntity + " vs " + kq.Classe.ToString() + " " + this._pointsDeVie + " pv restants à l'ogre batisseur");
                        }
                    }
                    EviteCollision(kq.Position);
                }
                //Rencontre d'une tour
                else if (kq.Classe == Classe.Tour)
                {
                    //Si la tour n'est pas complète
                    if (kq.Parametre == "False")
                    {
                        _tourCible = new TourInfo(kq.Nom, kq.Position);
                        if (_cube != null)
                        {
                            //Essaie de donner le cube à la tour
                            if (Environnement.getInstance().giveCube(_cube, _tourCible.nom))
                            {
                                _cube = null;
                            }
                            else //Si le cube n'est pas accepté
                            {
                                _cube.Dispose();
                                _cube = null;
                            }
                        }
                    }
                    EviteCollision(kq.Position);
                }
                //Rencontre d'une maison
                else if (kq.Classe == Classe.Maison)
                {
                    EviteCollision(kq.Position);
                }
            }
            else
            {
                //Si l'ogre a un cube
                if (_cube != null)
                {
                    //Si l'ogre a une connaissance de la tour
                    if (_tourCible.isEmpty())
                    {
                        //Donne une chance de crée une tour
                        if (Environnement.getInstance().getUneChanceSur(CHANCEPOURUNETOUR))
                        {
                            KnowledgeQuery kq = new KnowledgeQuery(NomEntity, Classe.Tour, "", Position);
                            Environnement.getInstance().send(kq);
                        }
                        else
                        {
                            //Donne une nouvelle destination aléatoire
                            Destination = Environnement.getRandomDestination(Position);
                        }
                    }
                    //Sinon recherche d'une position idéale
                    else
                    {
                        Destination = _tourCible.position;
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
        }
        #endregion
    }
}
