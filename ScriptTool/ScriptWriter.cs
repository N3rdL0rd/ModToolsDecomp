// Decompiled with JetBrains decompiler
// Type: ScriptTool.ScriptWriter
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

using System.Collections.Generic;

#nullable disable
namespace ScriptTool
{
  internal class ScriptWriter
  {
    private static ScriptWriter m_Instance;
    private List<ScriptSection> m_ScriptSections = new List<ScriptSection>();

    public static ScriptWriter instance
    {
      get
      {
        if (ScriptWriter.m_Instance == null)
        {
          ScriptWriter.m_Instance = new ScriptWriter();
          ScriptWriter.m_Instance.m_ScriptSections.Add((ScriptSection) new BuildMainRooms());
          ScriptWriter.m_Instance.m_ScriptSections.Add((ScriptSection) new BuildSecondaryRooms());
          ScriptWriter.m_Instance.m_ScriptSections.Add((ScriptSection) new BuildTriggeredDoors());
          ScriptWriter.m_Instance.m_ScriptSections.Add((ScriptSection) new BuildTimedDoors());
          ScriptWriter.m_Instance.m_ScriptSections.Add((ScriptSection) new AddTeleports());
          ScriptWriter.m_Instance.m_ScriptSections.Add((ScriptSection) new Finalize());
          ScriptWriter.m_Instance.m_ScriptSections.Add((ScriptSection) new BuildMobRoster());
          ScriptWriter.m_Instance.m_ScriptSections.Add((ScriptSection) new SetLevelInfo());
          ScriptWriter.m_Instance.m_ScriptSections.Add((ScriptSection) new SetLevelProps());
        }
        return ScriptWriter.m_Instance;
      }
    }

    public string WriteWholeScript()
    {
      string str = "";
      foreach (ScriptSection scriptSection in this.m_ScriptSections)
        str = str + scriptSection.ToString() + "\r\n\r\n";
      return str;
    }

    private ScriptWriter()
    {
    }
  }
}
