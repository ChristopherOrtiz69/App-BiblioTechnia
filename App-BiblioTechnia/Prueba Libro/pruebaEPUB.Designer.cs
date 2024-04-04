namespace Prueba_Libro
{
    partial class pruebaEPUB
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
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblAuthors = new System.Windows.Forms.Label();
            this.picCover = new System.Windows.Forms.Label();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1158, 831);
            this.webBrowser1.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(105, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(35, 13);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "label1";
            // 
            // lblAuthors
            // 
            this.lblAuthors.AutoSize = true;
            this.lblAuthors.Location = new System.Drawing.Point(528, 9);
            this.lblAuthors.Name = "lblAuthors";
            this.lblAuthors.Size = new System.Drawing.Size(35, 13);
            this.lblAuthors.TabIndex = 2;
            this.lblAuthors.Text = "label2";
            // 
            // picCover
            // 
            this.picCover.AutoSize = true;
            this.picCover.Location = new System.Drawing.Point(893, 9);
            this.picCover.Name = "picCover";
            this.picCover.Size = new System.Drawing.Size(35, 13);
            this.picCover.TabIndex = 3;
            this.picCover.Text = "label1";
            // 
            // webBrowser
            // 
            this.webBrowser.Location = new System.Drawing.Point(283, 58);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(628, 571);
            this.webBrowser.TabIndex = 4;
            // 
            // pruebaEPUB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1158, 831);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.picCover);
            this.Controls.Add(this.lblAuthors);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.webBrowser1);
            this.Name = "pruebaEPUB";
            this.Text = "pruebaEPUB";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblAuthors;
        private System.Windows.Forms.Label picCover;
        private System.Windows.Forms.WebBrowser webBrowser;
    }
}