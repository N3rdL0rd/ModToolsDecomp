// Decompiled with JetBrains decompiler
// Type: AtlasTool
// Assembly: AtlasTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BBF9A07-DA73-4842-AC1B-D544ACDDB4B5
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\AtlasTool.exe

using ModTools;
using Packer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

#nullable disable
namespace AtlasTool
{
    internal class AtlasTool
    {
        private static byte[] buffer = new byte[67108864];
        private Dictionary<string, Tile> hashes = new Dictionary<string, Tile>();

        public void Expand(string _atlasPath, string _outDirPath)
        {
            DirectoryInfo _outDir = new DirectoryInfo(_outDirPath);
            if (_outDir.Exists)
                _outDir.Delete(true);
            _outDir.Create();
            FileInfo _atlasInfo = new FileInfo(_atlasPath);
            _atlasInfo.Name.Substring(0, _atlasInfo.Name.Length - 6);
            Stream input = (Stream)File.OpenRead(_atlasPath);
            BinaryReader _reader = new BinaryReader(input);
            string str = new string(_reader.ReadChars(4));
            while (_reader.BaseStream.Position + 18L < input.Length)
            {
                List<Tile> _tiles = new List<Tile>();
                string _atlasName = this.ReadString(_reader);
                if (!(_atlasName == ""))
                {
                    while (_reader.BaseStream.Position + 18L < input.Length)
                    {
                        Tile tile = new Tile();
                        tile.name = this.ReadString(_reader);
                        if (!(tile.name == ""))
                        {
                            tile.index = (int)_reader.ReadUInt16();
                            tile.x = (int)_reader.ReadUInt16();
                            tile.y = (int)_reader.ReadUInt16();
                            tile.width = (int)_reader.ReadUInt16();
                            tile.height = (int)_reader.ReadUInt16();
                            tile.offsetX = (int)_reader.ReadUInt16();
                            tile.offsetY = (int)_reader.ReadUInt16();
                            tile.originalWidth = (int)_reader.ReadUInt16();
                            tile.originalHeight = (int)_reader.ReadUInt16();
                            _tiles.Add(tile);
                        }
                        else
                            break;
                    }
                    this.CreateBitmapTree(_tiles, _outDir, _atlasInfo, _atlasName);
                    try
                    {
                        this.CreateBitmapTree(_tiles, _outDir, _atlasInfo, _atlasName.Substring(0, _atlasName.Length - 4) + "_n.png", "_n");
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                    break;
            }
            _reader.Close();
        }

        public void CreateBitmapTree(
          List<Tile> _tiles,
          DirectoryInfo _outDir,
          FileInfo _atlasInfo,
          string _atlasName,
          string _suffix = "")
        {
            Bitmap _atlas = (Bitmap)Image.FromFile(Path.Combine(_atlasInfo.DirectoryName, _atlasName));
            Directory.CreateDirectory(_outDir.FullName);
            foreach (Tile tile in _tiles)
                this.CopyBitmapFromAtlas(tile, _outDir.FullName, _atlas, _suffix);
            _atlas.Dispose();
        }

        public void Collapse(string _inputDirPath, string _atlasPath, bool _exportBinaryAtlases)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(_inputDirPath);
            new DirectoryInfo(new FileInfo(_atlasPath).DirectoryName).Create();
            if (!directoryInfo.Exists)
                throw new DirectoryNotFoundException("Directory not found : " + _inputDirPath);
            List<Tile> tileList = new List<Tile>();
            HashSet<Bitmap> bitmapSet = new HashSet<Bitmap>();
            foreach (FileInfo fileInfo in ((IEnumerable<FileInfo>)directoryInfo.GetFiles("*.png", SearchOption.AllDirectories)).Where<FileInfo>((Func<FileInfo, bool>)(file => !file.Name.EndsWith("_n.png"))))
            {
                try
                {
                    Bitmap bitmap = (Bitmap)Image.FromFile(fileInfo.FullName);
                    Tile _tile = new Tile();
                    _tile.width = _tile.originalWidth = bitmap.Width;
                    _tile.height = _tile.originalHeight = bitmap.Height;
                    _tile.bitmap = bitmap;
                    _tile.hasNormal = File.Exists(Path.Combine(fileInfo.DirectoryName, fileInfo.Name.Substring(0, fileInfo.Name.Length - 4) + "_n.png"));
                    _tile.name = fileInfo.FullName.Replace(directoryInfo.FullName, "").Substring(1);
                    _tile.name = _tile.name.Substring(0, _tile.name.Length - 4).Replace("\\", "/");
                    int length = _tile.name.IndexOf("-=-");
                    if (length != -1)
                    {
                        int num = _tile.name.IndexOf("-=-", length + 3);
                        if (num != -1)
                        {
                            string str = _tile.name.Substring(length + 3);
                            int.TryParse(str.Substring(0, str.IndexOf("-=-")), out _tile.index);
                            _tile.name = _tile.name.Substring(0, length) + _tile.name.Substring(num + 3);
                        }
                    }
                    _tile.originalFilePath = fileInfo.FullName;
                    this.TrimTile(ref _tile);
                    this.ExtrudeTile(ref _tile);
                    if (_tile.height != 0)
                    {
                        tileList.Add(_tile);
                        if (_tile.width == 0)
                            throw new Exception("?? width should not be at 0, when height != 0");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error collapsing " + fileInfo.Name);
                    throw ex;
                }
            }
            tileList.Sort((Comparison<Tile>)((a, b) =>
           {
               if (a.width > b.width)
                   return -1;
               if (a.width != b.width)
                   return 1;
               if (a.height > b.height)
                   return -1;
               return a.height == b.height ? 0 : 1;
           }));
            Bin2DPacker bin2Dpacker = new Bin2DPacker(new Size(32, 32), new Size(4096, 4096), Bin2DPacker.Algorithm.MaxRects);
            bin2Dpacker.margin = new Size(1, 1);
            bin2Dpacker.marginType = MarginType.All;
            for (int index = 0; index < tileList.Count; ++index)
            {
                if (tileList[index].bitmap != null)
                    bin2Dpacker.InsertElement((uint)index, new Size(tileList[index].width, tileList[index].height), out bool _);
            }
            int count = bin2Dpacker.bins.Count;
            BinaryWriter _writer = (BinaryWriter)null;
            StreamWriter streamWriter = (StreamWriter)null;
            if (_exportBinaryAtlases)
                _writer = new BinaryWriter((Stream)File.OpenWrite(_atlasPath.Substring(0, _atlasPath.Length - 4) + ".atlas"));
            else
                streamWriter = new StreamWriter(_atlasPath.Substring(0, _atlasPath.Length - 4) + ".atlas");
            for (int index = 0; index < count; ++index)
            {
                Bin2D bin = bin2Dpacker.bins[index];
                Bitmap _atlas1 = new Bitmap(bin.size.Width, bin.size.Height);
                Size size = bin.size;
                int width = size.Width;
                size = bin.size;
                int height = size.Height;
                Bitmap _atlas2 = new Bitmap(width, height);
                bool flag = false;
                foreach (KeyValuePair<uint, Rectangle> element in bin.elements)
                {
                    Tile _tile = tileList[(int)element.Key];
                    _tile.x = element.Value.X;
                    _tile.y = element.Value.Y;
                    _tile.atlasIndex = index;
                    this.CopyBitmapToAtlas(_tile, _atlas1);
                    if (_tile.hasNormal)
                    {
                        _tile.bitmap = (Bitmap)Image.FromFile(_tile.originalFilePath.Substring(0, _tile.originalFilePath.Length - 4) + "_n.png");
                        this.ExtrudeTile(ref _tile, true);
                        this.CopyBitmapToAtlas(_tile, _atlas2);
                        flag = true;
                    }
                }
                string filename = new FileInfo(_atlasPath).FullName;
                if (count > 1)
                    filename = filename.Substring(0, filename.Length - 4) + index.ToString() + ".png";
                _atlas1.Save(filename);
                if (flag)
                {
                    _atlas2.Save(filename.Substring(0, filename.Length - 4) + "_n.png");
                    _atlas2.Dispose();
                }
                if (_exportBinaryAtlases)
                {
                    string str = "BATL";
                    _writer.Write(str.ToCharArray());
                    this.WriteString(_writer, filename.Substring(filename.LastIndexOf('\\') + 1));
                    foreach (Tile tile in tileList)
                    {
                        if (tile.duplicateOf != null)
                        {
                            tile.x = tile.duplicateOf.x;
                            tile.y = tile.duplicateOf.y;
                        }
                        this.WriteString(_writer, tile.name);
                        _writer.Write((ushort)tile.index);
                        _writer.Write((int)(ushort)tile.x + 1);
                        _writer.Write((int)(ushort)tile.y + 1);
                        _writer.Write((ushort)tile.width);
                        _writer.Write((ushort)tile.height);
                        _writer.Write((ushort)tile.offsetX);
                        _writer.Write((ushort)tile.offsetY);
                        _writer.Write((ushort)tile.originalWidth);
                        _writer.Write((ushort)tile.originalHeight);
                    }
                    _writer.Write((byte)0);
                }
                else
                {
                    streamWriter.WriteLine("");
                    streamWriter.WriteLine(filename.Substring(filename.LastIndexOf('\\') + 1));
                    streamWriter.WriteLine("size: {0},{1}", (object)_atlas1.Width, (object)_atlas1.Height);
                    streamWriter.WriteLine("format: RGBA8888");
                    streamWriter.WriteLine("filter: Linear,Linear");
                    streamWriter.WriteLine("repeat: none");
                    foreach (Tile tile in tileList)
                    {
                        if (tile.duplicateOf != null)
                        {
                            tile.x = tile.duplicateOf.x;
                            tile.y = tile.duplicateOf.y;
                            tile.atlasIndex = tile.duplicateOf.atlasIndex;
                        }
                        if (tile.atlasIndex == index)
                        {
                            streamWriter.WriteLine(tile.name);
                            streamWriter.WriteLine("  rotate: false");
                            streamWriter.WriteLine("  xy: {0}, {1}", (object)(tile.x + 1), (object)(tile.y + 1));
                            streamWriter.WriteLine("  size: {0}, {1}", (object)(tile.width - 2), (object)(tile.height - 2));
                            streamWriter.WriteLine("  orig: {0}, {1}", (object)(tile.originalWidth - 2), (object)(tile.originalHeight - 2));
                            streamWriter.WriteLine("  offset: {0}, {1}", (object)tile.offsetX, (object)(tile.originalHeight - 2 - (tile.height - 2 + tile.offsetY)));
                            streamWriter.WriteLine("  index: {0}", (object)tile.index);
                        }
                    }
                }
                _atlas1.Dispose();
            }
            if (_exportBinaryAtlases)
            {
                _writer.Write((byte)0);
                _writer.Close();
            }
            else
                streamWriter.Close();
        }

        private void CopyBitmapFromAtlas(Tile _tile, string _path, Bitmap _atlas, string _suffix)
        {
            string[] strArray = _tile.name.Split('/');
            string path1 = _path;
            for (int index = 0; index < strArray.Length - 1; ++index)
                path1 = Directory.CreateDirectory(Path.Combine(path1, strArray[index])).FullName;
            string str = strArray[strArray.Length - 1];
            if (_tile.index != -1)
                str = str + "-=-" + _tile.index.ToString() + "-=-";
            BitmapData bitmapdata1 = _atlas.LockBits(new Rectangle(_tile.x, _tile.y, _tile.width, _tile.height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            Bitmap bitmap = new Bitmap(_tile.originalWidth, _tile.originalHeight);
            BitmapData bitmapdata2 = bitmap.LockBits(new Rectangle(_tile.offsetX, _tile.offsetY, _tile.width, _tile.height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            for (int index = 0; index < _tile.height; ++index)
                Core.CopyMemory(bitmapdata2.Scan0 + index * bitmapdata2.Stride, bitmapdata1.Scan0 + index * bitmapdata1.Stride, (uint)(_tile.width * 4));
            bitmap.UnlockBits(bitmapdata2);
            _atlas.UnlockBits(bitmapdata1);
            string filename = Path.Combine(path1, str + _suffix + ".png");
            bitmap.Save(filename);
            bitmap.Dispose();
        }

        private void TrimTile(ref Tile _tile)
        {
            BitmapData bitmapdata = _tile.bitmap.LockBits(new Rectangle(0, 0, _tile.width, _tile.height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            string base64String;
            lock (AtlasTool.buffer)
            {
                Marshal.Copy(bitmapdata.Scan0, AtlasTool.buffer, 0, _tile.originalWidth * _tile.originalHeight * 4);
                base64String = Convert.ToBase64String(SHA256.Create().ComputeHash(AtlasTool.buffer, 0, _tile.originalWidth * _tile.originalHeight * 4));
            }
            Tile tile;
            if (!this.hashes.TryGetValue(base64String, out tile))
            {
                this.hashes.Add(base64String, _tile);
                bool flag1 = false;
                for (int index1 = 0; index1 < _tile.originalHeight && !flag1 && _tile.height > 1; ++index1)
                {
                    for (int index2 = 0; index2 < _tile.originalWidth && !flag1 && _tile.height > 1; ++index2)
                        flag1 = ((ulong)Marshal.ReadInt32(bitmapdata.Scan0 + (index1 * _tile.originalWidth + index2) * 4) & 4278190080UL) > 0UL;
                    if (!flag1)
                    {
                        ++_tile.offsetY;
                        --_tile.height;
                    }
                }
                bool flag2 = false;
                for (int index = 0; index < _tile.originalWidth && !flag2 && _tile.width > 1; ++index)
                {
                    for (int offsetY = _tile.offsetY; offsetY < _tile.originalHeight && !flag2 && _tile.width > 1; ++offsetY)
                        flag2 = ((ulong)Marshal.ReadInt32(bitmapdata.Scan0 + (offsetY * _tile.originalWidth + index) * 4) & 4278190080UL) > 0UL;
                    if (!flag2)
                    {
                        ++_tile.offsetX;
                        --_tile.width;
                    }
                }
                bool flag3 = false;
                for (int index = _tile.originalHeight - 1; index >= _tile.offsetY && !flag3 && _tile.height > 1; --index)
                {
                    for (int offsetX = _tile.offsetX; offsetX < _tile.originalWidth && !flag3 && _tile.height > 1; ++offsetX)
                        flag3 = ((ulong)Marshal.ReadInt32(bitmapdata.Scan0 + (index * _tile.originalWidth + offsetX) * 4) & 4278190080UL) > 0UL;
                    if (!flag3)
                        --_tile.height;
                }
                bool flag4 = false;
                for (int index = _tile.originalWidth - 1; index >= _tile.offsetX && !flag4 && _tile.width > 1; --index)
                {
                    for (int offsetY = _tile.offsetY; offsetY < _tile.originalHeight && !flag4 && _tile.width > 1; ++offsetY)
                        flag4 = ((ulong)Marshal.ReadInt32(bitmapdata.Scan0 + (offsetY * _tile.originalWidth + index) * 4) & 4278190080UL) > 0UL;
                    if (!flag4)
                        --_tile.width;
                }
                _tile.bitmap.UnlockBits(bitmapdata);
            }
            else
            {
                _tile.duplicateOf = tile;
                _tile.x = tile.x;
                _tile.y = tile.y;
                _tile.offsetX = tile.offsetX;
                _tile.offsetY = tile.offsetY;
                _tile.originalWidth = tile.originalWidth;
                _tile.originalHeight = tile.originalHeight;
                _tile.width = tile.width;
                _tile.height = tile.height;
                _tile.bitmap = (Bitmap)null;
            }
        }

        private void ExtrudeTile(ref Tile _tile, bool _bForceBitmapResizeAndPreventUpdateTileInfo = false)
        {
            if (_tile.bitmap == null)
                return;
            string str = "none";
            try
            {
                if (_bForceBitmapResizeAndPreventUpdateTileInfo || _tile.offsetX == 0 || _tile.offsetY == 0 || _tile.originalWidth - _tile.width < _tile.offsetX + 2 || _tile.originalHeight - _tile.height < _tile.offsetY + 2)
                {
                    int width = _tile.bitmap.Width;
                    int height = _tile.bitmap.Height;
                    Bitmap bitmap = new Bitmap(width + 2, height + 2);
                    str = "reading old";
                    BitmapData bitmapdata1 = _tile.bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    str = "reading new";
                    BitmapData bitmapdata2 = bitmap.LockBits(new Rectangle(1, 1, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    for (int index = 0; index < height; ++index)
                        Core.CopyMemory(bitmapdata2.Scan0 + index * bitmapdata2.Stride, bitmapdata1.Scan0 + index * bitmapdata1.Stride, (uint)(width * 4));
                    _tile.bitmap.UnlockBits(bitmapdata1);
                    bitmap.UnlockBits(bitmapdata2);
                    _tile.bitmap.Dispose();
                    _tile.bitmap = bitmap;
                    if (!_bForceBitmapResizeAndPreventUpdateTileInfo)
                    {
                        ++_tile.offsetX;
                        ++_tile.offsetY;
                        _tile.originalHeight += 2;
                        _tile.originalWidth += 2;
                    }
                }
                if (!_bForceBitmapResizeAndPreventUpdateTileInfo)
                {
                    --_tile.offsetX;
                    --_tile.offsetY;
                    _tile.width += 2;
                    _tile.height += 2;
                }
                str = "writing";
                string.Format("_tile.offsetX = {0}, _tile.offsetY = {1}, _tile.width = {2}, _tile.height = {3}", (object)_tile.offsetX, (object)_tile.offsetY, (object)_tile.width, (object)_tile.height);
                BitmapData bitmapdata = _tile.bitmap.LockBits(new Rectangle(_tile.offsetX, _tile.offsetY, _tile.width, _tile.height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                for (int index = 0; index < _tile.height; ++index)
                {
                    Marshal.WriteInt32(bitmapdata.Scan0 + index * bitmapdata.Stride, Marshal.ReadInt32(bitmapdata.Scan0 + index * bitmapdata.Stride + 4));
                    Marshal.WriteInt32(bitmapdata.Scan0 + index * bitmapdata.Stride + (_tile.width - 1) * 4, Marshal.ReadInt32(bitmapdata.Scan0 + index * bitmapdata.Stride + (_tile.width - 2) * 4));
                }
                int num = (_tile.height - 1) * bitmapdata.Stride;
                for (int index = 0; index < _tile.width; ++index)
                {
                    Marshal.WriteInt32(bitmapdata.Scan0 + index * 4, Marshal.ReadInt32(bitmapdata.Scan0 + bitmapdata.Stride + index * 4));
                    int val = Marshal.ReadInt32(bitmapdata.Scan0 + num - bitmapdata.Stride + index * 4);
                    Marshal.WriteInt32(bitmapdata.Scan0 + num + index * 4, val);
                }
                _tile.bitmap.UnlockBits(bitmapdata);
            }
            catch (Exception ex)
            {
                Log.Error("Problem " + str + " " + _tile.name + " when extruding");
                throw ex;
            }
        }

        private void CopyBitmapToAtlas(Tile _tile, Bitmap _atlas)
        {
            if (_tile.bitmap == null)
                return;
            BitmapData bitmapdata1 = _atlas.LockBits(new Rectangle(_tile.x, _tile.y, _tile.width, _tile.height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData bitmapdata2 = _tile.bitmap.LockBits(new Rectangle(_tile.offsetX, _tile.offsetY, _tile.width, _tile.height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            for (int index = 0; index < _tile.height; ++index)
                Core.CopyMemory(bitmapdata1.Scan0 + index * bitmapdata1.Stride, bitmapdata2.Scan0 + index * bitmapdata2.Stride, (uint)(_tile.width * 4));
            _atlas.UnlockBits(bitmapdata1);
            _tile.bitmap.UnlockBits(bitmapdata2);
        }

        private string ReadString(BinaryReader _reader)
        {
            int count = (int)_reader.ReadByte();
            if (count == (int)byte.MaxValue)
                count = (int)_reader.ReadUInt16();
            return count != 0 ? new string(_reader.ReadChars(count)) : "";
        }

        private void WriteString(BinaryWriter _writer, string _stringToWrite)
        {
            if (_stringToWrite.Length >= (int)byte.MaxValue)
                _writer.Write((ushort)_stringToWrite.Length);
            else
                _writer.Write((byte)_stringToWrite.Length);
            _writer.Write(_stringToWrite.ToCharArray());
        }
    }
}
