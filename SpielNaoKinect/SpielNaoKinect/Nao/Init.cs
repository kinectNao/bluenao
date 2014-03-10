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
            private MainWindow mw;

           public void Nao(MainWindow mw)
            {
                // TODO: Complete member initialization
                this.mw = mw;
            }


            public void Initialisierung(String ip, Int32 port)
            {
                motion = new MotionProxy(ip, port);
                rpp = new RobotPostureProxy(ip, port);
                tts = new TextToSpeechProxy(ip, port);

                Start Start = new Start();


                //Nao geht in die Startposition
                Start.Startposition(rpp, tts);
            }

            public void Bew_Winkel()
            {
                Console.WriteLine("Start des Threads");
                Bewegen Bewegen = new Bewegen(rpp);
                Winkel Winkel = new Winkel(motion);
                Thread[] ta = new Thread[2];
                if (mw.Neue_Beweg)
                {
                    ta[0] = new Thread(Bewegen.Bewegung_erzeugen);
                }
                else
                {
                    ta[0] = new Thread(Bewegen.Bewegung);
                }
                ta[1] = new Thread(Winkel.PositionLArm);
                ta[0].Start();
                ta[1].Start();

                while (ta[0].IsAlive) ;
                Winkel.RequestStop_Winkel();
                Console.WriteLine("Ende der Threads");
            }
    }
}
