using System.Diagnostics;
using System.Drawing;

namespace AndroidEmulatorHelper.Emulator
{
    public class BlueStacks : EmulatorBase
    {
        private readonly nint _hwnd;

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

        public override nint GetHwnd()
        {
            return _hwnd;
        }

        public override async Task Click(Point position)
        {
            nint hwnd = GetHwnd();
            nint ptr = CalculatePositionValue(position);

            Win32Api.PostMessage(hwnd, (uint)WMessages.WM_ACTIVATE, nint.Zero, nint.Zero);
            Win32Api.PostMessage(hwnd, (int)WMessages.WM_LBUTTONDOWN, nint.Zero, ptr);
            await Task.Delay(5);

            Win32Api.PostMessage(hwnd, (int)WMessages.WM_LBUTTONUP, nint.Zero, ptr);
            await Task.Delay(5);
        }

        private nint FindProcessHwnd()
        {
            nint parentHandle = Win32Api.FindWindow("Qt5154QWindowOwnDCIcon", GetProcessName());
            nint childHandle = Win32Api.FindWindowEx(parentHandle, 0, "Qt5154QWindowIcon", "HD-Player");

            if (childHandle == nint.Zero)
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
