﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NetStat
{
    class Program
    {
        //Core RNG
        static Random rng = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);

        //List Sources
        static List<string> States = new List<string> { "ESTABLISHED", "CLOSE_WAIT", "TIME_WAIT", "OPEN_WAIT", "SPIN_WAIT", "TICK_WAIT", "LAG_WAIT" };
        static List<string> LocalAddr = new List<string> {"127.0.0.1","192.168.0.20" };
        static List<string> ForeignAddr = new List<string> //Multiple %rand to increase chances
        { "choice", "uksm", "wasd", "egx", "i66", "loki6444266", "z115m", "br", "phyra76534",
            "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand",
            "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand" };
        static int LPortHigh = 99999; static int LPortLow = 1;
        static List<entry> entries = new List<entry>(); 

        //Main Code
        static void Main(string[] args)
        {
            Console.WriteLine("\nActive Connections\n\n  Proto  Local Address          Foreign Address        State");

            int count = rng.Next(3, 150);
            for (int i = 0; i < count; i++)
            {
                entries.Add(new entry(LocalAddr[rng.Next(1, LocalAddr.Count - 1)] + ":" + rng.Next(LPortLow, LPortHigh), ForeignAddr[rng.Next(1, ForeignAddr.Count - 1)] + ":" + rng.Next(LPortLow, LPortHigh), States[rng.Next(1, States.Count - 1)], rng.Next(1, 1000)));
            }

            foreach (entry e in entries)
            {
                Thread.Sleep(e.delay);
                Console.WriteLine("  TCP    " + e.local + "  " + e.foreign + "  " + e.state);
            }

            Console.WriteLine("\n\n  Clear all active connections? (Y/N)");
            int cl = Console.CursorLeft;
            bool check = false;
            while (!check)
            {
                ConsoleKeyInfo cki = Console.ReadKey();
                switch (cki.Key)
                {
                    case ConsoleKey.Y:
                        Console.CursorLeft = cl;
                        Console.WriteLine("Y");
                        check = true;
                        break;
                    case ConsoleKey.N:
                        Console.CursorLeft = cl;
                        Console.WriteLine("Y");
                        check = true;
                        break;
                    default:
                        Console.CursorLeft = cl;
                        Console.Write(" ");
                        Console.CursorLeft = cl;
                        break;
                }
            }
            Console.WriteLine("Removing connections...");
            foreach (entry e in entries)
            {
                Console.Write(e.foreign+"                                         ");
                Thread.Sleep(500);
            }
            Console.WriteLine("\n\nAll connections closed!\n");
        }
    }

    class entry
    {
        public string local; public string foreign; public string state; public int delay;
        public entry(string l, string f, string s, int d)
        {
            local = l; foreign = f; state = s; delay = d;
        }
    }
}
