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
    public partial class FrmFileSystemNavigation : Form
    {
        string currentPath = string.Empty;
        ArchiveReader currentArchiveReader;

        public FrmFileSystemNavigation()
        {
            InitializeComponent();
        }

        private void FrmFileSystemNavigation_Load(object sender, EventArgs e)
        {
            //TODO: Bisogna mettere in una classe a parte le logiche per il sorting dei file e per la navigazione
            // servono sia qui che in ArchiveReader per il next chapter, a meno che non vada a spostare quella logica
            // dall'Archive alla Library (fondamentalmente, quando cambio capitolo, creo un nuovo ArchiveReader)

            LstFileSystem.Items.AddRange(Directory.GetLogicalDrives());
        }

        public ArchiveReader GetNewReader()
        {
            ShowDialog();

            if (currentArchiveReader.FileNames.Count == 0)
                return null;

            return currentArchiveReader;
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

            currentArchiveReader = new ArchiveReader(currentPath);

            foreach (string collectionName in currentArchiveReader.SiblingCollections)
                LstFileSystem.Items.Add(collectionName);

            foreach (string fileName in currentArchiveReader.FileNames)
                LstImages.Items.Add(fileName);
        }


    }
}
