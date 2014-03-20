using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using SpielNaoKinect.Nao;
using SpielNaoKinect.Kinect;
using Microsoft.Kinect;
using System.IO;
using System.Collections.ObjectModel;



namespace SpielNaoKinect
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
// VARIABLEN
        KinectSensor mySensor;
        WriteableBitmap myBitmap;
        byte[] myColorArray;
        Skeleton[] mySkeletonArray;
        public delegate void ImageVonKinect();
        public delegate void nachBewegung();
        public bool Neue_Beweg { get; set; }
        public Thread[] Th_Bewegung;
        public Thread[] Th_Init;
        public Thread[] Th_Spieler;
        private Init Init;
        private Angle Angle;
        private Skeleton currentSkeleton;
        bool SkeletonDa;
        bool TimerEnde;
        int Sekunden;
        System.Timers.Timer Timer;




// MAIN
        [System.STAThread()]
        static void Main(string[] args)
        {
            Console.WriteLine("Starte gesamte Applikation");
            new Application().Run(new MainWindow());
        }

// KINECT starten
        public MainWindow()
        {
            Console.WriteLine("Starte MainWindow");
            InitializeComponent();

            //Warte bis Kinect-Sensor verbunden
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                //Sensorreferenz erstellen
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    Console.WriteLine("KinectSensor ist connected");
                    mySensor = potentialSensor;
                    break;
                }
            }
            

            //Wenn Sensorreferenz vorhanden
            if (null != mySensor)
            {
                mySensor.SkeletonStream.Enable();
                mySensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                myColorArray = new byte[this.mySensor.ColorStream.FramePixelDataLength];
                mySkeletonArray = new Skeleton[this.mySensor.SkeletonStream.FrameSkeletonArrayLength];
                myBitmap = new WriteableBitmap(this.mySensor.ColorStream.FrameWidth, this.mySensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);
                KinectImage.Source = myBitmap;
                //mySensor.ColorFrameReady += this.SensorColorFrameReady;



                if (null != Application.Current)
                {
                    Application.Current.Dispatcher.BeginInvoke((ImageVonKinect)delegate
                    {
                        mySensor.AllFramesReady += mySensor_AllFramesReady;
                    });
                }
                

                try
                {
                    this.mySensor.Start();
                }
                catch (IOException)
                {
                    this.mySensor = null;
                }

                //Initialisierung Nao
                Thread_Init();

                //neues Objekt Kinect erzeugen
                Angle = new Angle(this);
            }
        }


