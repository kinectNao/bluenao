using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aldebaran.Proxies;

namespace Microsoft.Samples.Kinect.KinectExplorer
{
    class Nao_Start
    {
        public void Startposition(RobotPostureProxy rpp, TextToSpeechProxy tts)
        {
            try
            {
                rpp.goToPosture("StandInit", 1);
                tts.say("Herzlich Willkommen zum Spiel.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Startposition Fehler" + e.Message);
            }
        }
    }
}
