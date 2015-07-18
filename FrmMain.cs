using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace comicReader.NET
{
    public partial class FrmMain : Form
    {
        const int wordWrapLineLenght = 90;

        enum DisplayMode
        {
            FitWidth = 0,
            FullPicture = 1,
            Zoom = 2
        }

        DisplayMode currentDisplayMode = DisplayMode.Zoom;
        Bitmap originalBitmap;
        Bitmap resizedBitmap;
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

            currentComic = new Comic();
        }

        public void ResizeImage()
        {
            DateTime start = DateTime.Now;

            resizedBitmap = new Bitmap((int)(originalBitmap.Width * currentComic.Zoom), (int)(originalBitmap.Height * currentComic.Zoom));

            using (Graphics graphics = Graphics.FromImage(resizedBitmap))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

                graphics.DrawImage(originalBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height);

                //System.Drawing.Imaging.ImageAttributes attr = new System.Drawing.Imaging.ImageAttributes();
                //System.Drawing.Imaging.ColorMatrix matrix = new System.Drawing.Imaging.ColorMatrix();
                //attr.SetColorMatrix(matrix);
                //graphics.DrawImage(originalBitmap, 
                //                   new Rectangle(0, 0, resizedBitmap.Width, resizedBitmap.Height), 
                //                   0,
                //                   0,
                //                   originalBitmap.Width, 
                //                   originalBitmap.Height,
                //                   GraphicsUnit.Pixel,
                //                   new System.Drawing.Imaging.ImageAttributes());
            }

            Debug.Print("Resizing time: " + DateTime.Now.Subtract(start).ToString());

            SetWindowTitle(string.Format("{0} - {1}{2}",
                                         System.IO.Path.GetFileName(currentArchiveReader.CurrentPath),
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

        /// <summary>
        /// Draw a picture containing the text (with crude word-wrapping).
        /// </summary>
        private Bitmap DrawTextToBitmap(string text)
        {
            text = text.Replace("\r\n", "\n").Replace("\t", "    ");

            StringBuilder wordWrappedText = new StringBuilder(text.Length + 100);
            int linesCount = 0;

            foreach (string l in text.Split('\n'))
            {
                int i = 0;
                while (i < l.Length)
                {
                    int lenght = i + wordWrapLineLenght > l.Length ? l.Length - i : wordWrapLineLenght;
                    wordWrappedText.Append(l.Substring(i, lenght) + Environment.NewLine);
                    i += wordWrapLineLenght;
                    linesCount++;
                    if (linesCount > 1000)
                    {
                        wordWrappedText.Append("...");
                        goto doneWithReading;
                    }
                }
            }

        doneWithReading:

            Font f = new Font(FontFamily.GenericMonospace, 10, FontStyle.Bold);

            Graphics tmpGraphics = Graphics.FromImage(new Bitmap(1, 1));
            int characterWidth = (int)(tmpGraphics.MeasureString("A", f).Width);
            int width = (int)(tmpGraphics.MeasureString(new String('A', wordWrapLineLenght + 3), f).Width);
            int height = (int)((linesCount + 2) * (f.GetHeight()));

            Bitmap output = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(output);

            g.FillRectangle(Brushes.White, 0, 0, output.Width, output.Height);
            g.DrawString(wordWrappedText.ToString(), f, Brushes.Black, f.GetHeight(), characterWidth);

            return output;
        }

        private void LoadCurrentFile()
        {
            try
            {
                if (!currentArchiveReader.GetCurrentFileName().ToLower().EndsWith(".txt"))
                    originalBitmap = new Bitmap(new System.IO.MemoryStream(currentArchiveReader.GetCurrentFile()));
                else
                {
                    //Rather quick and dirty (and wrong) way to detect the character encoding: I assume that the shortest result between UTF8 and SHIFT-JIS is the correct one
                    string textUTF8 = UTF8Encoding.UTF8.GetString(currentArchiveReader.GetCurrentFile());
                    string textSJIS = Encoding.GetEncoding("shift_jis").GetString(currentArchiveReader.GetCurrentFile());

                    string text = textUTF8.Length < textSJIS.Length ? textUTF8 : textSJIS;

                    originalBitmap = DrawTextToBitmap(text);
                }
            }
            catch (Exception e)
            {
                originalBitmap = DrawTextToBitmap(e.Message + "\n" + e.StackTrace);
            }
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            DateTime keyPressTime = DateTime.Now;
            Debug.Print("Key pressed");


            //Commands that make sense even when there's no open comic at the moment
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    this.WindowState = FormWindowState.Minimized;
                    return;
                case Keys.L:
                    // Open library window
                    FrmLibrary libraryDialog = new FrmLibrary(currentLibrary,
                                                              currentArchiveReader == null ? null : currentArchiveReader.CurrentPath,
                                                              currentComic.Id);

                    Comic newComic = libraryDialog.GetComic();

                    if (newComic == null) return;

                    currentComic = newComic;
                    try
                    {
                        currentArchiveReader = currentComic.CreateArchiveReader();
                        LoadCurrentFile();
                        ResizeImage();
                        RepaintAll();
                    }
                    catch (FileNotFoundException)
                    {
                        currentArchiveReader = null;
                        SetWindowTitle(currentComic.Title);
                        MessageBox.Show(string.Format("Path {0} does not exists!", currentComic.Path));
                    }
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

                    currentArchiveReader.SetParentComic(currentComic);

                    LoadCurrentFile();
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
                    currentComic.Zoom *= 1.1;
                    ResizeImage();
                    break;
                case Keys.Subtract:
                    currentComic.Zoom *= 0.9;
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
                    currentHorizontalPosition = (this.Width - resizedBitmap.Width) / 2;
                    currentVerticalPosition = 0;
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
                    currentHorizontalPosition = (this.Width - resizedBitmap.Width) / 2;
                    currentVerticalPosition = 0;
                    break;
                case Keys.NumPad9:
                    if (ClientSize.Height < resizedBitmap.Height)
                        currentVerticalPosition = 0;
                    if (ClientSize.Width < resizedBitmap.Width)
                        currentHorizontalPosition = maxHorizontalOffset;
                    break;
                // PAGE KEYS
                case Keys.PageDown:
                    if (e.Shift)
                        currentArchiveReader.CurrentPosition = (new Random(DateTime.Now.Millisecond)).Next(currentArchiveReader.FileNames.Count - 1);

                    currentArchiveReader.MoveToNextFile();
                    LoadCurrentFile();
                    ResizeImage();
                    break;
                case Keys.PageUp:
                    if (e.Shift)
                        currentArchiveReader.CurrentPosition = (new Random(DateTime.Now.Millisecond)).Next(currentArchiveReader.FileNames.Count - 1);

                    currentArchiveReader.MoveToPreviousFile();
                    LoadCurrentFile();
                    ResizeImage();
                    break;
                case Keys.Insert:
                    if (e.Shift)
                        currentArchiveReader.MoveToNextCollection(ArchiveReader.CollectionMovementDirection.Backwards);
                    else
                        currentArchiveReader.MoveToNextCollection(ArchiveReader.CollectionMovementDirection.Forward);

                    LoadCurrentFile();
                    ResizeImage();
                    break;
                // LIBRARY STUFF
                case Keys.S:
                    if (!currentComic.Saved) return; //TODO: could open a popup for adding this new comic to the library, instead

                    currentLibrary.SaveComic(currentComic);
                    osdText = "SAVED";
                    break;
                case Keys.C:
                    if (!currentComic.Saved) return;

                    try
                    {
                        currentComic = currentLibrary.GetComic(currentComic.Id);
                    }
                    catch (Library.NotExistingComicException)
                    {
                        MessageBox.Show("This comic doesn't seem to exist in the database. Maybe you already deleted it?");
                        return;
                    }

                    try
                    {
                        currentArchiveReader = currentComic.CreateArchiveReader();
                    }
                    catch (FileNotFoundException)
                    {
                        currentArchiveReader = null;
                        SetWindowTitle(currentComic.Title);
                        MessageBox.Show(string.Format("Path {0} does not exists!", currentComic.Path));
                        return;
                    }

                    LoadCurrentFile();
                    ResizeImage();
                    break;

                default:
                    return;
            }

            RepaintAll();

            Debug.Print("Key handling over: " + DateTime.Now.Subtract(keyPressTime).ToString());
        }

        private void FrmMain_MouseWheel(object sender, MouseEventArgs e)
        {
            if (currentArchiveReader == null)
                return;

            if (resizedBitmap.Height <= ClientSize.Height) return;

            int maxVerticalOffset = ClientSize.Height - resizedBitmap.Height;

            if (e.Delta < 0)
            {
                currentVerticalPosition -= 100;
                if (currentVerticalPosition <= maxVerticalOffset) currentVerticalPosition = maxVerticalOffset;
            }
            else
            {
                currentVerticalPosition += 100;
                if (currentVerticalPosition > 0) currentVerticalPosition = 0;
            }

            RepaintAll();

        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg != 0x020E) return; //WM_MOUSEHWHEEL
            if (this.IsDisposed) return;
            if (m.HWnd != this.Handle) return;


            int delta = m.WParam.ToInt64() < 0 ? 100 : -100;

            if (currentArchiveReader == null) return;
            if (resizedBitmap.Width <= ClientSize.Width) return;

            int maxHorizontalOffset = ClientSize.Width - resizedBitmap.Width;

            if (delta < 0)
            {
                currentHorizontalPosition -= 100;
                if (currentHorizontalPosition <= maxHorizontalOffset) currentHorizontalPosition = maxHorizontalOffset;
            }
            else
            {
                currentHorizontalPosition += 100;
                if (currentHorizontalPosition > 0) currentHorizontalPosition = 0;
            }

            RepaintAll();

            m.Result = (IntPtr)1;
        }


        private void RepaintAll()
        {
            Graphics g = Graphics.FromHwnd(this.Handle);
            PaintImage(g);
        }

        private void SetDefaultPosition()
        {
            if (this.Width > resizedBitmap.Width)
                currentHorizontalPosition = (this.ClientSize.Width - resizedBitmap.Width) / 2;
            else
                currentHorizontalPosition = this.ClientSize.Width - resizedBitmap.Width; //TODO: enable the user to choose between comic style and manga style
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
            //passare a PaintImage() e.ClipRectangle, in modo da disegnare solo il pezzo di immagine che serve
            PaintImage(e.Graphics);
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            if (currentArchiveReader == null) return;

            SetDefaultPosition();
            RepaintAll();
        }

        private void FrmMain_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor.Show();

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
            if (currentArchiveReader == null) return;

            mouseDragStart = e.Location;
            originalImagePosition.X = currentHorizontalPosition;
            originalImagePosition.Y = currentVerticalPosition;
        }

        private void FrmMain_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDragStart = null;
        }

        private void FrmMain_DragDrop(object sender, DragEventArgs e)
        {

            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);

            currentArchiveReader = new ArchiveReader(filenames[0], null);

            if (currentComic != null) currentArchiveReader.SetParentComic(currentComic);

            LoadCurrentFile();
            ResizeImage();
            RepaintAll();
        }

        private void FrmMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }


    }
}
