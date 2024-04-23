using System.Drawing;

namespace AndroidEmulatorHelper.Emulator
{
    public interface IEmulatorBase
    {
        nint GetHwnd();
        string GetProcessName();

        Bitmap CaptureScreen();
        Size GetScreenSize();

        Task Click(Point position);
        void SendChar(char key);
        Task SendString(string str);
    }
}
