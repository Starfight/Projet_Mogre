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
        #endregion

        #region Variables
        /// <summary>
        /// Maison en cours de construction
        /// </summary>
        private MaisonInfo _currentMaison;

        /// <summary>
        /// Type du prochain cube à chercher
        /// </summary>
        private volatile TypeCube _typeNextCube;
        #endregion

        #region constructeurs
        public OgreOuvrier(ref SceneManager scm, Vector3 position)
            : base(ref scm, position, 5, 5)
        {
            this._pointsDeVie = PVMAX;
            
            //init
            _currentMaison.Reset();
            _typeNextCube = TypeCube.Aucun;
        }
        #endregion

        #region méthodes privées
        protected override void Start()
        {
            //Variables pour la boucle
            KnowledgeQuery kq;

            //boucle principale
            while (!_stop)
            {                
                //envoi un message pour savoir où construire/chercher une maison
                //--------------------------------------------------------------
                //Crée la requète
                kq = new KnowledgeQuery(Motif.Position, this.NomEntity);
                kq.parametres.Add(KnowledgeQuery.CLASSE, "maison");
                envoyer(kq);

                //Attente du retour message pour la construction
                while (_currentMaison.isEmpty())
                {
                    Thread.Sleep(10);
                }

                //va à l'emplacement si maison en construction
                GoTo(_currentMaison.position);

                //demande le prochain cube pour la maison
                //---------------------------------------
                //Crée la requète
                kq = new KnowledgeQuery(Motif.InfoCube, this.NomEntity);
                kq.parametres.Add(KnowledgeQuery.CLASSE, "maison");
                kq.parametres.Add(KnowledgeQuery.NOM, _currentMaison.nom);
                envoyer(kq);

                //Attente du retour message pour le prochain cube
                while (_typeNextCube == TypeCube.Aucun)
                {
                    Thread.Sleep(10);
                }

                //va à la position des entrepôts 
                //------------------------------
                String nextcube = "";
                switch (_typeNextCube)
                {
                    case TypeCube.Bois : GoTo(Environnement.getInstance().PositionEntrepotBois);
                                         nextcube = "bois";
                        break;
                    case TypeCube.Pierre : GoTo(Environnement.getInstance().PositionEntrepotPierre);
                                           nextcube = "pierre";
                        break;
                }

                //demande un cube
                kq = new KnowledgeQuery(Motif.ObtientCube, this.NomEntity);
                kq.parametres.Add(KnowledgeQuery.TYPECUBE, nextcube);
                envoyer(kq);

                //Attente du retour message pour l'obtention du cube
                while (_cube == null)
                {
                    Thread.Sleep(10);
                }

                //retourne à la maison
                GoTo(_currentMaison.position);

                //demande de donner le cube
                kq = new KnowledgeQuery(Motif.DonneCube, this.NomEntity);
                kq.parametres.Add(KnowledgeQuery.TYPECUBE, nextcube);
                envoyer(kq);
                utiliseCube();
            }
        }

        protected override bool Update(FrameEvent fEvt)
        {
            //TODO
            return true;
        }

        /// <summary>
        /// Méthode pour aller à une position donnée
        /// </summary>
        /// <param name="position"></param>
        private void GoTo(Vector3 position)
        {
            //TODO
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
