using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseMogre
{
    public interface IComKnowledgeQuery
    {
        /// <summary>
        /// Envoie une requète
        /// </summary>
        /// <param name="iKQ">Requète</param>
        /// <returns></returns>
        Result send(KnowledgeQuery iKQ);
    }

    public enum Result
    {
        FAIL,
        OK
    }

    public enum Motif
    {
        Localisation,
        Donner,
        Prendre,
        Coup
    }

    public struct KnowledgeQuery
    {
        #region Variables
        public Motif objectif;
        public List<String> parametres;
        #endregion

        #region Constructeur
        KnowledgeQuery(Motif iObj, List<String> iParametres)
        {
            objectif = iObj;
            parametres = new List<String>(iParametres);
        }
        KnowledgeQuery(Motif iObj, params String[] iParametres)
        {
            objectif = iObj;
            parametres = new List<String>();
            foreach (String p in iParametres)
            {
                parametres.Add(p);
            }
        }
        #endregion
    }
}
