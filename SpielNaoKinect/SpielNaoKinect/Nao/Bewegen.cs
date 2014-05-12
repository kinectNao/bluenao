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
        private TextToSpeechProxy tts;
        public int degree;
        public float radiant;

        public Bewegen(RobotPostureProxy rpp, MotionProxy motion, MainWindow mw, TextToSpeechProxy tts)
        {
            this.rpp = rpp;
            this.motion = motion;
            this.mw = mw;
            this.tts = tts;
        }


        public void Bewegung_erzeugen()
        {
            Random r = new Random();
            Bewegungsnummer= r.Next(1, 16);
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
                        float[] Winkel1 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(90), 
                                              UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(90) };
                        float[] Winkel2 = { UmrechnungDegRad(0), UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel3 = { UmrechnungDegRad(-85), UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(85), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.08f);
                        int IDMotion2 = motion.post.angleInterpolationWithSpeed(Joints, Winkel2, 0.08f);
                        int IDMotion3 = motion.post.angleInterpolationWithSpeed(Joints, Winkel3, 0.08f);

                        while (motion.isRunning(IDMotion1) || motion.isRunning(IDMotion2) || motion.isRunning(IDMotion3))
                        {
                            SpeichereAlleWerte();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung8 Fehler" + e.Message);
                    }
                    break;
                case 9: //linker Arme zuerst Seite, rechter Arm hoch; dann links und rechts mitte; dann links hoch, rechts seite; dann mitte
                    try
                    {
                        string[] Joints = { "LShoulderRoll", "LShoulderPitch", "RShoulderRoll", "RShoulderPitch" };
                        float[] Winkel1 = { UmrechnungDegRad(0), UmrechnungDegRad(-90), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel2 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0) };
                        float[] Winkel3 = { UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-90) };
                        float[] Winkel4 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.12f);
                        int IDMotion2 = motion.post.angleInterpolationWithSpeed(Joints, Winkel2, 0.12f);
                        int IDMotion3 = motion.post.angleInterpolationWithSpeed(Joints, Winkel3, 0.12f);
                        int IDMotion4 = motion.post.angleInterpolationWithSpeed(Joints, Winkel4, 0.12f);

                        while (motion.isRunning(IDMotion1) || motion.isRunning(IDMotion2) || motion.isRunning(IDMotion3) || motion.isRunning(IDMotion4))
                        {
                            SpeichereAlleWerte();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung9 Fehler" + e.Message);
                    }
                    break;
                case 10: //linker Arm im rechten Winkel nach unten (außen), rechter Arm im rechten Winkel nach oben (außen), dann beide zur Mitte, dann erst linker Arm im rechten Winkel nach außen, dann der rechte Arm im rechten Winkel nach außen und linker zur Mitte, dann weider auf Mitte
                    try
                    {
                        string[] Joints = { "LElbowRoll", "LShoulderRoll", "LShoulderPitch", "RElbowRoll", "RShoulderRoll", "RShoulderPitch" };
                        float[] Winkel1 = { UmrechnungDegRad(-85), UmrechnungDegRad(75), UmrechnungDegRad(90), UmrechnungDegRad(85), UmrechnungDegRad(-75), UmrechnungDegRad(-90) };
                        float[] Winkel2 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0) };
                        float[] Winkel3 = { UmrechnungDegRad(-85), UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0) };
                        float[] Winkel4 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(85), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel5 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.12f);
                        int IDMotion2 = motion.post.angleInterpolationWithSpeed(Joints, Winkel2, 0.12f);
                        int IDMotion3 = motion.post.angleInterpolationWithSpeed(Joints, Winkel3, 0.12f);
                        int IDMotion4 = motion.post.angleInterpolationWithSpeed(Joints, Winkel4, 0.12f);
                        int IDMotion5 = motion.post.angleInterpolationWithSpeed(Joints, Winkel5, 0.12f);

                        while (motion.isRunning(IDMotion1) || motion.isRunning(IDMotion2) || motion.isRunning(IDMotion3) || motion.isRunning(IDMotion4) || motion.isRunning(IDMotion5))
                        {
                            SpeichereAlleWerte();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung10 Fehler" + e.Message);
                    }
                    break;
                case 11: //beide Arme hoch, über außen nach unten, wieder nach außen in die Mitte
                    try
                    {
                        string[] Joints = { "LShoulderRoll", "LShoulderPitch", "RShoulderRoll", "RShoulderPitch" };
                        float[] Winkel1 = { UmrechnungDegRad(0), UmrechnungDegRad(-90), UmrechnungDegRad(0), UmrechnungDegRad(-90) };
                        float[] Winkel2 = { UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel3 = { UmrechnungDegRad(0), UmrechnungDegRad(90), UmrechnungDegRad(0), UmrechnungDegRad(90) };
                        float[] Winkel4 = { UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel5 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.12f);
                        int IDMotion2 = motion.post.angleInterpolationWithSpeed(Joints, Winkel2, 0.12f);
                        int IDMotion3 = motion.post.angleInterpolationWithSpeed(Joints, Winkel3, 0.12f);
                        int IDMotion4 = motion.post.angleInterpolationWithSpeed(Joints, Winkel4, 0.12f);
                        int IDMotion5 = motion.post.angleInterpolationWithSpeed(Joints, Winkel5, 0.12f);

                        while (motion.isRunning(IDMotion1) || motion.isRunning(IDMotion2) || motion.isRunning(IDMotion3) || motion.isRunning(IDMotion4) || motion.isRunning(IDMotion5))
                        {
                            SpeichereAlleWerte();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung11 Fehler" + e.Message);
                    }
                    break;
                case 12: //Linker Arm runter, dann lienker wieder zur Mitte und rechter im rechten Winkel zur Seite, dann rechten Arm ausstrecken, dann wieder zum rechten Winkel und dann wieder zur Mitte
                    try
                    {
                        string[] Joints = { "LElbowRoll", "LShoulderRoll", "LShoulderPitch", "RElbowRoll", "RShoulderRoll", "RShoulderPitch" };
                        float[] Winkel1 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(90), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0) };
                        float[] Winkel2 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(85), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel3 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel4 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(85), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel5 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.12f);
                        int IDMotion2 = motion.post.angleInterpolationWithSpeed(Joints, Winkel2, 0.12f);
                        int IDMotion3 = motion.post.angleInterpolationWithSpeed(Joints, Winkel3, 0.12f);
                        int IDMotion4 = motion.post.angleInterpolationWithSpeed(Joints, Winkel4, 0.12f);
                        int IDMotion5 = motion.post.angleInterpolationWithSpeed(Joints, Winkel5, 0.12f);

                        while (motion.isRunning(IDMotion1) || motion.isRunning(IDMotion2) || motion.isRunning(IDMotion3) || motion.isRunning(IDMotion4) || motion.isRunning(IDMotion5))
                        {
                            SpeichereAlleWerte();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung12 Fehler" + e.Message);
                    }
                    break;
                case 13: // beide Arme drei Mal hoch und runter
                    try
                    {
                        string[] Joints = { "LShoulderRoll", "LShoulderPitch", "RShoulderRoll", "RShoulderPitch" };
                        float[] Winkel1 = { UmrechnungDegRad(0), UmrechnungDegRad(-90), UmrechnungDegRad(0), UmrechnungDegRad(-90) };
                        float[] Winkel2 = { UmrechnungDegRad(0), UmrechnungDegRad(90), UmrechnungDegRad(0), UmrechnungDegRad(90) };
                        float[] Winkel3 = { UmrechnungDegRad(0), UmrechnungDegRad(-90), UmrechnungDegRad(0), UmrechnungDegRad(-90) };
                        float[] Winkel4 = { UmrechnungDegRad(0), UmrechnungDegRad(90), UmrechnungDegRad(0), UmrechnungDegRad(90) };
                        float[] Winkel5 = { UmrechnungDegRad(0), UmrechnungDegRad(-90), UmrechnungDegRad(0), UmrechnungDegRad(-90) };
                        float[] Winkel6 = { UmrechnungDegRad(0), UmrechnungDegRad(90), UmrechnungDegRad(0), UmrechnungDegRad(90) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.25f);
                        int IDMotion2 = motion.post.angleInterpolationWithSpeed(Joints, Winkel2, 0.25f);
                        int IDMotion3 = motion.post.angleInterpolationWithSpeed(Joints, Winkel3, 0.25f);
                        int IDMotion4 = motion.post.angleInterpolationWithSpeed(Joints, Winkel4, 0.25f);
                        int IDMotion5 = motion.post.angleInterpolationWithSpeed(Joints, Winkel5, 0.25f);
                        int IDMotion6 = motion.post.angleInterpolationWithSpeed(Joints, Winkel6, 0.25f);

                        while (motion.isRunning(IDMotion1) || motion.isRunning(IDMotion2) || motion.isRunning(IDMotion3) || motion.isRunning(IDMotion4) || motion.isRunning(IDMotion5) || motion.isRunning(IDMotion6))
                        {
                            SpeichereAlleWerte();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung13 Fehler" + e.Message);
                    }
                    break;
                case 14: // Beide Arme im 45° Winkel nach außen, dann den Ellbogen im rechten Winkel nach innen knicken lassen, wieder nach außen und zur Startposition
                    try
                    {
                        string[] Joints = { "LElbowRoll", "LShoulderRoll", "LShoulderPitch", "RElbowRoll", "RShoulderRoll", "RShoulderPitch" };
                        float[] Winkel1 = { UmrechnungDegRad(0), UmrechnungDegRad(45), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-45), UmrechnungDegRad(0) };
                        float[] Winkel2 = { UmrechnungDegRad(-85), UmrechnungDegRad(45), UmrechnungDegRad(0), UmrechnungDegRad(85), UmrechnungDegRad(-45), UmrechnungDegRad(0) };
                        float[] Winkel3 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.12f);
                        int IDMotion2 = motion.post.angleInterpolationWithSpeed(Joints, Winkel2, 0.12f);
                        int IDMotion3 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.12f);
                        int IDMotion4 = motion.post.angleInterpolationWithSpeed(Joints, Winkel3, 0.12f);

                        while (motion.isRunning(IDMotion1) || motion.isRunning(IDMotion2) || motion.isRunning(IDMotion3) || motion.isRunning(IDMotion4))
                        {
                            SpeichereAlleWerte();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung14 Fehler" + e.Message);
                    }
                    break;
                case 15: // 
                    try
                    {
                        string[] Joints = { "LShoulderRoll", "LShoulderPitch", "RShoulderRoll", "RShoulderPitch" };
                        float[] Winkel1 = { UmrechnungDegRad(75), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0) };
                        float[] Winkel2 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(-75), UmrechnungDegRad(0) };
                        float[] Winkel3 = { UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0), UmrechnungDegRad(0) };
                        int IDMotion1 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.12f);
                        int IDMotion2 = motion.post.angleInterpolationWithSpeed(Joints, Winkel2, 0.12f);
                        int IDMotion3 = motion.post.angleInterpolationWithSpeed(Joints, Winkel1, 0.12f);
                        int IDMotion4 = motion.post.angleInterpolationWithSpeed(Joints, Winkel2, 0.12f);
                        int IDMotion5 = motion.post.angleInterpolationWithSpeed(Joints, Winkel3, 0.12f);

                        while (motion.isRunning(IDMotion1) || motion.isRunning(IDMotion2) || motion.isRunning(IDMotion3) || motion.isRunning(IDMotion4) || motion.isRunning(IDMotion5))
                        {
                            SpeichereAlleWerte();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bewegung15 Fehler" + e.Message);
                    }
                    break;
                default:
                    Console.WriteLine("Fehler: Es ist keine Bewegung ausgewählt worden.");
                    break;
            }
            tts.say("Nun mache die Bewegung nach");
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
