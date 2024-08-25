// Decompiled with JetBrains decompiler
// Type: Packer.Bin2DGuillotine
// Assembly: AtlasTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BBF9A07-DA73-4842-AC1B-D544ACDDB4B5
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\AtlasTool.exe

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Packer
{
  internal class Bin2DGuillotine : Bin2D
  {
    private Bin2DNodeGuillotine m_Root;

    public Bin2DGuillotine(Size _startSize, Size _margin, MarginType _marginType)
      : base(_startSize, _margin, _marginType)
    {
      this.Reset();
    }

    protected override bool InsertElement(uint _id, Size _elementSize, out Rectangle _area)
    {
      Bin2DNodeGuillotine bin2DnodeGuillotine = this.m_Root.Insert(_id, _elementSize, this.margin, this.marginType);
      if (bin2DnodeGuillotine == null)
      {
        _area = new Rectangle();
        return false;
      }
      _area = bin2DnodeGuillotine.GetAreaWithoutMargin(bin2DnodeGuillotine.area.Size, this.marginType);
      return true;
    }

    protected override void RetrieveSizes(ref List<Size> _sizeList)
    {
      this.m_Root.RetrieveSizes(ref _sizeList);
    }

    protected override void RetrieveIDs(ref List<uint> _idList)
    {
      this.m_Root.RetrieveIDs(ref _idList);
    }

    protected override void Reset()
    {
      this.m_Root = new Bin2DNodeGuillotine(this);
      Bin2DNodeGuillotine root = this.m_Root;
      Size size = this.size;
      int width = size.Width;
      size = this.size;
      int height = size.Height;
      Rectangle rectangle = new Rectangle(0, 0, width, height);
      root.area = rectangle;
    }
  }
}
