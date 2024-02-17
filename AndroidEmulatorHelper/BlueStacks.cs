using System.Diagnostics;
using System.Drawing;

namespace AndroidEmulatorHelper
{
    public class BlueStacks : AndroidEmulator
    {
        private readonly IntPtr _hwnd;

        public BlueStacks(Process ldProc) : base(ldProc)
        {
            _hwnd = FindProcessHwnd();
        }

        public override string GetProcessName()
        {
            return BaseProcess.MainWindowTitle;
        }

        public static BlueStacks[] GetList()
        {
            Process[] processes = Process.GetProcessesByName("HD-Player");

            return processes.Select(x => new BlueStacks(x)).ToArray();
        }

        public override IntPtr GetHwnd()
        {
            return _hwnd;
        }

        public override async Task Click(Point position)
        {
            IntPtr hwnd = GetHwnd();
            IntPtr ptr = CalculatePositionValue(position);

            Win32Api.PostMessage(hwnd, (uint)WMessages.WM_ACTIVATE, IntPtr.Zero, IntPtr.Zero);
            Win32Api.PostMessage(hwnd, (int)WMessages.WM_LBUTTONDOWN, IntPtr.Zero, ptr);
            await Task.Delay(5);

            Win32Api.PostMessage(hwnd, (int)WMessages.WM_LBUTTONUP, IntPtr.Zero, ptr);
            await Task.Delay(5);
        }

        private IntPtr FindProcessHwnd()
        {
            IntPtr parentHandle = Win32Api.FindWindow("Qt5154QWindowOwnDCIcon", GetProcessName());
            IntPtr childHandle = Win32Api.FindWindowEx(parentHandle, 0, "Qt5154QWindowIcon", "HD-Player");

            if (childHandle == IntPtr.Zero)
            {
                throw new Exception("Cannot find screen handle.");
            }
            return childHandle;
        }

        public override string ToString()
        {
            Size screenSize = GetScreenSize();
            return $"[BlueStacks] {GetProcessName()} ({screenSize.Width}x{screenSize.Height})";
        }
    }
}
