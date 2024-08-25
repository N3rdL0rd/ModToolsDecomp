// Decompiled with JetBrains decompiler
// Type: Packer.Extensions
// Assembly: AtlasTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BBF9A07-DA73-4842-AC1B-D544ACDDB4B5
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\AtlasTool.exe

using System.Drawing;

#nullable disable
namespace Packer
{
  internal static class Extensions
  {
    public static bool CanFit(this Size _this, Size _sizeToFit)
    {
      return _this.Width >= _sizeToFit.Width && _this.Height >= _sizeToFit.Height;
    }

    public static bool DoesIntersect(this Rectangle _this, Rectangle _rectangle)
    {
      return Rectangle.Intersect(_this, _rectangle).GetArea() > 0;
    }

    public static int GetArea(this Rectangle _this) => _this.Width * _this.Height;
  }
}
