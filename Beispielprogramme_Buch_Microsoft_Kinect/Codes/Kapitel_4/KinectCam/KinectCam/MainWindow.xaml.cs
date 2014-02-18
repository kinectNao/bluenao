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
        public WriteableBitmap myBitmap;
        Manager myBGW;
        public byte[] myArray;
        public byte[] myCacheArray;

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



            if (null != mySensor)
            {
                mySensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                
                myBGW = new Manager(this);
                myArray = new byte[this.mySensor.ColorStream.FramePixelDataLength];
                myCacheArray = new byte[this.mySensor.ColorStream.FramePixelDataLength];
                myBGW.myArray = new byte[this.mySensor.ColorStream.FramePixelDataLength];
                myBGW.myCacheArray = new byte[this.mySensor.ColorStream.FramePixelDataLength];
                myBGW.mySensorFrameWidth = mySensor.ColorStream.FrameWidth;

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
                        if (myBGW.IsBusy == false && myBGW.myIsWorkingFlag==false)
                        {
                            myBGW.myArray = (byte[])myArray.Clone();
                            myBGW.myCacheArray = (byte[])myCacheArray.Clone();
                            myBGW.RunWorkerAsync();
                        }
                    }
                    else
                    {
                        myBitmap.WritePixels(
                            new Int32Rect(0, 0, myBitmap.PixelWidth, myBitmap.PixelHeight),
                            myArray,
                            myBitmap.PixelWidth * sizeof(int),
                            0);

                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mySensor.Stop();
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
