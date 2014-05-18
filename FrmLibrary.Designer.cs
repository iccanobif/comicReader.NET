namespace comicReader.NET
{
    partial class FrmLibrary
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
            this.LstComics = new System.Windows.Forms.ListBox();
            this.TxtFilter = new System.Windows.Forms.TextBox();
            this.LstFileSystem = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // LstComics
            // 
            this.LstComics.FormattingEnabled = true;
            this.LstComics.Location = new System.Drawing.Point(12, 45);
            this.LstComics.Name = "LstComics";
            this.LstComics.Size = new System.Drawing.Size(258, 212);
            this.LstComics.TabIndex = 0;
            // 
            // TxtFilter
            // 
            this.TxtFilter.Location = new System.Drawing.Point(13, 13);
            this.TxtFilter.Name = "TxtFilter";
            this.TxtFilter.Size = new System.Drawing.Size(257, 20);
            this.TxtFilter.TabIndex = 1;
            // 
            // LstFileSystem
            // 
            this.LstFileSystem.FormattingEnabled = true;
            this.LstFileSystem.Location = new System.Drawing.Point(276, 45);
            this.LstFileSystem.Name = "LstFileSystem";
            this.LstFileSystem.Size = new System.Drawing.Size(282, 212);
            this.LstFileSystem.TabIndex = 2;
            // 
            // FrmLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 531);
            this.Controls.Add(this.LstFileSystem);
            this.Controls.Add(this.TxtFilter);
            this.Controls.Add(this.LstComics);
            this.Name = "FrmLibrary";
            this.Text = "Comic Library";
            this.Load += new System.EventHandler(this.FrmLibrary_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LstComics;
        private System.Windows.Forms.TextBox TxtFilter;
        private System.Windows.Forms.ListBox LstFileSystem;

    }
}