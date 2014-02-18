using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Kinect.KinectExplorer
{
    public static class PositionAbfrage
    {
        public static void PositionLArm()
        {
            string name = "LArm";
            int space = 2;
            bool useSensorValues = true;
            List<float> result = new List<float>();
            //result = motion.getPosition(name, space, useSensorValues);
            Console.WriteLine("test " + name);
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine("LArm: Wert an Stelle " + i + " ist:  " + result.ElementAt(i));
            }
        }

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
    }
}
