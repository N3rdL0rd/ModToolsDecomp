// Decompiled with JetBrains decompiler
// Type: ScriptTool.Program
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

using ModTools;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace ScriptTool
{
  internal static class Program
  {
    [STAThread]
    private static void Main(string[] args)
    {
      Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
      bool flag1 = false;
      bool flag2 = true;
      string str1 = "";
      string fileName = "";
      for (int index = 0; index < args.Length; ++index)
      {
        string str2 = args[index];
        if (str2[0] == '-' || str2[0] == '/')
        {
          string upper = str2.Substring(1).ToUpper();
          switch (upper)
          {
            case "COMMANDLINE":
              flag1 = true;
              continue;
            case "OUTSCRIPT":
              fileName = args[++index];
              continue;
            case "SILENT":
            case "S":
              flag2 = false;
              continue;
            case "?":
              Console.WriteLine("-? : Display this help");
              Console.WriteLine("-CommandLine -NewFile -OutScript <output file name> [-s] : create a script file with default function and basic doc inside.");
              Console.WriteLine("arguments :");
              Console.WriteLine("-s/-silent : Do not display message error (deactivated by default)");
              return;
            default:
              str1 = upper;
              continue;
          }
        }
      }
      if (args.Length != 0 && !flag1)
      {
        int num1 = (int) MessageBox.Show("The application has been launched with arguments, if you want to use this exe as command line, add \"-CommandLine\" in the arguments.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      if (!flag1)
      {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run((Form) new ScriptTool.Main());
      }
      else
      {
        try
        {
          Console.WriteLine("Launching ScriptTool v" + Versionning.currentVersion + " action: " + str1);
          if (!(str1 == "NEWFILE"))
            throw new ArgumentException(string.Format("The action for \"{0}\" argument is not found, please refer to the doc", (object) str1), "strAction");
          FileInfo fileInfo = new FileInfo(fileName);
          fileInfo.Directory.Create();
          string fullName = fileInfo.FullName;
          if (fileInfo.Extension != ".hx")
            fullName += ".hx";
          File.WriteAllText(fullName, ScriptWriter.instance.WriteWholeScript());
        }
        catch (Exception ex)
        {
          int num2 = flag2 ? 1 : 0;
          Error.Show(ex, num2 != 0);
        }
      }
    }
  }
}
