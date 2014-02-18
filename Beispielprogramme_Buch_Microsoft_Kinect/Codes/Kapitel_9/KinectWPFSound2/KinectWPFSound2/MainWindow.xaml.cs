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
using Microsoft.Speech.Recognition;
using System.IO;
using Microsoft.Speech.AudioFormat;

namespace KinectWPFSound2
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor mySensor;
        SpeechRecognitionEngine myEngine;
        KinectSensorChooser myChooser;
        byte[] myColorArray;

        public MainWindow()
        {
            InitializeComponent();

            myChooser = new KinectSensorChooser();
            myColorArray = new byte[640 * 480 * 4];
            myChooser.KinectChanged += new EventHandler<KinectChangedEventArgs>(myChooser_KinectChanged);
            this.SensorChooserUI.KinectSensorChooser = myChooser;
            myChooser.Start();
        }

        private static RecognizerInfo GetKinectRecognizer()
        {
            foreach (RecognizerInfo recognizer in SpeechRecognitionEngine.InstalledRecognizers())
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
            }
            
            return null;
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
                
            RecognizerInfo ri = GetKinectRecognizer();
            mySensor = e.NewSensor;
            if (null != ri)
            {
                myEngine = new SpeechRecognitionEngine(ri.Id);

                using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(Properties.Resources.Grammar)))
                {
                    var g = new Grammar(memoryStream);
                    myEngine.LoadGrammar(g);
                }

                myEngine.SpeechRecognized+=new EventHandler<SpeechRecognizedEventArgs>(myEngine_SpeechRecognized);
                myEngine.SpeechRecognitionRejected+=new EventHandler<SpeechRecognitionRejectedEventArgs>(myEngine_SpeechRecognitionRejected);

                myEngine.SetInputToAudioStream(mySensor.AudioSource.Start(), new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                myEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
        }
    }

void  myEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
{
            const double ConfidenceThreshold = 0.3;

            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                Console.WriteLine(e.Result.Semantics.Value.ToString());
            }
                
}

void  myEngine_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
{

}
    }
}
