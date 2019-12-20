using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Reach
{
    class Program
    {
        
        static void Main(string[] args)
        {

            ThreadStart ThreadProc = new ThreadStart(Listener.Listen);
            Thread t = new Thread(ThreadProc);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            

            //bmp.Save(@"C:\Users\JM\Desktop\test.bmp");
        }
        
    }

    class Listener
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern int SendInput(int nInputs, ref INPUT pInputs, int cbSize);
        public struct INPUT
        {
            public int type;
            public MOUSEINPUT mi;

        }
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public int dwExtraInfo;
        }

        static Random shoot = new Random();
        static Random click = new Random();
        static Random preshoot = new Random();
        public static void Listen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            System.Drawing.Point center = new System.Drawing.Point(Convert.ToInt32(screenWidth / 2), Convert.ToInt32(screenHeight / 2));

            Rectangle cRect = new Rectangle(center.X - 10, center.Y - 10, 20, 20);
            Color red1 = Color.FromArgb(195, 35, 30);
            Color red2 = Color.FromArgb(204, 56, 44);
            Color red3 = Color.FromArgb(178, 28, 25);

            //Bitmap bmp = new Bitmap(20, 20); //center -9
            Bitmap bmp = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp); // check pixels(8/9/10,1)

            while (true)
            {
                System.Threading.Thread.Sleep(50);
                string window = GetActiveWindowTitle();
                if (window != null)
                {
                    while (window[0] == 'H' && window[1] == 'a')
                    {
                        
                       // Color c2 = bmp.GetPixel(9, 1);
                        //Color c3 = bmp.GetPixel(10, 1);
                        while ((Keyboard.GetKeyStates(Key.LeftAlt) & KeyStates.Down) > 0)
                        {
                            //Console.WriteLine("alt down");
                            g.CopyFromScreen(cRect.Location.X + 8, cRect.Location.Y + 1, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
                            //Color c1 = bmp.GetPixel(8, 1);
                            // 
                            // 
                            Color c1 = bmp.GetPixel(0, 0);
                            if ((c1.R > 170 && c1.R < 220)) //|| (c2.R > 170 && c2.R < 220) || (c3.R > 170 && c3.R < 220))
                            {
                                //Thread.Sleep(2);
                                Click();
                                //Console.WriteLine("boom");
                                Thread.Sleep(shoot.Next(111, 196));
                               // Console.WriteLine("sleep done");
                            }
                        }
                    }
                }
            }
        }

        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        public static void Click()
        {
            INPUT Input =  new INPUT();
            // left down 
            Input.type = 0; //mouse
            Input.mi.dwFlags = 0x0002;
            SendInput(1, ref Input, Marshal.SizeOf(Input));
            Thread.Sleep(click.Next(18, 37));
            // left up
            INPUT input2 = new INPUT();
            input2.type = 0;
            input2.mi.dwFlags = 0x0004;
            SendInput(1, ref input2, Marshal.SizeOf(input2));
        }
    }
}