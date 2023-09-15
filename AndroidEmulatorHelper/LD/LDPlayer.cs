using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AndroidEmulatorHelper.LD
{
    public abstract class LDPlayer : IAndroidEmulator
    {
        public Process proc;
        public IntPtr screenHwnd;

        public LDPlayer(Process ldProc) : base(ldProc)
        {
            proc = ldProc;

            screenHwnd = GetHwnd(GetProcessName());
        }

        public override string GetProcessName()
        {
            return proc.MainWindowTitle;
        }

        public override IntPtr GetScreenHwnd()
        {
            return screenHwnd;
        }

        private static IntPtr GetHwnd(string windowName)
        {
            IntPtr parantWindow = FindWindow("LDPlayerMainFrame", windowName);
            IntPtr childWindow = FindWindowEx(parantWindow, 0, "RenderWindow", "TheRender");

            return childWindow;
        }

        public abstract string RunAdbCommand(string command);
        public abstract string GetVersion();

        public bool TestAdbRunning()
        {
            string res = RunAdbCommand("shell echo 1");

            return res == "1";
        }

        [DllImport("user32", EntryPoint = "FindWindow", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string className, string windowTitle);

        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindowEx(IntPtr hWnd1, int hWnd2, string className, string caption);
    }
}
