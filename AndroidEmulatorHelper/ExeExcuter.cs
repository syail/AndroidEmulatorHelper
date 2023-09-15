using System.Diagnostics;

namespace AndroidEmulatorHelper
{
    internal class ExeExecuter
    {
        private readonly string _path;

        public ExeExecuter(string filepath)
        {
            _path = filepath;
        }

        public string Execute(string args, int timeout = 5000, int retry = 3)
        {
            try
            {
                for (int i = 0; i < retry; i++)
                {
                    using Process process = new()
                    {
                        StartInfo = new ProcessStartInfo(_path)
                        {
                            UseShellExecute = false,
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true,
                            Arguments = args,
                            CreateNoWindow = true
                        }
                    };
                    process.Start();

                    var res = process.WaitForExit(timeout);

                    if (!res)
                    {
                        Debug.WriteLine($"AndroidEmulatorHelper: Execute timeout, retry {i + 1} of {retry}");
                        continue;
                    };
                    return process.StandardOutput.ReadToEnd().Trim();
                }
                throw new Exception($"Execute {_path} {args} failed");
            }
            catch (Exception ex)
            {
                throw new Exception($"Execute {_path} {args} failed", ex);
            }
        }
    }
}
