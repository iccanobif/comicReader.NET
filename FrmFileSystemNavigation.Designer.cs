namespace comicReader.NET
{
    partial class FrmFileSystemNavigation
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
            this.LstFileSystem = new System.Windows.Forms.ListBox();
            this.LstImages = new System.Windows.Forms.ListBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LstFileSystem
            // 
            this.LstFileSystem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LstFileSystem.FormattingEnabled = true;
            this.LstFileSystem.Location = new System.Drawing.Point(12, 12);
            this.LstFileSystem.Name = "LstFileSystem";
            this.LstFileSystem.Size = new System.Drawing.Size(989, 251);
            this.LstFileSystem.TabIndex = 2;
            this.LstFileSystem.DoubleClick += new System.EventHandler(this.LstFileSystem_DoubleClick);
            // 
            // LstImages
            // 
            this.LstImages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LstImages.FormattingEnabled = true;
            this.LstImages.Location = new System.Drawing.Point(12, 279);
            this.LstImages.Name = "LstImages";
            this.LstImages.Size = new System.Drawing.Size(989, 251);
            this.LstImages.TabIndex = 3;
            // 
            // BtnOk
            // 
            this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOk.Location = new System.Drawing.Point(12, 536);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(989, 40);
            this.BtnOk.TabIndex = 4;
            this.BtnOk.Text = "Ok";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // FrmFileSystemNavigation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 588);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.LstImages);
            this.Controls.Add(this.LstFileSystem);
            this.Name = "FrmFileSystemNavigation";
            this.Text = "Navigate";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox LstFileSystem;
        private System.Windows.Forms.ListBox LstImages;
        private System.Windows.Forms.Button BtnOk;
    }
}