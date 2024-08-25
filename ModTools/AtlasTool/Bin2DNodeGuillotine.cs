// Decompiled with JetBrains decompiler
// Type: Packer.Bin2DNodeGuillotine
// Assembly: AtlasTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BBF9A07-DA73-4842-AC1B-D544ACDDB4B5
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\AtlasTool.exe

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Packer
{
    internal class Bin2DNodeGuillotine
    {
        private const uint invalidID = 4294967295;
        private Bin2DNodeGuillotine m_LeftChild;
        private Bin2DNodeGuillotine m_RightChild;
        private Bin2DGuillotine m_Bin;

        public Rectangle area { get; set; }

        public bool isLeaf => this.m_LeftChild == null && this.m_RightChild == null;

        public Bin2DNodeGuillotine(Bin2DGuillotine _bin)
        {
            this.id = uint.MaxValue;
            this.m_Bin = _bin;
        }

        public Bin2DNodeGuillotine Insert(uint _id, Size _size, Size _margins, MarginType _marginType)
        {
            if (!this.isLeaf)
                return this.m_LeftChild.Insert(_id, _size, _margins, _marginType) ?? this.m_RightChild.Insert(_id, _size, _margins, _marginType);
            if (this.id != uint.MaxValue)
                return (Bin2DNodeGuillotine)null;
            Size sizeWithMargin1 = this.GetSizeWithMargin(_size, _margins, _marginType);
            if (sizeWithMargin1.Width <= this.area.Width)
            {
                int height1 = sizeWithMargin1.Height;
                Rectangle area = this.area;
                int height2 = area.Height;
                if (height1 <= height2)
                {
                    int width1 = sizeWithMargin1.Width;
                    area = this.area;
                    int width2 = area.Width;
                    if (width1 == width2)
                    {
                        int height3 = sizeWithMargin1.Height;
                        area = this.area;
                        int height4 = area.Height;
                        if (height3 == height4)
                        {
                            this.id = _id;
                            return this;
                        }
                    }
                    this.m_LeftChild = new Bin2DNodeGuillotine(this.m_Bin);
                    this.m_RightChild = new Bin2DNodeGuillotine(this.m_Bin);
                    this.m_LeftChild.m_Border = Bin2DNodeGuillotine.BorderType.None;
                    this.m_RightChild.m_Border = Bin2DNodeGuillotine.BorderType.None;
                    area = this.area;
                    int num1 = area.Width - sizeWithMargin1.Width;
                    area = this.area;
                    int num2 = area.Height - sizeWithMargin1.Height;
                    if (num1 > num2)
                    {
                        this.m_LeftChild.m_Border = this.m_Border & Bin2DNodeGuillotine.BorderType.Left | this.m_Border & Bin2DNodeGuillotine.BorderType.Top | this.m_Border & Bin2DNodeGuillotine.BorderType.Bottom;
                        Size sizeWithMargin2 = this.GetSizeWithMargin(_size, _margins, _marginType);
                        Bin2DNodeGuillotine leftChild = this.m_LeftChild;
                        area = this.area;
                        Point location = area.Location;
                        int width3 = sizeWithMargin2.Width;
                        area = this.area;
                        int height5 = area.Height;
                        Size size = new Size(width3, height5);
                        Rectangle rectangle1 = new Rectangle(location, size);
                        leftChild.area = rectangle1;
                        this.m_RightChild.m_Border = this.m_Border & Bin2DNodeGuillotine.BorderType.Right | this.m_Border & Bin2DNodeGuillotine.BorderType.Top | this.m_Border & Bin2DNodeGuillotine.BorderType.Bottom;
                        Bin2DNodeGuillotine rightChild = this.m_RightChild;
                        area = this.area;
                        int x = area.Left + sizeWithMargin2.Width;
                        area = this.area;
                        int top = area.Top;
                        area = this.area;
                        int width4 = area.Width - sizeWithMargin2.Width;
                        area = this.area;
                        int height6 = area.Height;
                        Rectangle rectangle2 = new Rectangle(x, top, width4, height6);
                        rightChild.area = rectangle2;
                    }
                    else
                    {
                        this.m_LeftChild.m_Border = this.m_Border & Bin2DNodeGuillotine.BorderType.Left | this.m_Border & Bin2DNodeGuillotine.BorderType.Top | this.m_Border & Bin2DNodeGuillotine.BorderType.Right;
                        Size sizeWithMargin3 = this.GetSizeWithMargin(_size, _margins, _marginType);
                        Bin2DNodeGuillotine leftChild = this.m_LeftChild;
                        area = this.area;
                        Point location = area.Location;
                        area = this.area;
                        Size size = new Size(area.Width, sizeWithMargin3.Height);
                        Rectangle rectangle3 = new Rectangle(location, size);
                        leftChild.area = rectangle3;
                        this.m_RightChild.m_Border = this.m_Border & Bin2DNodeGuillotine.BorderType.Left | this.m_Border & Bin2DNodeGuillotine.BorderType.Bottom | this.m_Border & Bin2DNodeGuillotine.BorderType.Right;
                        Bin2DNodeGuillotine rightChild = this.m_RightChild;
                        area = this.area;
                        int left = area.Left;
                        area = this.area;
                        int y = area.Top + sizeWithMargin3.Height;
                        area = this.area;
                        int width5 = area.Width;
                        area = this.area;
                        int height7 = area.Height - sizeWithMargin3.Height;
                        Rectangle rectangle4 = new Rectangle(left, y, width5, height7);
                        rightChild.area = rectangle4;
                    }
                    return this.m_LeftChild.Insert(_id, _size, _margins, _marginType);
                }
            }
            return (Bin2DNodeGuillotine)null;
        }

        public void RetrieveSizes(ref List<Size> _sizeList)
        {
            if (this.isLeaf)
            {
                if (this.id == uint.MaxValue)
                    return;
                _sizeList.Add(this.GetAreaWithoutMargin(this.m_Bin.margin, this.m_Bin.marginType).Size);
            }
            else
            {
                this.m_LeftChild.RetrieveSizes(ref _sizeList);
                this.m_RightChild.RetrieveSizes(ref _sizeList);
            }
        }

        private Size GetSizeWithMargin(Size _sizeWithoutMargins, Size _margin, MarginType _marginType)
        {
            Size sizeWithMargin = new Size(_sizeWithoutMargins.Width, _sizeWithoutMargins.Height);
            if ((_marginType == MarginType.OnlyBorder || _marginType == MarginType.All) && (this.m_Border & Bin2DNodeGuillotine.BorderType.Left) != Bin2DNodeGuillotine.BorderType.None)
                sizeWithMargin.Width += _margin.Width;
            if (_marginType == MarginType.All || _marginType == MarginType.OnlyBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Right) != Bin2DNodeGuillotine.BorderType.None || _marginType == MarginType.NoBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Right) == Bin2DNodeGuillotine.BorderType.None)
                sizeWithMargin.Width += _margin.Width;
            if ((_marginType == MarginType.OnlyBorder || _marginType == MarginType.All) && (this.m_Border & Bin2DNodeGuillotine.BorderType.Top) != Bin2DNodeGuillotine.BorderType.None)
                sizeWithMargin.Height += _margin.Height;
            if (_marginType == MarginType.All || _marginType == MarginType.OnlyBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Bottom) != Bin2DNodeGuillotine.BorderType.None || _marginType == MarginType.NoBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Bottom) == Bin2DNodeGuillotine.BorderType.None)
                sizeWithMargin.Height += _margin.Height;
            return sizeWithMargin;
        }

        public Rectangle GetAreaWithoutMargin(Size _margin, MarginType _marginType)
        {
            Rectangle areaWithoutMargin = new Rectangle();

            Rectangle area = this.area;
            Point location = area.Location;
            area = this.area;
            Size size = area.Size;

            areaWithoutMargin = new Rectangle(location, size);

            if ((_marginType == MarginType.OnlyBorder || _marginType == MarginType.All) &&
                (this.m_Border & Bin2DNodeGuillotine.BorderType.Left) != Bin2DNodeGuillotine.BorderType.None)
            {
                areaWithoutMargin.X += _margin.Width;
                areaWithoutMargin.Width -= _margin.Width;
            }

            if (_marginType == MarginType.All ||
                (_marginType == MarginType.OnlyBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Right) != Bin2DNodeGuillotine.BorderType.None) ||
                (_marginType == MarginType.NoBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Right) == Bin2DNodeGuillotine.BorderType.None))
            {
                areaWithoutMargin.Width -= _margin.Width;
            }

            if ((_marginType == MarginType.OnlyBorder || _marginType == MarginType.All) &&
                (this.m_Border & Bin2DNodeGuillotine.BorderType.Top) != Bin2DNodeGuillotine.BorderType.None)
            {
                areaWithoutMargin.Y += _margin.Height;
                areaWithoutMargin.Height -= _margin.Height;
            }

            if (_marginType == MarginType.All ||
                (_marginType == MarginType.OnlyBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Bottom) != Bin2DNodeGuillotine.BorderType.None) ||
                (_marginType == MarginType.NoBorder && (this.m_Border & Bin2DNodeGuillotine.BorderType.Bottom) == Bin2DNodeGuillotine.BorderType.None))
            {
                areaWithoutMargin.Height -= _margin.Height;
            }

            return areaWithoutMargin;
        }

        public void RetrieveIDs(ref List<uint> _idList)
        {
            if (this.isLeaf)
            {
                if (this.id == uint.MaxValue)
                    return;
                _idList.Add(this.id);
            }
            else
            {
                this.m_LeftChild.RetrieveIDs(ref _idList);
                this.m_RightChild.RetrieveIDs(ref _idList);
            }
        }

        private uint id { get; set; }

        private Bin2DNodeGuillotine.BorderType m_Border { get; set; }

        private enum BorderType
        {
            None = 0,
            Left = 1,
            Top = 2,
            Right = 4,
            Bottom = 8,
        }
    }
}
