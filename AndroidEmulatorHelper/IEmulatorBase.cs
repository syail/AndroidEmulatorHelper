﻿using System.Drawing;

namespace AndroidEmulatorHelper
{
    public interface IEmulatorBase
    {
        IntPtr GetHwnd();
        string GetProcessName();

        Bitmap CaptureScreen();
        Size GetScreenSize();

        Task Click(Point position);
        void SendChar(char key);
        Task SendString(string str);
    }
}