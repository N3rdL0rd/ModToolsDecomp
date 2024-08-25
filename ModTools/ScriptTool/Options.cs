// Decompiled with JetBrains decompiler
// Type: ScriptTool.Options
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

using Newtonsoft.Json;

#nullable disable
namespace ScriptTool
{
  internal class Options
  {
    public string refCDBPath { get; set; } = "";

    public static Options FromJson(string _jsonString)
    {
      return JsonConvert.DeserializeObject<Options>(_jsonString);
    }

    public string ToJson() => JsonConvert.SerializeObject((object) this);
  }
}
