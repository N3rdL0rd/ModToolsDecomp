// Decompiled with JetBrains decompiler
// Type: CDBTool.Program
// Assembly: CDBTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 42BFB734-5A26-4F73-8548-F69108A06C4F
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\CDBTool.exe

using ModTools;
using System;
using System.Globalization;
using System.Threading;

#nullable disable
namespace CDBTool
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
      string str1 = "";
      string _destCDBPath = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      bool flag = true;
      for (int index = 0; index < args.Length; ++index)
      {
        string str5 = args[index];
        if (str5[0] == '-' || str5[0] == '/')
        {
          string upper = str5.Substring(1).ToUpper();
          switch (upper)
          {
            case "OUTDIR":
              str2 = args[++index];
              continue;
            case "INDIR":
              str3 = args[++index];
              continue;
            case "REFCDB":
              str1 = args[++index];
              continue;
            case "OUTCDB":
              _destCDBPath = args[++index];
              continue;
            case "SILENT":
            case "S":
              flag = false;
              continue;
            case "?":
              Console.WriteLine("-? : Display this help");
              Console.WriteLine("-Expand -outDir <output directory> -refCDB <input cdb path> [-s]: Expands a given CDB to a file tree");
              Console.WriteLine("-Collapse -inDir <input directory> -outCDB <output cdb path> [-s]: Collapse a given file tree to a cdb");
              Console.WriteLine("-CreateDiffCDB -inDir <input directory> -outDir <output cdb path> -refCDB <reference cdb path> [-s]: Copy only changed or added expanded CDB files from a directory path to an outputpath(typically for mods)");
              Console.WriteLine("arguments :");
              Console.WriteLine("-s/-silent : Do not display message error (deactivated by default)");
              return;
            default:
              str4 = upper.ToUpper();
              continue;
          }
        }
      }
      try
      {
        CDBTool cdbTool = new CDBTool();
        Console.WriteLine("Launching CDBTool v" + Versionning.currentVersion + ", action: " + str4);
        switch (str4)
        {
          case "EXPAND":
            cdbTool.Expand(str1, str2);
            break;
          case "COLLAPSE":
            cdbTool.Collapse(str3, _destCDBPath);
            break;
          case "CREATEDIFFCDB":
            cdbTool.BuildDiffCDB(str1, str3, str2);
            break;
          default:
            throw new ArgumentException(string.Format("The action for \"{0}\" argument is not found, please refer to the doc", (object) str4), "strAction");
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
