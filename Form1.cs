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
            //this.BackgroundImage = ResizeImage(bmp, this.Width, this.Height);
            this.BackColor = Color.Black;
            ResizeImage();
        }
        
        public void ResizeImage()
        {
            resizedBitmap = new Bitmap((int)(originalBitmap.Width * zoom), (int)(originalBitmap.Height * zoom));
            resizedBitmap.SetResolution(originalBitmap.HorizontalResolution, originalBitmap.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(resizedBitmap))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(originalBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height);
            }
        }
        
        private void FrmMain_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Add:
                    zoom = zoom * 1.1;
                    ResizeImage();
                    break;
                case Keys.Subtract:
                    zoom = zoom * 0.9;
                    ResizeImage();
                    break;
                case Keys.Up:
                    if (currentVerticalPosition == 0) 
                        return;
                    currentVerticalPosition -= 10;
                    if (currentVerticalPosition < 0) 
                        currentVerticalPosition = 0;
                    break;
                case Keys.Down:
                    currentVerticalPosition += 10;
                    break;
                default:
                    return;
            }
            
            RepaintAll();
        }

        private void RepaintAll()
        {
            Graphics g = Graphics.FromHwnd(this.Handle);
            PaintImage(g);
        }

        private void PaintImage(Graphics g)
        {
            int x = (this.Width - resizedBitmap.Width) / 2;
            int y = 0;
            g.DrawImage(resizedBitmap, new Point(x, y));
            g.FillRectangle(Brushes.Black, 0, 0, x, this.Height);
            g.FillRectangle(Brushes.Black, x + resizedBitmap.Width, 0, this.Width, this.Height);
            g.FillRectangle(Brushes.Black, x, resizedBitmap.Height, resizedBitmap.Width + 1, this.Height);
        }

        private void FrmMain_Paint(object sender, PaintEventArgs e)
        {
            PaintImage(e.Graphics);
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            RepaintAll();
        }

    }
}


/*
        public static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height, bool highQuality)
        {
            Bitmap result = new Bitmap(width, height);
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

                if (highQuality)
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                else
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            return result;
        }
        */
