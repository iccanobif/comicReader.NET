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
        int startPosition = -1;
        ArchiveReader currentArchiveReader;
        ArchiveReader outputArchiveReader = null;
        List<string> directoryList;

        public FrmFileSystemNavigation(string path, int position)
        {
            InitializeComponent();

            currentPath = path;
            startPosition = position;

            UpdateLists();

            LstImages.SelectedIndex = startPosition;
        }

        public FrmFileSystemNavigation()
            : this(string.Empty, -1)
        {
        }

        public ArchiveReader GetNewReader()
        {
            base.ShowDialog();

            if (outputArchiveReader == null || outputArchiveReader.FileNames.Count == 0)
                return null;

            return outputArchiveReader;
        }

        private void ApplyFilterToDirectoryList()
        {
            LstFileSystem.DataSource = (from d in directoryList
                                        where d.ToUpper().Contains(TxtDirectoryFilter.Text.ToUpper())
                                        select d).ToList<string>();

            if (LstFileSystem.Items.Count > 0)
                LstFileSystem.SelectedIndex = 0;
        }

        private void UpdateLists()
        {
            //TODO: Bisogna mettere in una classe a parte le logiche per il sorting dei file e per la navigazione
            // servono sia qui che in ArchiveReader per il next chapter, a meno che non vada a spostare quella logica
            // dall'Archive alla Library (fondamentalmente, quando cambio capitolo, creo un nuovo ArchiveReader)
            LstFileSystem.DataSource = null;
            LstImages.Items.Clear();
            directoryList = new List<string>();

            if (string.IsNullOrEmpty(currentPath))
            {
                directoryList = Directory.GetLogicalDrives().ToList<string>();
                LstFileSystem.DataSource = directoryList;
                return;
            }

            directoryList.Add("..");

            currentArchiveReader = new ArchiveReader(currentPath, null);

            foreach (string collectionName in currentArchiveReader.SiblingCollections)
                directoryList.Add(Path.GetFileName(collectionName));

            LstFileSystem.DataSource = directoryList;

            foreach (string fileName in currentArchiveReader.FileNames)
                LstImages.Items.Add(fileName);

            if (LstImages.Items.Count > 0)
                LstImages.SelectedIndex = 0;
        }

        private void EnterDirectory()
        {
            if (LstFileSystem.SelectedIndex < 0) return;

            if (string.IsNullOrEmpty(currentPath))
                currentPath = LstFileSystem.SelectedItem.ToString();
            else
                if (LstFileSystem.SelectedItem.ToString() == "..")
                    currentPath = Path.GetDirectoryName(currentPath);
                else
                    currentPath = Path.Combine(currentPath, LstFileSystem.SelectedItem.ToString());

            UpdateLists();
        }

        private void ConfirmSelection()
        {
            currentArchiveReader.CurrentPosition = LstImages.SelectedIndex == -1 ? 0 : LstImages.SelectedIndex;
            outputArchiveReader = currentArchiveReader;
            Close();
        }

        private void LstFileSystem_DoubleClick(object sender, EventArgs e)
        {
            EnterDirectory();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            ConfirmSelection();
        }

        private void LstFileSystem_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Back:
                    currentPath = Path.GetDirectoryName(currentPath);
                    UpdateLists();
                    return;

                case (char)Keys.Enter:
                    EnterDirectory();
                    return;
            }

            if (Char.IsControl(e.KeyChar))
                return;

            TxtDirectoryFilter.Text = e.KeyChar.ToString();
            TxtDirectoryFilter.Visible = true;
            TxtDirectoryFilter.SelectionStart = 1;
            TxtDirectoryFilter.Focus();
            ApplyFilterToDirectoryList();
        }

        private void FrmFileSystemNavigation_Resize(object sender, EventArgs e)
        {
            TxtDirectoryFilter.Top = LstFileSystem.Top + LstFileSystem.Height - TxtDirectoryFilter.Height;
        }

        private void TxtDirectoryFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Escape:
                    TxtDirectoryFilter.Text = string.Empty;
                    TxtDirectoryFilter.Visible = false;
                    return;

                case (char)Keys.Return:
                    if (LstFileSystem.Items.Count == 0)
                        return;
                    EnterDirectory();
                    TxtDirectoryFilter.Text = string.Empty;
                    TxtDirectoryFilter.Visible = false;
                    return;
            }
        }

        private void TxtDirectoryFilter_TextChanged(object sender, EventArgs e)
        {
            ApplyFilterToDirectoryList();
        }

        private void LstImages_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                ConfirmSelection();
        }


    }
}
