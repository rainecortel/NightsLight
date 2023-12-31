﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NightsLight
{
    static class Program
    {
        private static Thread thread;
        private static GameMenu gameMenu;
        public static string currentDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

        static void Main(string[] args)
        {
            thread = new Thread(() =>
            {
                gameMenu = new GameMenu();

                // Used for catching the closing event game log.
                Application.ApplicationExit += new EventHandler(gameMenu.OnApplicationExit);
                Application.Run(gameMenu);
            });
            thread.IsBackground = false;
            thread.Start();

            Console.WriteLine("GAME LOGS");
        }

        public static GameMenu getGameMenuForm()
        {
            return gameMenu;
        }

        public static string GetCurrentTime()
        {
            DateTime time = DateTime.Now;
            string currTime = time.ToString("MM/dd/yyyy HH:mm:ss");
            return currTime;
        }
    }
}