using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BaseMogre
{
    public static class Log
    {
        static string log_path = "resultats.log";
        public static bool writeNewLine(string texte)
        {
            try
            {
                   File.AppendAllText(log_path, DateTime.Now.ToString() + " => " + texte + Environment.NewLine);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public static bool writeNewLine(string[] textes)
        {
            string chaine = DateTime.Now.ToString() + " => " + Environment.NewLine;
            try
            {
                foreach (string S in textes)
                {
                    chaine += S + Environment.NewLine;
                }
                File.AppendAllText(log_path, chaine);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
