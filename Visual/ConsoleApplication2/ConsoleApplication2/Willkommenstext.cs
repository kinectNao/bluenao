using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aldebaran.Proxies;
using System.Collections;

namespace ConsoleApplication2
{
    class Willkommenstext
    {
        public static string ip = "127.0.0.1";
        public static TextToSpeechProxy tts = new TextToSpeechProxy(ip, 9559);

        public void sayName()
        {
            tts.say("Hallo ich bin Cocco");
            BatteryProxy battery = new BatteryProxy(ip, 9559);
            tts.say("Meine Batterie ist zu:" + battery.getBatteryCharge() + "Prozent geladen");
        }
    }
}
