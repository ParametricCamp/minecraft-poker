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

        
        static int Timeout;
        static int RandomDelta;
        static int ClickLength;
        static readonly Random rnd = new Random();

        static int ClickerMode = 1;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Minecraft Poker!");
            Console.WriteLine("A mighy clicker app for Minecraft AFK");
            Console.WriteLine("");

            // Choose clicker mode
            Console.WriteLine("Which clicker mode do you need?");
            Console.WriteLine("    (1) Left-Right clicker, useful to prevent your Minecraft Realms from idling...");
            Console.WriteLine("    (2) Left clicker, useful for XP farms...");
            Console.Write("Enter clicker mode: ");

            string clickerModeStr = Console.ReadLine();
            try
            {
                if (!clickerModeStr.Equals(""))
                {
                    ClickerMode = Convert.ToInt32(clickerModeStr);
                }
            }
            catch
            {
                Console.WriteLine("Wrong input, please enter only numerical values");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("");

            // The threadedFunction
            Action threadedFunction = null;

            switch (ClickerMode)
            {
                // LEFT-RIGHT CLICK
                case 1:
                    // Case specific defaults
                    Timeout = 60000;
                    RandomDelta = 10000;
                    ClickLength = 50;

                    threadedFunction = LeftRightClicker;
                    break;

                // LEFT-CLICK only
                case 2:
                    // Case specific defaults
                    Timeout = 2000;
                    RandomDelta = 500;
                    ClickLength = 50;

                    threadedFunction = LeftClicker;
                    break;

                default:
                    Console.WriteLine("Unknown clicker mode, exiting...");
                    Console.ReadKey();
                    return;
            }

            Console.WriteLine($"Enter time between clicks in seconds (default {Timeout / 1000}): ");
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
            Console.WriteLine("");


            // Threaded
            ThreadStart threadRef = new ThreadStart(threadedFunction);
            Thread tickerThread = new Thread(threadRef);
            tickerThread.Start();

            // Hold the app running until user presses any key
            //Console.WriteLine("Press any key to exit the Poker...");
            Console.ReadKey();

            Console.WriteLine("Exiting poker...");
            tickerThread.Abort();
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Threaded Ticker function.
        /// </summary>
        public static void LeftRightClicker()
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
                    if (time < 100) time = 100;
                    Thread.Sleep(time);

                    Console.WriteLine("Left click at " + DateTime.Now.ToString());
                    isim.Mouse.LeftButtonDown();
                    Thread.Sleep(ClickLength);
                    isim.Mouse.LeftButtonUp();

                    delta = RandomDelta * rnd.NextDouble();
                    time = Timeout + (int)(delta - 0.5 * RandomDelta);
                    Thread.Sleep(time);

                    Console.WriteLine("Left click at " + DateTime.Now.ToString());
                    isim.Mouse.RightButtonDown();
                    Thread.Sleep(ClickLength);
                    isim.Mouse.RightButtonUp();
                }
            }
        }

        /// <summary>
        /// Threaded Ticker function.
        /// </summary>
        public static void LeftClicker()
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

                    Console.WriteLine("Left click at " + DateTime.Now.ToString());
                    isim.Mouse.LeftButtonDown();
                    Thread.Sleep(ClickLength);
                    isim.Mouse.LeftButtonUp();
                }
            }

        }
    }
}
