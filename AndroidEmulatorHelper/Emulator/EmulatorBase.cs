﻿using System.Diagnostics;
using System.Drawing;
using System.Runtime.Versioning;

namespace AndroidEmulatorHelper.Emulator
{
    public abstract class EmulatorBase : IEmulatorBase
    {
        public Process BaseProcess { get; }

        public EmulatorBase(Process emulator)
        {
            BaseProcess = emulator;
        }

        [SupportedOSPlatform("windows")]
        public Bitmap CaptureScreen()
        {
            nint hwnd = GetHwnd();
            using Graphics gdata = Graphics.FromHwnd(hwnd);

            Rectangle rect = Rectangle.Round(gdata.VisibleClipBounds);
            Bitmap bmp = new(rect.Width, rect.Height);

            using Graphics g = Graphics.FromImage(bmp);

            nint hdc = g.GetHdc();

            Win32Api.PrintWindow(hwnd, hdc, 0x2);
            g.ReleaseHdc(hdc);

            return bmp;
        }

        public Size GetScreenSize()
        {
            _ = Win32Api.GetWindowRect(GetHwnd(), out Rectangle rect);

            return new Size(rect.Width - rect.X, rect.Height - rect.Y);
        }

        /**
         * Bluestacks에서는 기존의 Click이 작동하지 않아서 override 가능하게 구현.
         */
        public virtual async Task Click(Point position)
        {
            nint hwnd = GetHwnd();
            nint ptr = CalculatePositionValue(position);

            Win32Api.PostMessage(hwnd, (int)WMessages.WM_LBUTTONDOWN, nint.Zero, ptr);
            await Task.Delay(5);

            Win32Api.PostMessage(hwnd, (int)WMessages.WM_LBUTTONUP, nint.Zero, ptr);
            await Task.Delay(5);
        }

        public void SendChar(char key)
        {
            Win32Api.PostMessage(GetHwnd(), (int)WMessages.WM_CHAR, key, nint.Zero);
        }

        public async Task SendString(string str)
        {
            nint hwnd = GetHwnd();
            foreach (char i in str)
            {
                Win32Api.PostMessage(hwnd, (int)WMessages.WM_CHAR, i, nint.Zero);
                await Task.Delay(50);
            }
        }

        public bool PostMessage(WMessages message, nint wParam, nint lParam)
        {
            return Win32Api.PostMessage(GetHwnd(), (uint)message, wParam, lParam);
        }

        public override string ToString()
        {
            Size screenSize = GetScreenSize();
            return $"[AndroidEmulator] {GetProcessName()} ({screenSize.Width}x{screenSize.Height})";
        }

        protected static nint CalculatePositionValue(Point position)
        {
            return new(position.X | position.Y << 16);
        }

        public abstract nint GetHwnd();
        public abstract string GetProcessName();
    }

    public enum WMessages : int
    {
        WM_ACTIVATE = 0x6,
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
