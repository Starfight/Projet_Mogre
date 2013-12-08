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
            this.bQuitter = new System.Windows.Forms.Button();
            this.bNewEnv = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bQuitter
            // 
            this.bQuitter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bQuitter.Location = new System.Drawing.Point(198, 226);
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
            this.bNewEnv.Location = new System.Drawing.Point(198, 197);
            this.bNewEnv.Name = "bNewEnv";
            this.bNewEnv.Size = new System.Drawing.Size(75, 23);
            this.bNewEnv.TabIndex = 1;
            this.bNewEnv.Text = "Nouveau";
            this.bNewEnv.UseVisualStyleBackColor = true;
            this.bNewEnv.Click += new System.EventHandler(this.bNewEnv_Click);
            // 
            // OgreForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.bNewEnv);
            this.Controls.Add(this.bQuitter);
            this.KeyPreview = true;
            this.Name = "OgreForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bQuitter;
        private System.Windows.Forms.Button bNewEnv;

    }
}

