// Decompiled with JetBrains decompiler
// Type: ScriptTool.AddTeleports
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

#nullable disable
namespace ScriptTool
{
  internal class AddTeleports : ScriptSection
  {
    private const string defaultAddTeleports = "\r\n//Optional : allows you to define how to add teleports in your level\r\n//keep it commented to have a default behavior : some teleport will be created automatically\r\n//uncomment and keep empty to have no teleport at all\r\n//function AddTeleports(){\r\n    //Add here the code to add teleporters inside the level\r\n//}\r\n";

    public override bool isOptional => true;

    public override string ToString()
    {
      return "\r\n//Optional : allows you to define how to add teleports in your level\r\n//keep it commented to have a default behavior : some teleport will be created automatically\r\n//uncomment and keep empty to have no teleport at all\r\n//function AddTeleports(){\r\n    //Add here the code to add teleporters inside the level\r\n//}\r\n";
    }
  }
}
