// Decompiled with JetBrains decompiler
// Type: ScriptTool.Finalize
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

#nullable disable
namespace ScriptTool
{
  internal class Finalize : ScriptSection
  {
    private const string defaultFinalize = "\r\n//Optional\r\n//last function called during the level structure building\r\n//function Finalize(){\r\n    //Add here the structure of your room\r\n//}\r\n";

    public override bool isOptional => true;

    public override string ToString()
    {
      return "\r\n//Optional\r\n//last function called during the level structure building\r\n//function Finalize(){\r\n    //Add here the structure of your room\r\n//}\r\n";
    }
  }
}
