using System;
using System.IO;
using System.Threading.Tasks;
using AssetStudioGUI;

namespace AssetStudioGUI
{
    static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Usage: `mono Program.exe <sourcePath> <outputPath>`");
            var path = args[0];
            var savePath = args[1];
            Console.WriteLine(path);
            Console.WriteLine(savePath);

            await Task.Run(() => Studio.assetsManager.LoadFolder(path));
            await Task.Run(() => Studio.BuildAssetData());
            await Task.Run(() => Studio.BuildClassStructure());

            foreach (var asset in Studio.exportableAssets)
            {
                string exportPath;
                switch (Properties.Settings.Default.assetGroupOption)
                {
                    case 0: //type name
                        exportPath = Path.Combine(savePath, asset.TypeString);
                        break;
                    case 1: //container path
                        if (!string.IsNullOrEmpty(asset.Container))
                        {
                            exportPath = Path.Combine(savePath, Path.GetDirectoryName(asset.Container));
                        }
                        else
                        {
                            exportPath = savePath;
                        }
                        break;
                    case 2: //source file
                        exportPath = Path.Combine(savePath, asset.SourceFile.fullName + "_export");
                        break;
                    default:
                        exportPath = savePath;
                        break;
                }
                exportPath += Path.DirectorySeparatorChar;
                Exporter.ExportConvertFile(asset, exportPath);
            }
        }
    }
}