using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aldebaran.Proxies;
using SpielNaoKinect;

namespace SpielNaoKinect.Nao
{
    public class Bewegen
    {
        private RobotPostureProxy rpp;
        private MotionProxy motion;
        private int Bewegungsnummer;
        private MainWindow mw;
        public int degree;
        public float radiant;

        public Bewegen(RobotPostureProxy rpp, MotionProxy motion, MainWindow mw)
        {
            this.rpp = rpp;
            this.motion = motion;
            this.mw = mw;
        }


        public void Bewegung_erzeugen()
        {
            Random r = new Random();
            Bewegungsnummer= r.Next(1, 9);
            Bewegung();
        }


        public void Ausgangsposition()
        {
            try
            {
                rpp.goToPosture("StandZero", 1);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ausgangsposition Fehler" + e.Message);
            }
        }


        public void Bewegung()
        {
            mw._LShoulderPitch.Clear();
            mw._RShoulderPitch.Clear();
            mw._LShoulderRoll.Clear();
            mw._RShoulderRoll.Clear();
            mw._LElbowRoll.Clear();
            mw._RElbowRoll.Clear();

            Console.WriteLine("Nao führt Bewegung Nummer " + Bewegungsnummer + " aus.");
            switch (Bewegungsnummer)
            {
                case 1:
                    try
                    {
                        string[] Joints = { "LElbowRoll", "LShoulderRoll", "LShoulderPitch", "RElbowRoll", "RShoulderRoll", "RShoulderPitch" };
                        float[] Winkel1 = { UmrechnungDegRad(-85), UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(85), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel2 = { UmrechnungDegRad(0), UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(85), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel3 = { UmrechnungDegRad(-85), UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel4 = { UmrechnungDegRad(0), UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(85), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel5 = { UmrechnungDegRad(-85), UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.08f);
                        int IDMotion2 = motion.post.angleInterpolationWithSpeed(Joints, Winkel2, 0.2f);
                        int IDMotion3 = motion.post.angleInterpolationWithSpeed(Joints, Winkel3, 0.2f);
                        int IDMotion4 = motion.post.angleInterpolationWithSpeed(Joints, Winkel4, 0.2f);
                        int IDMotion5 = motion.post.angleInterpolationWithSpeed(Joints, Winkel5, 0.2f);

                        while (motion.isRunning(IDMotion1) || motion.isRunning(IDMotion2) || motion.isRunning(IDMotion3) || motion.isRunning(IDMotion4) || motion.isRunning(IDMotion5))
                        {
                            SpeichereAlleWerte();
                        }
                        Console.WriteLine("Es wurden insgesamt " + mw._LShoulderPitch.Count + " Werte vom Nao gespeichert.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung1 Fehler" + e.Message);
                    }
                    break;
                case 2: //fertig (rechter Arm vorne hoch und linker Arm vorne runter)
                    try
                    {
                        string[] Joints = { "LElbowRoll", "LShoulderRoll", "LShoulderPitch", "RElbowRoll", "RShoulderRoll", "RShoulderPitch" };
                        float[] Winkel1 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(90), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-90) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.08f);

                        while(motion.isRunning(IDMotion1))
                        {
                            SpeichereAlleWerte();
                        }
                        Console.WriteLine("Es wurden insgesamt " + mw._LShoulderPitch.Count + " Werte vom Nao gespeichert.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung2 Fehler" + e.Message);
                    }
                    break;
                case 3: //fertig (beide Arme nach außen)
                    try
                    {
                        string[] Joints = { "LElbowRoll", "LShoulderRoll", "LShoulderPitch", "RElbowRoll", "RShoulderRoll", "RShoulderPitch" };
                        float[] Winkel1 = { UmrechnungDegRad(0), UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.08f);

                        while(motion.isRunning(IDMotion1))
                        {
                            SpeichereAlleWerte();
                        }
                        Console.WriteLine("Es wurden insgesamt " + mw._LShoulderPitch.Count + " Werte vom Nao gespeichert.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung3 Fehler" + e.Message);
                    }
                    break;
                case 4: //fertig (beide Arm zuerst nach aussen und dann Winken rechter Arm)
                    try
                    {
                        string[] Joints = { "LElbowRoll", "LShoulderRoll", "LShoulderPitch", "RElbowRoll", "RShoulderRoll", "RShoulderPitch" };
                        float[] Winkel1 = { UmrechnungDegRad(0), UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel2 = { UmrechnungDegRad(0), UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(45), UmrechnungDegRad(0), UmrechnungDegRad(-70) };
                        float[] Winkel3 = { UmrechnungDegRad(0), UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.08f);
                        int IDMotion2 = motion.post.angleInterpolationWithSpeed(Joints, Winkel2, 0.08f);
                        int IDMotion3 = motion.post.angleInterpolationWithSpeed(Joints, Winkel3, 0.08f);

                        while (motion.isRunning(IDMotion1) || motion.isRunning(IDMotion2) || motion.isRunning(IDMotion3))
                        {
                            SpeichereAlleWerte();
                        }
                        Console.WriteLine("Es wurden insgesamt " + mw._LShoulderPitch.Count + " Werte vom Nao gespeichert.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung4 Fehler" + e.Message);
                    }
                    break;
                case 5: //fahre ein Dreieck vor dem Körper
                    try
                    {
                        string[] Joints = { "LElbowRoll", "LShoulderRoll", "LShoulderPitch", "RElbowRoll", "RShoulderRoll", "RShoulderPitch" };
                        float[] Winkel1 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-40), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-40) };
                        float[] Winkel2 = { UmrechnungDegRad(0), UmrechnungDegRad(40), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-40), UmrechnungDegRad(0) };
                        float[] Winkel3 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(40), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(40) };
                        float[] Winkel4 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-40), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-40) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.08f);
                        int IDMotion2 = motion.post.angleInterpolationWithSpeed(Joints, Winkel2, 0.08f);
                        int IDMotion3 = motion.post.angleInterpolationWithSpeed(Joints, Winkel3, 0.08f);
                        int IDMotion4 = motion.post.angleInterpolationWithSpeed(Joints, Winkel4, 0.08f);

                        while (motion.isRunning(IDMotion1) || motion.isRunning(IDMotion2) || motion.isRunning(IDMotion3) || motion.isRunning(IDMotion4))
                        {
                            SpeichereAlleWerte();
                        }
                        Console.WriteLine("Es wurden insgesamt " + mw._LShoulderPitch.Count + " Werte vom Nao gespeichert.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung5 Fehler" + e.Message);
                    }
                    break;
                case 6: //Arme bisschen nach Außen und dann Ellenbogen in 80 Grad anwinkeln
                    try
                    {
                        string[] Joints = { "LElbowRoll", "LShoulderRoll", "LShoulderPitch", "LElbowYaw", "RElbowRoll", "RShoulderRoll", "RShoulderPitch", "RElbowYaw" };
                        float[] Winkel1 = { UmrechnungDegRad(0), UmrechnungDegRad(40), UmrechnungDegRad(0), UmrechnungDegRad(-90), UmrechnungDegRad(0), UmrechnungDegRad(-40), UmrechnungDegRad(0), UmrechnungDegRad(90) };
                        float[] Winkel2 = { UmrechnungDegRad(-80), UmrechnungDegRad(40), UmrechnungDegRad(0), UmrechnungDegRad(-90), UmrechnungDegRad(80), UmrechnungDegRad(-40), UmrechnungDegRad(0), UmrechnungDegRad(90) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.2f);
                        int IDMotion2 = motion.post.angleInterpolationWithSpeed(Joints, Winkel2, 0.08f);

                        while (motion.isRunning(IDMotion1) || motion.isRunning(IDMotion2))
                        {
                            SpeichereAlleWerte();
                        }
                        Console.WriteLine("Es wurden insgesamt " + mw._LShoulderPitch.Count + " Werte vom Nao gespeichert.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung6 Fehler" + e.Message);
                    }
                    break;
                case 7: //beide Arme zuerst nach außen, dann wieder in die Mitte, dann rechts nach oben & links nach unten, dann wieder in die Mitte
                    try
                    {
                        string[] Joints = { "LElbowRoll", "LShoulderRoll", "LShoulderPitch", "RElbowRoll", "RShoulderRoll", "RShoulderPitch" };
                        float[] Winkel1 = { UmrechnungDegRad(0), UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel2 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0) };
                        float[] Winkel3 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(80), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-80) };
                        float[] Winkel4 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.08f);
                        int IDMotion2 = motion.post.angleInterpolationWithSpeed(Joints, Winkel2, 0.08f);
                        int IDMotion3 = motion.post.angleInterpolationWithSpeed(Joints, Winkel3, 0.08f);
                        int IDMotion4 = motion.post.angleInterpolationWithSpeed(Joints, Winkel4, 0.08f);

                        while (motion.isRunning(IDMotion1) || motion.isRunning(IDMotion2) || motion.isRunning(IDMotion3) || motion.isRunning(IDMotion4))
                        {
                            SpeichereAlleWerte();
                        }
                        Console.WriteLine("Es wurden insgesamt " + mw._LShoulderPitch.Count + " Werte vom Nao gespeichert.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung7 Fehler" + e.Message);
                    }
                    break;
                case 8: //Arme zuerst runter, dann zur Seite und dann Ellenbogen anwinkeln
                    try
                    {
                        string[] Joints = { "LElbowRoll", "LShoulderRoll", "LShoulderPitch", "RElbowRoll", "RShoulderRoll", "RShoulderPitch" };
                        float[] Winkel1 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(90), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(90) };
                        float[] Winkel2 = { UmrechnungDegRad(0), UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel3 = { UmrechnungDegRad(-85), UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(85), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.08f);
                        int IDMotion2 = motion.post.angleInterpolationWithSpeed(Joints, Winkel2, 0.08f);
                        int IDMotion3 = motion.post.angleInterpolationWithSpeed(Joints, Winkel3, 0.08f);

                        while (motion.isRunning(IDMotion1) || motion.isRunning(IDMotion2) || motion.isRunning(IDMotion3))
                        {
                            SpeichereAlleWerte();
                        }
                        Console.WriteLine("Es wurden insgesamt " + mw._LShoulderPitch.Count + " Werte vom Nao gespeichert.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung8 Fehler" + e.Message);
                    }
                    break;
                default:
                    Console.WriteLine("Bewegungsnummer wird nicht genutzt");
                    break;
            }
        }

        private void SpeichereAlleWerte()
        {
            mw._LShoulderPitch.Add(UmrechnungRadDeg(motion.getAngles("LShoulderPitch", false).Last()));
            mw._RShoulderPitch.Add(UmrechnungRadDeg(motion.getAngles("RShoulderPitch", false).Last()));
            mw._LShoulderRoll.Add(UmrechnungRadDeg(motion.getAngles("LShoulderRoll", false).Last()));
            mw._RShoulderRoll.Add(UmrechnungRadDeg(motion.getAngles("RShoulderRoll", false).Last()));
            mw._LElbowRoll.Add(UmrechnungRadDeg(motion.getAngles("LElbowRoll", false).Last()));
            mw._RElbowRoll.Add(UmrechnungRadDeg(motion.getAngles("RElbowRoll", false).Last()));
        }

        public float UmrechnungDegRad(int degree)
        {
            radiant = (float)(degree * Math.PI / 180);
            return radiant;
        }
        

        public int UmrechnungRadDeg(float radiant)
        {
            degree = Convert.ToInt32(radiant * 180 / Math.PI);
            return degree;
        }
    }
}
