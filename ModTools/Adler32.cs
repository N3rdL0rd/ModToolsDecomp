// Decompiled with JetBrains decompiler
// Type: ModTools.Adler32
// Assembly: Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE2489D0-6C26-4149-81CC-2AB9D282DECD
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\Common.dll

using System.IO;

#nullable disable
namespace ModTools
{
  public class Adler32
  {
    private int m_A1;
    private int m_A2;

    public int value => this.m_A2 << 16 | this.m_A1;

    public Adler32()
    {
      this.m_A1 = 1;
      this.m_A2 = 0;
    }

    public void Update(byte[] _bytes, int _position, int _length)
    {
      int num = _position + _length;
      for (int index = _position; index < num; ++index)
      {
        this.m_A1 = (this.m_A1 + (int) _bytes[index]) % 65521;
        this.m_A2 = (this.m_A2 + this.m_A1) % 65521;
      }
    }

    public int Make(Stream _stream)
    {
      return this.Make(new BinaryReader(_stream).ReadBytes((int) _stream.Length));
    }

    public int Make(byte[] _bytes)
    {
      this.m_A1 = 1;
      this.m_A2 = 0;
      this.Update(_bytes, 0, _bytes.Length);
      return this.value;
    }
  }
}
