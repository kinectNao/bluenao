using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SpielNaoKinect.Nao;

namespace SpielNaoKinect.Kinect
{
    public class Programm
    {
        void Main(string[] args)
        {
            Thread thread = new Thread(new ThreadStart(InitialisierungWindow));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }


        private void InitialisierungWindow()
        {
            MainWindow Mainwindow = new MainWindow();
        }
    }
}
