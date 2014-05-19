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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LstImages = new System.Windows.Forms.ListBox();
            this.BtnAddComic = new System.Windows.Forms.Button();
            this.TxtComicName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LstComics
            // 
            this.LstComics.FormattingEnabled = true;
            this.LstComics.Location = new System.Drawing.Point(12, 45);
            this.LstComics.Name = "LstComics";
            this.LstComics.Size = new System.Drawing.Size(258, 212);
            this.LstComics.TabIndex = 0;
            this.LstComics.SelectedIndexChanged += new System.EventHandler(this.LstComics_SelectedIndexChanged);
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
            this.LstFileSystem.Location = new System.Drawing.Point(15, 19);
            this.LstFileSystem.Name = "LstFileSystem";
            this.LstFileSystem.Size = new System.Drawing.Size(282, 160);
            this.LstFileSystem.TabIndex = 2;
            this.LstFileSystem.DoubleClick += new System.EventHandler(this.LstFileSystem_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LstImages);
            this.groupBox1.Controls.Add(this.LstFileSystem);
            this.groupBox1.Location = new System.Drawing.Point(291, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(388, 370);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // LstImages
            // 
            this.LstImages.FormattingEnabled = true;
            this.LstImages.Location = new System.Drawing.Point(15, 185);
            this.LstImages.Name = "LstImages";
            this.LstImages.Size = new System.Drawing.Size(282, 160);
            this.LstImages.TabIndex = 3;
            // 
            // BtnAddComic
            // 
            this.BtnAddComic.Location = new System.Drawing.Point(174, 332);
            this.BtnAddComic.Name = "BtnAddComic";
            this.BtnAddComic.Size = new System.Drawing.Size(60, 47);
            this.BtnAddComic.TabIndex = 4;
            this.BtnAddComic.Text = "Add";
            this.BtnAddComic.UseVisualStyleBackColor = true;
            this.BtnAddComic.Click += new System.EventHandler(this.BtnAddComic_Click);
            // 
            // TxtComicName
            // 
            this.TxtComicName.Location = new System.Drawing.Point(18, 332);
            this.TxtComicName.Name = "TxtComicName";
            this.TxtComicName.Size = new System.Drawing.Size(150, 20);
            this.TxtComicName.TabIndex = 5;
            // 
            // FrmLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 531);
            this.Controls.Add(this.TxtComicName);
            this.Controls.Add(this.BtnAddComic);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.TxtFilter);
            this.Controls.Add(this.LstComics);
            this.Name = "FrmLibrary";
            this.Text = "Comic Library";
            this.Load += new System.EventHandler(this.FrmLibrary_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LstComics;
        private System.Windows.Forms.TextBox TxtFilter;
        private System.Windows.Forms.ListBox LstFileSystem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox LstImages;
        private System.Windows.Forms.Button BtnAddComic;
        private System.Windows.Forms.TextBox TxtComicName;

    }
}