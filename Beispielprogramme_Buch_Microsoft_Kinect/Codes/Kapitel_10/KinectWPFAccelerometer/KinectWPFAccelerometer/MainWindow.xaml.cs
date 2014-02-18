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

namespace KinectWPFAccelerometer
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor mySensor;
        KinectSensorChooser myChooser;
        System.Windows.Threading.DispatcherTimer myTimer;

        public MainWindow()
        {
            InitializeComponent();

            myTimer = new System.Windows.Threading.DispatcherTimer();
            myTimer.Tick += new EventHandler(myTimer_Tick);
            myTimer.Interval = new TimeSpan(0, 0, 0, 0, 250);

            myChooser = new KinectSensorChooser();
            myChooser.KinectChanged += new EventHandler<KinectChangedEventArgs>(myChooser_KinectChanged);
            this.SensorChooserUI.KinectSensorChooser = myChooser;
            myChooser.Start();
        }

        private void myTimer_Tick(object sender, EventArgs e)
        {
            Vector4 myAccelero = mySensor.AccelerometerGetCurrentReading();
            LblX.Content = "X: " + myAccelero.X;
            LblY.Content = "Y: " + myAccelero.Y;
            LblZ.Content = "Z: " + myAccelero.Z;
        }

        void myChooser_KinectChanged(object sender, KinectChangedEventArgs e)
        {
            if (null != e.OldSensor)
            {
                //Alten Kinect deaktivieren
                if (mySensor != null)
                {
                    mySensor.Dispose();
                    myTimer.Stop();
                }
            }

            if (null != e.NewSensor)
            {
                mySensor = e.NewSensor;
                mySensor.Start();
                myTimer.Start();
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mySensor.Stop();
        }
    }
}
