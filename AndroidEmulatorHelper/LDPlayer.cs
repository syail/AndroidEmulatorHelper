﻿using System.Diagnostics;

namespace AndroidEmulatorHelper
{
    public class LDPlayer : IAndroidEmulator
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

        public override BlueStacks[] GetList()
        {
            Process[] processes = Process.GetProcessesByName("dnplayer");

            return processes.Select(x => new BlueStacks(x)).ToArray();
        }

        public override IntPtr GetHwnd()
        {
            return _hwnd;
        }

        private IntPtr FindProcessHwnd()
        {
            IntPtr parantWindow = Win32Api.FindWindow("LDPlayerMainFrame", GetProcessName());
            IntPtr childWindow = Win32Api.FindWindowEx(parantWindow, 0, "RenderWindow", "TheRender");

            return childWindow;
        }
    }
}
