namespace BaseMogre
{
    partial class FormParametresEnv
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.bValidate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nupNbogres = new System.Windows.Forms.NumericUpDown();
            this.nupNbrobots = new System.Windows.Forms.NumericUpDown();
            this.nupNbcubes = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nupNbogres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupNbrobots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupNbcubes)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 261);
            this.label1.TabIndex = 0;
            this.label1.Text = "Paramètres de l\'environnement";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // bValidate
            // 
            this.bValidate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bValidate.Location = new System.Drawing.Point(109, 226);
            this.bValidate.Name = "bValidate";
            this.bValidate.Size = new System.Drawing.Size(75, 23);
            this.bValidate.TabIndex = 1;
            this.bValidate.Text = "Valider";
            this.bValidate.UseVisualStyleBackColor = true;
            this.bValidate.Click += new System.EventHandler(this.bValidate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Nombre d\'ogres :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Nombre de robots :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Nombre de cubes :";
            // 
            // nupNbogres
            // 
            this.nupNbogres.Location = new System.Drawing.Point(153, 56);
            this.nupNbogres.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nupNbogres.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupNbogres.Name = "nupNbogres";
            this.nupNbogres.Size = new System.Drawing.Size(91, 20);
            this.nupNbogres.TabIndex = 5;
            this.nupNbogres.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // nupNbrobots
            // 
            this.nupNbrobots.Location = new System.Drawing.Point(153, 108);
            this.nupNbrobots.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nupNbrobots.Name = "nupNbrobots";
            this.nupNbrobots.Size = new System.Drawing.Size(91, 20);
            this.nupNbrobots.TabIndex = 6;
            this.nupNbrobots.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // nupNbcubes
            // 
            this.nupNbcubes.Location = new System.Drawing.Point(153, 159);
            this.nupNbcubes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupNbcubes.Name = "nupNbcubes";
            this.nupNbcubes.Size = new System.Drawing.Size(91, 20);
            this.nupNbcubes.TabIndex = 7;
            this.nupNbcubes.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // FormParametresEnv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.nupNbcubes);
            this.Controls.Add(this.nupNbrobots);
            this.Controls.Add(this.nupNbogres);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bValidate);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormParametresEnv";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nouvel Environnement";
            ((System.ComponentModel.ISupportInitialize)(this.nupNbogres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupNbrobots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupNbcubes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bValidate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nupNbogres;
        private System.Windows.Forms.NumericUpDown nupNbrobots;
        private System.Windows.Forms.NumericUpDown nupNbcubes;
    }
}