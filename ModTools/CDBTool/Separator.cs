// Decompiled with JetBrains decompiler
// Type: CDBTool.Separator
// Assembly: CDBTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 42BFB734-5A26-4F73-8548-F69108A06C4F
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\CDBTool.exe

#nullable disable
namespace CDBTool
{
  internal class Separator
  {
    public int id { get; set; }

    public string name { get; private set; }

    public int lineIndex { get; private set; }

    public Separator(int _id, string _name, int _lineIndex)
    {
      this.id = _id;
      this.name = _name;
      this.lineIndex = _lineIndex;
    }

    public void pushLine() => ++this.lineIndex;
  }
}
