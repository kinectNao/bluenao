

namespace Microsoft.Samples.Kinect.KinectExplorer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Aldebaran.Proxies;
    using System.Collections;


    public class Nao_start
    {
        /*public static string ip = "192.168.100.6";
        public static MotionProxy motion = new MotionProxy(ip, 9559);
        public static TextToSpeechProxy tts = new TextToSpeechProxy(ip, 9559);

        */
        public void willkommen(String ip, Int32 port)
        {
            MotionProxy motion = new MotionProxy(ip, port);
            RobotPostureProxy rpp = new RobotPostureProxy(ip, port);
            Startposition(rpp);
            Winkelabfrage(motion);
            /*tts.say("Es hat geklappt");
            BatteryProxy battery = new BatteryProxy(ip, 9559);
            tts.say("Meine Batterie ist zu:" + battery.getBatteryCharge() + "Prozent geladen");
             
            try
            {
                RobotPostureProxy rpp = new RobotPostureProxy("127.0.0.1", 9559);
                MotionProxy motion = new MotionProxy("127.0.0.1", 9559);
                BehaviorManagerProxy beh = new BehaviorManagerProxy("192.168.100.6", 9559);
                beh.preloadBehavior("StandUp");     //Behavior muss vorher in Tool gespeichert werden
                beh.runBehavior("StandUp");         //Behavior muss vorher in Tool gespeichert werden
                
                rpp.goToPosture("StandZero", 1);
            }
            catch (Exception e)
            {
                Console.WriteLine("blabla " + e.Message);
            }

            */
        }

        public void Startposition(RobotPostureProxy rpp)
        {
            try
            {
                rpp.goToPosture("StandInit", 1);
            }
            catch (Exception e)
            {
                Console.WriteLine("Startposition Fehler" + e.Message);
            }
        }


        public void Winkelabfrage(MotionProxy motion)
        {
            PositionAbfrage.PositionLArm();

        }
    }
}
