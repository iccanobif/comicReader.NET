using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace comicReader.NET
{
    public partial class FrmMain : Form
    {
        enum DisplayMode
        {
            FitWidth = 0,
            FullPicture = 1,
            Zoom = 2
        }

        DisplayMode currentDisplayMode = DisplayMode.Zoom;
        Bitmap originalBitmap;
        Bitmap resizedBitmap;
        double zoom = 1;
        int currentVerticalPosition = 0;
        int currentHorizontalPosition = 0;
        ArchiveReader currentArchiveReader;
        Comic currentComic = null;
        Library currentLibrary = new Library();
        string osdText = string.Empty;

        Point? mouseDragStart;
        Point originalImagePosition;

        public FrmMain()
        {
            InitializeComponent();

            //currentComic = new Comic();
            //currentComic.path = @"f:\mieiProgrammi\comicReader.NET\testImages\arthur";

            //archiveReader = new ArchiveReader(currentComic.path);
        }

        public void ResizeImage()
        {
            DateTime start = DateTime.Now;

            resizedBitmap = new Bitmap((int)(originalBitmap.Width * zoom), (int)(originalBitmap.Height * zoom));

            using (Graphics graphics = Graphics.FromImage(resizedBitmap))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

                graphics.DrawImage(originalBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height);
            }

            Debug.Print("Resizing time: " + DateTime.Now.Subtract(start).ToString());

            SetWindowTitle(string.Format("{0} - {1}{2}", 
                                         currentComic != null ? currentComic.Title : System.IO.Path.GetFileName(currentArchiveReader.CurrentPath), 
                                         currentArchiveReader.GetCurrentFileName(),
                                         currentArchiveReader.CurrentPosition == currentArchiveReader.FileNames.Count - 1 ? " [END]" : string.Empty
                                         ));

            osdText = currentArchiveReader.CurrentPosition == currentArchiveReader.FileNames.Count - 1 ? "END" : string.Empty;

            SetDefaultPosition();
        }

        private void SetWindowTitle(string title)
        {
            Text = "Comic Reader - " + title;
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            //Commands that make sense even when there's no open comic at the moment
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    this.WindowState = FormWindowState.Minimized;
                    return;
                case Keys.L:
                    // Open library window
                    FrmLibrary libraryDialog = new FrmLibrary(currentLibrary);
                    Comic newComic = libraryDialog.GetComic();

                    if (newComic == null) return;
                    
                    currentComic = newComic;
                    currentArchiveReader = currentComic.CreateArchiveReader();
                    originalBitmap = new Bitmap(new System.IO.MemoryStream(currentArchiveReader.GetCurrentFile()));
                    ResizeImage();
                    RepaintAll();
                    return;
                case Keys.N:
                    FrmFileSystemNavigation navigatorDialog;
                    if (currentArchiveReader == null)
                        navigatorDialog = new FrmFileSystemNavigation();
                    else
                        navigatorDialog = new FrmFileSystemNavigation(currentArchiveReader.CurrentPath, currentArchiveReader.CurrentPosition);

                    ArchiveReader newArchiveReader = navigatorDialog.GetNewReader();

                    currentArchiveReader = newArchiveReader == null ? currentArchiveReader : newArchiveReader;

                    if (currentArchiveReader == null) return;

                    originalBitmap = new Bitmap(new System.IO.MemoryStream(currentArchiveReader.GetCurrentFile()));
                    ResizeImage();
                    RepaintAll();
                    return;
            }

            if (currentArchiveReader == null)
                return;

            int maxVerticalOffset = ClientSize.Height - resizedBitmap.Height;
            int maxHorizontalOffset = ClientSize.Width - resizedBitmap.Width;

            //Commands that are only relevant when there's an open comic
            switch (e.KeyCode)
            {
                // ZOOM KEYS
                case Keys.Add:
                    zoom = zoom * 1.1;
                    ResizeImage();
                    break;
                case Keys.Subtract:
                    zoom = zoom * 0.9;
                    ResizeImage();
                    break;
                //ARROW KEYS
                case Keys.Down:
                    if (currentVerticalPosition <= maxVerticalOffset) return;
                    currentVerticalPosition -= e.Control ? 50 : 10;
                    if (currentVerticalPosition <= maxVerticalOffset)
                        currentVerticalPosition = maxVerticalOffset;
                    break;
                case Keys.Up:
                    if (currentVerticalPosition >= 0) return;
                    currentVerticalPosition += e.Control ? 50 : 10;
                    if (currentVerticalPosition >= 0)
                        currentVerticalPosition = 0;
                    break;
                case Keys.Left:
                    if (currentHorizontalPosition > 0) return;
                    currentHorizontalPosition += e.Control ? 50 : 10;
                    if (currentHorizontalPosition > 0) currentHorizontalPosition = 0;
                    break;
                case Keys.Right:
                    if (currentHorizontalPosition < maxHorizontalOffset) return;
                    currentHorizontalPosition -= e.Control ? 50 : 10;
                    if (currentHorizontalPosition < maxHorizontalOffset)
                        currentHorizontalPosition = maxHorizontalOffset;
                    break;
                // NAVIGATION KEYS
                case Keys.Home:
                    if (this.ClientSize.Height > resizedBitmap.Height) return;
                    if (currentVerticalPosition < (maxVerticalOffset) / 2)
                        currentVerticalPosition = (maxVerticalOffset) / 2;
                    else
                        currentVerticalPosition = 0;
                    break;
                case Keys.End:
                    if (this.ClientSize.Height > resizedBitmap.Height) return;
                    if (currentVerticalPosition > (maxVerticalOffset) / 2)
                        currentVerticalPosition = (maxVerticalOffset) / 2;
                    else
                        currentVerticalPosition = maxVerticalOffset;
                    break;
                case Keys.NumPad4:
                    if (ClientSize.Height < resizedBitmap.Height)
                        currentVerticalPosition = maxVerticalOffset;
                    if (ClientSize.Width < resizedBitmap.Width)
                        currentHorizontalPosition = 0;
                    break;
                case Keys.NumPad5:
                    SetDefaultPosition();
                    if (ClientSize.Height < resizedBitmap.Height)
                        currentVerticalPosition = maxVerticalOffset;
                    break;
                case Keys.NumPad6:
                    if (ClientSize.Height < resizedBitmap.Height)
                        currentVerticalPosition = maxVerticalOffset;
                    if (ClientSize.Width < resizedBitmap.Width)
                        currentHorizontalPosition = maxHorizontalOffset;
                    break;
                case Keys.NumPad7:
                    if (ClientSize.Height < resizedBitmap.Height)
                        currentVerticalPosition = 0;
                    if (ClientSize.Width < resizedBitmap.Width)
                        currentHorizontalPosition = 0;
                    break;
                case Keys.NumPad8:
                    SetDefaultPosition();
                    break;
                case Keys.NumPad9:
                    if (ClientSize.Height < resizedBitmap.Height)
                        currentVerticalPosition = 0;
                    if (ClientSize.Width < resizedBitmap.Width)
                        currentHorizontalPosition = maxHorizontalOffset;
                    break;
                // PAGE KEYS
                case Keys.PageDown:
                    originalBitmap = new Bitmap(new System.IO.MemoryStream(currentArchiveReader.GetNextFile()));
                    ResizeImage();
                    break;
                case Keys.PageUp:
                    originalBitmap = new Bitmap(new System.IO.MemoryStream(currentArchiveReader.GetPreviousFile()));
                    ResizeImage();
                    break;
                case Keys.Insert:
                    currentArchiveReader.MoveToNextCollection();
                    originalBitmap = new Bitmap(new System.IO.MemoryStream(currentArchiveReader.GetCurrentFile()));
                    ResizeImage();
                    break;
                // LIBRARY STUFF
                case Keys.S:
                    if (currentComic == null) return;

                    currentLibrary.SaveComic(currentComic);
                    osdText = "SAVED";
                    break;
                case Keys.C:
                    if (currentComic == null) return;

                    currentComic = currentLibrary.GetComic(currentComic.Id);
                    currentArchiveReader = currentComic.CreateArchiveReader();
                    originalBitmap = new Bitmap(new System.IO.MemoryStream(currentArchiveReader.GetCurrentFile()));
                    ResizeImage();
                    break;

                default:
                    return;
            }

            RepaintAll();
        }

        private void UpdateComic()
        {
            
        }

        private void FrmMain_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
                FrmMain_KeyDown(null, new KeyEventArgs(Keys.Down));
            else
                FrmMain_KeyDown(null, new KeyEventArgs(Keys.Up));
        }

        private void RepaintAll()
        {
            Graphics g = Graphics.FromHwnd(this.Handle);
            PaintImage(g);
        }

        private void SetDefaultPosition()
        {
            currentHorizontalPosition = (this.Width - resizedBitmap.Width) / 2;
            currentVerticalPosition = 0;
        }

        private void PaintImage(Graphics g)
        {
            if (resizedBitmap == null) return;

            DateTime start = DateTime.Now;

            //TODO
            if (currentDisplayMode != DisplayMode.Zoom) return;

            g.DrawImage(resizedBitmap, new Point(currentHorizontalPosition, currentVerticalPosition));
            g.FillRectangle(Brushes.Black, 0, 0, currentHorizontalPosition, this.ClientSize.Height);
            g.FillRectangle(Brushes.Black, currentHorizontalPosition + resizedBitmap.Width, 0, this.Width, this.ClientSize.Height);
            g.FillRectangle(Brushes.Black, currentHorizontalPosition, resizedBitmap.Height, resizedBitmap.Width + 1, this.ClientSize.Height);

            if (!string.IsNullOrEmpty(osdText))
            {
                g.DrawString(osdText, new Font(FontFamily.GenericMonospace, 20, FontStyle.Bold), Brushes.Red, 30, 30);
            }

            Debug.Print("Painting time: " + DateTime.Now.Subtract(start).ToString());
        }

        private void FrmMain_Paint(object sender, PaintEventArgs e)
        {
            PaintImage(e.Graphics);
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            SetDefaultPosition();
            RepaintAll();
        }

        private void FrmMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDragStart.HasValue) return;

            int horizontalOffset = mouseDragStart.Value.X - e.X;
            int verticalOffset = mouseDragStart.Value.Y - e.Y;

            if (resizedBitmap.Width > ClientSize.Width)
            {
                currentHorizontalPosition = originalImagePosition.X - horizontalOffset;
                if (currentHorizontalPosition > 0) 
                    currentHorizontalPosition = 0;
                if (currentHorizontalPosition < ClientSize.Width - resizedBitmap.Width)
                    currentHorizontalPosition = ClientSize.Width - resizedBitmap.Width;
            }

            if (resizedBitmap.Height > ClientSize.Height)
            {
                currentVerticalPosition = originalImagePosition.Y - verticalOffset;
                if (currentVerticalPosition > 0)
                    currentVerticalPosition = 0;
                if (currentVerticalPosition < ClientSize.Height - resizedBitmap.Height)
                    currentVerticalPosition = ClientSize.Height - resizedBitmap.Height;
            }

            RepaintAll();
        }

        private void FrmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentComic == null) return;

            mouseDragStart = e.Location;
            originalImagePosition.X = currentHorizontalPosition;
            originalImagePosition.Y = currentVerticalPosition;
        }

        private void FrmMain_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDragStart = null;
        }
    }
}
