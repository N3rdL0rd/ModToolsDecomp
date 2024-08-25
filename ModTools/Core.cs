// Decompiled with JetBrains decompiler
// Type: ModTools.Core
// Assembly: Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE2489D0-6C26-4149-81CC-2AB9D282DECD
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\Common.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace ModTools
{
  public class Core
  {
    [DllImport("kernel32.dll")]
    public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
  }
}
