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
            degrees += 90;
            if (degrees >= (mw._LShoulderRoll.First() - mw.Schwierigkeit) && degrees <= (mw._LShoulderRoll.First() + mw.Schwierigkeit))
            {
                Console.WriteLine("Startwert 1 erreicht");
                if (degrees >= (mw._LShoulderRoll.Last() - mw.Schwierigkeit) && degrees <= (mw._LShoulderRoll.Last() + mw.Schwierigkeit))
                {
                    Console.WriteLine("zielwert 1 erreicht");
                    mw.Achsel_links_roll_erreicht = true;
                }
            }
        }

        public void Achsel_rechts_roll(int degrees)
        {
            degrees -= 90;
            if (degrees >= (mw._RShoulderRoll.First() - mw.Schwierigkeit) && degrees <= (mw._RShoulderRoll.First() + mw.Schwierigkeit))
            {
                Console.WriteLine("Startwert 2 erreicht");
                if (degrees >= (mw._RShoulderRoll.Last() - mw.Schwierigkeit) && degrees <= (mw._RShoulderRoll.Last() + mw.Schwierigkeit))
                {
                    Console.WriteLine("zielwert 2 erreicht");
                    mw.Achsel_rechts_roll_erreicht = true;
                }
            }
        }

        public void Ellenbogen_rechts_roll(int degrees)
        {
            if (degrees >= (mw._RElbowRoll.First() - mw.Schwierigkeit) && degrees <= (mw._RElbowRoll.First() + mw.Schwierigkeit))
            {
                Console.WriteLine("Startwert 3 erreicht");
                if (degrees >= (mw._RElbowRoll.Last() - mw.Schwierigkeit) && degrees <= (mw._RElbowRoll.Last() + mw.Schwierigkeit))
                {
                    Console.WriteLine("zielwert 3 erreicht");
                    mw.Ellenbogen_rechts_roll_erreicht = true;
                }
            }
        }

        public void Ellenbogen_links_roll(int degrees)
        {
            if (degrees >= (mw._LElbowRoll.First() - mw.Schwierigkeit) && degrees <= (mw._LElbowRoll.First() + mw.Schwierigkeit))
            {
                Console.WriteLine("Startwert 4 erreicht");
                if (degrees >= (mw._LElbowRoll.Last() - mw.Schwierigkeit) && degrees <= (mw._LElbowRoll.Last() + mw.Schwierigkeit))
                {
                    Console.WriteLine("zielwert 4 erreicht");
                    mw.Ellenbogen_links_roll_erreicht = true;
                }
            }
        }

        public void Achsel_rechts_pitch(int degrees)
        {
            degrees -= 90;
            if (degrees >= (mw._RShoulderPitch.First() - mw.Schwierigkeit) && degrees <= (mw._RShoulderPitch.First() + mw.Schwierigkeit))
            {
                Console.WriteLine("Startwert 5 erreicht");
                if (degrees >= (mw._RShoulderPitch.Last() - mw.Schwierigkeit) && degrees <= (mw._RShoulderPitch.Last() + mw.Schwierigkeit))
                {
                    Console.WriteLine("zielwert 5 erreicht");
                    mw.Achsel_rechts_pitch_erreicht = true;
                }
            }
        }

        public void Achsel_links_pitch(int degrees)
        {
            degrees += 90;
            degrees = degrees * (-1);
            if (degrees >= (mw._LShoulderPitch.First() - mw.Schwierigkeit) && degrees <= (mw._LShoulderPitch.First() + mw.Schwierigkeit))
            {
                Console.WriteLine("Startwert 6 erreicht");
                if (degrees >= (mw._LShoulderPitch.Last() - mw.Schwierigkeit) && degrees <= (mw._LShoulderPitch.Last() + mw.Schwierigkeit))
                {
                    Console.WriteLine("zielwert 6 erreicht");
                    mw.Achsel_links_pitch_erreicht = true;
                }
            }
            //Console.WriteLine("Winkel der linken Achsel (Pitch): " + degrees);
            //System.Threading.Thread.Sleep(50);
        }
    }
}
