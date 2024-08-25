// Decompiled with JetBrains decompiler
// Type: ScriptTool.BuildTimedDoors
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

#nullable disable
namespace ScriptTool
{
  internal class BuildTimedDoors : ScriptSection
  {
    private const string defaultBuildTimedDoors = "\r\n//Optional\r\n//Note that if you want to build timed doors, be sure your setLevelProps function is defined and sets timed properties properly\r\n//function BuildTimedDoors(){\r\n    //Build timed doors here\r\n//}\r\n";

    public override bool isOptional => true;

    public override string ToString()
    {
      return "\r\n//Optional\r\n//Note that if you want to build timed doors, be sure your setLevelProps function is defined and sets timed properties properly\r\n//function BuildTimedDoors(){\r\n    //Build timed doors here\r\n//}\r\n";
    }
  }
}
