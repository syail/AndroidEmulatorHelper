using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AndroidEmulatorHelper
{
    public abstract class IAndroidEmulator
    {
        public Process EmulatorProcess { get; }

        public IAndroidEmulator(Process emulator)
        {
            EmulatorProcess = emulator;
        }

        public abstract IntPtr GetScreenHwnd();
        public abstract string GetProcessName();

        public Bitmap CaptureScreen()
        {
            IntPtr hwnd = GetScreenHwnd();
            using Graphics gdata = Graphics.FromHwnd(hwnd);

            Rectangle rect = Rectangle.Round(gdata.VisibleClipBounds);
            Bitmap bmp = new(rect.Width, rect.Height);

            using Graphics g = Graphics.FromImage(bmp);

            IntPtr hdc = g.GetHdc();

            PrintWindow(hwnd, hdc, 0x2);
            g.ReleaseHdc(hdc);

            return bmp;
        }

        public Size GetScreenSize()
        {
            _ = GetWindowRect(GetScreenHwnd(), out Rectangle rect);

            return new Size(rect.Width - rect.X, rect.Height - rect.Y);
        }

        public async Task Click(Point position)
        {
            IntPtr ptr = CalculatePositionValue(position);

            SendMessage(GetScreenHwnd(), (int)WMessages.WM_LBUTTONDOWN, IntPtr.Zero, ptr);
            await Task.Delay(5);

            SendMessage(GetScreenHwnd(), (int)WMessages.WM_LBUTTONUP, IntPtr.Zero, ptr);
            await Task.Delay(5);
        }

        public void SendChar(char key)
        {
            PostMessage(GetScreenHwnd(), (int)WMessages.WM_CHAR, (IntPtr)key, IntPtr.Zero);
        }

        public async Task SendString(string str)
        {
            foreach (char i in str)
            {
                SendChar(i);
                await Task.Delay(50);
            }
        }

        private static IntPtr CalculatePositionValue(Point position)
        {
            return new(position.X | (position.Y << 16));
        }

        [DllImport("user32")]
        private static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcblt, int nFlags);

        [DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out Rectangle rect);
    }

    public enum WMessages : int
    {
        WM_MOUSEMOVE = 0x200,
        WM_LBUTTONDOWN = 0x201, //Left mousebutton down
        WM_LBUTTONUP = 0x202,  //Left mousebutton up
        WM_LBUTTONDBLCLK = 0x203, //Left mousebutton doubleclick
        WM_RBUTTONDOWN = 0x204, //Right mousebutton down
        WM_RBUTTONUP = 0x205,   //Right mousebutton up
        WM_RBUTTONDBLCLK = 0x206, //Right mousebutton doubleclick
        WM_KEYDOWN = 0x100,  //Key down
        WM_KEYUP = 0x101,   //Key up
        WM_SYSKEYDOWN = 0x104,
        WM_SYSKEYUP = 0x105,
        WM_CHAR = 0x102,     //char
        WM_COMMAND = 0x111
    }
}
