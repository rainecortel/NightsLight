using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NightsLight
{
    static class Program
    {
        private static Thread thread;

        static void Main(string[] args)
        {
            thread = new Thread(() =>
            {
                GameMenu mainPage = new GameMenu();

                // Used for catching the closing event game log.
                Application.ApplicationExit += new EventHandler(mainPage.OnApplicationExit);
                Application.Run(mainPage);
            });
            thread.IsBackground = false;
            thread.Start();

            Console.WriteLine("GAME LOGS");
        }

        public static string GetCurrentTime()
        {
            DateTime time = DateTime.Now;
            string currTime = time.ToString("MM/dd/yyyy HH:mm:ss");
            return currTime;
        }
    }
}