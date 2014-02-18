using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using System.IO;

namespace KinectWPF2
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor mySensor;
        WriteableBitmap myBitmap;
        byte[] myArray;

        public MainWindow()
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

            if (null != mySensor)
            {
                mySensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                myArray = new byte[this.mySensor.ColorStream.FramePixelDataLength];
                myBitmap = new WriteableBitmap(this.mySensor.ColorStream.FrameWidth, this.mySensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);
                image1.Source = myBitmap;
                mySensor.ColorFrameReady += this.SensorColorFrameReady;
                try
                {
                    this.mySensor.Start();
                }
                catch (IOException)
                {
                    this.mySensor = null;
                }
            }
        }

        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    colorFrame.CopyPixelDataTo(myArray);
                    myBitmap.WritePixels(
                        new Int32Rect(0, 0, myBitmap.PixelWidth, myBitmap.PixelHeight),
                        myArray,
                        myBitmap.PixelWidth * sizeof(int),
                        0);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mySensor.Stop();
        }





    }
}
