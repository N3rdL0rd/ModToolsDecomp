// Decompiled with JetBrains decompiler
// Type: CDBTool.CDBTool
// Assembly: CDBTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 42BFB734-5A26-4F73-8548-F69108A06C4F
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\CDBTool.exe

using Microsoft.CSharp.RuntimeBinder;
using ModTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace CDBTool
{
  public class CDBTool
  {
    public const string structureFileName = "__STRUCTURE__.json";
    public const string propertyFileName = "__PROPS__.json";
    public const string columnsNodeName = "__columns";
    public const string tableIndexName = "__table_index";
    public const string separatorGroupIDName = "__separator_group_ID";
    public const string separatorGroupNameName = "__separator_group_Name";
    public const string originalIndexName = "__original_Index";
    public bool bMultiThread = true;

    public void Expand(string _sourceCDBPath, string _output)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(_output);
      if (!File.Exists(_sourceCDBPath))
        throw new FileNotFoundException("CDB file not found : " + _sourceCDBPath, _sourceCDBPath);
      directoryInfo.Create();
      string[] strArray = new string[5]
      {
        "id",
        "item",
        "name",
        "room",
        "animId"
      };
      JObject jobject1 = (JObject) JsonConvert.DeserializeObject(File.ReadAllText(_sourceCDBPath));
      int num1 = 0;
      foreach (JObject jobject2 in (IEnumerable<JToken>) jobject1["sheets"])
      {
        string path = jobject2["name"].ToString();
        DirectoryInfo subdirectory = directoryInfo.CreateSubdirectory(path);
        JArray jarray1 = jobject2.Value<JArray>((object) "columns");
        JObject jobject3 = new JObject();
        jobject3.Add("__columns", (JToken) jarray1);
        jobject3.Add("__table_index", (JToken) num1);
        if (this.bMultiThread)
          this.WriteContentAsync(Path.Combine(subdirectory.FullName, "__STRUCTURE__.json"), jobject3.ToString()).ContinueWith((Action<Task>) (t => Error.Show((Exception) t.Exception, false)), TaskContinuationOptions.OnlyOnFaulted);
        else
          CDBTool.CDBTool.WriteContentSync(Path.Combine(subdirectory.FullName, "__STRUCTURE__.json"), jobject3.ToString());
        List<Separator> separatorList = new List<Separator>();
        JArray jarray2 = jobject2.Value<JArray>((object) "separators");
        JArray jarray3 = (JArray) null;
        if (jarray2 != null)
        {
          JObject jobject4 = jobject2.Value<JObject>((object) "props");
          if (jobject4 != null)
            jarray3 = jobject4.Value<JArray>((object) "separatorTitles");
        }
        if (jarray3 != null)
        {
          List<int> intList = new List<int>();
          foreach (JToken jtoken in jarray2)
          {
            int num2 = (int) jtoken;
            intList.Add(num2);
          }
          List<string> stringList = new List<string>();
          foreach (JToken jtoken in jarray3)
          {
            string str = (string) jtoken;
            stringList.Add(str);
          }
          for (int index = 0; index < intList.Count; ++index)
            separatorList.Add(new Separator(index, stringList[index], intList[index]));
        }
        int count = separatorList.Count;
        if (count > 0)
          separatorList.Sort((Comparison<Separator>) ((a, b) => a.lineIndex.CompareTo(b.lineIndex)));
        JObject jobject5 = (JObject) jobject2["props"];
        jobject5.Remove("separatorTitles");
        if (this.bMultiThread)
          this.WriteContentAsync(Path.Combine(subdirectory.FullName, "__PROPS__.json"), jobject5.ToString()).ContinueWith((Action<Task>) (t => Error.Show((Exception) t.Exception, false)), TaskContinuationOptions.OnlyOnFaulted);
        else
          CDBTool.CDBTool.WriteContentSync(Path.Combine(subdirectory.FullName, "__PROPS__.json"), jobject5.ToString());
        int num3 = 0;
        string propertyName = "";
        JArray jarray4 = jobject2.Value<JArray>((object) "lines");
        string format = "D" + (object) ((int) Math.Floor(Math.Log10((double) jarray4.Count)) + 1);
        int index1 = -1;
        foreach (JObject jobject6 in jarray4)
        {
          while (count > 0 && index1 + 1 < count && num3 >= separatorList[index1 + 1].lineIndex)
            ++index1;
          jobject6.Add("__separator_group_ID", (JToken) index1);
          string path2 = "";
          if (index1 >= 0 && index1 < count)
            path2 = separatorList[index1].name;
          jobject6.Add("__separator_group_Name", (JToken) path2);
          jobject6.Add("__original_Index", (JToken) num3);
          string str1 = (string) null;
          if (propertyName == "")
          {
            for (int index2 = 0; index2 < strArray.Length; ++index2)
            {
              if (str1 == null)
              {
                try
                {
                  str1 = jobject6[strArray[index2]].ToString();
                  propertyName = strArray[index2];
                }
                catch (Exception ex)
                {
                }
              }
              else
                break;
            }
          }
          else
          {
            try
            {
              str1 = jobject6[propertyName].ToString();
            }
            catch (Exception ex)
            {
            }
          }
          string str2 = num3.ToString(format);
          if (string.IsNullOrEmpty(str1))
            str1 = str2 + "-UnnamedLine";
          string _content = jobject6.ToString();
          string str3 = Path.Combine(subdirectory.FullName, path2);
          Directory.CreateDirectory(str3);
          if (this.bMultiThread)
            this.WriteContentAsync(Path.Combine(str3, str2 + "---" + str1 + ".json"), _content).ContinueWith((Action<Task>) (t => Error.Show((Exception) t.Exception, false)), TaskContinuationOptions.OnlyOnFaulted);
          else
            CDBTool.CDBTool.WriteContentSync(Path.Combine(subdirectory.FullName, path2, str2 + "---" + str1 + ".json"), _content);
          ++num3;
        }
        ++num1;
      }
    }

    public void Collapse(string _rootInput, string _destCDBPath)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(_rootInput);
      if (!directoryInfo.Exists)
        throw new DirectoryNotFoundException("Input directory not found : " + _rootInput);
      JObject _root = new JObject();
      JArray jarray = new JArray();
      _root.Add("sheets", (JToken) jarray);
      List<KeyValuePair<int, JObject>> keyValuePairList1 = new List<KeyValuePair<int, JObject>>();
      foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
      {
        JObject jobject1 = new JObject();
        JProperty content1 = new JProperty("name", (object) directory.Name);
        JArray content2 = new JArray();
        JObject content3 = (JObject) null;
        JArray content4 = new JArray();
        JProperty content5 = new JProperty("lines", (object) content2);
        JProperty content6 = new JProperty("separators", (object) content4);
        jobject1.Add((object) content1);
        List<Separator> separatorList = new List<Separator>();
        foreach (FileInfo file in directory.GetFiles("*.json", SearchOption.AllDirectories))
        {
          if (file.Name != "__STRUCTURE__.json" && file.Name != "__PROPS__.json")
          {
            object obj1 = JsonConvert.DeserializeObject(File.ReadAllText(file.FullName));
            // ISSUE: reference to a compiler-generated field
            if (CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__1 == null)
            {
              // ISSUE: reference to a compiler-generated field
              CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, JObject>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (JObject), typeof (CDBTool.CDBTool)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, JObject> target = CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__1.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, JObject>> p1 = CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__1;
            // ISSUE: reference to a compiler-generated field
            if (CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeepClone", (IEnumerable<Type>) null, typeof (CDBTool.CDBTool), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj2 = CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__0.Target((CallSite) CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__0, obj1);
            JObject jobject2 = target((CallSite) p1, obj2);
            int _id = jobject2.Value<int>((object) "__separator_group_ID");
            string name = jobject2.Value<string>((object) "__separator_group_Name");
            Separator separator1 = new Separator(_id, name, 0);
            if (_id != -1 && separatorList.Find((Predicate<Separator>) (s => s.name == name)) == null)
            {
              bool flag = false;
              do
              {
                if (separatorList.Count != 0)
                {
                  int index = 0;
                  while (index < separatorList.Count && separatorList[index].id != separator1.id)
                    ++index;
                  flag = index < separatorList.Count;
                  if (flag)
                  {
                    foreach (Separator separator2 in separatorList)
                    {
                      if (separator2.id > separator1.id)
                        ++separator2.id;
                    }
                    ++separator1.id;
                  }
                }
              }
              while (flag);
              separatorList.Add(separator1);
            }
          }
        }
        List<KeyValuePair<long, int>> keyValuePairList2 = new List<KeyValuePair<long, int>>();
        List<JObject> jobjectList = new List<JObject>();
        foreach (FileInfo file in directory.GetFiles("*.json", SearchOption.AllDirectories))
        {
          object obj3 = JsonConvert.DeserializeObject(File.ReadAllText(file.FullName));
          if (file.Name == "__STRUCTURE__.json")
          {
            // ISSUE: reference to a compiler-generated field
            if (CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__2 == null)
            {
              // ISSUE: reference to a compiler-generated field
              CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, JObject>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (JObject), typeof (CDBTool.CDBTool)));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            JObject jobject3 = CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__2.Target((CallSite) CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__2, obj3);
            jobject1.Add((object) new JProperty("columns", (object) jobject3.Value<JArray>((object) "__columns").DeepClone()));
            keyValuePairList1.Add(new KeyValuePair<int, JObject>(jobject3.Value<int>((object) "__table_index"), jobject1));
          }
          else if (file.Name == "__PROPS__.json")
          {
            // ISSUE: reference to a compiler-generated field
            if (CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, JObject>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (JObject), typeof (CDBTool.CDBTool)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, JObject> target = CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__4.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, JObject>> p4 = CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__4;
            // ISSUE: reference to a compiler-generated field
            if (CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeepClone", (IEnumerable<Type>) null, typeof (CDBTool.CDBTool), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj4 = CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__3.Target((CallSite) CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__3, obj3);
            content3 = target((CallSite) p4, obj4);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__6 == null)
            {
              // ISSUE: reference to a compiler-generated field
              CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, JObject>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (JObject), typeof (CDBTool.CDBTool)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, JObject> target = CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__6.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, JObject>> p6 = CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__6;
            // ISSUE: reference to a compiler-generated field
            if (CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeepClone", (IEnumerable<Type>) null, typeof (CDBTool.CDBTool), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj5 = CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__5.Target((CallSite) CDBTool.CDBTool.\u003C\u003Eo__9.\u003C\u003Ep__5, obj3);
            JObject jobject4 = target((CallSite) p6, obj5);
            int num1 = -1;
            string str1 = jobject4.Value<string>((object) "__separator_group_Name");
            if (separatorList.Count > 0 && !string.IsNullOrEmpty(str1))
            {
              if (separatorList.Count == 0 || str1 == "")
                ;
              int index = 0;
              while (index < separatorList.Count && separatorList[index].name != str1)
                ++index;
              num1 = separatorList[index].id;
            }
            for (int index = 0; index < separatorList.Count; ++index)
            {
              if (separatorList[index].id > num1)
                separatorList[index].pushLine();
              else if (separatorList[index].id == num1)
              {
                string str2 = jobject4.Value<string>((object) "__separator_group_Name");
                if (separatorList[index].name != str2)
                  separatorList[index].pushLine();
              }
            }
            int num2 = jobject4.Value<int>((object) "__original_Index");
            keyValuePairList2.Add(new KeyValuePair<long, int>((long) num2 << 32 | (long) (num1 + 1), jobjectList.Count));
            jobjectList.Add(jobject4);
            jobject4.Remove("__separator_group_ID");
            jobject4.Remove("__separator_group_Name");
            jobject4.Remove("__original_Index");
          }
        }
        keyValuePairList2.Sort((Comparison<KeyValuePair<long, int>>) ((a, b) => a.Key.CompareTo(b.Key)));
        for (int index = 0; index < keyValuePairList2.Count; ++index)
          content2.Add((JToken) jobjectList[keyValuePairList2[index].Value]);
        separatorList.Sort((Comparison<Separator>) ((a, b) => a.lineIndex.CompareTo(b.lineIndex)));
        JArray content7 = new JArray();
        JProperty content8 = new JProperty("separatorTitles", (object) content7);
        content3.AddFirst((object) content8);
        foreach (Separator separator in separatorList)
        {
          if (separator.lineIndex > -1)
          {
            content4.Add((JToken) separator.lineIndex);
            content7.Add((JToken) separator.name);
          }
        }
        jobject1.Add((object) content5);
        jobject1.Add((object) content6);
        jobject1.Add((object) new JProperty("props", (object) content3));
      }
      keyValuePairList1.Sort((Comparison<KeyValuePair<int, JObject>>) ((a, b) => a.Key.CompareTo(b.Key)));
      for (int index = 0; index < keyValuePairList1.Count; ++index)
        jarray.Add((JToken) keyValuePairList1[index].Value);
      _root.Add("compress", (JToken) false);
      _root.Add("customTypes", (JToken) new JArray());
      Directory.CreateDirectory(new FileInfo(_destCDBPath).Directory.FullName);
      string cdbjObjectAsString = CDBTool.CDBTool.GetCDBJObjectAsString(_root);
      this.WriteContentAsync(_destCDBPath, cdbjObjectAsString).Wait();
    }

    public static string GetCDBJObjectAsString(JObject _root)
    {
      StringWriter _writer = new StringWriter();
      CastleJsonTextWriter castleJsonTextWriter1 = new CastleJsonTextWriter((TextWriter) _writer);
      castleJsonTextWriter1.Formatting = Formatting.Indented;
      castleJsonTextWriter1.Indentation = 1;
      castleJsonTextWriter1.IndentChar = '\t';
      CastleJsonTextWriter castleJsonTextWriter2 = castleJsonTextWriter1;
      new JsonSerializer().Serialize((JsonWriter) castleJsonTextWriter2, (object) _root);
      string cdbjObjectAsString = _writer.ToString().Replace("\r", "");
      castleJsonTextWriter2.Close();
      _writer.Close();
      return cdbjObjectAsString;
    }

    public void BuildDiffCDB(string _referenceCDBPath, string _inputDirPath, string _outputDirPath)
    {
      DirectoryInfo _rootDir = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
      string fullName = _rootDir.FullName;
      this.Expand(_referenceCDBPath, fullName);
      Dictionary<string, CDBTool.CDBTool.Test> dictionary1 = this.BuildCRCFileMap(_rootDir);
      Dictionary<string, CDBTool.CDBTool.Test> dictionary2 = this.BuildCRCFileMap(new DirectoryInfo(_inputDirPath));
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, CDBTool.CDBTool.Test> keyValuePair in dictionary2)
      {
        CDBTool.CDBTool.Test test;
        if (dictionary1.TryGetValue(keyValuePair.Key, out test))
        {
          if (test.i != keyValuePair.Value.i)
            stringList.Add(keyValuePair.Value.originalFileName);
        }
        else
          stringList.Add(keyValuePair.Value.originalFileName);
      }
      if (stringList.Count > 0)
      {
        new DirectoryInfo(_outputDirPath).Create();
        foreach (string str in stringList)
        {
          FileInfo fileInfo = new FileInfo(Path.Combine(_outputDirPath, this.GetOriginalName(str)));
          Directory.CreateDirectory(fileInfo.Directory.FullName);
          File.Copy(Path.Combine(_inputDirPath, str), fileInfo.FullName, true);
        }
      }
      _rootDir.Delete(true);
    }

    private Dictionary<string, CDBTool.CDBTool.Test> BuildCRCFileMap(DirectoryInfo _rootDir)
    {
      int length = _rootDir.FullName.Length;
      Dictionary<string, CDBTool.CDBTool.Test> dictionary = new Dictionary<string, CDBTool.CDBTool.Test>();
      List<DirectoryInfo> directoryInfoList = new List<DirectoryInfo>();
      directoryInfoList.Add(_rootDir);
      Adler32 adler32 = new Adler32();
      for (int index = 0; index < directoryInfoList.Count; ++index)
      {
        foreach (DirectoryInfo directory in directoryInfoList[index].GetDirectories())
          directoryInfoList.Add(directory);
        foreach (FileInfo file in directoryInfoList[index].GetFiles())
        {
          string originalName = this.GetOriginalName(file.FullName.Substring(length + 1));
          try
          {
            JObject jobject = (JObject) JsonConvert.DeserializeObject(File.ReadAllText(file.FullName));
            int _i = !jobject.Remove("__original_Index") ? adler32.Make((Stream) File.OpenRead(file.FullName)) : adler32.Make((Stream) new MemoryStream(Encoding.UTF8.GetBytes(jobject.ToString())));
            dictionary.Add(originalName, new CDBTool.CDBTool.Test(_i, jobject, file.FullName.Substring(length + 1)));
          }
          catch (Exception ex)
          {
            Log.Error("Failing to build CRC for file : " + originalName + " from " + file.FullName);
            throw ex;
          }
        }
      }
      return dictionary;
    }

    private string GetOriginalName(string _indexedAndCategorizedName)
    {
      string str1 = _indexedAndCategorizedName;
      string[] collection = _indexedAndCategorizedName.Split('\\');
      if (collection.Length == 0)
        collection = str1.Split('/');
      List<string> stringList = new List<string>((IEnumerable<string>) collection);
      while (stringList.Count > 2)
        stringList.RemoveAt(1);
      string str2 = stringList[stringList.Count - 1];
      int num = str2.IndexOf("---");
      if (num != -1)
        stringList[stringList.Count - 1] = str2.Substring(num + 3);
      return Path.Combine(stringList.ToArray());
    }

    private static void WriteContentSync(string _fileName, string _content)
    {
      StreamWriter text = new FileInfo(_fileName).CreateText();
      text.Write(_content);
      text.Close();
    }

    private async Task WriteContentAsync(string _fileName, string _content)
    {
      StreamWriter writer = new FileInfo(_fileName).CreateText();
      await writer.WriteAsync(_content);
      writer.Close();
    }

    private class Test
    {
      public int i;
      public JObject obj;
      public string originalFileName;

      public Test(int _i, JObject _obj, string _originalFileName)
      {
        this.i = _i;
        this.obj = _obj;
        this.originalFileName = _originalFileName;
      }
    }
  }
}
