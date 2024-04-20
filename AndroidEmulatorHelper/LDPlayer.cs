using System.Diagnostics;
using System.Drawing;

namespace AndroidEmulatorHelper
{
    public class LDPlayer : AndroidEmulatorBase
    {
        private readonly IntPtr _hwnd;

        public LDPlayer(Process ldProc) : base(ldProc)
        {
            _hwnd = FindProcessHwnd();
        }

        public override string GetProcessName()
        {
            return BaseProcess.MainWindowTitle;
        }

        public static LDPlayer[] GetList()
        {
            Process[] processes = Process.GetProcessesByName("dnplayer");

            return processes.Select(x => new LDPlayer(x)).ToArray();
        }

        public override IntPtr GetHwnd()
        {
            return _hwnd;
        }

        private IntPtr FindProcessHwnd()
        {
            IntPtr mainFrame = Win32Api.FindWindow("LDPlayerMainFrame", GetProcessName());
            IntPtr screenRenderer = Win32Api.FindWindowEx(mainFrame, 0, "RenderWindow", "TheRender");

            return screenRenderer;
        }

        public override string ToString()
        {
            Size screenSize = GetScreenSize();
            return $"[LDPlayer] {GetProcessName()} ({screenSize.Width}x{screenSize.Height})";
        }
    }
}
