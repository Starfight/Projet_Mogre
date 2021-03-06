﻿using System;
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

    /// <summary>
    /// types de résultats
    /// </summary>
    public enum Result
    {
        FAIL,
        OK
    }

    /// <summary>
    /// Classes utilisées
    /// </summary>
    public enum Classe
    {
        Cube,
        Robot,
        Ogre,
        Maison,
        Tour
    }

    /// <summary>
    /// structure d'une requette
    /// </summary>
    public struct KnowledgeQuery
    {
        #region Variables
        /// <summary>
        /// Emetteur de la requete
        /// </summary>
        private String _nomPerso;

        /// <summary>
        /// Données de la requète
        /// </summary>
        private Classe _classe;
        private String _nom;
        private Vector3 _position;
        private String _parametre;
        #endregion 

        #region Constructeur
        public KnowledgeQuery(String nomPerso, Classe classe, String nom, Vector3 position, String parametre = null)
        {
            _nomPerso = nomPerso;
            _classe = classe;
            _nom = nom;
            _position = position;
            _parametre = parametre;
        }
        #endregion

        #region Getters et Setters
        public String NomPerso
        {
            get { return _nomPerso; }
            set { _nomPerso = value; }
        }
        public Classe Classe
        {
            get { return _classe; }
            set { _classe = value; }
        }
        public String Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public String Parametre
        {
            get { return _parametre; }
            set { _parametre = value; }
        }
        #endregion
    }
}
