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

namespace KinectWPFSound1
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor mySensor;
        WriteableBitmap myBitmap;
        KinectSensorChooser myChooser;
        double beamAngle = -20;
        double soundSourceAngle = -20;
        double soundConfidence;
        byte[] myColorArray;

        public MainWindow()
        {
            InitializeComponent();

            myChooser = new KinectSensorChooser();
            myColorArray = new byte[640*480*4];
            myChooser.KinectChanged += new EventHandler<KinectChangedEventArgs>(myChooser_KinectChanged);
            this.SensorChooserUI.KinectSensorChooser = myChooser;
            myChooser.Start();
            updateWindow();
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
                mySensor.AudioSource.Start();
                mySensor.AudioSource.BeamAngleChanged += new EventHandler<BeamAngleChangedEventArgs>(AudioSource_BeamAngleChanged);
                mySensor.AudioSource.SoundSourceAngleChanged += new EventHandler<SoundSourceAngleChangedEventArgs>(AudioSource_SoundSourceAngleChanged);
                myBitmap = new WriteableBitmap(640,480, 96.0, 96.0, PixelFormats.Pbgra32, null);
                image1.Source = myBitmap;
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

        void AudioSource_BeamAngleChanged(object sender, BeamAngleChangedEventArgs e)
        {
            beamAngle = -e.Angle;
            updateWindow();
        }

        void AudioSource_SoundSourceAngleChanged(object sender, SoundSourceAngleChangedEventArgs e)
        {
            soundSourceAngle = -e.Angle;
            soundConfidence = e.ConfidenceLevel;
            updateWindow();
        }

        void updateWindow()
        {
            BitmapSource bs = BitmapSource.Create(640, 480, 96, 96, PixelFormats.Bgr32, null, myColorArray, 640 * 4);
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            Brush kBrush = new SolidColorBrush(Color.FromRgb(25,25,25));
            Pen kPen = new Pen(kBrush, 1);

            drawingContext.DrawRectangle(kBrush, kPen, new Rect(new Point(20,200), new Point(40,280)));


            if (soundSourceAngle != -99)
            {
                int dx = (int)(400 * Math.Cos(soundSourceAngle * Math.PI / 180));
                int dy = (int)(400 * Math.Sin(soundSourceAngle * Math.PI / 180))+240;
                Pen bPen = new Pen(new SolidColorBrush(Color.FromRgb(255, 0, 0)), 60*(1-soundConfidence)+1);
                drawingContext.DrawLine(bPen, new Point(40, 240), new Point(dx, dy));
            }

            if (beamAngle != -99)
            {
                int dx = (int)(400 * Math.Cos(beamAngle * Math.PI / 180));
                int dy = (int)(400 * Math.Sin(beamAngle * Math.PI / 180)) + 240;
                Pen bPen=new Pen(new SolidColorBrush(Color.FromRgb(150,0,150)), 12);
                drawingContext.DrawLine(bPen, new Point(40,240), new Point(dx,dy));
            }


            drawingContext.Close();
            RenderTargetBitmap myTarget = new RenderTargetBitmap(640, 480, 96, 96, PixelFormats.Pbgra32);
            myTarget.Render(drawingVisual);
            image1.Source = myTarget;
        }
    }
}
