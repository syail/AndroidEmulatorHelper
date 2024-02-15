using System.Drawing;
using System.Runtime.InteropServices;

namespace AndroidEmulatorHelper
{
    internal class Win32Api
    {
        [DllImport("user32", EntryPoint = "FindWindow", CharSet = CharSet.Unicode)]
        internal static extern IntPtr FindWindow(string className, string windowTitle);

        [DllImport("user32", CharSet = CharSet.Unicode)]
        internal static extern IntPtr FindWindowEx(IntPtr hWnd1, int hWnd2, string className, string caption);

        [DllImport("user32")]
        internal static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcblt, int nFlags);

        [DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern int GetWindowRect(IntPtr hwnd, out Rectangle rect);
    }
}
