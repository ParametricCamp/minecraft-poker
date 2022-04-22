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

namespace minecraft_poker
{
    class Program
    {

        // import the function in your class
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        static void Main(string[] args)
        {

            Process[] ps = Process.GetProcessesByName("javaw");
            //Console.WriteLine(ps.Length);
            //foreach (var p in ps)
            //{
            //    Console.WriteLine(p);
            //}
            Process minecraftProcess = ps.FirstOrDefault();
            Console.WriteLine(minecraftProcess);

            if (minecraftProcess != null)
            {
                Console.WriteLine("Bringing Minecraft on focus...");
                IntPtr h = minecraftProcess.MainWindowHandle;
                SetForegroundWindow(h);

                //Thread.Sleep(3000);
                //Console.WriteLine("Sending keystroke");
                //SendKeys.SendWait("W");

                Thread.Sleep(1000);
                Console.WriteLine("Getting Minecraft out of Game Menu");
                InputSimulator isim = new InputSimulator();
                isim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.ESCAPE);

                Console.WriteLine("Starting jumping routine");
                for (int i = 0; i < 10; i++)
                {
                    //Console.WriteLine("Sending jump");
                    //isim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SPACE);
                    //Thread.Sleep(50);
                    //isim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SPACE);

                    Thread.Sleep(2000);
                    Console.WriteLine("Place a torch");
                    isim.Mouse.RightButtonDown();
                    Thread.Sleep(50);
                    isim.Mouse.RightButtonUp();

                    Thread.Sleep(2000);
                    Console.WriteLine("Remove torch");
                    isim.Mouse.LeftButtonDown();
                    Thread.Sleep(50);
                    isim.Mouse.LeftButtonUp();

                }

            }



            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();
        }
    }
}
