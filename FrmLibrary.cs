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
        List<Comic> comicList;

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
            PopulateComicList();

            TxtFilter.Focus();
        }

        private void PopulateComicList()
        {
            LstComics.DataSource = comicList = library.GetComicList();
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

        private void ConfirmSelection()
        {
            outputComic = library.GetComic(LstComics.SelectedValue.ToString());
            Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            ConfirmSelection();
        }

        private void LstComics_DoubleClick(object sender, EventArgs e)
        {
            ConfirmSelection();
        }

        private void TxtFilter_TextChanged(object sender, EventArgs e)
        {
            LstComics.DataSource = (from c in comicList
                                    where c.Title.ToUpper().Contains(TxtFilter.Text.ToUpper())
                                    select c).ToList<Comic>();

            if (LstComics.Items.Count > 0)
                LstComics.SelectedIndex = 0;
        }

        private void TxtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    LstComics.SelectedIndex = 1;
                    LstComics.Focus();
                    break;

                case Keys.Enter:
                    ConfirmSelection();
                    break;
            }
        }

        private void LstComics_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    ConfirmSelection();
                    break;
            }
        }

        private void LstComics_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
