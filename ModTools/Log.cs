// Decompiled with JetBrains decompiler
// Type: ModTools.Log
// Assembly: Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE2489D0-6C26-4149-81CC-2AB9D282DECD
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\Common.dll

using System;

#nullable disable
namespace ModTools
{
  public class Log
  {
    public static Log.Level level { get; set; } = Log.Level.Error;

    public static void Error(string _message, string _callstack = "")
    {
      if (Log.level < Log.Level.Error)
        return;
      int foregroundColor = (int) Console.ForegroundColor;
      Console.ForegroundColor = ConsoleColor.DarkRed;
      Console.WriteLine(_message);
      if (_callstack != "")
        Console.Write(_callstack);
      Console.ForegroundColor = (ConsoleColor) foregroundColor;
    }

    public static void Message(string _message)
    {
      if (Log.level < Log.Level.Message)
        return;
      Console.WriteLine(_message);
    }

    public enum Level
    {
      NoMessage,
      Error,
      Warning,
      Message,
      VerboseMessage,
      Full,
    }
  }
}
