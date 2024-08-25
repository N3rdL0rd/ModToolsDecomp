// Decompiled with JetBrains decompiler
// Type: ScriptTool.ScriptSection
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

#nullable disable
namespace ScriptTool
{
  internal abstract class ScriptSection
  {
    public abstract override string ToString();

    public abstract bool isOptional { get; }
  }
}
