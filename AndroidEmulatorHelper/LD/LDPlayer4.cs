using System.Diagnostics;
using AndroidEmulatorHelper.LD;

namespace AndroidEmulatorHelper.LD
{
    public class LDPlayer4 : LDPlayer
    {
        private readonly string _drive;

        public LDPlayer4(Process ldProc, string ldDrive) : base(ldProc)
        {
            _drive = ldDrive;
        }

        public override string RunAdbCommand(string command)
        {
            ExeExecuter exeExecuter = new($"{_drive}:\\LDPlayer\\LDPlayer4.0\\ldconsole.exe");

            string res = exeExecuter.Execute($"adb --name \"{GetProcessName()}\" --command \"{command}\"");

            return res;
        }

        public override string GetVersion()
        {
            return "4.0";
        }   
    }
}
