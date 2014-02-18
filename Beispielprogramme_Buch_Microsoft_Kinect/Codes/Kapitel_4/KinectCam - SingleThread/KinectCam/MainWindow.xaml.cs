using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using System.IO;
using System.Threading;

namespace KinectCam
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor mySensor;
        WriteableBitmap myBitmap;
        byte[] myArray;
        byte[] myCacheArray;

        int[,] cMatrix = new int[3, 3];
        TColor[,] aColor = new TColor[3, 3];

        public MainWindow()
        {
            InitializeComponent();

            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    mySensor = potentialSensor;
                    break;
                }
            }

            cMatrix[0, 0] = 0;
            cMatrix[0, 1] = -2;
            cMatrix[0, 2] = 0;
            cMatrix[1, 0] = -2;
            cMatrix[1, 1] = 11;
            cMatrix[1, 2] = -2;
            cMatrix[2, 0] = 0;
            cMatrix[2, 1] = -2;
            cMatrix[2, 2] = 0;

            if (null != mySensor)
            {
                mySensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                myArray = new byte[this.mySensor.ColorStream.FramePixelDataLength];
                myCacheArray = new byte[this.mySensor.ColorStream.FramePixelDataLength];
                myBitmap = new WriteableBitmap(this.mySensor.ColorStream.FrameWidth, this.mySensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);
                image1.Source = myBitmap;
                mySensor.ColorFrameReady += this.SensorColorFrameReady;
                try
                {
                    this.mySensor.Start();
                }
                catch (IOException)
                {
                    this.mySensor = null;
                }
            }
        }

        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    colorFrame.CopyPixelDataTo(myArray);
                    if (ChkBoost.IsChecked == true)
                    {
                        processBoost();
                    }
                    if (ChkSharpen.IsChecked == true)
                    {
                        colorFrame.CopyPixelDataTo(myCacheArray);
                        processSharpen();
                    }
                    myBitmap.WritePixels(
                        new Int32Rect(0, 0, myBitmap.PixelWidth, myBitmap.PixelHeight),
                        myArray,
                        myBitmap.PixelWidth * sizeof(int),
                        0);


                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mySensor.Stop();
        }

        private void processSharpen()
        {
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

            retval.r=myCacheArray[anIndex + 2]; //r 
            retval.g= myCacheArray[anIndex + 1];
            retval.b= myCacheArray[anIndex];
            
            return retval;
        }


        private void SetAPixel(int _x, int _y, byte _r, byte _g, byte _b)
        {
            long anIndex = _y * mySensor.ColorStream.FrameWidth + _x;
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

        private void processBoost()
        {
            /* NAIVE LÖSUNG, siehe Buch
            byte minr = 255;
            byte ming = 255;
            byte minb = 255;
            byte maxr = 0;
            byte maxg = 0;
            byte maxb = 0;

            for (int i = 0; i < myArray.Length; i += 4)
            {
                minb = myArray[i] < minb ? myArray[i] : minb;
                ming = myArray[i+1] < ming ? myArray[i+1] : ming;
                minr = myArray[i+2] < minr ? myArray[i+2] : minr;
                maxb = myArray[i] > maxb ? myArray[i] : maxb;
                maxg = myArray[i + 1] > maxg ? myArray[i + 1] : maxg;
                maxr = myArray[i + 2] > maxr ? myArray[i + 2] : maxr;
            }
              
            byte lowest = minr;
            if (ming < lowest) lowest = ming;
            if (minb < lowest) lowest = minb;

            byte highest = maxr;
            if (maxg > highest) highest = maxg;
            if (maxb > highest) highest = maxb;
             
            for (int i = 0; i < myArray.Length; i += 4)
            {
                myArray[i] = (byte)((myArray[i] - lowest) * ((float)255 / (highest-lowest)));
                myArray[i + 1] = (byte)((myArray[i + 1] - lowest) * ((float)255 / (highest - lowest)));
                myArray[i + 2] = (byte)((myArray[i + 2] - lowest) * ((float)255 / (highest - lowest)));
               
            }

            */

            int[] myRHisto = new int[256];
            int[] myGHisto = new int[256];
            int[] myBHisto = new int[256];

            for (int i = 0; i < myArray.Length; i += 4)
            {
                myBHisto[myArray[i]]++;
                myGHisto[myArray[i+1]]++;
                myRHisto[myArray[i+2]]++;
            }

            byte minr = find5PercentHurdle(myRHisto);
            byte ming = find5PercentHurdle(myGHisto);
            byte minb = find5PercentHurdle(myBHisto);
            byte maxr = find95PercentHurdle(myRHisto);
            byte maxg = find95PercentHurdle(myGHisto);
            byte maxb = find95PercentHurdle(myBHisto);

            byte lowest = minr;
            if (ming < lowest) lowest = ming;
            if (minb < lowest) lowest = minb;

            byte highest = maxr;
            if (maxg > highest) highest = maxg;
            if (maxb > highest) highest = maxb;

            for (int i = 0; i < myArray.Length; i += 4)
            {
                myArray[i] = clampandInt((myArray[i] - lowest) * ((float)255 / (highest-lowest)));
                myArray[i + 1] = clampandInt((myArray[i + 1] - lowest) * ((float)255 / (highest - lowest)));
                myArray[i + 2] = clampandInt((myArray[i + 2] - lowest) * ((float)255 / (highest - lowest)));
               
            }


        }

        private byte clampandInt(float _val)
        {
            if (_val < 0)
            {
                return 0;
            }
            else if (_val > 255)
            {
                return 255;
            }
            else
            {
                return (byte)_val;
            }
        }

        private byte find5PercentHurdle(int[] _array)
        {
            long totalsum = 0;
            for (int i = 0; i < _array.Length; i++)
            {
                totalsum += _array[i];
            }

            long countsum = 0;
            for (int i = 0; i < _array.Length; i++)
            {
                countsum += _array[i];
                if(countsum>=totalsum*0.05)
                    return (byte) i;
            }

            return 0;
        }

        private byte find95PercentHurdle(int[] _array)
        {
            long totalsum = 0;
            for (int i = 0; i < _array.Length; i++)
            {
                totalsum += _array[i];
            }

            long countsum = 0;
            for (int i = 0; i < _array.Length; i++)
            {
                countsum += _array[i];
                if (countsum >= totalsum * 0.95)
                    return (byte)i;
            }

            return 0;
        }
    }
}
