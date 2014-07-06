using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace comicReader.NET
{
    public partial class FrmLibrary : Form
    {

        Library library;
        Comic outputComic = null;

        public FrmLibrary(Library l)
        {
            library = l;
            InitializeComponent();
        }

        public Comic GetComic()
        {
            this.ShowDialog();
            return outputComic;
        }

        private void FrmLibrary_Load(object sender, EventArgs e)
        {
            LstComics.ValueMember = "Id";
            LstComics.DisplayMember = "Title";
            LstComics.DataSource = library.GetComicList();
        }

        private void PopulateComicList()
        {
            LstComics.DataSource = library.GetComicList();
        }

        private void BtnAddComic_Click(object sender, EventArgs e)
        {
            if (TxtComicName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Insert a comic name.");
                return;
            }

            if (TxtPath.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Insert a path.");
                return;
            }

            //FrmFileSystemNavigation navigatorDialog = new FrmFileSystemNavigation();
            //ArchiveReader reader = navigatorDialog.GetNewReader();

            //if (reader == null) return;

            Comic c = new Comic();
            c.Title = TxtComicName.Text;
            c.Path = TxtPath.Text;
            c.Position = 0;
            string newComicId = library.SaveComic(c);

            PopulateComicList();

            LstComics.SelectedValue = c.Id;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            outputComic = library.GetComic(LstComics.SelectedValue.ToString());
            Close();
        }
    }
}
