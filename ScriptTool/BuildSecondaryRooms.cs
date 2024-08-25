// Decompiled with JetBrains decompiler
// Type: ScriptTool.BuildSecondaryRooms
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

#nullable disable
namespace ScriptTool
{
  internal class BuildSecondaryRooms : ScriptSection
  {
    private const string defaultBuildSecondaryRooms = "\r\n//This is optional\r\n//function BuildSecondaryRooms(){\r\n    //Add here some more rooms\r\n//}\r\n";

    public override bool isOptional => true;

    public override string ToString()
    {
      return "\r\n//This is optional\r\n//function BuildSecondaryRooms(){\r\n    //Add here some more rooms\r\n//}\r\n";
    }
  }
}
