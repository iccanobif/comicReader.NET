using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
        ArchiveReader archiveReader;

        public FrmMain()
        {
            InitializeComponent();

            archiveReader = new ArchiveReader(@"f:\mieiProgrammi\comicReader.NET\testImages\");
            originalBitmap = new Bitmap(new System.IO.MemoryStream(archiveReader.GetCurrentFile()));
            this.BackColor = Color.Black;
            ResizeImage();
            SetDefaultPosition();
        }

        public void ResizeImage()
        {
            resizedBitmap = new Bitmap((int)(originalBitmap.Width * zoom), (int)(originalBitmap.Height * zoom));
            //resizedBitmap.SetResolution(originalBitmap.HorizontalResolution, originalBitmap.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(resizedBitmap))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

                graphics.DrawImage(originalBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height);
            }
        }

        private void FrmMain_KeyUp(object sender, KeyEventArgs e)
        {
            int maxVerticalOffset = ClientSize.Height - resizedBitmap.Height;
            int maxHorizontalOffset = ClientSize.Width - resizedBitmap.Width;

            switch (e.KeyCode)
            {
                // ZOOM KEYS
                case Keys.Add:
                    zoom = zoom * 1.1;
                    ResizeImage();
                    SetDefaultPosition();
                    break;
                case Keys.Subtract:
                    zoom = zoom * 0.9;
                    ResizeImage();
                    SetDefaultPosition();
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
                    originalBitmap = new Bitmap(new System.IO.MemoryStream(archiveReader.GetNextFile()));
                    ResizeImage();
                    SetDefaultPosition();
                    break;
                case Keys.PageUp:
                    originalBitmap = new Bitmap(new System.IO.MemoryStream(archiveReader.GetPreviousFile()));
                    ResizeImage();
                    SetDefaultPosition();
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
            if (currentDisplayMode != DisplayMode.Zoom) return;

            g.DrawImage(resizedBitmap, new Point(currentHorizontalPosition, currentVerticalPosition));
            g.FillRectangle(Brushes.Black, 0, 0, currentHorizontalPosition, this.ClientSize.Height);
            g.FillRectangle(Brushes.Black, currentHorizontalPosition + resizedBitmap.Width, 0, this.Width, this.ClientSize.Height);
            g.FillRectangle(Brushes.Black, currentHorizontalPosition, resizedBitmap.Height, resizedBitmap.Width + 1, this.ClientSize.Height);


            //g.DrawImage(resizedBitmap, new Point(0, 0));
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
