// Decompiled with JetBrains decompiler
// Type: ModTools.CastleJsonTextWriter
// Assembly: Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE2489D0-6C26-4149-81CC-2AB9D282DECD
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\Common.dll

using Newtonsoft.Json;
using System.IO;

#nullable disable
namespace ModTools
{
  public class CastleJsonTextWriter : JsonTextWriter
  {
    public CastleJsonTextWriter(TextWriter _writer)
      : base(_writer)
    {
    }

    public override void WriteValue(float _value)
    {
      if ((double) _value == (double) (int) _value)
        this.WriteValue((int) _value);
      else
        base.WriteValue(_value);
    }

    public override void WriteValue(float? _value)
    {
      if (_value.HasValue)
      {
        float? nullable = _value;
        float num = (float) (int) _value.Value;
        if ((double) nullable.GetValueOrDefault() == (double) num & nullable.HasValue)
        {
          this.WriteValue((int) _value.Value);
          return;
        }
      }
      base.WriteValue(_value);
    }
  }
}
