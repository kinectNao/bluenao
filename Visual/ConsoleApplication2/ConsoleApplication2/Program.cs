using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aldebaran.Proxies;
using System.Collections;
using System.Windows;
using System.Windows.Input;

namespace ConsoleApplication2
{
    class Program
    {
        public static string ip = "127.0.0.1";
        public static MotionProxy motion = new MotionProxy(ip, 9559);
        public static TextToSpeechProxy tts = new TextToSpeechProxy(ip, 9559);


        static void Main(string[] args)
        {
            //Willkommenstext willkommenstext = new Willkommenstext();
            //willkommenstext.sayName();
            BewegunglinkerArm();
            //gespeicherteBewegung();

            /*
            Schritt();
            gespeicherteBewegung();
            BewegunglinkerArm();
            StandUpVersuch();
            System.Threading.Thread.Sleep(10000);
            */
        }

        private static void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        
        

        public static void StandUpVersuch()
        {
            List<string> Body = new List<string>();
            Body.Add("LArm");
            Body.Add("RArm");
            Body.Add("RLeg");
            Body.Add("LLeg");
            Body.Add("Head");
            Body.Add("Torso");

            ArrayList gesamt = new ArrayList();
            object pathLArm = new object[] { 0.04206285f, 0.09271394f, 0.242649f, -1.150778f, 1.053954f, -0.08910272f };
            object pathRArm = new object[] { 0.04476468f, -0.08921376f, 0.2481826f, 1.30114f, 1.049861f, 0.05752527f };
            object pathRLeg = new object[] { -0.003651698f, -0.06664718f, -0.00000005724367f, -0.0000000004881115f, -0.00000001035981f, -0.1347331f };
            object pathLLeg = new object[] { -0.005333865f, 0.06653368f, -0.001843915f, -0.03139023f, 0.004599969f, 0.1345889f };
            object pathHead = new object[] { -0.01613378f, 0.00377181f, 0.4573836f, -0.01960159f, -0.1915345f, -0.01829854f };
            object pathTorso = new object[] { -0.01408146f, 0.001292234f, 0.3309246f, -0.01954962f, -0.01629153f, -0.003260953f };

            gesamt.Add(pathLArm);
            gesamt.Add(pathRArm);
            gesamt.Add(pathRLeg);
            gesamt.Add(pathLLeg);
            gesamt.Add(pathHead);
            gesamt.Add(pathTorso);

            object axismasks = new object[] { 7, 7, 7, 7, 7, 7 };
            object times = new object[] { 3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 3.0f };

            motion.positionInterpolations(Body, 2, gesamt, axismasks, times, true);
        }
        
        public static void BewegunglinkerArm()
        {
            ArrayList list4 = new ArrayList();
            object path2 = new object[] { 0.0707f, 0.0053f, 0.3803f, -1.1620f, 0.3273f, -1.4607f };
            list4.Add(path2);
            motion.positionInterpolation("LArm", 2, list4, 7, 3.0f, true);
            motion.openHand("LHand");
            ArrayList list3 = new ArrayList();
            object path = new object[] { 0.1270f, 0.2020f, 0.5253f, -1.6326f, -0.5186f, 0.7099f };
            list3.Add(path);
            motion.positionInterpolation("LArm", 2, list3, 7, 3.0f, true);
        }
                
        public static void gespeicherteBewegung()
        {
            try
            {
                BehaviorManagerProxy beh = new BehaviorManagerProxy(ip, 54010);
                beh.preloadBehavior("StandUp");     //Behavior muss vorher in Tool gespeichert werden
                beh.runBehavior("StandUp");         //Behavior muss vorher in Tool gespeichert werden

            }
            catch (Exception e)
            {
                Console.WriteLine("blabla " + e.Message);
            }
        }
        
        public static void Schritt()
        {

            List<string> LegName = new List<string>();
            LegName.Add("RLeg");
            ArrayList list = new ArrayList(); 
            object footSteps = new object[] { 0.1f, 0.2f, 0.3f }; 
            list.Add(footSteps);
            List<float> TimeList = new List<float>();
            TimeList.Add(1);
            bool clearexisting = false;

            motion.setFootSteps(LegName, list, TimeList, clearexisting);
        }
    }
}
