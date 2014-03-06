

namespace Microsoft.Samples.Kinect.KinectExplorer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Aldebaran.Proxies;
    using System.Collections;
    using System.Threading;


    public class Nao
    {

        private MotionProxy motion;
        private RobotPostureProxy rpp;
        private TextToSpeechProxy tts;
        private KinectWindow kinectWindow;

        public Nao(KinectWindow kinectWindow)
        {
            // TODO: Complete member initialization
            this.kinectWindow = kinectWindow;
        }


        public void Initialisierung(String ip, Int32 port)
        {
            motion = new MotionProxy(ip, port);
            rpp = new RobotPostureProxy(ip, port);
            tts = new TextToSpeechProxy(ip, port);

            Nao_Start Nao_Start = new Nao_Start();


            //Nao geht in die Startposition
            Nao_Start.Startposition(rpp, tts);
        }

        public void Bew_Winkel()
        {
            Console.WriteLine("Start des Threads");
            Nao_Bewegen Nao_Bewegen = new Nao_Bewegen(rpp);
            Nao_Winkel Nao_Winkel = new Nao_Winkel(motion);
            Thread[] ta = new Thread[2];
            if (kinectWindow.Neue_Beweg)
            {
                ta[0] = new Thread(Nao_Bewegen.Bewegung_erzeugen);
            }
            else
            {
                ta[0] = new Thread(Nao_Bewegen.Bewegen);
            }
            ta[1] = new Thread(Nao_Winkel.PositionLArm);
            ta[0].Start();
            ta[1].Start();

            while (ta[0].IsAlive) ;
            Nao_Winkel.RequestStop_Winkel();
            Console.WriteLine("Ende der Threads");
        }
    }
}
