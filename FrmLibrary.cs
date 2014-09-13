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

        public FrmLibrary(Library l, string defaultPath, string defaultComicId)
        {
            InitializeComponent();
            library = l;

            LstComics.ValueMember = "Id";
            LstComics.DisplayMember = "Title";

            PopulateComicList();

            TxtFilter.Focus();

            if (defaultPath != null)
                TxtPath.Text = defaultPath;

            if (defaultComicId != null)
                LstComics.SelectedValue = defaultComicId;
        }

        public Comic GetComic()
        {
            this.ShowDialog();
            return outputComic;
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
            if (LstComics.SelectedIndex < 0) return;

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

        private void ApplyFilter()
        {
            LstComics.DataSource = (from c in comicList
                                    where c.Title.ToUpper().Contains(TxtFilter.Text.ToUpper())
                                    select c).ToList<Comic>();

            if (LstComics.Items.Count > 0)
                LstComics.SelectedIndex = 0;
        }

        private void TxtFilter_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void TxtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (LstComics.SelectedIndex == LstComics.Items.Count - 1)
                        LstComics.SelectedIndex = 0;
                    else
                        LstComics.SelectedIndex += 1;
                    LstComics.Focus();
                    break;
                case Keys.Up:
                    if (LstComics.SelectedIndex == 0)
                        LstComics.SelectedIndex = LstComics.Items.Count - 1;
                    else
                        LstComics.SelectedIndex -= 1;
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
        
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int previousSelectedIndex = LstComics.SelectedIndex;
            library.DeleteComic(LstComics.SelectedValue.ToString());
            PopulateComicList();
            ApplyFilter();

            if (LstComics.Items.Count > 0)
                LstComics.SelectedIndex = LstComics.Items.Count - 1 > previousSelectedIndex ? previousSelectedIndex : LstComics.Items.Count - 1;
        }

    }
}
