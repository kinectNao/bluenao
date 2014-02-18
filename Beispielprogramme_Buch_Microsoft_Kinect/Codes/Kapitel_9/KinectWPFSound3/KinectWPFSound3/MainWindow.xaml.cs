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
using System.IO;
using Microsoft.Speech.Recognition;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Speech.AudioFormat;

namespace KinectWPFSound3
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

                    var drinks = new Choices();
                    drinks.Add(new SemanticResultValue("beer", "pivo"));
                    drinks.Add(new SemanticResultValue("bud", "pivo"));
                    drinks.Add(new SemanticResultValue("vodka", "vodka"));
                    drinks.Add(new SemanticResultValue("russian", "vodka"));

                    var gb = new GrammarBuilder { Culture = ri.Culture };
                    gb.Append(drinks);
                    var g = new Grammar(gb);
                    myEngine.LoadGrammar(g);

                    myEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(myEngine_SpeechRecognized);
                    myEngine.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(myEngine_SpeechRecognitionRejected);

                    myEngine.SetInputToAudioStream(mySensor.AudioSource.Start(), new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                    myEngine.RecognizeAsync(RecognizeMode.Multiple);
                }
            }
        }

        void myEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            const double ConfidenceThreshold = 0.3;

            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                Console.WriteLine(e.Result.Semantics.Value.ToString());
            }

        }

        void myEngine_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {

        }

        private void CmdLimitRange_Click(object sender, RoutedEventArgs e)
        {
            mySensor.AudioSource.BeamAngleMode = BeamAngleMode.Manual;
            mySensor.AudioSource.ManualBeamAngle = 25;
        }
    }
}
