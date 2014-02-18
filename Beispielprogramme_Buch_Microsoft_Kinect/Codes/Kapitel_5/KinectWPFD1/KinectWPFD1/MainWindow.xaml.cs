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
using Microsoft.Kinect.Toolkit;

namespace KinectWPFD1
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       KinectSensor mySensor;
        WriteableBitmap myBitmap;
        short[] myArray;
        KinectSensorChooser myChooser;

        public MainWindow()
        {
            InitializeComponent();

            myChooser = new KinectSensorChooser();
            myChooser.KinectChanged += new EventHandler<KinectChangedEventArgs>(myChooser_KinectChanged);
            this.SensorChooserUI.KinectSensorChooser = myChooser;
            myChooser.Start();
        }

        void myChooser_KinectChanged(object sender, KinectChangedEventArgs e)
        {
            if (null != e.OldSensor)
            {
                //Alten Kinect deaktivieren
                if (mySensor != null)
                {
                    mySensor.Dispose();
                }
            }

            if (null != e.NewSensor)
            {
                mySensor = e.NewSensor;
                mySensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                myArray = new short[this.mySensor.DepthStream.FramePixelDataLength];
                myBitmap = new WriteableBitmap(this.mySensor.DepthStream.FrameWidth, this.mySensor.DepthStream.FrameHeight, 96.0, 96.0, PixelFormats.Gray16, null);
                image1.Source = myBitmap;
                mySensor.DepthFrameReady += this.SensorDepthFrameReady;
                try
                {
                    this.mySensor.Start();
                    //SensorChooserUI.Visibility = Visibility.Hidden;
                }
                catch (IOException)
                {
                    this.mySensor = null;
                }
            }
        }

        private void SensorDepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame dFrame = e.OpenDepthImageFrame())
            {
                if (dFrame != null)
                {
                    dFrame.CopyPixelDataTo(myArray);
                    myBitmap.WritePixels(
                        new Int32Rect(0, 0, myBitmap.PixelWidth, myBitmap.PixelHeight),
                        myArray,
                        myBitmap.PixelWidth * sizeof(short),
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
