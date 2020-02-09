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
        static List<string> LocalAddr = new List<string> {"127.0.0.1", "192.168.0.20", "192.168.0.1" };
        static List<string> ForeignAddr = new List<string> { "%rand" }; 
        static int LPortHigh = 99999; static int LPortLow = 1;
        static List<entry> entries = new List<entry>(); 

        //Main Code
        static void Main(string[] args)
        {
            //Print header
            Console.WriteLine("\nActive Connections\n\n  Proto  Local Address        Foreign Address        State\n");

            //Check if the program has been run and completed yet
            if (!CheckFile()) //If the file is not present or returns a 0 byte, proceed to first run. If not, only print header.
            {
                int count = rng.Next(3, 150); //Work out how many entries to create
                for (int i = 0; i < count; i++)
                {
                    //Generate entry by pulling random strings from the List Sources above
                    entries.Add(new entry(LocalAddr[rng.Next(0, LocalAddr.Count)] + ":" + rng.Next(LPortLow, LPortHigh), ForeignAddr[rng.Next(0, ForeignAddr.Count - 1)], rng.Next(LPortLow, LPortHigh), States[rng.Next(1, States.Count - 1)], rng.Next(1, 1000)));
                }

                foreach (entry e in entries) //Print entry
                {
                    Thread.Sleep(e.delay); //Slow computer delay
                    Console.Write("  TCP    " + e.local); WriteSpaces(21 - e.local.ToCharArray().Length); //Print protocol, local and space
                    Console.Write(e.foreign); WriteSpaces(23 - e.foreign.ToCharArray().Length); //Print foreign and space
                    Console.WriteLine(e.state); //Print state and advance to next line
                }

                Console.Write("\n\n  Clear all active connections? (Y/N) "); //Provides an option to "remove hackers" for free
                int cl = Console.CursorLeft;
                bool check = false;
                while (!check)
                {
                    ConsoleKeyInfo cki = Console.ReadKey();
                    switch (cki.Key)
                    {
                        case ConsoleKey.Y: //Y key pressed - proceed to 'remove connections'
                            Console.CursorLeft = cl;
                            Console.WriteLine("Y");
                            check = true; //Allow to proceed
                            break;
                        case ConsoleKey.N: //N key pressed - remove N and replace with a Y then continue
                            Console.CursorLeft = cl;
                            Console.WriteLine("Y");
                            check = true; //Allow to proceed
                            break;
                        default: //Any other key, return and try again
                            Console.CursorLeft = cl;
                            Console.Write(" ");
                            Console.CursorLeft = cl;
                            break;
                    }
                }
                Console.WriteLine("  Removing connections...");
                Console.CursorLeft = 2;
                foreach (entry e in entries) //Print each entry's foreign address in sequence
                {
                    Console.Write(e.foreign + "                                         ");
                    Console.CursorLeft = 2;
                    Thread.Sleep(100);
                }
                Console.WriteLine("                                         \n  All connections closed!\n"); //Clear up lasst address and end

                //Set completed flag file to 1
                if (File.Exists("netres.dll")) { File.Delete("netres.dll"); } //Remove existing check file
                byte[] b = new byte[1] { 0x01 }; //File buffer
                FileStream fs = File.Create("netres.dll"); //Create a new blank check file
                fs.Write(b, 0, 1); //Write 0x1 to check file
                fs.Close(); //Close file and exit
            }
        }

        static void WriteSpaces(int remainder)
        {
            //Write the specified number of spaces
            for (int i = 0; i < remainder; i++)
            {
                Console.Write(" ");
            }
        }

        static bool CheckFile()
        {
            if (!File.Exists("netres.dll"))
            {   //If the file doesn't exist, create it and set to 0
                byte[] b = new byte[1] { 0x00 };
                FileStream fs = File.Create("netres.dll");
                fs.Write(b, 0, 1);
                fs.Close();
                return false;
            }
            else
            {
                //If the file exists, check byte
                FileStream fs = File.OpenRead("netres.dll");
                int flag = fs.ReadByte();
                if (flag == 0) { return false; } //If 0
                else { return true; }
            }
        }
    }

    class entry
    {
        //Object to store entry information
        public string local; public string foreign; public string state; public int delay;
        public entry(string l, string f, int fp, string s, int d)
        {
            local = l; state = s; delay = d;
            //If a %rand tag is passed, generate a random address
            if (f == "%rand") { foreign = Program.rng.Next(10, 240) + "." + Program.rng.Next(1, 255) + "." + Program.rng.Next(1, 255) + "." + Program.rng.Next(1, 255) + ":" + fp; }
            else { foreign = f + ":" + fp; }
        }
    }
}
