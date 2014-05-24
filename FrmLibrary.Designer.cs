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
            this.BtnAddComic = new System.Windows.Forms.Button();
            this.TxtComicName = new System.Windows.Forms.TextBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LstComics
            // 
            this.LstComics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LstComics.FormattingEnabled = true;
            this.LstComics.Location = new System.Drawing.Point(12, 45);
            this.LstComics.Name = "LstComics";
            this.LstComics.Size = new System.Drawing.Size(614, 446);
            this.LstComics.TabIndex = 0;
            this.LstComics.SelectedIndexChanged += new System.EventHandler(this.LstComics_SelectedIndexChanged);
            // 
            // TxtFilter
            // 
            this.TxtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtFilter.Location = new System.Drawing.Point(13, 13);
            this.TxtFilter.Name = "TxtFilter";
            this.TxtFilter.Size = new System.Drawing.Size(613, 20);
            this.TxtFilter.TabIndex = 1;
            // 
            // BtnAddComic
            // 
            this.BtnAddComic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAddComic.Location = new System.Drawing.Point(567, 513);
            this.BtnAddComic.Name = "BtnAddComic";
            this.BtnAddComic.Size = new System.Drawing.Size(59, 20);
            this.BtnAddComic.TabIndex = 4;
            this.BtnAddComic.Text = "Add";
            this.BtnAddComic.UseVisualStyleBackColor = true;
            this.BtnAddComic.Click += new System.EventHandler(this.BtnAddComic_Click);
            // 
            // TxtComicName
            // 
            this.TxtComicName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtComicName.Location = new System.Drawing.Point(12, 514);
            this.TxtComicName.Name = "TxtComicName";
            this.TxtComicName.Size = new System.Drawing.Size(549, 20);
            this.TxtComicName.TabIndex = 5;
            // 
            // BtnOk
            // 
            this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOk.Location = new System.Drawing.Point(13, 540);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(613, 30);
            this.BtnOk.TabIndex = 6;
            this.BtnOk.Text = "Ok";
            this.BtnOk.UseVisualStyleBackColor = true;
            // 
            // FrmLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 582);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.TxtComicName);
            this.Controls.Add(this.BtnAddComic);
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
        private System.Windows.Forms.Button BtnAddComic;
        private System.Windows.Forms.TextBox TxtComicName;
        private System.Windows.Forms.Button BtnOk;

    }
}