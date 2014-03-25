using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Aldebaran.Proxies;

namespace SpielNaoKinect.Nao
{
    public class Init
    {
            private MotionProxy motion;
            private RobotPostureProxy rpp;
            private TextToSpeechProxy tts;
            private Bewegen Bewegen;
            private MainWindow mw;
            public Init(MainWindow mw)
            {
                this.mw = mw;
            }

            public void Initialisierung(String ip, Int32 port)
            {
                motion = new MotionProxy(ip, port);
                rpp = new RobotPostureProxy(ip, port);
                tts = new TextToSpeechProxy(ip, port);
                Bewegen = new Bewegen(rpp, motion);
                Start Start = new Start();

                //Nao geht in die Startposition
                Start.Startposition(rpp, tts);
            }

            public void Bew_Winkel()
            {
                if (mw.Neue_Beweg)
                {
                    Bewegen.Bewegung_erzeugen();
                }
                else
                {
                    Bewegen.Bewegung();
                }
            }

            public void Bew_Ausgangspos()
            {
                Bewegen.Ausgangsposition();
            }
    }
}
