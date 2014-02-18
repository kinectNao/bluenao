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
using Microsoft.Kinect.Toolkit;
using System.IO;

namespace KinectWPFDAvg
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor mySensor;
        WriteableBitmap myBitmap;
        short[] myFinalArray;
        short[] myDArray1;
        short[] myDArray2;
        short[] myDArray3;
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
                myDArray1 = new short[this.mySensor.DepthStream.FramePixelDataLength];
                myDArray2 = new short[this.mySensor.DepthStream.FramePixelDataLength];
                myDArray3 = new short[this.mySensor.DepthStream.FramePixelDataLength];
                myFinalArray = new short[this.mySensor.DepthStream.FramePixelDataLength];
                myBitmap = new WriteableBitmap(this.mySensor.DepthStream.FrameWidth, this.mySensor.DepthStream.FrameHeight, 96.0, 96.0, PixelFormats.Gray16, null);
                image1.Source = myBitmap;
                mySensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(mySensor_AllFramesReady);
                try
                {
                    this.mySensor.Start();
                    SensorChooserUI.Visibility = Visibility.Hidden;
                }
                catch (IOException)
                {
                    this.mySensor = null;
                }
            }
        }

        void mySensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            DepthImageFrame d = e.OpenDepthImageFrame();

            if (d == null) return;

            myDArray3 = (short[])myDArray2.Clone();
            myDArray2 = (short[])myDArray1.Clone();
            d.CopyPixelDataTo(myDArray1);

            for (int x = 0; x < 640; x++)
            {
                for (int y = 0; y < 480; y++)
                {
                    //Get Depth
                    int innerCoord = y * 640 + x;
                    short depthVal = myDArray1[innerCoord];
                    depthVal = (short)(depthVal >> DepthImageFrame.PlayerIndexBitmaskWidth);
                    depthVal = (short)(depthVal << DepthImageFrame.PlayerIndexBitmaskWidth);
                    myDArray1[innerCoord] = depthVal;
                }
            }

            for (int x = 0; x < 640; x++)
            {
                for (int y = 0; y < 480; y++)
                {
                    int innerCoord = y * 640 + x;
                    short depth1Val = myDArray1[innerCoord];
                    short depth2Val = myDArray2[innerCoord];
                    short depth3Val = myDArray3[innerCoord];
                    myFinalArray[innerCoord] = (short)(depth1Val / 3 + depth2Val / 3 + depth3Val / 3);
                }
            }

            myBitmap.WritePixels(
                        new Int32Rect(0, 0, myBitmap.PixelWidth, myBitmap.PixelHeight),
                        myFinalArray,
                        myBitmap.PixelWidth * 2,
                        0);

            d.Dispose();
        }

    }
}
