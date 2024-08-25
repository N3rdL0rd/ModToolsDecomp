// Decompiled with JetBrains decompiler
// Type: ScriptTool.MobList
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

#nullable disable
namespace ScriptTool
{
  internal class MobList
  {
    public static List<string> names { get; private set; } = new List<string>();

    public static List<string> ids { get; private set; } = new List<string>();

    public static void InitMobList(string _CDBJson)
    {
      MobList.names.Clear();
      JArray jarray = (JArray) ((JObject) JsonConvert.DeserializeObject(_CDBJson))["sheets"];
      JObject jobject = (JObject) null;
      foreach (JToken jtoken in jarray)
      {
        if (jtoken[(object) "name"].ToString() == "mob")
        {
          jobject = (JObject) jtoken;
          break;
        }
      }
      int result;
      int.TryParse(jobject["separators"][(object) 1].ToString(), out result);
      for (int key = 0; key < result; ++key)
      {
        JToken jtoken = jobject["lines"][(object) key];
        MobList.names.Add(jtoken[(object) "name"].ToString());
        MobList.ids.Add(jtoken[(object) "id"].ToString());
      }
    }
  }
}
