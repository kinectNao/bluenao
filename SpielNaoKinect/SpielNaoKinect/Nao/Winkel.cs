using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aldebaran.Proxies;

namespace SpielNaoKinect.Nao
{
    public class Winkel
    {
        private volatile bool StopThread_Winkel;
        private MotionProxy motion;



        public Winkel(MotionProxy motion)
        {
            // TODO: Complete member initialization
            this.motion = motion;
        }

        public void PositionLArm()
        {
            StopThread_Winkel = false;
            while (!StopThread_Winkel)
            {
                string name = "Joints";
                bool useSensors = false;
                /*int space = 2;

                List<float> result = new List<float>();
                result = motion.getPosition(name, space, useSensorValues);
                for (int i = 0; i < 6; i++)
                {
                   Console.WriteLine("LArm: Wert an Stelle " + i + " ist:  " + result.ElementAt(i));
                }*/
                Console.WriteLine(motion.getAngles(name, useSensors).Last().ToString());
            }
        }

        public void RequestStop_Winkel()
        {
            StopThread_Winkel = true;
        }



        /*
        public static void PositionRArm()
        {
            string name = "RArm";
            int space = 2;
            bool useSensorValues = true;
            List<float> result = new List<float>();
            //result = motion.getPosition(name, space, useSensorValues);
            Console.WriteLine("test " + name);
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine("RArm: Wert an Stelle " + i + " ist:  " + result.ElementAt(i));
            }
        }

        public static void PositionHead()
        {
            string name = "Head";
            int space = 2;
            bool useSensorValues = true;
            List<float> result = new List<float>();
            //result = motion.getPosition(name, space, useSensorValues);
            Console.WriteLine("test " + name);
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine("Head: Wert an Stelle " + i + " ist:  " + result.ElementAt(i));
            }
        }

        public static void PositionLLeg()
        {
            string name = "LLeg";
            int space = 2;
            bool useSensorValues = true;
            List<float> result = new List<float>();
            //result = motion.getPosition(name, space, useSensorValues);
            Console.WriteLine("test " + name);
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine("LLeg: Wert an Stelle " + i + " ist:  " + result.ElementAt(i));
            }
        }

        public static void PositionRLeg()
        {
            string name = "RLeg";
            int space = 2;
            bool useSensorValues = true;
            List<float> result = new List<float>();
            //result = motion.getPosition(name, space, useSensorValues);
            Console.WriteLine("test " + name);
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine("RLeg: Wert an Stelle " + i + " ist:  " + result.ElementAt(i));
            }
        }

        public static void PositionTorso()
        {
            string name = "Torso";
            int space = 2;
            bool useSensorValues = true;
            List<float> result = new List<float>();
            //result = motion.getPosition(name, space, useSensorValues);
            Console.WriteLine("test " + name);
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine("Torso: Wert an Stelle " + i + " ist:  " + result.ElementAt(i));
            }
        }
         */
    }
}
