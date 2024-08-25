// Decompiled with JetBrains decompiler
// Type: AtlasTool.Tile
// Assembly: AtlasTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BBF9A07-DA73-4842-AC1B-D544ACDDB4B5
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\AtlasTool.exe

using System.Drawing;

#nullable disable
namespace AtlasTool
{
  internal class Tile
  {
    public string name;
    public int index;
    public int x;
    public int y;
    public int width;
    public int height;
    public int offsetX;
    public int offsetY;
    public int originalWidth;
    public int originalHeight;
    public string originalFilePath;
    public Bitmap bitmap;
    public bool hasNormal;
    public Tile duplicateOf;
    public int atlasIndex;
  }
}
