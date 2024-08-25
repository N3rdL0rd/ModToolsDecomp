// Decompiled with JetBrains decompiler
// Type: PAKTool.Program
// Assembly: PAKTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC9A4AB-62C6-4F7C-95CE-FBC3CF0F40A2
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\PAKTool.exe

using ModTools;
using System;
using System.Globalization;
using System.Threading;

#nullable disable
namespace PAKTool
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
      string str1 = "";
      string _destination = "";
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
              _destination = args[++index];
              continue;
            case "INDIR":
              str1 = args[++index];
              continue;
            case "REFPAK":
              str2 = args[++index];
              continue;
            case "OUTPAK":
              str3 = args[++index];
              continue;
            case "SILENT":
            case "S":
              flag = false;
              continue;
            case "?":
              Console.WriteLine("-? : Display this help");
              Console.WriteLine("-Expand -outdir <output directory> -refpak <input pak path> [-s]: Expands a given PAK to a file tree");
              Console.WriteLine("-Collapse -indir <input directory> -outpak <output pak path> [-s]: Collapse a given file tree to a pak");
              Console.WriteLine("-CreateDiffPak -refpak <input pak path> -indir <input directory> -outPak <output pak path> [-s]: Create a pak from a directory with only what has changed or been added from the ref pak (typically for mods)");
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
        PAKTool.PAKTool pakTool = new PAKTool.PAKTool();
        Console.WriteLine("Launching PAKTool v" + Versionning.currentVersion + " action: " + str4);
        switch (str4)
        {
          case "EXPAND":
            pakTool.ExpandPAK(str2, _destination);
            break;
          case "COLLAPSE":
            pakTool.BuildPAK(str1, str3);
            break;
          case "CREATEDIFFPAK":
            pakTool.BuildDiffPAK(str2, str1, str3);
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
