using GameLauncher.Update;
using System;
using System.Diagnostics;
using System.IO;

namespace ManifestGenerator
{
    class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine();
                Console.WriteLine("    ManifestGenerator <game-dir> <output-dir>\\content.xml <game.exe> <overwrite-manifest:true|false>");
                Console.ReadLine();
                return;
            }

            string inputPath = args[0];
            string outputPath = args[1];
            string gameName = args[2];
            bool overwrite = Boolean.Parse(args[3]);

            HashGenerator.RootDirectory = inputPath;

            if (!overwrite && (File.Exists(outputPath + ".xml") || File.Exists(outputPath + ".version")))
            {
                Console.WriteLine("Target File already exists.");
                Console.ReadLine();
                return;
            }

            var contentFile = new HashGenerator.ContentFile();
            try
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(inputPath, gameName));
                contentFile.GameVersion = versionInfo.ProductVersion;
            }
            catch 
            {
                Console.WriteLine("Could not determine game version. Please enter yourself.");
            }
            AddFilesToManifest(contentFile);
            contentFile.GenerateXml(outputPath);
            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        private static void AddFilesToManifest(HashGenerator.ContentFile contentFile)
        {
            DirectoryInfo root = new DirectoryInfo(HashGenerator.RootDirectory);
            foreach (var fileInfo in root.GetFiles("*.*", SearchOption.AllDirectories))
            {
                var stateInfo = HashGenerator.FileStateInfo.FromFile(fileInfo);
                contentFile.FileList.Add(stateInfo);
            }
        }
    }
}
