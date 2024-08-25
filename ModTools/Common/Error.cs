// Decompiled with JetBrains decompiler
// Type: ModTools.Error
// Assembly: Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE2489D0-6C26-4149-81CC-2AB9D282DECD
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\Common.dll

using System;
using System.Windows.Forms;

#nullable disable
namespace ModTools
{
  public class Error
  {
    public static void Show(Exception _e, bool _bShowMessage)
    {
      Error.Show(_bShowMessage, _e.Message, _e.StackTrace);
    }

    public static void Show(bool _bShowMsgBox, string _message, string _callstack)
    {
      if (_bShowMsgBox)
      {
        int num = (int) MessageBox.Show(_message, nameof (Error), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Log.Error(_message, _callstack);
    }
  }
}
