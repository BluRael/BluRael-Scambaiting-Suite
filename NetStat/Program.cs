using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace NetStat
{
    class Program
    {
        //Core RNG
        public static Random rng = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);

        //List Sources
        static List<string> States = new List<string> //Multiple 'ESTABLISHED' to increase chances
        { "ESTABLISHED", "ESTABLISHED", "ESTABLISHED", "ESTABLISHED", "ESTABLISHED", "ESTABLISHED", "ESTABLISHED", "ESTABLISHED",
            "ESTABLISHED", "ESTABLISHED", "ESTABLISHED", "ESTABLISHED", "ESTABLISHED", "ESTABLISHED", "ESTABLISHED", "ESTABLISHED",
            "CLOSE_WAIT", "TIME_WAIT", "OPEN_WAIT", "SPIN_WAIT", "TICK_WAIT", "LAG_WAIT" };
        static List<string> LocalAddr = new List<string> {"127.0.0.1","192.168.0.20" };
        static List<string> ForeignAddr = new List<string> //Multiple '%rand' to increase chances
        { "choice", "uksm", "wasd", "egx", "i66", "loki6444266", "z115m", "br", "phyra76534",
            "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand",
            "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand", "%rand" };
        static int LPortHigh = 99999; static int LPortLow = 1;
        static List<entry> entries = new List<entry>(); 

        //Main Code
        static void Main(string[] args)
        {
            Console.WriteLine("\nActive Connections\n\n  Proto  Local Address          Foreign Address        State\n");

            if (!CheckFile())
            {
                int count = rng.Next(3, 150);
                for (int i = 0; i < count; i++)
                {
                    entries.Add(new entry(LocalAddr[rng.Next(0, LocalAddr.Count - 1)] + ":" + rng.Next(LPortLow, LPortHigh), ForeignAddr[rng.Next(0, ForeignAddr.Count - 1)], rng.Next(LPortLow, LPortHigh), States[rng.Next(1, States.Count - 1)], rng.Next(1, 1000)));
                }

                foreach (entry e in entries)
                {
                    Thread.Sleep(e.delay);
                    Console.Write("  TCP    " + e.local); WriteSpaces(23 - e.local.ToCharArray().Length);
                    Console.Write(e.foreign); WriteSpaces(23 - e.foreign.ToCharArray().Length);
                    Console.WriteLine(e.state);
                }

                Console.Write("\n\n  Clear all active connections? (Y/N) ");
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
                Console.WriteLine("  Removing connections...");
                Console.CursorLeft = 2;
                foreach (entry e in entries)
                {
                    Console.Write(e.foreign + "                                         ");
                    Console.CursorLeft = 2;
                    Thread.Sleep(100);
                }
                Console.WriteLine("\n\n  All connections closed!\n");

                //Set completed flag
                if (File.Exists("netres.dll")) { File.Delete("netres.dll"); }
                byte[] b = new byte[1] { 0x01 };
                FileStream fs = File.Create("netres.dll");
                fs.Write(b, 0, 1);
                fs.Close();
            }
        }

        static void WriteSpaces(int remainder)
        {
            for (int i = 0; i < remainder; i++)
            {
                Console.Write(" ");
            }
        }

        static bool CheckFile()
        {
            if (!File.Exists("netres.dll"))
            {
                byte[] b = new byte[1] { 0x00 };
                FileStream fs = File.Create("netres.dll");
                fs.Write(b, 0, 1);
                fs.Close();
                return false;
            }
            else
            {
                FileStream fs = File.OpenRead("netres.dll");
                int flag = fs.ReadByte();
                if (flag == 0) { return false; }
                else { return true; }
            }
        }
    }

    class entry
    {
        public string local; public string foreign; public string state; public int delay;
        public entry(string l, string f, int fp, string s, int d)
        {
            local = l; state = s; delay = d;
            if (f == "%rand") { foreign = Program.rng.Next(10, 240) + "." + Program.rng.Next(1, 255) + "." + Program.rng.Next(1, 255) + "." + Program.rng.Next(1, 255) + ":" + fp; }
            else { foreign = f + ":" + fp; }
        }
    }
}
