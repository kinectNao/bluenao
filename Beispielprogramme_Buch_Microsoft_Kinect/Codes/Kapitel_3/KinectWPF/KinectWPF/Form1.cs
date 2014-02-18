using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Kinect;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace KinectWPF
{
    public partial class Form1 : Form
    {
	    Microsoft.Kinect.KinectSensor mySensor;
        byte[] myArray;

        public Form1()
        {
            InitializeComponent();
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    mySensor = potentialSensor;
                    break;
                }
            }
            mySensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            myArray = new byte[mySensor.ColorStream.FramePixelDataLength];
            mySensor.Start();
            timer1.Start();

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            mySensor.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ColorImageFrame myFrame=mySensor.ColorStream.OpenNextFrame(50);
            myFrame.CopyPixelDataTo(myArray);
            Bitmap myBitmap = new Bitmap(myFrame.Width, myFrame.Height, PixelFormat.Format32bppRgb);
            BitmapData bmapdata = myBitmap.LockBits(new Rectangle(0, 0, myFrame.Width, myFrame.Height), ImageLockMode.WriteOnly, myBitmap.PixelFormat);
            IntPtr ptr = bmapdata.Scan0;
            Marshal.Copy(myArray, 0, ptr, myFrame.Width * myFrame.BytesPerPixel * myFrame.Height);
            myBitmap.UnlockBits(bmapdata);
            pictureBox1.Image = myBitmap;

            myFrame.Dispose();
        }
    }
}
