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
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect;
using System.IO;

namespace KinectWPFS3
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
        Skeleton[] mySkeletonArray;
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
                CmdMode.Content = "Normal";
                mySensor.SkeletonStream.Enable();
                myArray = new short[this.mySensor.DepthStream.FramePixelDataLength];
                myColorArray = new byte[this.mySensor.ColorStream.FramePixelDataLength];
                myFinalArray = new byte[this.mySensor.ColorStream.FramePixelDataLength];
                mySkeletonArray = new Skeleton[this.mySensor.SkeletonStream.FrameSkeletonArrayLength];
                myColorCoordArray = new ColorImagePoint[this.mySensor.ColorStream.FramePixelDataLength / 4];
                myBitmap = new WriteableBitmap(this.mySensor.DepthStream.FrameWidth, this.mySensor.DepthStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);
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

        private void CmdMode_Click(object sender, RoutedEventArgs e)
        {
            if(CmdMode.Content.ToString() == "Normal")
            {
                CmdMode.Content = "Seated";
                mySensor.SkeletonStream.TrackingMode=SkeletonTrackingMode.Seated;
            }
            else
            {
                CmdMode.Content = "Normal";
                mySensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;
            }
        }

        void mySensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            ColorImageFrame c = e.OpenColorImageFrame();
            DepthImageFrame d = e.OpenDepthImageFrame();
            SkeletonFrame s = e.OpenSkeletonFrame();

            if (c == null || d == null || s == null) return;

            c.CopyPixelDataTo(myColorArray);
            d.CopyPixelDataTo(myArray);
            s.CopySkeletonDataTo(mySkeletonArray);

            //Rendern der Masken
            mySensor.MapDepthFrameToColorFrame(DepthImageFormat.Resolution640x480Fps30, myArray, ColorImageFormat.RgbResolution640x480Fps30, myColorCoordArray);

            for (int x = 0; x < 640; x++)
            {
                for (int y = 0; y < 480; y++)
                {
                    //Get Depth
                    int innerCoord = y * 640 + x;
                    short depthVal = myArray[y * 640 + x];
                    depthVal = (short)(depthVal & DepthImageFrame.PlayerIndexBitmaskWidth);

                    //Get ColVal
                    int realX = myColorCoordArray[innerCoord].X;
                    int realY = myColorCoordArray[innerCoord].Y;

                    switch (depthVal)
                    {
                        case 0:
                            //Nichts tun
                            break;
                        case 1:             //Spieler 1 - Skelett 0:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                            myColorArray[(realY * 640 + realX) * 4 + 0] = (byte)(33 * depthVal);
                            myColorArray[(realY * 640 + realX) * 4 + 1] = (byte)(33 * depthVal);
                            myColorArray[(realY * 640 + realX) * 4 + 2] = (byte)(33 * depthVal);
                            break;
                    }

                    //myColorArray[(y * 640 + x) * 4 + 3] = 255;
                }
            }

            //Aufbauen
            BitmapSource bs = BitmapSource.Create(640, 480, 96, 96, PixelFormats.Bgr32, null, myColorArray, 640 * 4);
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawImage(bs, new Rect(0, 0, 640, 480));

            



            //Rendern des Skeletts
            Pen armPen = new System.Windows.Media.Pen(new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0)), 2);
            Pen legPen = new System.Windows.Media.Pen(new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 255)), 2);
            Pen spinePen = new System.Windows.Media.Pen(new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0)), 2);

            foreach (Skeleton aSkeleton in mySkeletonArray)
            {

                DrawBone(aSkeleton.Joints[JointType.HandLeft], aSkeleton.Joints[JointType.WristLeft], armPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.WristLeft], aSkeleton.Joints[JointType.ElbowLeft], armPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.ElbowLeft], aSkeleton.Joints[JointType.ShoulderLeft], armPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.ShoulderLeft], aSkeleton.Joints[JointType.ShoulderCenter], armPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.HandRight], aSkeleton.Joints[JointType.WristRight], armPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.WristRight], aSkeleton.Joints[JointType.ElbowRight], armPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.ElbowRight], aSkeleton.Joints[JointType.ShoulderRight], armPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.ShoulderRight], aSkeleton.Joints[JointType.ShoulderCenter], armPen, drawingContext);

                DrawBone(aSkeleton.Joints[JointType.HipCenter], aSkeleton.Joints[JointType.HipLeft], legPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.HipLeft], aSkeleton.Joints[JointType.KneeLeft], legPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.KneeLeft], aSkeleton.Joints[JointType.AnkleLeft], legPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.AnkleLeft], aSkeleton.Joints[JointType.FootLeft], legPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.HipCenter], aSkeleton.Joints[JointType.HipRight], legPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.HipRight], aSkeleton.Joints[JointType.KneeRight], legPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.KneeRight], aSkeleton.Joints[JointType.AnkleRight], legPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.AnkleRight], aSkeleton.Joints[JointType.FootRight], legPen, drawingContext);

                DrawBone(aSkeleton.Joints[JointType.Head], aSkeleton.Joints[JointType.ShoulderCenter], spinePen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.ShoulderCenter], aSkeleton.Joints[JointType.Spine], spinePen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.Spine], aSkeleton.Joints[JointType.HipCenter], spinePen, drawingContext);


            }

            //Abbauen
            drawingContext.Close();
            RenderTargetBitmap myTarget = new RenderTargetBitmap(640, 480, 96, 96, PixelFormats.Pbgra32);
            myTarget.Render(drawingVisual);
            image1.Source = myTarget;

            c.Dispose();
            d.Dispose();
            s.Dispose();


        }

        private void DrawBone(Joint jointFrom, Joint jointTo, Pen aPen, DrawingContext aContext)
        {
            if (jointFrom.TrackingState == JointTrackingState.NotTracked ||
            jointTo.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            if (jointFrom.TrackingState == JointTrackingState.Inferred ||
            jointTo.TrackingState == JointTrackingState.Inferred)
            {
                ColorImagePoint p1 = mySensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointFrom.Position, ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint p2 = mySensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointTo.Position, ColorImageFormat.RgbResolution640x480Fps30);
                //Thin line
                aPen.DashStyle = DashStyles.Dash;
                aContext.DrawLine(aPen, new Point(p1.X, p1.Y), new Point(p2.X, p2.Y));

            }
            if (jointFrom.TrackingState == JointTrackingState.Tracked ||
            jointTo.TrackingState == JointTrackingState.Tracked)
            {
                ColorImagePoint p1 = mySensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointFrom.Position, ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint p2 = mySensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointTo.Position, ColorImageFormat.RgbResolution640x480Fps30);
                //Thick line
                aPen.DashStyle = DashStyles.Solid;
                aContext.DrawLine(aPen, new Point(p1.X, p1.Y), new Point(p2.X, p2.Y));
            }
        }

    }
}
