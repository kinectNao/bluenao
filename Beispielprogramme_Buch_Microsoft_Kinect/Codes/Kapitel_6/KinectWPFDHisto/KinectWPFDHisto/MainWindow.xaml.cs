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

namespace KinectWPFDHisto
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor mySensor;
        short[] myFinalArray;
        short[] myDArray1;
        short[] myDArray2;
        short[] myDArray3;
        int[] myHistoArray;
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
                myHistoArray = new int[50];
                myFinalArray = new short[this.mySensor.DepthStream.FramePixelDataLength];

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
            myHistoArray = new int[50];

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
                    
                    //Perform binning
                    int histoCoord=myFinalArray[innerCoord] /100;
                    if(histoCoord>49)histoCoord=49;
                    myHistoArray[histoCoord]++;
                }
            }

            int maxVal = 1; //Prevent divide by zero
            for (int i = 49; i > 0; i--)
            {
                if (myHistoArray[i] > maxVal) maxVal = myHistoArray[i];
            } 

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            SolidColorBrush brickBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            Pen brickPen = new Pen(brickBrush, 10);
            for (int i = 49; i > 0; i--)
            {
                drawingContext.DrawLine(brickPen, new Point(i * 10, 480), new Point(i * 10, 480 - myHistoArray[i] * 480 / maxVal));

            }

            SolidColorBrush gridBrush = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
            Pen gridPen = new Pen(gridBrush, 1);
            for (int i = 0; i <= 49; i = i + 10)
            {
                drawingContext.DrawLine(gridPen, new Point(i * 10, 480), new Point(i * 10, 0));

            }

            //Calculate maximum height

            int percentage = maxVal / ((640 * 480) / 100);
            label1.Content = percentage.ToString() + "%";

            drawingContext.Close();
            RenderTargetBitmap myTarget = new RenderTargetBitmap(640, 480, 96, 96, PixelFormats.Pbgra32);
            myTarget.Render(drawingVisual);
            image1.Source = myTarget; 

            d.Dispose();
        }

    }
}
