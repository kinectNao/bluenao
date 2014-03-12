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
using Microsoft.Kinect;
using System.IO;

//using SpielNaoKinect.Kinect;

namespace SpielNaoKinect
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor mySensor;
        WriteableBitmap myBitmap;
        byte[] myArray;
        public delegate void PixelData();
        public delegate void nachBewegung();
        public bool Neue_Beweg { get; set; }
        public Thread[] Th_Bewegung;
        public Thread[] Th_Init;
        private Init Init;




        [System.STAThread()]
        static void Main(string[] args)
        {
            Console.WriteLine("Starte gesamte Applikation");
            new Application().Run(new MainWindow());
        }


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
                mySensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                myArray = new byte[this.mySensor.ColorStream.FramePixelDataLength];
                myBitmap = new WriteableBitmap(this.mySensor.ColorStream.FrameWidth, this.mySensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);
                KinectImage.Source = myBitmap;
                mySensor.ColorFrameReady += this.SensorColorFrameReady;
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
            }
        }


        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    colorFrame.CopyPixelDataTo(myArray);
                    if (null != Application.Current)
                    {
                        Application.Current.Dispatcher.BeginInvoke((PixelData)delegate
                        {
                            myBitmap.WritePixels(
                            new Int32Rect(0, 0, myBitmap.PixelWidth, myBitmap.PixelHeight),
                            myArray,
                            myBitmap.PixelWidth * sizeof(int),
                            0);
                        });
                    }
                }
            }
        }
        

        //Fenster geschlossen
        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Console.WriteLine("Unloading Programm...");
            if (null != mySensor)
            {
                mySensor.Stop();
            }
            Console.WriteLine("Ende");
        }


        

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


        private void Thread_Bewegung()
        {
            LabelBewegung.Visibility = Visibility.Visible;
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

        private void Thread_Bewegung_Gui()
        {
            //Ganz am Ende, wenn die Bewegung auch nachgemacht wurde, werden die Buttons wieder klickbar
            while (Th_Bewegung[0].IsAlive) ;
            Init.Bew_Ausgangspos();
            //Hier muss der Spieler die Bewegung nachmachen --> Kinect teil einbinden
            if (null != Application.Current)
            {
                Application.Current.Dispatcher.BeginInvoke((nachBewegung)delegate
                {
                    LabelBewegung.Visibility = Visibility.Hidden;
                    Button_NeueBewegung.IsEnabled = true;
                    Button_Wiederholen.IsEnabled = true;
                    Button_NeuesSpiel.IsEnabled = true;
                });
            }
        }

        private void Thread_Bewegung_Nao()
        {
            Init.Bew_Winkel();
        }


        private void Thread_Init()
        {
            LabelBewegung.Visibility = Visibility.Visible;
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
                    LabelBewegung.Visibility = Visibility.Hidden;
                    Button_NeueBewegung.IsEnabled = true;
                });
            }
        }



        private void Button_NeuesSpiel_Click(object sender, RoutedEventArgs e)
        {
            Init.Bew_Ausgangspos();
        }


        /*
        //AB HIER TIMER... KANN ABER NICHT IN THREAD, DA THREAD NICHT AUF GUI ZUGREIFEN KANN
        private System.Windows.Forms.Timer Timer;
        private int Haeufigkeit = 10;
        private void Button_Beenden_Click(object sender, RoutedEventArgs e)
        {
            textboxLog.Visibility = Visibility.Visible;
            textboxLog.Text = "Noch " + Haeufigkeit.ToString() + " Sekunden Zeit für die Bewegung!";
            Timer = new System.Windows.Forms.Timer();
            Timer.Interval = 1000;
            Timer.Enabled = true;
            Timer.Tick += new System.EventHandler(Timer_Tick);
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Haeufigkeit--;
            textboxLog.Text = "Noch " + Haeufigkeit.ToString() + " Sekunden Zeit für die Bewegung!";
            if (Haeufigkeit == 0)
            {
                Timer.Stop();
                textboxLog.Visibility = Visibility.Hidden;
                Haeufigkeit = 10;
            }
        }
         * */
    }
}
