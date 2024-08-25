// Decompiled with JetBrains decompiler
// Type: Packer.Bin2D
// Assembly: AtlasTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BBF9A07-DA73-4842-AC1B-D544ACDDB4B5
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\AtlasTool.exe

using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Packer
{
  internal abstract class Bin2D
  {
    private Dictionary<uint, Rectangle> m_Elements = new Dictionary<uint, Rectangle>();

    public Size size { get; private set; }

    public Dictionary<uint, Rectangle> elements => this.m_Elements;

    public Size margin { get; private set; }

    public MarginType marginType { get; private set; }

    public Size nextSize
    {
      get
      {
        if (this.currentGrowthState == Bin2D.GrowthState.GrowWidth)
          return new Size(this.size.Width * 2, this.size.Height);
        if (this.currentGrowthState == Bin2D.GrowthState.SwapWidthHeight)
          return new Size(this.size.Height, this.size.Width);
        throw new NotImplementedException();
      }
    }

    public Bin2D(Size _startSize, Size _margin, MarginType _marginType)
    {
      this.size = _startSize;
      this.margin = _margin;
      this.marginType = _marginType;
      this.currentGrowthState = Bin2D.GrowthState.GrowWidth;
      this.startSize = _startSize;
    }

    public void IncreaseSize()
    {
      this.size = this.nextSize;
      this.currentGrowthState = (Bin2D.GrowthState) ((int) (this.currentGrowthState + 1) % 2);
    }

    public bool InsertElement(uint _id, Size _elementSize)
    {
      Rectangle _area;
      if (!this.InsertElement(_id, _elementSize, out _area))
        return false;
      this.m_Elements.Add(_id, _area);
      return true;
    }

    protected abstract bool InsertElement(uint _id, Size _elementSize, out Rectangle _area);

    protected abstract void RetrieveSizes(ref List<Size> _areaList);

    protected abstract void RetrieveIDs(ref List<uint> _idList);

    protected abstract void Reset();

    public void RearrangeBin()
    {
      List<Size> _areaList = new List<Size>();
      List<uint> _idList = new List<uint>();
      this.RetrieveSizes(ref _areaList);
      this.RetrieveIDs(ref _idList);
      bool flag;
      do
      {
        flag = true;
        this.m_Elements.Clear();
        this.Reset();
        int count = _areaList.Count;
        for (int index = 0; index < count & flag; ++index)
        {
          if (!this.InsertElement(_idList[index], _areaList[index]))
          {
            flag = false;
            this.IncreaseSize();
          }
        }
      }
      while (!flag);
    }

    protected Size startSize { get; private set; }

    private Bin2D.GrowthState currentGrowthState { get; set; }

    private enum GrowthState
    {
      GrowWidth,
      SwapWidthHeight,
      Count,
    }
  }
}
