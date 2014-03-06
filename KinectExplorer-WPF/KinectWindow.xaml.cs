//------------------------------------------------------------------------------
// <copyright file="KinectWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.KinectExplorer
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using Microsoft.Kinect;
    using Microsoft.Samples.Kinect.WpfViewers;
    using System.Threading;
    using System;
    using System.Windows.Forms;


    /// <summary>
    /// Interaction logic for KinectWindow.xaml.
    /// </summary>
    public partial class KinectWindow : Window
    {
        public static readonly DependencyProperty KinectSensorProperty =
            DependencyProperty.Register(
                "KinectSensor",
                typeof(KinectSensor),
                typeof(KinectWindow),
                new PropertyMetadata(null));

        private readonly KinectWindowViewModel viewModel;
        /// <summary>
        /// Initializes a new instance of the KinectWindow class, which provides access to many KinectSensor settings
        /// and output visualization.
        /// </summary>
        public KinectWindow()
        {
            this.viewModel = new KinectWindowViewModel();
            
            // The KinectSensorManager class is a wrapper for a KinectSensor that adds
            // state logic and property change/binding/etc support, and is the data model
            // for KinectDiagnosticViewer.
            this.viewModel.KinectSensorManager = new KinectSensorManager();

            //
            //private System.Windows.Controls.TextBlock Text_Vordergrund_Hauptbildschirm;
            //public loeschen(System.Windows.Controls.TextBlock Text_Vordergrund_Hauptbildschirm)

            System.Windows.Data.Binding sensorBinding = new System.Windows.Data.Binding("KinectSensor");
            sensorBinding.Source = this;
            BindingOperations.SetBinding(this.viewModel.KinectSensorManager, KinectSensorManager.KinectSensorProperty, sensorBinding);

            // Attempt to turn on Skeleton Tracking for each Kinect Sensor
            this.viewModel.KinectSensorManager.SkeletonStreamEnabled = true;

            this.DataContext = this.viewModel;
            InitializeComponent();
        }



        public KinectSensor KinectSensor
        {
            get { return (KinectSensor)GetValue(KinectSensorProperty); }
            set { SetValue(KinectSensorProperty, value); }
        }

        public void StatusChanged(KinectStatus status)
        {
            this.viewModel.KinectSensorManager.KinectSensorStatus = status;
        }

        private Nao Nao;

        

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            Thread InitNao = new Thread(new ThreadStart(InitialisierungNao));
            InitNao.SetApartmentState(ApartmentState.STA);
            InitNao.Start();
            Button_Start.Visibility = Visibility.Hidden;
        }
           
        private void InitialisierungNao()
        {
            Nao = new Nao(this);
            Nao.Initialisierung("127.0.0.1", 9559);
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
            while (ta[0].IsAlive);
            Console.WriteLine("Jetzt ist Thread fertig");
            //Image_Nao.Visibility = Visibility.Hidden;

        }

        private void Thread_Nao()
        {
            Nao.Bew_Winkel();
            //Image_Nao.Visibility = Visibility.Hidden;
        }



        


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
    }


    /// <summary>
    /// A ViewModel for a KinectWindow.
    /// </summary>
    public class KinectWindowViewModel : DependencyObject
    {
        public static readonly DependencyProperty KinectSensorManagerProperty =
            DependencyProperty.Register(
                "KinectSensorManager",
                typeof(KinectSensorManager),
                typeof(KinectWindowViewModel),
                new PropertyMetadata(null));

        public static readonly DependencyProperty DepthTreatmentProperty =
            DependencyProperty.Register(
                "DepthTreatment",
                typeof(KinectDepthTreatment),
                typeof(KinectWindowViewModel),
                new PropertyMetadata(KinectDepthTreatment.ClampUnreliableDepths));

        public KinectSensorManager KinectSensorManager
        {
            get { return (KinectSensorManager)GetValue(KinectSensorManagerProperty); }
            set { SetValue(KinectSensorManagerProperty, value); }
        }

        public KinectDepthTreatment DepthTreatment
        {
            get { return (KinectDepthTreatment)GetValue(DepthTreatmentProperty); }
            set { SetValue(DepthTreatmentProperty, value); }
        }
    }

    /// <summary>
    /// The Command to swap the viewer in the main panel with the viewer in the side panel.
    /// </summary>
    public class KinectWindowsViewerSwapCommand : RoutedCommand
    {  
    }
}
