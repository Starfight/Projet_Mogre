using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BaseMogre
{
    public static class Log
    {
        #region attributs statics
        /// <summary>
        /// Chemin d'accés au fichier de log
        /// </summary>
        static string log_path = "resultats.log";

        /// <summary>
        /// objet pour le verrouillage de l'écriture dans le fichier
        /// </summary>
        private static object threadlock;
        #endregion

        #region Méthodes
        /// <summary>
        /// écriture d'une ligne dans le fichier
        /// </summary>
        /// <param name="texte">texte à acrire</param>
        /// <returns>réussite ou echec de l'ecriture</returns>
        public static bool writeNewLine(string texte)
        {
            try
            {
                lock (threadlock)
                {
                    File.AppendAllText(log_path, DateTime.Now.ToString() + " => " + texte + Environment.NewLine);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// écriture de plusieurs lignes dans le fichier
        /// </summary>
        /// <param name="textes">tableau de textes a ecrire</param>
        /// <returns>réussite ou echec de l'ecriture</returns>
        public static bool writeNewLine(string[] textes)
        {
            string chaine = DateTime.Now.ToString() + " => " + Environment.NewLine;
            try
            {
                foreach (string S in textes)
                {
                    chaine += S + Environment.NewLine;
                }
                lock (threadlock)
                {
                    File.AppendAllText(log_path, chaine);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// RAZ du fichier de log
        /// </summary>
        static Log()
        {
            threadlock = new object();
            if (File.Exists(log_path))
                File.Delete(log_path);
        }
        #endregion
    }
}
