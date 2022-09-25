using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace valheimEnhancments.Build.Helpers
{
    public class RunHelper
    {
        private static string GetMsBuildPath()
        {
            var vsWhere = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft Visual Studio", "Installer", "vswhere.exe");
            var path = Read(vsWhere, "-products * -requires Microsoft.Component.MSBuild -property installationPath -version 16.0 -prerelease", true)
                .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .First();

            var msbuildPath = Path.Combine(path, "MSBuild", "Current", "Bin", "MSBuild.exe");

            if (File.Exists(msbuildPath) == false)
                throw new Exception("msbuild.exe not found. Is Visual Studio 2019 installed?");

            return msbuildPath;
        }
        public static void Run(string name, string args, string workingDirectory = null, bool waitForExit = true)
        {
            Read(name, args, waitForExit, workingDirectory);
        }

        public static void RunMsBuild(string arguments)
        {
            Run(GetMsBuildPath(), arguments, waitForExit:true);
        }

        private static string Read(string name, string args, bool waitForExit, string workingDirectory = null)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = name,
                    Arguments = args,
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardOutput = true,
                };

                process.Start();

                if (waitForExit == false)
                    return string.Empty;

                var output = process.StandardOutput.ReadToEnd(); //Make sure to read the output before we WaitForExit, or the process might hang forever
                process.WaitForExit();

                if (process.ExitCode != 0)
                    throw new Exception($"The command \"{name} {args}\" failed!{Environment.NewLine}{output}");

                return output;
            }
        }
    }
}