// SKELETON
        private void mySensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            ColorImageFrame cf = e.OpenColorImageFrame();
            SkeletonFrame sf = e.OpenSkeletonFrame();
            if (cf == null || sf == null) return;

            //ERKENNE SKELLETON und führe folgendes durch
            mySkeletonArray = new Skeleton[sf.SkeletonArrayLength];
            sf.CopySkeletonDataTo(mySkeletonArray);
            currentSkeleton = (from s in mySkeletonArray 
                where s.TrackingState == SkeletonTrackingState.Tracked
                select s).FirstOrDefault();
            if (currentSkeleton != null) // wird Skelett erkannt?
            {
                SkeletonDa = true;
            }
            else
            {
                SkeletonDa = false;
            }

            cf.CopyPixelDataTo(myColorArray);

            BitmapSource bs = BitmapSource.Create(640, 480, 96, 96, PixelFormats.Bgr32, null, myColorArray, 640 * 4);
            DrawingVisual DrawingVisual = new DrawingVisual();
            DrawingContext DrawingContext = DrawingVisual.RenderOpen();
            DrawingContext.DrawImage(bs, new Rect(0, 0, 640, 480));

            //Rendern
            Pen armPen = new System.Windows.Media.Pen(new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0)), 2);
            Pen legPen = new System.Windows.Media.Pen(new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 255)), 2);
            Pen spinePen = new System.Windows.Media.Pen(new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0)), 2);

            
            foreach (Skeleton aSkeleton in mySkeletonArray)
            {
                DrawBone(aSkeleton.Joints[JointType.HandLeft], aSkeleton.Joints[JointType.WristLeft], armPen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.WristLeft], aSkeleton.Joints[JointType.ElbowLeft], armPen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.ElbowLeft], aSkeleton.Joints[JointType.ShoulderLeft], armPen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.ShoulderLeft], aSkeleton.Joints[JointType.ShoulderCenter], armPen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.HandRight], aSkeleton.Joints[JointType.WristRight], armPen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.WristRight], aSkeleton.Joints[JointType.ElbowRight], armPen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.ElbowRight], aSkeleton.Joints[JointType.ShoulderRight], armPen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.ShoulderRight], aSkeleton.Joints[JointType.ShoulderCenter], armPen, DrawingContext);

                DrawBone(aSkeleton.Joints[JointType.HipCenter], aSkeleton.Joints[JointType.HipLeft], legPen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.HipLeft], aSkeleton.Joints[JointType.KneeLeft], legPen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.KneeLeft], aSkeleton.Joints[JointType.AnkleLeft], legPen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.AnkleLeft], aSkeleton.Joints[JointType.FootLeft], legPen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.HipCenter], aSkeleton.Joints[JointType.HipRight], legPen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.HipRight], aSkeleton.Joints[JointType.KneeRight], legPen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.KneeRight], aSkeleton.Joints[JointType.AnkleRight], legPen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.AnkleRight], aSkeleton.Joints[JointType.FootRight], legPen, DrawingContext);

                DrawBone(aSkeleton.Joints[JointType.Head], aSkeleton.Joints[JointType.ShoulderCenter], spinePen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.ShoulderCenter], aSkeleton.Joints[JointType.Spine], spinePen, DrawingContext);
                DrawBone(aSkeleton.Joints[JointType.Spine], aSkeleton.Joints[JointType.HipCenter], spinePen, DrawingContext);
            }
            DrawingContext.Close();
            RenderTargetBitmap myTarget = new RenderTargetBitmap(640, 480, 96, 96, PixelFormats.Pbgra32);
            myTarget.Render(DrawingVisual);
            KinectImage.Source = myTarget;
        }


        private void DrawBone(Joint jointFrom, Joint jointTo, Pen aPen, DrawingContext aContext)
        {

            if (jointFrom.TrackingState == JointTrackingState.NotTracked || jointTo.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            if (jointFrom.TrackingState == JointTrackingState.Inferred || jointTo.TrackingState == JointTrackingState.Inferred)
            {
                ColorImagePoint p1 = mySensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointFrom.Position, ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint p2 = mySensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointTo.Position, ColorImageFormat.RgbResolution640x480Fps30);
                //Thin line
                aPen.DashStyle = DashStyles.Dash;
                aContext.DrawLine(aPen, new Point(p1.X, p1.Y), new Point(p2.X, p2.Y));
            }

            if (jointFrom.TrackingState == JointTrackingState.Tracked || jointTo.TrackingState == JointTrackingState.Tracked)
            {
                ColorImagePoint p1 = mySensor.CoordinateMapper.MapSkeletonPointToColorPoint(jointFrom.Position, ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint p2 = mySensor.CoordinateMapper.MapSkeletonPointToColorPoint (jointTo.Position, ColorImageFormat.RgbResolution640x480Fps30);
                //Thick line
                aPen.DashStyle = DashStyles.Solid;
                aContext.DrawLine(aPen, new Point(p1.X, p1.Y), new Point(p2.X, p2.Y));
            }
        }


//WIRD ZZ NICHT BENUTZT - war für SensorColorFrameReady (ohne Skelett) zuständig
        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    colorFrame.CopyPixelDataTo(myColorArray);
                    if (null != Application.Current)
                    {
                        Application.Current.Dispatcher.BeginInvoke((ImageVonKinect)delegate
                        {
                            myBitmap.WritePixels(
                            new Int32Rect(0, 0, myBitmap.PixelWidth, myBitmap.PixelHeight),
                            myColorArray,
                            myBitmap.PixelWidth * sizeof(int),
                            0);
                        });
                    }
                }
            }
        }



// FENSTER geschlossen
        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Console.WriteLine("Unloading Programm...");
            if (null != mySensor)
            {
                mySensor.Stop();
            }
            Console.WriteLine("Ende");
        }

// BUTTONS

        private void Button_NeueBewegung_Click(object sender, RoutedEventArgs e)
        {
            Neue_Beweg = true;
            Thread_Bewegung();
        }

        private void Button_Wiederholen_Click(object sender, RoutedEventArgs e)
        {
            Neue_Beweg = false;
            Thread_Bewegung();
        }

        private void Button_NeuesSpiel_Click(object sender, RoutedEventArgs e)
        {

        }



// THREAD Bewegung
        private void Thread_Bewegung()
        {
            LabelBewegung.Content = "Nao macht eine Bewegung";
            Button_NeueBewegung.IsEnabled = false;
            Button_Wiederholen.IsEnabled = false;
            Button_NeuesSpiel.IsEnabled = false;
            Th_Bewegung = new Thread[2];
            Th_Bewegung[0] = new Thread(new ThreadStart(Thread_Bewegung_Nao));
            Th_Bewegung[1] = new Thread(new ThreadStart(Thread_Bewegung_Gui));
            Th_Bewegung[0].SetApartmentState(ApartmentState.STA);
            Th_Bewegung[1].SetApartmentState(ApartmentState.STA);
            Th_Bewegung[0].Start();
            Th_Bewegung[1].Start();

        }

