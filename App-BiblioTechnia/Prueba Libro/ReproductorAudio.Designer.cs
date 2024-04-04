namespace Prueba_Libro
{
    partial class ReproductorAudio
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReproductorAudio));
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.labelTiempo = new System.Windows.Forms.Label();
            this.statusLabel1 = new System.Windows.Forms.Label();
            this.treintaSec = new System.Windows.Forms.Label();
            this.cincoSegundosMenos = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.reiniciar = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar
            // 
            this.trackBar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.trackBar.Location = new System.Drawing.Point(33, 196);
            this.trackBar.Margin = new System.Windows.Forms.Padding(5);
            this.trackBar.Maximum = 0;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(653, 45);
            this.trackBar.TabIndex = 0;
            // 
            // labelTiempo
            // 
            this.labelTiempo.AutoSize = true;
            this.labelTiempo.Location = new System.Drawing.Point(42, 228);
            this.labelTiempo.Name = "labelTiempo";
            this.labelTiempo.Size = new System.Drawing.Size(13, 13);
            this.labelTiempo.TabIndex = 1;
            this.labelTiempo.Text = "--";
            // 
            // statusLabel1
            // 
            this.statusLabel1.AutoSize = true;
            this.statusLabel1.Location = new System.Drawing.Point(302, 321);
            this.statusLabel1.Name = "statusLabel1";
            this.statusLabel1.Size = new System.Drawing.Size(80, 13);
            this.statusLabel1.TabIndex = 3;
            this.statusLabel1.Text = "Reproduciendo";
            // 
            // treintaSec
            // 
            this.treintaSec.AutoSize = true;
            this.treintaSec.Location = new System.Drawing.Point(402, 321);
            this.treintaSec.Name = "treintaSec";
            this.treintaSec.Size = new System.Drawing.Size(45, 13);
            this.treintaSec.TabIndex = 4;
            this.treintaSec.Text = "+30 sec";
            this.treintaSec.Click += new System.EventHandler(this.label1_Click);
            // 
            // cincoSegundosMenos
            // 
            this.cincoSegundosMenos.AutoSize = true;
            this.cincoSegundosMenos.Location = new System.Drawing.Point(226, 321);
            this.cincoSegundosMenos.Name = "cincoSegundosMenos";
            this.cincoSegundosMenos.Size = new System.Drawing.Size(42, 13);
            this.cincoSegundosMenos.TabIndex = 5;
            this.cincoSegundosMenos.Text = "-10 sec";
            this.cincoSegundosMenos.Click += new System.EventHandler(this.cincoSegundosMenos_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(339, 321);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 13);
            this.statusLabel.TabIndex = 6;
            // 
            // reiniciar
            // 
            this.reiniciar.AutoSize = true;
            this.reiniciar.Location = new System.Drawing.Point(530, 321);
            this.reiniciar.Name = "reiniciar";
            this.reiniciar.Size = new System.Drawing.Size(48, 13);
            this.reiniciar.TabIndex = 7;
            this.reiniciar.Text = "Reiniciar";
            // 
            // ReproductorAudio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(722, 418);
            this.Controls.Add(this.reiniciar);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.cincoSegundosMenos);
            this.Controls.Add(this.treintaSec);
            this.Controls.Add(this.statusLabel1);
            this.Controls.Add(this.labelTiempo);
            this.Controls.Add(this.trackBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReproductorAudio";
            this.Text = "ReproductorAudio";
            this.Load += new System.EventHandler(this.ReproductorAudio_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label labelTiempo;
        private System.Windows.Forms.Label statusLabel1;
        private System.Windows.Forms.Label treintaSec;
        private System.Windows.Forms.Label cincoSegundosMenos;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label reiniciar;
    }
}