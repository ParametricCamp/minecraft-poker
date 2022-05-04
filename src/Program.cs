using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using WindowsInput;
using WindowsInput.Native;

namespace minecraft_poker
{
    class Program
    {
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        /// <summary>
        /// Threaded Ticker function.
        /// </summary>
        public static void Poker()
        {
            // Catch and focus the app
            // Inspired by https://stackoverflow.com/a/15292428/1934487
            Process[] ps = Process.GetProcessesByName("javaw");
            Process p = ps?.FirstOrDefault();
            if (p != null)
            {
                Console.WriteLine("Found a running Minecraft");

                Console.WriteLine("Bringing the app on focus...");
                IntPtr h = p.MainWindowHandle;
                SetForegroundWindow(h);

                // Alternative to SendWait()
                // Found on https://stackoverflow.com/a/55251816/1934487
                InputSimulator isim = new InputSimulator();

                Thread.Sleep(2000);
                Console.WriteLine("Turning Game Menu off...");
                isim.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);

                double delta;
                int time;
                while (true)
                {
                    delta = RandomDelta * rnd.NextDouble();
                    time = Timeout + (int)(delta - 0.5 * RandomDelta);

                    Thread.Sleep(time);
                    Console.WriteLine("Poke at " + DateTime.Now.ToString());

                    isim.Mouse.RightButtonDown();
                    Thread.Sleep(ClickLength);
                    isim.Mouse.RightButtonUp();

                    delta = RandomDelta * rnd.NextDouble();
                    time = Timeout + (int)(delta - 0.5 * RandomDelta);

                    Thread.Sleep(time);
                    Console.WriteLine("Poke at " + DateTime.Now.ToString());
                    isim.Mouse.LeftButtonDown();
                    Thread.Sleep(ClickLength);
                    isim.Mouse.LeftButtonUp();
                }
            }

        }

        static int Timeout = 60000;
        static int RandomDelta = 10000;
        static int ClickLength = 50;
        static Random rnd = new Random();

        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Minecraft Poker! An AFK app to prevent your Minecraft Realms from idling...");

            Console.WriteLine($"Enter time between ticks in seconds (default {Timeout / 1000}): ");
            string timeoutStr = Console.ReadLine();
            try
            {
                if (!timeoutStr.Equals(""))
                {
                    Timeout = Convert.ToInt32(timeoutStr) * 1000;
                }
            }
            catch
            {
                Console.WriteLine("Wrong input, please enter only numerical values");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Enter random variation in seconds ({RandomDelta / 1000}): ");
            string randomDeltaStr = Console.ReadLine();
            try
            {
                if (!randomDeltaStr.Equals(""))
                {
                    RandomDelta = Convert.ToInt32(randomDeltaStr) * 1000;
                }
            }
            catch
            {
                Console.WriteLine("Wrong input, please enter only numerical values");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Enter click length in milliseconds ({ClickLength}): ");
            string clickLenStr = Console.ReadLine();
            try
            {
                if (!clickLenStr.Equals(""))
                {
                    ClickLength = Convert.ToInt32(clickLenStr);
                }
            }
            catch
            {
                Console.WriteLine("Wrong input, please enter only numerical values");
                Console.ReadKey();
                return;
            }


            // Threaded
            ThreadStart threadRef = new ThreadStart(Poker);
            Thread tickerThread = new Thread(threadRef);
            tickerThread.Start();

            Console.WriteLine("Press any key to exit the Poker...");
            Console.ReadKey();

            tickerThread.Abort();
            Thread.Sleep(1000);

            Console.ReadKey();

        }
    }
}
