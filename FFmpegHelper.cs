using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WinFormsApp1
{
    public class FFmpegHelper
    {
        public static bool RunFFmpegCommand(string arguments)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg", // Assumes FFmpeg is in the system PATH. Otherwise, provide the full path to the FFmpeg executable.
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false,
                
            };

            var process = new Process();
            process.StartInfo = processStartInfo;

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                    Console.WriteLine(e.Data);
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                    Console.WriteLine(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();
            return process.ExitCode == 0;
        }
    }
}
