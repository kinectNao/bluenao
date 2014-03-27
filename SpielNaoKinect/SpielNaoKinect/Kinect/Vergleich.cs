using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpielNaoKinect;
using Microsoft.Kinect;
using System.Windows.Media.Media3D;
using SpielNaoKinect.Nao;


namespace SpielNaoKinect.Kinect
{
    public class Vergleich
    {
        private MainWindow mw;

        public Vergleich(MainWindow mw)
        {
            this.mw = mw;
        }

        public void Achsel_links_roll(double degrees)
        {
            // if (WinkelNao == degrees);
            Console.WriteLine("Winkel der linken Achsel (Roll): " + degrees);
            System.Threading.Thread.Sleep(500);
            //HIER KANN AUF DIE LISTE VOM NAO ZUGEFGRIFFEN WERDEN
            //Console.WriteLine("test " + mw._LShoulderPitch.Count + " Werte vom Nao gespeichert.");
        }

        public void Achsel_rechts_roll(double degrees)
        {
            // if (WinkelNao == degrees);
            Console.WriteLine("Winkel der rechten Achsel (Roll): " + degrees);
            System.Threading.Thread.Sleep(500);
        }

        public void Ellenbogen_rechts_roll(double degrees)
        {
            // if (WinkelNao == degrees);
            Console.WriteLine("Winkel des rechten Ellenbogens (Roll): " + degrees);
            System.Threading.Thread.Sleep(500);
        }


        public void Ellenbogen_links_roll(double degrees)
        {
            // if (WinkelNao == degrees);
            Console.WriteLine("Winkel des linken Ellenbogens (Roll): " + degrees);
            System.Threading.Thread.Sleep(500);
        }


        public void Achsel_rechts_pitch(double degrees)
        {
            // if (WinkelNao == degrees);
            Console.WriteLine("Winkel der rechten Achsel (Pitch): " + degrees);
            System.Threading.Thread.Sleep(500);
        }

        public void Achsel_links_pitch(double degrees)
        {
            // if (WinkelNao == degrees);
            Console.WriteLine("Winkel der linken Achsel (Pitch): " + degrees);
            System.Threading.Thread.Sleep(500);
        }

    }
}
