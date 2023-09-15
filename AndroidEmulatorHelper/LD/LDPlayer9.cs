using System.Diagnostics;

namespace AndroidEmulatorHelper.LD
{
    public class LDPlayer9 : LDPlayer
    {
        private readonly string _drive;

        public LDPlayer9(Process ldProc, string ldDrive) : base(ldProc)
        {
            _drive = ldDrive;
        }

        public override string RunAdbCommand(string command)
        {
            ExeExecuter exeExecuter = new($"{_drive}:\\LDPlayer\\LDPlayer9\\ldconsole.exe");

            string res = exeExecuter.Execute($"adb --name \"{GetProcessName()}\" --command \"{command}\"");

            return res;
        }

        public override string GetVersion()
        {
            return "9.0";
        }
    }
}
