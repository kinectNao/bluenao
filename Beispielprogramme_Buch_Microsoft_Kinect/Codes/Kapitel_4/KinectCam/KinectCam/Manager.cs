using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace KinectCam
{
    class Manager:BackgroundWorker
    {
        public byte[] myArray;
        public byte[] myCacheArray;
        int[,] cMatrix = new int[3, 3];
        TColor[,] aColor = new TColor[3, 3];
        public int mySensorFrameWidth;
        public bool myIsWorkingFlag = false;
        MainWindow myMainWindow;

        public Manager(MainWindow _this)
        {
            DoWork += new DoWorkEventHandler(Manager_DoWork);
            RunWorkerCompleted += new RunWorkerCompletedEventHandler(Manager_RunWorkerCompleted);

            cMatrix[0, 0] = 0;
            cMatrix[0, 1] = -2;
            cMatrix[0, 2] = 0;
            cMatrix[1, 0] = -2;
            cMatrix[1, 1] = 11;
            cMatrix[1, 2] = -2;
            cMatrix[2, 0] = 0;
            cMatrix[2, 1] = -2;
            cMatrix[2, 2] = 0;

            myMainWindow = _this;

        }

        void Manager_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            myMainWindow.myBitmap.WritePixels(
                            new Int32Rect(0, 0, myMainWindow.myBitmap.PixelWidth, myMainWindow.myBitmap.PixelHeight),
                            myArray,
                            myMainWindow.myBitmap.PixelWidth * sizeof(int),
                            0);
            myIsWorkingFlag = false;
        }

        void Manager_DoWork(object sender, DoWorkEventArgs e)
        {
            myIsWorkingFlag = true;
            for (int x = 1; x < 639; x++)
            {
                for (int y = 1; y < 479; y++)
                {
                    aColor[0, 0] = GetAPixel(x - 1, y - 1);
                    aColor[0, 1] = GetAPixel(x - 1, y);
                    aColor[0, 2] = GetAPixel(x - 1, y + 1);
                    aColor[1, 0] = GetAPixel(x, y - 1);
                    aColor[1, 1] = GetAPixel(x, y);
                    aColor[1, 2] = GetAPixel(x, y + 1);
                    aColor[2, 0] = GetAPixel(x + 1, y - 1);
                    aColor[2, 1] = GetAPixel(x + 1, y);
                    aColor[2, 2] = GetAPixel(x + 1, y + 1);

                    byte[] newA = applyTransform(aColor, cMatrix);

                    SetAPixel(x, y, newA[0], newA[1], newA[2]);

                }
            }
            


        }

        private TColor GetAPixel(int _x, int _y)
        {
            int anIndex = (_y * 640 + _x) * 4;

            TColor retval = new TColor();

            retval.r = myCacheArray[anIndex + 2]; //r 
            retval.g = myCacheArray[anIndex + 1];
            retval.b = myCacheArray[anIndex];

            return retval;
        }


        private void SetAPixel(int _x, int _y, byte _r, byte _g, byte _b)
        {
            long anIndex = _y * mySensorFrameWidth + _x;
            anIndex = anIndex * 4;
            myArray[anIndex] = _b;
            myArray[anIndex + 1] = _g;
            myArray[anIndex + 2] = _r;
        }

        private byte[] applyTransform(TColor[,] _procColor, int[,] _procDouble)
        {
            int calcvalR = 0;
            int calcvalG = 0;
            int calcvalB = 0;
            for (int a = 0; a < 3; a++)
            {
                for (int b = 0; b < 3; b++)
                {
                    calcvalR += _procColor[a, b].r * _procDouble[a, b];
                    calcvalG += _procColor[a, b].g * _procDouble[a, b];
                    calcvalB += _procColor[a, b].b * _procDouble[a, b];
                }
            }

            calcvalR = calcvalR / 3;
            calcvalG = calcvalG / 3;
            calcvalB = calcvalB / 3;

            if (calcvalR < 0)
                calcvalR = 0;
            else if (calcvalR > 255)
                calcvalR = 255;

            if (calcvalG < 0)
                calcvalG = 0;
            else if (calcvalG > 255)
                calcvalG = 255;


            if (calcvalB < 0)
                calcvalB = 0;
            else if (calcvalB > 255)
                calcvalB = 255;

            byte[] retval = new byte[3];
            retval[0] = (byte)calcvalR;
            retval[1] = (byte)calcvalG;
            retval[2] = (byte)calcvalB;
            return retval;
        }


    }
}
