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

        public FrmLibrary(Library l)
        {
            library = l;
            InitializeComponent();
        }

        private void FrmLibrary_Load(object sender, EventArgs e)
        {

            LstComics.ValueMember = "Id";
            LstComics.DisplayMember = "Title";
            LstComics.DataSource = library.GetComicList();
        }

        private void BtnAddComic_Click(object sender, EventArgs e)
        {
            Comic c = new Comic();
            c.Title = TxtComicName.Text;
            c.Path = "";   //TODO
            library.AddComic(c);

            MessageBox.Show("Saved");
        }
    }
}
