using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aldebaran.Proxies;

namespace SpielNaoKinect.Nao
{
    public class Start
    {
        public void Startposition(RobotPostureProxy rpp, TextToSpeechProxy tts)
        {
            try
            {
                rpp.goToPosture("StandZero", 1);
                tts.say("Herzlich Willkommen zum Spiel.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Startposition Fehler" + e.Message);
            }
        }
    }
}
