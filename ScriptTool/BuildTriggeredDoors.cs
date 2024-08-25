// Decompiled with JetBrains decompiler
// Type: ScriptTool.BuildTriggeredDoors
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

#nullable disable
namespace ScriptTool
{
  internal class BuildTriggeredDoors : ScriptSection
  {
    private const string defaultBuildTriggeredDoors = "\r\n//Optional\r\n//function BuildTriggeredDoors(_combatRooms : Array<RoomNode>){\r\n//}\r\n";

    public override bool isOptional => true;

    public override string ToString()
    {
      return "\r\n//Optional\r\n//function BuildTriggeredDoors(_combatRooms : Array<RoomNode>){\r\n//}\r\n";
    }
  }
}
