// Decompiled with JetBrains decompiler
// Type: ScriptTool.Mob
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

#nullable disable
namespace ScriptTool
{
  public class Mob
  {
    public string mobName { get; set; }

    public int quantityFactor { get; set; }

    public bool singleRoom { get; set; }

    public int singleRoomRatio { get; set; }

    public int minCombatRoomsBefore { get; set; }

    public int maxCombatRoomsBefore { get; set; }

    public int minDifficulty { get; set; }

    public int maxDifficulty { get; set; }
  }
}
