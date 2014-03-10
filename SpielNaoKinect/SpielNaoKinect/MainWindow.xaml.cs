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
//using SpielNaoKinect.Kinect;

namespace SpielNaoKinect
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            Thread InitNao = new Thread(new ThreadStart(InitialisierungNao));
            InitNao.SetApartmentState(ApartmentState.STA);
            InitNao.Start();
            Button_Start.Visibility = Visibility.Hidden;
        }

        private Init Init;
        private void InitialisierungNao()
        {
            Init = new Init();
            Init.Initialisierung("127.0.0.1", 9559);
        }

        public bool Neue_Beweg { get; set; }
        public Thread[] ta;
        private void Button_NeueBewegung_Click(object sender, RoutedEventArgs e)
        {
            Neue_Beweg = true;
            Thread_Starte();
        }



        private void Button_Wiederholen_Click(object sender, RoutedEventArgs e)
        {
            Neue_Beweg = false;
            Thread_Starte();
        }

        private void Thread_Starte()
        {
            ta = new Thread[2];
            ta[0] = new Thread(new ThreadStart(Thread_Nao));
            ta[1] = new Thread(new ThreadStart(Thread_Kinect));
            ta[0].SetApartmentState(ApartmentState.STA);
            ta[1].SetApartmentState(ApartmentState.STA);
            ta[0].Start();
            ta[1].Start();
        }


        private void Thread_Kinect()
        {
            while (ta[0].IsAlive) ;
            Console.WriteLine("Jetzt ist Thread fertig");
            //Image_Nao.Visibility = Visibility.Hidden;

        }

        private void Thread_Nao()
        {
            Init.Bew_Winkel();
            //Image_Nao.Visibility = Visibility.Hidden;
        }

        private void Button_Beenden_Click(object sender, RoutedEventArgs e)
        {
            
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
