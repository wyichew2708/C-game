using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using System.Security.Principal;


namespace Windows_HelpTools
{
    public partial class MainFormInterface : Form
    {
        private string filePath = "";
        private Bitmap printscreen;
        private Graphics graphics;
        private bool ImageCropping = false;

        private Point Startpoint;
        private Point Endpoint;
        Graphics drawing;
        private Bitmap Cropprintscreen;
        private Graphics Cropgraphics;

        private Rectangle cropRect;

        public MainFormInterface()
        {
            InitializeComponent();
            this.TopMost = true;
            SetApplicationAutoStart();
        }

        private void SetApplicationAutoStart()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();

            RegistryKey key = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");

            try
            {
                key.CreateSubKey("HelpTools");
                key.SetValue("HelpTools", Assembly.GetExecutingAssembly().Location);

                if ((string)key.GetValue("HelpTools") != null)
                    key.DeleteValue("HelpTools");
            }
            catch (Exception e)
            {
                MessageBox.Show("Please Run as Administrator for better experiences.\n");
            }
        }

        private void MainFormInterface_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            this.Size = new Size(600, 2);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!ImageCropping)
            {
                if (MousePosition.X >= this.Location.X && MousePosition.X <= this.Location.X + this.Size.Width
                    && MousePosition.Y >= this.Location.Y && MousePosition.Y <= this.Location.Y + this.Size.Height)
                    this.Size = new Size(600, 70);
                else
                    this.Size = new Size(600, 2);
            }
        }

        private void PrintScreenButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            DateTime timenow = DateTime.Now;
            TimeSpan duration;
            while (true)
            {
                duration = DateTime.Now.Subtract(timenow);
                if (duration.TotalSeconds > 0.3)
                    break;
            }
            PrintScreen();
            this.Visible = true;
        }

        private void PrintScreen()
        {
            GetDesktopPath();

            printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            graphics = Graphics.FromImage(printscreen as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
            long filename = DateTime.Now.ToFileTimeUtc();

            printscreen.Save(filePath + "\\" + filename + ".bmp", ImageFormat.MemoryBmp);
        }

        private void GetDesktopPath()
        {
            filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }

        private void CropImageButton_Click(object sender, EventArgs e)
        {
            EnableCropImage(true);
            ImageCropping = true;

            if (CropImageButton.Text == "Crop")
            {
                SaveCropImage();
                EnableCropImage(false);
                ImageCropping = false;
            }
        }

        private void SaveCropImage()
        {
            GetDesktopPath();

            Cropprintscreen = new Bitmap(Math.Abs(Endpoint.X - Startpoint.X), Math.Abs(Endpoint.Y - Startpoint.Y));

            using (Cropgraphics = Graphics.FromImage(Cropprintscreen as Image))
            {
                Cropgraphics.DrawImage(printscreen, new Rectangle(0, 0, Cropprintscreen.Width, Cropprintscreen.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);
            }

            long filename = DateTime.Now.ToFileTimeUtc();

            Cropprintscreen.Save(filePath + "\\" + filename + ".bmp", ImageFormat.MemoryBmp);
        }

        private void EnableCropImage(bool bEnable)
        {
            if (bEnable)
            {
                this.Visible = false;
                DateTime timenow = DateTime.Now;
                TimeSpan duration;
                while (true)
                {
                    duration = DateTime.Now.Subtract(timenow);
                    if (duration.TotalSeconds > 0.3)
                        break;
                }
                printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                graphics = Graphics.FromImage(printscreen as Image);
                graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
                this.Visible = true;

                this.Location = new Point(0, 0);
                this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                this.BackgroundImage = printscreen as Image;
            }
            else
            {
                CropImageButton.Text = "Start Crop Image";
                this.Location = new Point(300, 0);
                this.Size = new Size(600,2);
                this.BackgroundImage = null;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            EnableCropImage(false);
            ImageCropping = false;
        }

        private void MainFormInterface_MouseDown(object sender, MouseEventArgs e)
        {
            if (!ImageCropping)
                return;

            Startpoint = MousePosition;
            this.Invalidate();
        }

        private void MainFormInterface_MouseUp(object sender, MouseEventArgs e)
        {
            if (!ImageCropping)
                return;

            Endpoint = MousePosition;

            //Cropprintscreen = new Bitmap(Math.Abs(Endpoint.X - Startpoint.X), Math.Abs(Endpoint.Y - Startpoint.Y));
            //Cropgraphics = Graphics.FromImage(Cropprintscreen as Image);

            drawing = this.CreateGraphics();
            if (Endpoint.X < Startpoint.X)
            {
                if (Endpoint.Y < Startpoint.Y)
                {
                    drawing.DrawRectangle(new Pen(Color.Red, 2), Endpoint.X, Endpoint.Y, Math.Abs(Endpoint.X - Startpoint.X), Math.Abs(Endpoint.Y - Startpoint.Y));

                    cropRect = new Rectangle(Endpoint.X, Endpoint.Y, Math.Abs(Endpoint.X - Startpoint.X), Math.Abs(Endpoint.Y - Startpoint.Y));
                    //Cropgraphics.CopyFromScreen(Endpoint.X, Endpoint.Y, Startpoint.X, Startpoint.Y, Cropprintscreen.Size);
                }
                else
                {
                    drawing.DrawRectangle(new Pen(Color.Red, 2), Endpoint.X, Startpoint.Y, Math.Abs(Endpoint.X - Startpoint.X), Math.Abs(Endpoint.Y - Startpoint.Y));

                    cropRect = new Rectangle(Endpoint.X, Startpoint.Y, Math.Abs(Endpoint.X - Startpoint.X), Math.Abs(Endpoint.Y - Startpoint.Y));
                    //Cropgraphics.CopyFromScreen(Endpoint.X, Startpoint.Y, Startpoint.X, Endpoint.Y, Cropprintscreen.Size);
                }
            }
            else
            {
                if (Endpoint.Y > Startpoint.Y)
                {
                    drawing.DrawRectangle(new Pen(Color.Red, 2), Startpoint.X, Startpoint.Y, Math.Abs(Endpoint.X - Startpoint.X), Math.Abs(Endpoint.Y - Startpoint.Y));

                    cropRect = new Rectangle(Startpoint.X, Startpoint.Y, Math.Abs(Endpoint.X - Startpoint.X), Math.Abs(Endpoint.Y - Startpoint.Y));
                    //Cropgraphics.CopyFromScreen(Startpoint.X, Endpoint.Y, Endpoint.X, Startpoint.Y, Cropprintscreen.Size);
                }
                else
                {
                    drawing.DrawRectangle(new Pen(Color.Red, 2), Startpoint.X, Endpoint.Y, Math.Abs(Endpoint.X - Startpoint.X), Math.Abs(Endpoint.Y - Startpoint.Y));

                    cropRect = new Rectangle(Startpoint.X, Endpoint.Y, Math.Abs(Endpoint.X - Startpoint.X), Math.Abs(Endpoint.Y - Startpoint.Y));
                    //Cropgraphics.CopyFromScreen(Startpoint.X, Startpoint.Y, Endpoint.X, Endpoint.Y, Cropprintscreen.Size);
                }
            }

            CropImageButton.Text = "Crop";
        }
    }
}