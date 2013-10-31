using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MogreFramework;
using System.Threading;

namespace BaseMogre
{
    class Environnement : IComKnowledgeQuery
    {
        #region Variables Statiques
        private static Environnement ENV_DEFAULT;
        private const String ENV_NAME = "Environnement";
        #endregion

        #region Variables
        /// <summary>
        /// Liste des personnages
        /// </summary>
        private Dictionary<String, Personnage> _ListPersonnages;

        /// <summary>
        /// Liste des maisons
        /// </summary>
        private Dictionary<String, Maison> _ListMaisons;

        /// <summary>
        /// Scenemanager
        /// </summary>
        private SceneManager _scm;

        /// <summary>
        /// Piles FIFO pour la communication
        /// </summary>
        private Queue<KnowledgeQuery> _ListOfComInput;
        private Queue<KnowledgeQuery> _ListOfComOutput;

        /// <summary>
        /// Thread qui traite les communications
        /// </summary>
        private Thread _ComThread;

        /// <summary>
        /// Booleen pour la gestion du thread de com
        /// </summary>
        private volatile bool _stop;
        #endregion

        #region Constructeur/Destructeur
        private Environnement(ref SceneManager scm)
        {
            _scm = scm;
            _ListPersonnages = new Dictionary<string, Personnage>();
            _ListMaisons = new Dictionary<string, Maison>();
            _ListOfComInput = new Queue<KnowledgeQuery>();
            _ListOfComOutput = new Queue<KnowledgeQuery>();
        }

        ~Environnement()
        {
            //fini le thread
            _stop = true;
            _ComThread.Join();
        }
        #endregion

        #region Méthodes Statiques
        public static void createEnvironnement(ref SceneManager scm)
        {
            ENV_DEFAULT = new Environnement(ref scm);
            ENV_DEFAULT.initPersonnages(0, 0);

            //Démarre le thread
            ENV_DEFAULT._ComThread = new Thread(ENV_DEFAULT.ProcessComInOut);
            ENV_DEFAULT._stop = false;
            ENV_DEFAULT._ComThread.Start();
        }
        public static Environnement getInstance()
        {
            return ENV_DEFAULT;
        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// Envoi un message à l'environnement
        /// </summary>
        /// <param name="iKQ">Requète de communication</param>
        /// <returns>Valeur si la communication s'est bien déroulée</returns>
        public Result send(KnowledgeQuery iKQ)
        {
            if (iKQ.parametres != null)
            {
                _ListOfComInput.Enqueue(iKQ);
                return Result.OK;
            }
            else
            {
                return Result.FAIL;
            }
        }
        #endregion

        #region Méthodes privées
        /// <summary>
        /// Initialisation des personnages
        /// </summary>
        /// <param name="iNbOgres">Nombre d'ogres ouvriers</param>
        /// <param name="iNbRobots">Nombre de robots</param>
        private void initPersonnages(int iNbOgres, int iNbRobots)
        {
            /*
            for (int i = 0; i < iNbOgres; i++)
            {
                OgreOuvrier o = new OgreOuvrier();
                //TODO
                //_ListPersonnages.Add(o);
            }

            for (int i = 0; i < iNbRobots; i++)
            {
                //TODO positionnement
                Robot r = new Robot(ref _scm, new Vector3());
                _ListPersonnages.Add(r.NomEntity,r);
            }
             */
        }

        /// <summary>
        /// Traite les communications dans un thread à part
        /// </summary>
        private void ProcessComInOut()
        {
            while (!_stop)
            {
                //Reception
                while (_ListOfComInput.Count > 0)
                {
                    KnowledgeQuery kq = _ListOfComInput.Dequeue();
                    ProcessKQ(kq);
                }

                //Envoi
                while (_ListOfComOutput.Count > 0)
                {
                    KnowledgeQuery kq = _ListOfComOutput.Dequeue();
                    if (_ListPersonnages.ContainsKey(kq.destinataire))
                    {
                        Personnage p;
                        _ListPersonnages.TryGetValue(kq.destinataire, out p);
                        p.send(kq);
                    }
                }

                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Traite la knowledge query
        /// </summary>
        /// <param name="iKQ"></param>
        private void ProcessKQ(KnowledgeQuery iKQ)
        {
            Motif obj = iKQ.objectif;
            switch (obj)
            {
                case Motif.Position: ProcessPosition(iKQ);
                    break;
                case Motif.InfoCube :
                    break;
                case Motif.Coup : 
                    break;
                case Motif.Assembler :
                    break;
            }
        }

        private void ProcessPosition(KnowledgeQuery iKQ)
        {
            KnowledgeQuery KQreturn = new KnowledgeQuery(Motif.Position, iKQ.destinataire);

            //Recherche par nom 
            String name = "noname";
            if (iKQ.parametres.ContainsKey(KnowledgeQuery.NOM))
            {
                iKQ.parametres.TryGetValue(KnowledgeQuery.NOM, out name);
                KQreturn.parametres.Add(KnowledgeQuery.NOM, name);
            }

            //Recherche par classe
            if (iKQ.parametres.ContainsKey(KnowledgeQuery.CLASSE))
            {
                String classe = "";
                iKQ.parametres.TryGetValue(KnowledgeQuery.CLASSE, out classe);
                Vector3 v3 = new Vector3();

                //Recherche de la position
                if ((name == "noname")&&(classe == "maison"))
                {
                    v3 = _ListMaisons.First().Value.Position;
                }
                else if ((name != "noname")&&(classe == "maison"))
                {
                    if (_ListMaisons.ContainsKey(name))
                    {
                        Maison m; 
                        _ListMaisons.TryGetValue(name, out m);
                        v3 = m.Position;
                    }
                }

                //Retour de la requète
                KQreturn.setPosition(v3);
                KQreturn.parametres.Add(KnowledgeQuery.CLASSE, classe);
            }
        }
        #endregion
    }
}
