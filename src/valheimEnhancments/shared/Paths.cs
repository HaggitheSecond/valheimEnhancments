using System.IO;

namespace valheimEnhancments.Shared
{
    public static class Paths
    {
        public static string SolutionDirectory
        {
            get
            {
                var path = Path.GetDirectoryName(typeof(Paths).Assembly.Location);
                return path.Substring(0, path.IndexOf(valheimEnhancementsPlugin.Name) + valheimEnhancementsPlugin.Name.Length);
            }
        }

        public static class valheim
        {
            private static string SteamLocation
            {
                get
                {
                    using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam"))                    
                        return key.GetValue("SteamPath").ToString();
                }
            }

            public static string Directory => Path.Combine(SteamLocation, "steamapps", "common", "Valheim");
            public static string BepInDirectory => Path.Combine(Directory, "BepInEx");
            public static string BepInPluginsDirectory => Path.Combine(BepInDirectory, "plugins");
            public static string Exe => Path.Combine(Directory, "valheim.exe");

            public static string OutputFileLocation => Path.Combine(BepInPluginsDirectory, valheimEnhancementsPlugin.OutputFileName);
            public static string ReadmeFileLocation => Path.Combine(BepInPluginsDirectory, valheimEnhancmentsDocumentation.ReadmeFileName);

            public static string ItemDumpFileName => "valheimEnhancments.Items.csv";
            public static string ItemDumpFileLocation => Path.Combine(BepInPluginsDirectory, ItemDumpFileName);
        }

        public static class valheimEnhancementsPlugin
        {
            public const string Name = "valheimEnhancments";
            public const string Description = "valheimEnhancments is a plugin for valheim";
            public const string Guid = "valheimEnhancments";

            public const string Version = "1.0.0.0";

            public static string Directory => Path.Combine(SolutionDirectory, "src", "valheimEnhancments");
            public static string ProjectFile => Path.Combine(Directory, "valheimEnhancments.csproj");
            public static string OutputFileName => "valheimEnhancments.dll";
            public static string OutputFileLocation => Path.Combine(ReleaseDirectory, OutputFileName);

            public static string BinDirectory => Path.Combine(Directory, "bin");
            public static string ObjDirectory => Path.Combine(Directory, "obj");
            public static string ReleaseDirectory => Path.Combine(BinDirectory, "Release");
        }

        public static class valheimEnhancmentsDocumentation
        {
            public static string Directory => Path.Combine(SolutionDirectory, "src", "valheimEnhancments.Documentation");
            public static string ProjectFile => Path.Combine(Directory, "valheimEnhancments.Documentation.csproj");
            public static string ReleaseExe => Path.Combine(ReleaseDirectory, "valheimEnhancments.Documentation.exe");

            public static string ReadmeFileName => "valheimEnhancments.Readme.txt";
            public static string ReadmeFileLocation => Path.Combine(ReleaseDirectory, ReadmeFileName);

            public static string BinDirectory => Path.Combine(Directory, "bin");
            public static string ObjDirectory => Path.Combine(Directory, "obj");
            public static string ReleaseDirectory => Path.Combine(BinDirectory, "Release");
        }
    }
}
