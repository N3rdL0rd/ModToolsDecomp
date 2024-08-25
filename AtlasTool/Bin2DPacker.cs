// Decompiled with JetBrains decompiler
// Type: Packer.Bin2DPacker
// Assembly: AtlasTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BBF9A07-DA73-4842-AC1B-D544ACDDB4B5
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\AtlasTool.exe

using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Packer
{
  internal class Bin2DPacker
  {
    private Size m_StartingSize;
    private List<Bin2D> m_Bins = new List<Bin2D>();
    private Bin2D m_CurrentBin;
    private bool m_bCanIncreaseSize;

    public bool isEmpty => this.m_Bins.Count == 0 && this.m_CurrentBin == null;

    public List<Bin2D> bins => this.m_Bins;

    public Size maximumSize { get; set; }

    public uint maximumBinCount { get; set; }

    public Size margin { get; set; }

    public MarginType marginType { get; set; }

    public Bin2DPacker.Algorithm algorithm { get; set; }

    public Bin2DPacker(Size _startingSize, Size _maximumSize, Bin2DPacker.Algorithm _algorithm)
    {
      this.m_StartingSize = _startingSize;
      this.maximumSize = _maximumSize;
      this.m_bCanIncreaseSize = true;
      this.marginType = MarginType.None;
      this.margin = new Size(0, 0);
      this.m_CurrentBin = (Bin2D) null;
      this.maximumBinCount = uint.MaxValue;
      this.algorithm = _algorithm;
    }

    public void Clear()
    {
      this.m_Bins.Clear();
      this.m_CurrentBin = (Bin2D) null;
    }

    public bool InsertElement(uint _id, Size _size, out bool _newBinCreated)
    {
      _newBinCreated = false;
      if (this.m_CurrentBin == null)
      {
        this.m_CurrentBin = this.CreateBin();
        this.m_Bins.Add(this.m_CurrentBin);
        _newBinCreated = true;
      }
      if (!this.m_CurrentBin.size.CanFit(_size + this.margin) && (this.m_bCanIncreaseSize && !this.maximumSize.CanFit(_size + this.margin) || !this.m_bCanIncreaseSize))
        throw new Exception("This element will never fit in an atlas with the given parameters");
      bool flag = false;
      for (int index = 0; index < this.m_Bins.Count && !flag; ++index)
        flag = this.m_Bins[index].InsertElement(_id, _size);
      for (; !flag && (long) this.maximumBinCount > (long) this.m_Bins.Count; flag = this.m_CurrentBin.InsertElement(_id, _size))
      {
        if (this.m_bCanIncreaseSize && this.maximumSize.CanFit(this.m_CurrentBin.nextSize))
        {
          this.m_CurrentBin.IncreaseSize();
          this.m_CurrentBin.RearrangeBin();
        }
        else if ((long) this.maximumBinCount > (long) this.m_Bins.Count)
        {
          this.m_CurrentBin = this.CreateBin();
          this.m_Bins.Add(this.m_CurrentBin);
          _newBinCreated = true;
        }
      }
      return flag;
    }

    private Bin2D CreateBin()
    {
      if (this.algorithm == Bin2DPacker.Algorithm.Guillotine)
        return (Bin2D) new Bin2DGuillotine(this.m_StartingSize, this.margin, this.marginType);
      if (this.algorithm == Bin2DPacker.Algorithm.MaxRects)
        return (Bin2D) new Bin2DMaxRects(this.m_StartingSize, this.margin, this.marginType);
      throw new NotImplementedException();
    }

    public enum Algorithm
    {
      Guillotine,
      MaxRects,
    }
  }
}
