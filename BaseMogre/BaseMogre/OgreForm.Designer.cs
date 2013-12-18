namespace BaseMogre
{
    partial class OgreForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OgreForm));
            this.bQuitter = new System.Windows.Forms.Button();
            this.bNewEnv = new System.Windows.Forms.Button();
            this.bRalentir = new System.Windows.Forms.Button();
            this.bPausePlay = new System.Windows.Forms.Button();
            this.bAccelerer = new System.Windows.Forms.Button();
            this.labelVitesse = new System.Windows.Forms.Label();
            this.lblFinish = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bQuitter
            // 
            this.bQuitter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bQuitter.Location = new System.Drawing.Point(278, 263);
            this.bQuitter.Name = "bQuitter";
            this.bQuitter.Size = new System.Drawing.Size(75, 23);
            this.bQuitter.TabIndex = 0;
            this.bQuitter.Text = "Quitter";
            this.bQuitter.UseVisualStyleBackColor = true;
            this.bQuitter.Click += new System.EventHandler(this.bQuitter_Click);
            // 
            // bNewEnv
            // 
            this.bNewEnv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bNewEnv.Location = new System.Drawing.Point(278, 234);
            this.bNewEnv.Name = "bNewEnv";
            this.bNewEnv.Size = new System.Drawing.Size(75, 23);
            this.bNewEnv.TabIndex = 1;
            this.bNewEnv.Text = "Nouveau";
            this.bNewEnv.UseVisualStyleBackColor = true;
            this.bNewEnv.Click += new System.EventHandler(this.bNewEnv_Click);
            // 
            // bRalentir
            // 
            this.bRalentir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bRalentir.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bRalentir.BackgroundImage")));
            this.bRalentir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bRalentir.Location = new System.Drawing.Point(12, 241);
            this.bRalentir.Name = "bRalentir";
            this.bRalentir.Size = new System.Drawing.Size(48, 48);
            this.bRalentir.TabIndex = 2;
            this.bRalentir.UseVisualStyleBackColor = true;
            this.bRalentir.Click += new System.EventHandler(this.bRalentir_Click);
            // 
            // bPausePlay
            // 
            this.bPausePlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bPausePlay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bPausePlay.BackgroundImage")));
            this.bPausePlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bPausePlay.Location = new System.Drawing.Point(66, 241);
            this.bPausePlay.Name = "bPausePlay";
            this.bPausePlay.Size = new System.Drawing.Size(48, 48);
            this.bPausePlay.TabIndex = 3;
            this.bPausePlay.UseVisualStyleBackColor = true;
            this.bPausePlay.Click += new System.EventHandler(this.bPausePlay_Click);
            // 
            // bAccelerer
            // 
            this.bAccelerer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAccelerer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bAccelerer.BackgroundImage")));
            this.bAccelerer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bAccelerer.Location = new System.Drawing.Point(120, 241);
            this.bAccelerer.Name = "bAccelerer";
            this.bAccelerer.Size = new System.Drawing.Size(48, 48);
            this.bAccelerer.TabIndex = 4;
            this.bAccelerer.UseVisualStyleBackColor = true;
            this.bAccelerer.Click += new System.EventHandler(this.bAccelerer_Click);
            // 
            // labelVitesse
            // 
            this.labelVitesse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelVitesse.AutoSize = true;
            this.labelVitesse.Location = new System.Drawing.Point(13, 222);
            this.labelVitesse.Name = "labelVitesse";
            this.labelVitesse.Size = new System.Drawing.Size(61, 13);
            this.labelVitesse.TabIndex = 5;
            this.labelVitesse.Text = "Vitesse : 1x";
            // 
            // lblFinish
            // 
            this.lblFinish.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblFinish.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFinish.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinish.Location = new System.Drawing.Point(0, 0);
            this.lblFinish.Margin = new System.Windows.Forms.Padding(3);
            this.lblFinish.Name = "lblFinish";
            this.lblFinish.Size = new System.Drawing.Size(364, 51);
            this.lblFinish.TabIndex = 6;
            this.lblFinish.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblFinish.Visible = false;
            // 
            // OgreForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 298);
            this.Controls.Add(this.lblFinish);
            this.Controls.Add(this.labelVitesse);
            this.Controls.Add(this.bAccelerer);
            this.Controls.Add(this.bPausePlay);
            this.Controls.Add(this.bRalentir);
            this.Controls.Add(this.bNewEnv);
            this.Controls.Add(this.bQuitter);
            this.KeyPreview = true;
            this.Name = "OgreForm";
            this.Text = "Ogres VS Robots";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bQuitter;
        private System.Windows.Forms.Button bNewEnv;
        private System.Windows.Forms.Button bRalentir;
        private System.Windows.Forms.Button bPausePlay;
        private System.Windows.Forms.Button bAccelerer;
        private System.Windows.Forms.Label labelVitesse;
        private System.Windows.Forms.Label lblFinish;

    }
}

