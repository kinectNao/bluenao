using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aldebaran.Proxies;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            TextToSpeechProxy tts = new TextToSpeechProxy("192.168.100.4", 9559);
            tts.say("Koko");
        }
    }
}
