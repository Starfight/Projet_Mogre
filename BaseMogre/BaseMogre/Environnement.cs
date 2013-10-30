using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MogreFramework;

namespace BaseMogre
{
    class Environnement : IComKnowledgeQuery
    {
        #region Variables Statiques
        private static Environnement ENV_DEFAULT;
        #endregion

        #region Variables
        /// <summary>
        /// Liste des personnages
        /// </summary>
        private Dictionary<String, Personnage> _ListPersonnages;

        /// <summary>
        /// Scenemanager
        /// </summary>
        private SceneManager _scm;

        /// <summary>
        /// Pile FIFO pour la communication
        /// </summary>
        private Queue<KnowledgeQuery> _ListOfCom;
        #endregion

        #region Constructeur
        private Environnement(ref SceneManager scm)
        {
            _scm = scm;
            _ListPersonnages = new Dictionary<string, Personnage>();
            _ListOfCom = new Queue<KnowledgeQuery>();
        }
        #endregion

        #region Méthodes Statiques
        public static void createEnvironnement(ref SceneManager scm)
        {
            ENV_DEFAULT = new Environnement(ref scm);
            ENV_DEFAULT.init(0, 0);
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
                _ListOfCom.Enqueue(iKQ);
                return Result.OK;
            }
            else
            {
                return Result.FAIL;
            }
        }
        #endregion

        #region Méthodes privées
        private void init(int iNbOgres, int iNbRobots)
        {
            //Commenter car pose des bug pour le lancement test
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
             * */
        }
        #endregion
    }
}
