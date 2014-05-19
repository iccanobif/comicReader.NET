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
        string currentPath = string.Empty;
        ArchiveReader currentArchiveReader;
        Library library;

        public FrmLibrary(Library l)
        {
            library = l;
            InitializeComponent();
        }

        public ArchiveReader GetNewReader()
        {
            ShowDialog();
            return currentArchiveReader;
        }

        private void FrmLibrary_Load(object sender, EventArgs e)
        {
            LstFileSystem.Items.AddRange(Directory.GetLogicalDrives());

            //TODO: Bisogna mettere in una classe a parte le logiche per il sorting dei file e per la navigazione
            // servono sia qui che in ArchiveReader per il next chapter, a meno che non vada a spostare quella logica
            // dall'Archive alla Library (fondamentalmente, quando cambio capitolo, creo un nuovo ArchiveReader)
            
            LstComics.ValueMember = "Id";
            LstComics.DisplayMember = "Title";
            LstComics.DataSource = library.GetComicList();
        }

        private void LstFileSystem_DoubleClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentPath))
                currentPath = LstFileSystem.SelectedItem.ToString();
            else
                if (LstFileSystem.SelectedItem.ToString() == "..")
                    currentPath = Path.GetDirectoryName(currentPath);
                else
                    currentPath = Path.Combine(currentPath, LstFileSystem.SelectedItem.ToString());

            LstFileSystem.Items.Clear();
            LstImages.Items.Clear();

            if (string.IsNullOrEmpty(currentPath))
            {
                LstFileSystem.Items.AddRange(Directory.GetLogicalDrives());
                return;
            }

            LstFileSystem.Items.Add("..");

            //foreach (string dir in Directory.GetDirectories(currentPath))
            //    LstFileSystem.Items.Add(Path.GetFileName(dir));

            //foreach (string file in Directory.GetFiles(currentPath, "*.zip"))
            //    LstFileSystem.Items.Add(Path.GetFileName(file));

            currentArchiveReader = new ArchiveReader(currentPath);

            foreach (string collectionName in currentArchiveReader.SiblingCollections)
                LstFileSystem.Items.Add(collectionName);

            foreach (string fileName in currentArchiveReader.FileNames)
                LstImages.Items.Add(fileName);
        }

        private void BtnAddComic_Click(object sender, EventArgs e)
        {
            Comic c = new Comic();
            c.Title = TxtComicName.Text;
            c.Path = currentPath;
            library.AddComic(c);

            MessageBox.Show("Saved");
        }

        private void LstComics_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: fare un metodo che aggiorni tutto in coerenza con i nuovi currentPath e currentArchiveReader
            currentPath = library.GetComic((long)(LstComics.SelectedValue)).Path;
            currentArchiveReader = new ArchiveReader(currentPath);
        }

    }
}
