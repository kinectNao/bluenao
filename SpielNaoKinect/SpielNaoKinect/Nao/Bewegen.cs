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
        List<float> LShoulderPitch = new List<float>();
        List<float> RShoulderPitch = new List<float>();
        List<float> LShoulderRoll = new List<float>();
        List<float> RShoulderRoll = new List<float>();
        List<float> LElbowRoll = new List<float>();
        List<float> RElbowRoll = new List<float>();
        private int AnzahlDurchlaeufe;


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
            AnzahlDurchlaeufe = 0;
            LShoulderPitch.Clear();
            RShoulderPitch.Clear();
            LShoulderRoll.Clear();
            RShoulderRoll.Clear();
            LElbowRoll.Clear();
            RElbowRoll.Clear();
            //WENN NOCH WEITERE LISTEN HINZUKOMMEN MÜSSEN SIE HIER WIEDER AUF 0 GESETZT WERDEN

            Console.WriteLine("Nao führt Bewegung Nummer " + Bewegungsnummer + " aus.");
            switch (Bewegungsnummer)
            {
                case 1:
                    try
                    {
                        string[] Joints = { "LElbowRoll", "LShoulderRoll", "RElbowRoll", "RShoulderRoll" };
                        float[] Winkel = { 0f, 1.31f, 0f, -1.31f };
                        int IDMotion = motion.post.angleInterpolationWithSpeed(Joints, Winkel, 0.08f);

                        while(motion.isRunning(IDMotion))
                        {
                            if (AnzahlDurchlaeufe % 100 == 0)
                            {
                                // if (WinkelNao == degrees);
                                LShoulderPitch.Add(motion.getAngles("LShoulderPitch", false).Last());
                                RShoulderPitch.Add(motion.getAngles("RShoulderPitch", false).Last());
                                LShoulderRoll.Add(motion.getAngles("LShoulderRoll", false).Last());
                                RShoulderRoll.Add(motion.getAngles("RShoulderRoll", false).Last());
                                LElbowRoll.Add(motion.getAngles("LElbowRoll", false).Last());
                                RElbowRoll.Add(motion.getAngles("RElbowRoll", false).Last());
                            }
                            AnzahlDurchlaeufe++;
                        }
                        Console.WriteLine("Es wurden insgesamt " + LShoulderPitch.Count + " Werte vom Nao gespeichert.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung1 Fehler" + e.Message);
                    }
                    break;
                case 2:
                    try
                    {
                        string[] Joints = { "LElbowRoll", "LShoulderPitch", "RElbowRoll", "RShoulderPitch" };
                        float[] Winkel = { 0f, -1.57079633f, 0f, -1.57079633f };
                        int IDMotion = motion.post.angleInterpolationWithSpeed(Joints, Winkel, 0.08f);

                        while(motion.isRunning(IDMotion))
                        {
                            if (AnzahlDurchlaeufe % 100 == 0)
                            {
                                // if (WinkelNao == degrees);
                                LShoulderPitch.Add(motion.getAngles("LShoulderPitch", false).Last());
                                RShoulderPitch.Add(motion.getAngles("RShoulderPitch", false).Last());
                                LShoulderRoll.Add(motion.getAngles("LShoulderRoll", false).Last());
                                RShoulderRoll.Add(motion.getAngles("RShoulderRoll", false).Last());
                                LElbowRoll.Add(motion.getAngles("LElbowRoll", false).Last());
                                RElbowRoll.Add(motion.getAngles("RElbowRoll", false).Last());
                            }
                            AnzahlDurchlaeufe++;
                        }
                        Console.WriteLine("Es wurden insgesamt " + LShoulderPitch.Count + " Werte vom Nao gespeichert.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung1 Fehler" + e.Message);
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
