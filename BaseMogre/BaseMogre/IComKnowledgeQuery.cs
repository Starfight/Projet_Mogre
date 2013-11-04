using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

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
        Position,
        InfoCube,
        ObtientCube,
        DonneCube,
        Coup,
        Assembler
    }

    public struct KnowledgeQuery
    {
        #region constantes publiques
        public const String POSITION = "position";
        public const String CLASSE = "classe";
        public const String NOM = "nom";
        public const String TYPECUBE = "cube";
        #endregion

        #region Variables
        public Motif objectif;
        public String destinataire;
        public Dictionary<String, String> parametres;
        #endregion

        #region Constructeur
        public KnowledgeQuery(Motif iObj, String iDestinataire, Dictionary<String, String> iParametres)
        {
            objectif = iObj;
            destinataire = iDestinataire;
            parametres = new Dictionary<String, String>(iParametres);
        }
        public KnowledgeQuery(Motif iObj, String iDestinataire, params String[] iParametres)
        {
            objectif = iObj;
            destinataire = iDestinataire;
            parametres = new Dictionary<String, String>();
            for (int i = 0; i < iParametres.Count()-iParametres.Count()%2; i+=2)
            {
                parametres.Add(iParametres[i], iParametres[i + 1]);
            }
        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// Obtient une position dans les paramètres si elle existe
        /// </summary>
        /// <returns></returns>
        public Vector3 getPosition()
        {
            Vector3 v3 = new Vector3();

            if (parametres.ContainsKey(POSITION))
            {
                String v3string = "";
                parametres.TryGetValue(POSITION, out v3string);
                String[] v3tab = v3string.Split(';');

                if (v3tab.Count() == 3)
                {
                    int[] v3intTab = new int[3];
                    for (int i = 0; i < 3; i++)
                    {
                        v3intTab[i] = Int32.Parse(v3tab[i]);
                    }
                    v3 = new Vector3(v3intTab[0], v3intTab[1], v3intTab[2]);
                }
            }
            return v3;
        }

        /// <summary>
        /// Met une position en paramètre
        /// </summary>
        /// <param name="v3"></param>
        public void setPosition(Vector3 v3)
        {
            String v3string = v3.x + ";" + v3.y + ";" + v3.z;
            parametres.Add(POSITION, v3string);
        }
        #endregion
    }
}
