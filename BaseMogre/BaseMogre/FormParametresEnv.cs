﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mogre;

namespace BaseMogre
{
    public partial class FormParametresEnv : Form
    {
        private SceneManager _scm;
        private Camera _cam;

        public FormParametresEnv(ref SceneManager scm, ref Camera cam)
        {
            InitializeComponent();
            _scm = scm;
            _cam = cam;
        }

        private void bValidate_Click(object sender, EventArgs e)
        {
            if (Environnement.getInstance() != null)
                Environnement.getInstance().Dispose();

            int nbOgres = Convert.ToInt32(nupNbogres.Value);
            int nbRobots = Convert.ToInt32(nupNbrobots.Value);
            int nbCubes = Convert.ToInt32(nupNbcubes.Value);

            //Création de l'environnement
            Environnement.createEnvironnement(ref _scm, nbOgres, nbRobots, nbCubes, ref this._cam);

            this.Close();
        }
    }
}
