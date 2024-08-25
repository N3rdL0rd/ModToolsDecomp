// Decompiled with JetBrains decompiler
// Type: Packer.Bin2DMaxRects
// Assembly: AtlasTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BBF9A07-DA73-4842-AC1B-D544ACDDB4B5
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\AtlasTool.exe

using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Packer
{
  internal class Bin2DMaxRects : Bin2D
  {
    private List<Bin2DMaxRects.Element> m_FreeAreas;
    private List<Bin2DMaxRects.Element> m_UsedAreas;

    public Bin2DMaxRects(Size _startSize, Size _margin, MarginType _marginType)
      : base(_startSize, _margin, _marginType)
    {
      this.Reset();
    }

    protected override bool InsertElement(uint _id, Size _elementSize, out Rectangle _area)
    {
      _area = new Rectangle();
      Size _elementSize1 = _elementSize + new Size(1, 1);
      int bestIndexForElement = this.GetBestIndexForElement(_elementSize1);
      if (bestIndexForElement == -1)
        return false;
      _area.Size = _elementSize1;
      _area.Location = this.m_FreeAreas[bestIndexForElement].area.Location;
      this.m_UsedAreas.Add(new Bin2DMaxRects.Element()
      {
        area = _area,
        id = _id
      });
      Rectangle area1 = this.m_FreeAreas[bestIndexForElement].area;
      Rectangle rectangle = new Rectangle(_area.X + _area.Width, _area.Y, area1.Width - _area.Width, area1.Height);
      if (rectangle.GetArea() > 0)
        this.m_FreeAreas.Add(new Bin2DMaxRects.Element(rectangle));
      rectangle = new Rectangle(_area.X, _area.Y + _area.Height, area1.Width, area1.Height - _area.Height);
      if (rectangle.GetArea() > 0)
        this.m_FreeAreas.Add(new Bin2DMaxRects.Element(rectangle));
      this.m_FreeAreas.RemoveAt(bestIndexForElement);
      List<Rectangle> rectangleList = new List<Rectangle>();
      int index1 = 0;
      while (index1 < this.m_FreeAreas.Count)
      {
        Rectangle _this1 = Rectangle.Intersect(this.m_FreeAreas[index1].area, _area);
        if (_this1.GetArea() > 0)
        {
          Rectangle area2 = this.m_FreeAreas[index1].area;
          Rectangle _this2 = new Rectangle(area2.X, area2.Y, _this1.X - area2.X, area2.Height);
          if (_this2.GetArea() > 0)
            rectangleList.Add(_this2);
          _this2 = new Rectangle(area2.X, _this1.Y + _this1.Height, area2.Width, area2.Height - (_this1.Y - area2.Y + _this1.Height));
          if (_this2.GetArea() > 0)
            rectangleList.Add(_this2);
          _this2 = new Rectangle(area2.X, area2.Y, area2.Width, _this1.Y - area2.Y);
          if (_this2.GetArea() > 0)
            rectangleList.Add(_this2);
          _this2 = new Rectangle(_this1.X + _this1.Width, area2.Y, area2.Width - (_this1.X - area2.X + _this1.Width), area2.Height);
          if (_this2.GetArea() > 0)
            rectangleList.Add(_this2);
          this.m_FreeAreas.RemoveAt(index1);
        }
        else
          ++index1;
      }
      for (int index2 = 0; index2 < rectangleList.Count; ++index2)
        this.m_FreeAreas.Add(new Bin2DMaxRects.Element(rectangleList[index2]));
      int index3 = 0;
      while (index3 < this.m_FreeAreas.Count - 1)
      {
        bool flag = false;
        int index4 = 1;
        while (index4 < this.m_FreeAreas.Count && index3 < this.m_FreeAreas.Count - 1)
        {
          if (index3 == index4)
            ++index4;
          else if (this.m_FreeAreas[index3].area.Contains(this.m_FreeAreas[index4].area))
          {
            this.m_FreeAreas.RemoveAt(index4);
          }
          else
          {
            if (this.m_FreeAreas[index4].area.Contains(this.m_FreeAreas[index3].area))
            {
              this.m_FreeAreas.RemoveAt(index3);
              flag = true;
              break;
            }
            ++index4;
          }
        }
        if (!flag)
          ++index3;
      }
      return true;
    }

    protected override void RetrieveSizes(ref List<Size> _sizeList)
    {
      foreach (Bin2DMaxRects.Element usedArea in this.m_UsedAreas)
        _sizeList.Add(usedArea.area.Size - this.margin);
    }

    protected override void RetrieveIDs(ref List<uint> _idList)
    {
      foreach (Bin2DMaxRects.Element usedArea in this.m_UsedAreas)
        _idList.Add(usedArea.id);
    }

    protected override void Reset()
    {
      this.m_FreeAreas = new List<Bin2DMaxRects.Element>();
      this.m_UsedAreas = new List<Bin2DMaxRects.Element>();
      List<Bin2DMaxRects.Element> freeAreas = this.m_FreeAreas;
      Size size = this.size;
      int width = size.Width;
      size = this.size;
      int height = size.Height;
      Bin2DMaxRects.Element element = new Bin2DMaxRects.Element(new Rectangle(0, 0, width, height));
      freeAreas.Add(element);
    }

    private int GetBestIndexForElement(Size _elementSize)
    {
      int num1 = int.MaxValue;
      int bestIndexForElement = -1;
      for (int index = 0; index < this.m_FreeAreas.Count; ++index)
      {
        Rectangle area = this.m_FreeAreas[index].area;
        if (area.Size.CanFit(_elementSize))
        {
          Size size = area.Size;
          int val1 = size.Width - _elementSize.Width;
          size = area.Size;
          int val2 = size.Height - _elementSize.Height;
          int num2 = Math.Min(val1, val2);
          if (num2 < num1)
          {
            bestIndexForElement = index;
            num1 = num2;
          }
        }
      }
      return bestIndexForElement;
    }

    private class Element
    {
      public Rectangle area;
      public uint id;

      public Element()
      {
        this.area = new Rectangle();
        this.id = uint.MaxValue;
      }

      public Element(Rectangle _rectangle)
      {
        this.area = _rectangle;
        this.id = uint.MaxValue;
      }
    }
  }
}
