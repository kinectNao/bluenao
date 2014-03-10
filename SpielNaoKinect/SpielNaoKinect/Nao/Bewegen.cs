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
        private int Bewegungsnummer;

        public Bewegen(RobotPostureProxy rpp)
        {
            this.rpp = rpp;
        }

        public void Bewegung_erzeugen()
        {
            Random r = new Random();
            Bewegungsnummer = r.Next(8);
            Bewegung();
        }

        public void Bewegung()
        {
            rpp.goToPosture("StandInit", 1);
            switch (Bewegungsnummer)
            {
                case 1:
                    try
                    {
                        rpp.goToPosture("Sit", 1);
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
