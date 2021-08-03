using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homeMonitor.Services
{
    public interface IOsCommands
    {
        string Execute(string command);
    }

    public class LinuxCommands : IOsCommands
    {
        public string Execute(string command)
        {
            string result = "";

            using System.Diagnostics.Process proc = new System.Diagnostics.Process
            {
                StartInfo =
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{command}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            proc.Start();

            result += proc.StandardOutput.ReadToEnd();
            result += proc.StandardError.ReadToEnd();

            proc.WaitForExit();

            return result;
        }
    }
}
