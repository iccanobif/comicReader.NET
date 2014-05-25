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

            SetWindowTitle(string.Format("{0} - {1}", System.IO.Path.GetFileName(currentArchiveReader.CurrentPath), currentArchiveReader.GetCurrentFileName()));

            SetDefaultPosition();
        }

        private void FrmMain_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void SetWindowTitle(string title)
        {
            Text = "Comic Reader - " + title;
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            int maxVerticalOffset = 0;
            int maxHorizontalOffset = 0;

            if (resizedBitmap != null)
            {
                maxVerticalOffset = ClientSize.Height - resizedBitmap.Height;
                maxHorizontalOffset = ClientSize.Width - resizedBitmap.Width;
            }

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
                case Keys.Delete:
                    this.WindowState = FormWindowState.Minimized;
                    return;
                case Keys.L:
                    // Open library window
                    break;
                case Keys.N:
                    FrmFileSystemNavigation dialog = new FrmFileSystemNavigation();
                    currentArchiveReader = dialog.GetNewReader();

                    if (currentArchiveReader == null) return;

                    originalBitmap = new Bitmap(new System.IO.MemoryStream(currentArchiveReader.GetCurrentFile()));
                    ResizeImage();
                    break;
                default:
                    return;
            }

            RepaintAll();
        }

        private void FrmMain_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void FrmMain_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
                FrmMain_KeyUp(null, new KeyEventArgs(Keys.Down));
            else
                FrmMain_KeyUp(null, new KeyEventArgs(Keys.Up));
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

    }
}
