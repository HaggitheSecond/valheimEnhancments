using System.Threading.Tasks;
using Bullseye;
using valheimEnhancments.Build.Helpers;
using System.IO;
using System;
using valheimEnhancments.Shared;

namespace valheimEnhancments.Build
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Targets.Target("ensuresetup", () =>
            {
                if (Directory.Exists(Paths.valheim.Directory) == false)
                    throw new Exception("Valheim directory does not exist");

                if (Directory.Exists(Paths.valheim.BepInDirectory) == false)
                    throw new Exception("Valheim bepinex directory does not exist");

                if (Directory.Exists(Paths.valheim.BepInPluginsDirectory) == false)
                    throw new Exception("Valheim bepinex plugins directory does not exist");
            });

            Targets.Target("cleanup", Targets.DependsOn("ensuresetup"), () =>
            {
                if (Directory.Exists(Paths.valheimEnhancementsPlugin.BinDirectory))
                    Directory.Delete(Paths.valheimEnhancementsPlugin.BinDirectory, true);

                if (Directory.Exists(Paths.valheimEnhancementsPlugin.ObjDirectory))
                    Directory.Delete(Paths.valheimEnhancementsPlugin.ObjDirectory, true);

                if (Directory.Exists(Paths.valheimEnhancmentsDocumentation.BinDirectory))
                    Directory.Delete(Paths.valheimEnhancmentsDocumentation.BinDirectory, true);

                if (Directory.Exists(Paths.valheimEnhancmentsDocumentation.ObjDirectory))
                    Directory.Delete(Paths.valheimEnhancmentsDocumentation.ObjDirectory, true);

                if (File.Exists(Paths.valheim.OutputFileLocation))
                    File.Delete(Paths.valheim.OutputFileLocation);

                if (File.Exists(Paths.valheim.ReadmeFileLocation))
                    File.Delete(Paths.valheim.ReadmeFileLocation);
            });

            Targets.Target("build", Targets.DependsOn("cleanup"), () =>
            {
                RunHelper.RunMsBuild($"\"{Paths.valheimEnhancementsPlugin.ProjectFile}\" /t:Build /p:Configuration=Release");
            });

            Targets.Target("writedocumentation", Targets.DependsOn("build"), () =>
            {
                RunHelper.RunMsBuild($"\"{Paths.valheimEnhancmentsDocumentation.ProjectFile}\" /t:Build /p:Configuration=Release");
                RunHelper.Run(Paths.valheimEnhancmentsDocumentation.ReleaseExe, string.Empty);
            });

            Targets.Target("updatevalheimdir", Targets.DependsOn("writedocumentation"), () =>
            {
                File.Copy(Paths.valheimEnhancementsPlugin.OutputFileLocation, Paths.valheim.OutputFileLocation);
                File.Copy(Paths.valheimEnhancmentsDocumentation.ReadmeFileLocation, Paths.valheim.ReadmeFileLocation);
            });

            Targets.Target("runvalheim", Targets.DependsOn("updatevalheimdir"), () =>
            {
#if DEBUG
                RunHelper.Run(Paths.valheim.Exe, "-console", waitForExit:false);
#endif
            });

            Targets.Target("default", Targets.DependsOn("runvalheim"));

            await Targets.RunTargetsAndExitAsync(args);
        }
    }
}