// TIMER
        private void Timer_Ausfuehrung(object sender, System.Timers.ElapsedEventArgs e)
        {
            Sekunden--;
            if (null != Application.Current)
            {
                Application.Current.Dispatcher.BeginInvoke((nachBewegung)delegate
                {
                    if (Sekunden == 0)
                    {
                        LabelTimer.Content = "";
                        LabelBewegung.Content = "";
                        Timer.Stop();
                        TimerEnde = true;
                    }
                    else
                    {
                        LabelBewegung.Content = "Mache die Bewegung nach: noch " + Sekunden.ToString() + " Sekunden";
                        //LabelTimer.Content = "Noch " + Sekunden.ToString() + " Sekunden";
                    }
                });
            }
        }





        private void Thread_Timer()
        {
            TimerEnde = false;
            Sekunden = 10;
            if (null != Application.Current)
            {
                Application.Current.Dispatcher.BeginInvoke((nachBewegung)delegate
                {
                    LabelBewegung.Content = "Mache die Bewegung nach: noch " + Sekunden.ToString() + " Sekunden";
                    //LabelTimer.Content = "Noch " + Sekunden.ToString() + " Sekunden";
                });
            }
            Timer = new System.Timers.Timer();
            this.Timer.Interval = 1000; //1 sec
            this.Timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Ausfuehrung);
            this.Timer.Start();
        }

        private void Thread_Kinect()
        {
            //while (Th_Spieler[0].IsAlive)
            while (TimerEnde == false)
            {
                Angle.Berechnen(currentSkeleton);
            }
        }
        


        private void Thread_Bewegung_Gui()
        {
//Nun hat Nao die Beweg. durchgeführt. Überprüfung ob Person vor Kinect ist.
            while (Th_Bewegung[0].IsAlive) ;

            if (SkeletonDa == false)
            {
                if (null != Application.Current)
                {
                    Application.Current.Dispatcher.BeginInvoke((nachBewegung)delegate
                    {
                        LabelBewegung.Content = "Stelle dich vor die Kinect...";
                    });
                }
            }
            while (SkeletonDa == false) ;


//Nun hat Kinect eine Person erkannt. Starte Timer
            Th_Spieler = new Thread[2];
            Th_Spieler[0] = new Thread(new ThreadStart(Thread_Timer));
            Th_Spieler[1] = new Thread(new ThreadStart(Thread_Kinect));
            Th_Spieler[0].SetApartmentState(ApartmentState.STA);
            Th_Spieler[1].SetApartmentState(ApartmentState.STA);
            Th_Spieler[0].Start();
            Th_Spieler[1].Start();
            

//Nao geht in seine Ausgangsposition
            Init.Bew_Ausgangspos();

//Timer ist fertig
            while (TimerEnde == false) ;

//Buttons einblenden
            if (null != Application.Current)
            {
                Application.Current.Dispatcher.BeginInvoke((nachBewegung)delegate
                {
                    LabelTimer.Content = "";
                    LabelBewegung.Content = "";
                    Button_NeueBewegung.IsEnabled = true;
                    Button_Wiederholen.IsEnabled = true;
                    Button_NeuesSpiel.IsEnabled = true;
                });
            }
        }



//Nao führt eine Bewegung aus
        private void Thread_Bewegung_Nao()
        {
            Init.Bew_Winkel();
        }

// THREAD Initialisierungen - einmal aufgerufen!
        private void Thread_Init()
        {
            Th_Init = new Thread[2];
            Th_Init[0] = new Thread(new ThreadStart(Thread_Init_Nao));
            Th_Init[1] = new Thread(new ThreadStart(Thread_Init_Gui));
            Th_Init[0].SetApartmentState(ApartmentState.STA);
            Th_Init[1].SetApartmentState(ApartmentState.STA);
            Th_Init[0].Start();
            Th_Init[1].Start();
        }


        private void Thread_Init_Nao()
        {
            Init = new Init(this);
            Init.Initialisierung("127.0.0.1", 9559);
        }


        private void Thread_Init_Gui()
        {
            while (Th_Init[0].IsAlive);
            if (null != Application.Current)
            {
                Application.Current.Dispatcher.BeginInvoke((nachBewegung)delegate
                {
                    Button_NeueBewegung.IsEnabled = true;
                });
            }
        }
    }
}
