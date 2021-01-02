using System;
using System.Diagnostics;
using System.IO;

namespace MarkDownAvalonia.Data
{
    public class CommandRunner
    {
        public string ExecutablePath { get; }
        public string WorkingDirectory { get; }
 
        public CommandRunner(string executablePath, string workingDirectory = null)
        {
            ExecutablePath = executablePath ?? throw new ArgumentNullException(nameof(executablePath));
            WorkingDirectory = workingDirectory ?? Path.GetDirectoryName(executablePath);
        }
 
        public string Run(string arguments)
        {
            var info = new ProcessStartInfo(ExecutablePath, arguments)
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = WorkingDirectory,
            };
            var process = new Process
            {
                StartInfo = info,
            };
            process.Start();
            string log = process.StandardOutput.ReadToEnd();
            Console.WriteLine(log);
            return log;
        }
    }
}