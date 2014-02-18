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

namespace KinectWPFS1
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor mySensor;
        WriteableBitmap myBitmap;
        byte[] myColorArray;
        Skeleton[] mySkeletonArray;
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
                mySensor.SkeletonStream.Enable();
                mySensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                myColorArray = new byte[this.mySensor.ColorStream.FramePixelDataLength];
                mySkeletonArray = new Skeleton[this.mySensor.SkeletonStream.FrameSkeletonArrayLength];
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
            SkeletonFrame s = e.OpenSkeletonFrame();

            if (c == null || s == null) return;

            c.CopyPixelDataTo(myColorArray);
            s.CopySkeletonDataTo(mySkeletonArray);


            //Aufbauen
            BitmapSource bs = BitmapSource.Create(640, 480, 96, 96, PixelFormats.Bgr32, null, myColorArray, 640 * 4);
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawImage(bs, new Rect(0, 0, 640, 480));

            //Rendern
            Pen armPen = new System.Windows.Media.Pen(new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0)), 2);
            Pen legPen = new System.Windows.Media.Pen(new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 255)), 2);
            Pen spinePen = new System.Windows.Media.Pen(new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0)), 2);

            foreach (Skeleton aSkeleton in mySkeletonArray)
            {

                DrawBone(aSkeleton.Joints[JointType.HandLeft], aSkeleton.Joints[JointType.WristLeft], armPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.WristLeft], aSkeleton.Joints[JointType.ElbowLeft], armPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.ElbowLeft], aSkeleton.Joints[JointType.ShoulderLeft], armPen, drawingContext);
                DrawBone(aSkeleton.Joints[JointType.ShoulderLeft], aSkeleton.Joints[JointType.ShoulderCenter], armPen, drawingContext);
                DrawBone(aSkeleton.Joints[ JointType.HandRight], aSkeleton.Joints[ JointType.WristRight], armPen, drawingContext);
                DrawBone(aSkeleton.Joints[ JointType.WristRight], aSkeleton.Joints[ JointType.ElbowRight], armPen, drawingContext);
                DrawBone(aSkeleton.Joints[ JointType.ElbowRight], aSkeleton.Joints[ JointType.ShoulderRight], armPen, drawingContext);
                DrawBone(aSkeleton.Joints[ JointType.ShoulderRight], aSkeleton.Joints[ JointType.ShoulderCenter], armPen, drawingContext);

                DrawBone(aSkeleton.Joints[ JointType.HipCenter], aSkeleton.Joints[ JointType.HipLeft], legPen, drawingContext);
                DrawBone(aSkeleton.Joints[ JointType.HipLeft], aSkeleton.Joints[ JointType.KneeLeft], legPen, drawingContext);
                DrawBone(aSkeleton.Joints[ JointType.KneeLeft], aSkeleton.Joints[ JointType.AnkleLeft], legPen, drawingContext);
                DrawBone(aSkeleton.Joints[ JointType.AnkleLeft], aSkeleton.Joints[ JointType.FootLeft], legPen, drawingContext);
                DrawBone(aSkeleton.Joints[ JointType.HipCenter], aSkeleton.Joints[ JointType.HipRight], legPen, drawingContext);
                DrawBone(aSkeleton.Joints[ JointType.HipRight], aSkeleton.Joints[ JointType.KneeRight], legPen, drawingContext);
                DrawBone(aSkeleton.Joints[ JointType.KneeRight], aSkeleton.Joints[ JointType.AnkleRight], legPen, drawingContext);
                DrawBone(aSkeleton.Joints[ JointType.AnkleRight], aSkeleton.Joints[ JointType.FootRight], legPen, drawingContext);

                DrawBone(aSkeleton.Joints[ JointType.Head], aSkeleton.Joints[ JointType.ShoulderCenter], spinePen, drawingContext);
                DrawBone(aSkeleton.Joints[ JointType.ShoulderCenter], aSkeleton.Joints[ JointType.Spine], spinePen, drawingContext);
                DrawBone(aSkeleton.Joints[ JointType.Spine], aSkeleton.Joints[ JointType.HipCenter], spinePen, drawingContext);
			

            }

            //Abbauen
            drawingContext.Close();
            RenderTargetBitmap myTarget = new RenderTargetBitmap(640, 480, 96, 96, PixelFormats.Pbgra32);
            myTarget.Render(drawingVisual);
            image1.Source = myTarget;


            c.Dispose();
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


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mySensor.Stop();
        }
    }
}
