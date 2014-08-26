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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtPath = new System.Windows.Forms.TextBox();
            this.BtnBrowse = new System.Windows.Forms.Button();
            this.BtnDelete = new System.Windows.Forms.Button();
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
            this.LstComics.Size = new System.Drawing.Size(673, 407);
            this.LstComics.TabIndex = 1;
            this.LstComics.DoubleClick += new System.EventHandler(this.LstComics_DoubleClick);
            this.LstComics.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LstComics_KeyUp);
            // 
            // TxtFilter
            // 
            this.TxtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtFilter.Location = new System.Drawing.Point(13, 13);
            this.TxtFilter.Name = "TxtFilter";
            this.TxtFilter.Size = new System.Drawing.Size(672, 20);
            this.TxtFilter.TabIndex = 0;
            this.TxtFilter.TextChanged += new System.EventHandler(this.TxtFilter_TextChanged);
            this.TxtFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtFilter_KeyDown);
            // 
            // BtnAddComic
            // 
            this.BtnAddComic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAddComic.Location = new System.Drawing.Point(557, 492);
            this.BtnAddComic.Name = "BtnAddComic";
            this.BtnAddComic.Size = new System.Drawing.Size(59, 21);
            this.BtnAddComic.TabIndex = 5;
            this.BtnAddComic.Text = "Add";
            this.BtnAddComic.UseVisualStyleBackColor = true;
            this.BtnAddComic.Click += new System.EventHandler(this.BtnAddComic_Click);
            // 
            // TxtComicName
            // 
            this.TxtComicName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtComicName.Location = new System.Drawing.Point(88, 492);
            this.TxtComicName.Name = "TxtComicName";
            this.TxtComicName.Size = new System.Drawing.Size(462, 20);
            this.TxtComicName.TabIndex = 4;
            // 
            // BtnOk
            // 
            this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOk.Location = new System.Drawing.Point(13, 518);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(672, 30);
            this.BtnOk.TabIndex = 6;
            this.BtnOk.Text = "Ok";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 495);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Comic Name:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 465);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Path:";
            // 
            // TxtPath
            // 
            this.TxtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtPath.Location = new System.Drawing.Point(88, 462);
            this.TxtPath.Name = "TxtPath";
            this.TxtPath.Size = new System.Drawing.Size(462, 20);
            this.TxtPath.TabIndex = 2;
            // 
            // BtnBrowse
            // 
            this.BtnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnBrowse.Location = new System.Drawing.Point(556, 462);
            this.BtnBrowse.Name = "BtnBrowse";
            this.BtnBrowse.Size = new System.Drawing.Size(60, 24);
            this.BtnBrowse.TabIndex = 3;
            this.BtnBrowse.Text = "Browse";
            this.BtnBrowse.UseVisualStyleBackColor = true;
            // 
            // BtnDelete
            // 
            this.BtnDelete.Location = new System.Drawing.Point(623, 463);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(61, 48);
            this.BtnDelete.TabIndex = 9;
            this.BtnDelete.Text = "Delete";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // FrmLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 560);
            this.Controls.Add(this.BtnDelete);
            this.Controls.Add(this.BtnBrowse);
            this.Controls.Add(this.TxtPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.TxtComicName);
            this.Controls.Add(this.BtnAddComic);
            this.Controls.Add(this.TxtFilter);
            this.Controls.Add(this.LstComics);
            this.Name = "FrmLibrary";
            this.Text = "Comic Library";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LstComics;
        private System.Windows.Forms.TextBox TxtFilter;
        private System.Windows.Forms.Button BtnAddComic;
        private System.Windows.Forms.TextBox TxtComicName;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtPath;
        private System.Windows.Forms.Button BtnBrowse;
        private System.Windows.Forms.Button BtnDelete;

    }
}