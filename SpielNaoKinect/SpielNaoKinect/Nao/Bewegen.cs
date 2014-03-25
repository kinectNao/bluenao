using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aldebaran.Proxies;

namespace SpielNaoKinect.Nao
{
    public class Bewegen
    {
        private RobotPostureProxy rpp;
        private MotionProxy motion;
        private int Bewegungsnummer;

        public Bewegen(RobotPostureProxy rpp, MotionProxy motion)
        {
            this.rpp = rpp;
            this.motion = motion;
        }

        public void Bewegung_erzeugen()
        {
            Random r = new Random();
            Bewegungsnummer= r.Next(1, 2);
            Bewegung();
        }


        public void Ausgangsposition()
        {
            try
            {
                rpp.goToPosture("StandInit", 1);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ausgangsposition Fehler" + e.Message);
            }
        }


        public void Bewegung()
        {
            Console.WriteLine(Bewegungsnummer);
            switch (Bewegungsnummer)
            {
                case 1:
                    try
                    {
                        /*motion.setAngles("LElbowRoll", 0f, 0.08f);
                        motion.setAngles("LShoulderPitch", -1.57079633f, 0.08f);
                        motion.setAngles("RElbowRoll", 0f, 0.08f);
                        motion.setAngles("RShoulderPitch", -1.57079633f, 0.08f);
                        */

                        int IDMotion = motion.post.angleInterpolationWithSpeed("LShoulderPitch", -1.57079633f, 0.08f);
                        string name = "LShoulderPitch";
                        bool useSensorValues = false;
                        while(motion.isRunning(IDMotion))
                        {
                            Console.WriteLine("Winkel Arm " + motion.getAngles(name, useSensorValues).Last().ToString());
                        }


                        //motion.wait(IDMotion, 30000);
                        //while (motion.isRunning(IDMotion)) ;
                        Console.WriteLine("Ende while");
                        //rpp.goToPosture("Sit", 1);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung1 Fehler" + e.Message);
                    }
                    break;
                case 2:
                    try
                    {
                        rpp.goToPosture("Stand", 1);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung2 Fehler" + e.Message);
                    }
                    break;
                case 3:
                    try
                    {
                        rpp.goToPosture("StandZero", 1);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung3 Fehler" + e.Message);
                    }
                    break;
                case 4:
                    try
                    {
                        rpp.goToPosture("Crouch", 1);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung4 Fehler" + e.Message);
                    }
                    break;
                case 5:
                    try
                    {
                        rpp.goToPosture("SitRelax", 1);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung5 Fehler" + e.Message);
                    }
                    break;
                case 6:
                    try
                    {
                        rpp.goToPosture("LyingBelly", 1);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung6 Fehler" + e.Message);
                    }
                    break;
                case 7:
                    try
                    {
                        rpp.goToPosture("LyingBack", 1);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung7 Fehler" + e.Message);
                    }
                    break;
                default:
                    Console.WriteLine("Bewegungsnummer wird nicht genutzt");
                    break;
            }
        }
    }
}
