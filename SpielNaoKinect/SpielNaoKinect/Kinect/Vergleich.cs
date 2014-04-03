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

// Achtung! Steckt in einer WHILE Schleife drin!
        public void Achsel_links_roll(double degrees)
        {
            degrees -= 90;
            degrees = degrees * (-1);
            
            for (int i = 0; i < mw._LShoulderRoll.Count(); i++)
            {
                if (degrees >= (mw._LShoulderRoll[i] - mw.Schwierigkeit) && degrees <= (mw._LShoulderRoll[i] + mw.Schwierigkeit))
                {
                    mw._LShoulderRoll.RemoveAt(i);
                }
            }
            if (mw._LShoulderRoll.Count() == 0)
            {
                mw.Achsel_links_roll_erreicht = true;
            }
        }

        public void Achsel_rechts_roll(int degrees)
        {
            degrees -= 90;
            //degrees = degrees * (-1);
            Console.WriteLine(degrees);
            for (int i = 0; i < mw._RShoulderRoll.Count(); i++)
            {
                if (degrees >= (mw._RShoulderRoll[i] - mw.Schwierigkeit) && degrees <= (mw._RShoulderRoll[i] + mw.Schwierigkeit))
                {
                    mw._RShoulderRoll.RemoveAt(i);
                    //Console.WriteLine(mw._RShoulderRoll.Count());
                }
            }
            if (mw._RShoulderRoll.Count() == 0)
            {
                mw.Achsel_rechts_roll_erreicht = true;
            }
            System.Threading.Thread.Sleep(200);
        }

        public void Ellenbogen_rechts_roll(int degrees)
        {
            for (int i = 0; i < mw._RElbowRoll.Count(); i++)
            {
                if (degrees >= (mw._RElbowRoll[i] - mw.Schwierigkeit) && degrees <= (mw._RElbowRoll[i] + mw.Schwierigkeit))
                {
                    mw._RElbowRoll.RemoveAt(i);
                }
            }
            if (mw._RElbowRoll.Count() == 0)
            {
                mw.Ellenbogen_rechts_roll_erreicht = true;
            }
        }

        public void Ellenbogen_links_roll(int degrees)
        {
            for (int i = 0; i < mw._LElbowRoll.Count(); i++)
            {
                if (degrees >= (mw._LElbowRoll[i] - mw.Schwierigkeit) && degrees <= (mw._LElbowRoll[i] + mw.Schwierigkeit))
                {
                    mw._LElbowRoll.RemoveAt(i);
                }
            }
            if (mw._LElbowRoll.Count() == 0)
            {
                mw.Ellenbogen_links_roll_erreicht = true;
            }
        }

        public void Achsel_rechts_pitch(int degrees)
        {
            degrees -= 90;
            for (int i = 0; i < mw._RShoulderPitch.Count(); i++)
            {
                if (degrees >= (mw._RShoulderPitch[i] - mw.Schwierigkeit) && degrees <= (mw._RShoulderPitch[i] + mw.Schwierigkeit))
                {
                    mw._RShoulderPitch.RemoveAt(i);
                }
            }
            if (mw._RShoulderPitch.Count() == 0)
            {
                mw.Achsel_rechts_pitch_erreicht = true;
            }
        }

        public void Achsel_links_pitch(int degrees)
        {
            degrees += 90;
            degrees = degrees * (-1);
            for (int i = 0; i < mw._LShoulderPitch.Count(); i++)
            {
                if (degrees >= (mw._LShoulderPitch[i] - mw.Schwierigkeit) && degrees <= (mw._LShoulderPitch[i] + mw.Schwierigkeit))
                {
                    mw._LShoulderPitch.RemoveAt(i);
                }
            }
            if (mw._LShoulderPitch.Count() == 0)
            {
                mw.Achsel_links_pitch_erreicht = true;
            }
            //Console.WriteLine("Winkel der linken Achsel (Pitch): " + degrees);
            //System.Threading.Thread.Sleep(50);
        }
    }
}
