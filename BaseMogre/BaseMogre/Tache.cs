using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BaseMogre
{
    class Tache : IDisposable
    {
        #region Delegate & Enum
        /// <summary>
        /// Delegate pour les tâches
        /// </summary>
        /// <param name="parametre">Parametre libre</param>
        public delegate void FctTache(object parametre);

        /// <summary>
        /// Enumérateur des priorités
        /// </summary>
        public enum Priority
        {
            Low,
            Medium,
            High
        }
        #endregion

        #region Variables
        /// <summary>
        /// Fonction a executer
        /// </summary>
        private FctTache _fonction;

        /// <summary>
        /// Thread pour l'execution
        /// </summary>
        private Thread _thread;

        /// <summary>
        /// Priorité de l'execution
        /// </summary>
        private Priority _priority;

        /// <summary>
        /// Parametre pour la fonction
        /// </summary>
        private object _parametre;
        #endregion

        #region Constructeur
        public Tache(FctTache fonction, Priority priority, object parametre)
        {
            _fonction = fonction;
            _priority = priority;
            _parametre = parametre;

            _thread = new Thread(new ParameterizedThreadStart(_fonction));
        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// Arret du thread à la suppression
        /// </summary>
        public void Dispose()
        {
            //Stoppe le thread (brutal)
            if (_thread.IsAlive)
                _thread.Abort();
        }

        /// <summary>
        /// Démarre le thread
        /// </summary>
        public void PlayThread()
        {
            _thread.Start(_parametre);
        }

        /// <summary>
        /// Stoppe le thread
        /// </summary>
        public void StopThread()
        {
            _thread.Abort();
        }
        #endregion
    }
}
