﻿using System;
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

namespace KinectWPFD3
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor mySensor;
        WriteableBitmap myBitmap;
        short[] myArray;
        byte[] myColorArray;
        byte[] myFinalArray;
        ColorImagePoint[] myColorCoordArray;
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
                mySensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                myArray = new short[this.mySensor.DepthStream.FramePixelDataLength];
                myColorArray = new byte[this.mySensor.ColorStream.FramePixelDataLength];
                myFinalArray = new byte[this.mySensor.ColorStream.FramePixelDataLength];
                myColorCoordArray = new ColorImagePoint[this.mySensor.ColorStream.FramePixelDataLength / 4];
                myBitmap = new WriteableBitmap(this.mySensor.DepthStream.FrameWidth, this.mySensor.DepthStream.FrameHeight, 96.0, 96.0, PixelFormats.Pbgra32, null);
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
            ColorImageFrame c = e.OpenColorImageFrame();
            DepthImageFrame d = e.OpenDepthImageFrame();

            if (c == null || d == null) return;       

            c.CopyPixelDataTo(myColorArray);
            d.CopyPixelDataTo(myArray);

            mySensor.MapDepthFrameToColorFrame(DepthImageFormat.Resolution640x480Fps30, myArray, ColorImageFormat.RgbResolution640x480Fps30, myColorCoordArray);
            for (int x = 0; x < 640; x++)
            {
                for (int y = 0; y < 480; y++)
                {
                    //Get Depth
                    int innerCoord = y * 640 + x;
                    short depthVal = myArray[y * 640 + x];
                    depthVal = (short)(depthVal >> DepthImageFrame.PlayerIndexBitmaskWidth);
                    depthVal = (short)(depthVal << DepthImageFrame.PlayerIndexBitmaskWidth);
                    depthVal /= 255;

                    //Get ColVal
                    int realX = myColorCoordArray[innerCoord].X;
                    int realY = myColorCoordArray[innerCoord].Y;


                    myColorArray[(realY * 640 + realX) * 4 + 1] = (byte)depthVal;
                    myColorArray[(y * 640 + x) * 4 + 3] = 255;
                }
            }

            myBitmap.WritePixels(
                        new Int32Rect(0, 0, myBitmap.PixelWidth, myBitmap.PixelHeight),
                        myColorArray,
                        myBitmap.PixelWidth * 4,
                        0);

            c.Dispose();
            d.Dispose();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mySensor.Stop();
        }
    }
}
