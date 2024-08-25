// Decompiled with JetBrains decompiler
// Type: AtlasTool.Program
// Assembly: AtlasTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BBF9A07-DA73-4842-AC1B-D544ACDDB4B5
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\AtlasTool.exe

using ModTools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace AtlasTool
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
      string _atlasPath1 = "";
      string inDir = "";
      string outDir = "";
      string str1 = "";
      bool flag = true;
      bool bBinary = true;
      for (int index = 0; index < args.Length; ++index)
      {
        string str2 = args[index];
        if (str2[0] == '-' || str2[0] == '/')
        {
          string upper = str2.Substring(1).ToUpper();
          switch (upper)
          {
            case "INDIR":
              inDir = args[++index];
              continue;
            case "OUTDIR":
              outDir = args[++index];
              continue;
            case "ATLAS":
            case "A":
              _atlasPath1 = args[++index];
              continue;
            case "SILENT":
            case "S":
              flag = false;
              continue;
            case "ASCII":
              bBinary = false;
              continue;
            case "?":
              Console.WriteLine("-? : Display this help");
              Console.WriteLine("-Expand -outdir <output directory> -Atlas <input atlas path> [-s]: Expands a given Atlas to a file tree");
              Console.WriteLine("-ExpandAll -indir <input atlases directory> -outdir <output directory> [-s]: Expands every atlas found in indir into outdir");
              Console.WriteLine("-Collapse -indir <input directory> -Atlas <output atlas path> [-s][-ascii]: Collapse a given file tree to an atlas");
              Console.WriteLine("-CollapseAll -indir <input directories> -outdir <output atlases path> [-s][-ascii]: Collapse every directory in the input directory into atlases");
              Console.WriteLine("arguments :");
              Console.WriteLine("-s/-silent : Do not display message error (deactivated by default)");
              Console.WriteLine("-ascii : Export atlases as ascii (binary by default)");
              return;
            default:
              str1 = upper.ToUpper();
              continue;
          }
        }
      }
      try
      {
        AtlasTool atlasTool1 = new AtlasTool();
        Console.WriteLine("Launching AtlasTool v" + Versionning.currentVersion + ", action: " + str1);
        switch (str1)
        {
          case "EXPAND":
            atlasTool1.Expand(_atlasPath1, outDir);
            break;
          case "COLLAPSE":
            atlasTool1.Collapse(inDir, _atlasPath1, bBinary);
            break;
          case "EXPANDALL":
            DirectoryInfo directoryInfo1 = new DirectoryInfo(inDir);
            List<Task> taskList1 = new List<Task>();
            foreach (FileInfo file in directoryInfo1.GetFiles("*.atlas"))
            {
              FileInfo atlas = file;
              Task task = Task.Factory.StartNew((Action) (() =>
              {
                AtlasTool atlasTool2 = new AtlasTool();
                string path2 = atlas.Name.Substring(0, atlas.Name.Length - 6);
                string fullName = atlas.FullName;
                string _outDirPath = Path.Combine(outDir, path2);
                atlasTool2.Expand(fullName, _outDirPath);
              }));
              taskList1.Add(task);
            }
            Task.WaitAll(taskList1.ToArray());
            break;
          case "COLLAPSEALL":
            DirectoryInfo directoryInfo2 = new DirectoryInfo(inDir);
            List<Task> taskList2 = new List<Task>();
            foreach (DirectoryInfo directory in directoryInfo2.GetDirectories())
            {
              DirectoryInfo dir = directory;
              Task task = Task.Factory.StartNew((Action) (() =>
              {
                AtlasTool atlasTool3 = new AtlasTool();
                string name = dir.Name;
                string _inputDirPath = Path.Combine(inDir, name);
                string _atlasPath2 = Path.Combine(outDir, name) + ".png";
                int num = bBinary ? 1 : 0;
                atlasTool3.Collapse(_inputDirPath, _atlasPath2, num != 0);
              }));
              taskList2.Add(task);
            }
            Task.WaitAll(taskList2.ToArray());
            break;
          default:
            throw new ArgumentException(string.Format("The action for \"{0}\" argument is not found, please refer to the doc", (object) str1), "strAction");
        }
      }
      catch (Exception ex)
      {
        int num = flag ? 1 : 0;
        Error.Show(ex, num != 0);
      }
    }
  }
}
