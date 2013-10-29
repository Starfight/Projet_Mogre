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
        private List<Robot> _ListOfRobots;
        private List<Ogres> _ListOfOgres;
        #endregion

        #region Constructeur
        private Environnement()
        {
            _ListOfOgres = new List<Ogres>();
            _ListOfRobots = new List<Robot>();
        }
        #endregion

        #region Méthodes Statiques
        public static void createEnvironnement()
        {
            ENV_DEFAULT = new Environnement();
            ENV_DEFAULT.init(0, 0);
        }
        public static Environnement getInstance()
        {
            return ENV_DEFAULT;
        }
        #endregion

        #region Méthodes publiques
        public Result send(KnowledgeQuery iKQ)
        {
            return Result.OK;
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
                _ListOfOgres.Add(o);
            }

            for (int i = 0; i < iNbRobots; i++)
            {
                //TODO
                //Robot r = new Robot();
                //_ListOfRobots.Add(r);
            }
             * */
        }
        #endregion
    }
}
