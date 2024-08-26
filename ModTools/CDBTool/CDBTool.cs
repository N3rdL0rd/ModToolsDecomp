using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using ModTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CDBTool
{
	// Token: 0x02000002 RID: 2
	public class CDBTool
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public void Expand(string _sourceCDBPath, string _output)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(_output);
			if (!File.Exists(_sourceCDBPath))
			{
				throw new FileNotFoundException("CDB file not found : " + _sourceCDBPath, _sourceCDBPath);
			}
			directoryInfo.Create();
			string[] array = new string[]
			{
				"id",
				"item",
				"name",
				"room",
				"animId"
			};
			JObject jobject = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(_sourceCDBPath));
			int num = 0;
			foreach (JToken jtoken in ((IEnumerable<JToken>)jobject["sheets"]))
			{
				JObject jobject2 = (JObject)jtoken;
				string path = jobject2["name"].ToString();
				DirectoryInfo directoryInfo2 = directoryInfo.CreateSubdirectory(path);
				JArray value = jobject2.Value<JArray>("columns");
				JObject jobject3 = new JObject();
				jobject3.Add("__columns", value);
				jobject3.Add("__table_index", num);
				if (this.bMultiThread)
				{
					this.WriteContentAsync(Path.Combine(directoryInfo2.FullName, "__STRUCTURE__.json"), jobject3.ToString()).ContinueWith(delegate(Task t)
					{
						Error.Show(t.Exception, false);
					}, TaskContinuationOptions.OnlyOnFaulted);
				}
				else
				{
					CDBTool.WriteContentSync(Path.Combine(directoryInfo2.FullName, "__STRUCTURE__.json"), jobject3.ToString());
				}
				List<Separator> list = new List<Separator>();
				JArray jarray = jobject2.Value<JArray>("separators");
				JArray jarray2 = null;
				if (jarray != null)
				{
					JObject jobject4 = jobject2.Value<JObject>("props");
					if (jobject4 != null)
					{
						jarray2 = jobject4.Value<JArray>("separatorTitles");
					}
				}
				if (jarray2 != null)
				{
					List<int> list2 = new List<int>();
					foreach (JToken value2 in jarray)
					{
						int item = (int)value2;
						list2.Add(item);
					}
					List<string> list3 = new List<string>();
					foreach (JToken value3 in jarray2)
					{
						string item2 = (string)value3;
						list3.Add(item2);
					}
					for (int i = 0; i < list2.Count; i++)
					{
						list.Add(new Separator(i, list3[i], list2[i]));
					}
				}
				int count = list.Count;
				if (count > 0)
				{
					list.Sort((Separator a, Separator b) => a.lineIndex.CompareTo(b.lineIndex));
				}
				JObject jobject5 = (JObject)jobject2["props"];
				jobject5.Remove("separatorTitles");
				if (this.bMultiThread)
				{
					this.WriteContentAsync(Path.Combine(directoryInfo2.FullName, "__PROPS__.json"), jobject5.ToString()).ContinueWith(delegate(Task t)
					{
						Error.Show(t.Exception, false);
					}, TaskContinuationOptions.OnlyOnFaulted);
				}
				else
				{
					CDBTool.WriteContentSync(Path.Combine(directoryInfo2.FullName, "__PROPS__.json"), jobject5.ToString());
				}
				int num2 = 0;
				string text = "";
				JArray jarray3 = jobject2.Value<JArray>("lines");
				int num3 = (int)Math.Floor(Math.Log10((double)jarray3.Count)) + 1;
				string format = "D" + num3;
				int num4 = -1;
				foreach (JToken jtoken2 in jarray3)
				{
					JObject jobject6 = (JObject)jtoken2;
					while (count > 0 && num4 + 1 < count && num2 >= list[num4 + 1].lineIndex)
					{
						num4++;
					}
					jobject6.Add("__separator_group_ID", num4);
					string text2 = "";
					if (num4 >= 0 && num4 < count)
					{
						text2 = list[num4].name;
					}
					jobject6.Add("__separator_group_Name", text2);
					jobject6.Add("__original_Index", num2);
					string text3 = null;
					if (text == "")
					{
						for (int j = 0; j < array.Length; j++)
						{
							if (text3 != null)
							{
								break;
							}
							try
							{
								text3 = jobject6[array[j]].ToString();
								text = array[j];
							}
							catch (Exception)
							{
							}
						}
					}
					else
					{
						try
						{
							text3 = jobject6[text].ToString();
						}
						catch (Exception)
						{
						}
					}
					string str = num2.ToString(format);
					if (string.IsNullOrEmpty(text3))
					{
						text3 = str + "-UnnamedLine";
					}
					string content = jobject6.ToString();
					string text4 = Path.Combine(directoryInfo2.FullName, text2);
					Directory.CreateDirectory(text4);
					if (this.bMultiThread)
					{
						this.WriteContentAsync(Path.Combine(text4, str + "---" + text3 + ".json"), content).ContinueWith(delegate(Task t)
						{
							Error.Show(t.Exception, false);
						}, TaskContinuationOptions.OnlyOnFaulted);
					}
					else
					{
						CDBTool.WriteContentSync(Path.Combine(directoryInfo2.FullName, text2, str + "---" + text3 + ".json"), content);
					}
					num2++;
				}
				num++;
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002634 File Offset: 0x00000834
		public void Collapse(string _rootInput, string _destCDBPath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(_rootInput);
			if (!directoryInfo.Exists)
			{
				throw new DirectoryNotFoundException("Input directory not found : " + _rootInput);
			}
			JObject jobject = new JObject();
			JArray jarray = new JArray();
			jobject.Add("sheets", jarray);
			List<KeyValuePair<int, JObject>> list = new List<KeyValuePair<int, JObject>>();
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				JObject jobject2 = new JObject();
				JProperty content = new JProperty("name", directoryInfo2.Name);
				JArray jarray2 = new JArray();
				JObject jobject3 = null;
				JArray jarray3 = new JArray();
				JProperty content2 = new JProperty("lines", jarray2);
				JProperty content3 = new JProperty("separators", jarray3);
				jobject2.Add(content);
				List<Separator> list2 = new List<Separator>();
				foreach (FileInfo fileInfo in directoryInfo2.GetFiles("*.json", SearchOption.AllDirectories))
				{
					if (fileInfo.Name != "__STRUCTURE__.json" && fileInfo.Name != "__PROPS__.json")
					{
						object arg = JsonConvert.DeserializeObject(File.ReadAllText(fileInfo.FullName));
						if (CDBTool.o__9.p__1 == null)
						{
							CDBTool.o__9.p__1 = CallSite<Func<CallSite, object, JObject>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(JObject), typeof(CDBTool)));
						}
						Func<CallSite, object, JObject> target = CDBTool.<>o__9.<>p__1.Target;
						CallSite <>p__ = CDBTool.<>o__9.<>p__1;
						if (CDBTool.<>o__9.<>p__0 == null)
						{
							CDBTool.<>o__9.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeepClone", null, typeof(CDBTool), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						JObject jobject4 = target(<>p__, CDBTool.<>o__9.<>p__0.Target(CDBTool.<>o__9.<>p__0, arg));
						int num = jobject4.Value<int>("__separator_group_ID");
						string name = jobject4.Value<string>("__separator_group_Name");
						Separator separator = new Separator(num, name, 0);
						if (num != -1 && list2.Find((Separator s) => s.name == name) == null)
						{
							bool flag = false;
							do
							{
								if (list2.Count != 0)
								{
									int num2 = 0;
									while (num2 < list2.Count && list2[num2].id != separator.id)
									{
										num2++;
									}
									flag = (num2 < list2.Count);
									if (flag)
									{
										foreach (Separator separator2 in list2)
										{
											if (separator2.id > separator.id)
											{
												separator2.id++;
											}
										}
										separator.id++;
									}
								}
							}
							while (flag);
							list2.Add(separator);
						}
					}
				}
				List<KeyValuePair<long, int>> list3 = new List<KeyValuePair<long, int>>();
				List<JObject> list4 = new List<JObject>();
				foreach (FileInfo fileInfo2 in directoryInfo2.GetFiles("*.json", SearchOption.AllDirectories))
				{
					object arg2 = JsonConvert.DeserializeObject(File.ReadAllText(fileInfo2.FullName));
					if (fileInfo2.Name == "__STRUCTURE__.json")
					{
						if (CDBTool.<>o__9.<>p__2 == null)
						{
							CDBTool.<>o__9.<>p__2 = CallSite<Func<CallSite, object, JObject>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(JObject), typeof(CDBTool)));
						}
						JObject jobject5 = CDBTool.<>o__9.<>p__2.Target(CDBTool.<>o__9.<>p__2, arg2);
						jobject2.Add(new JProperty("columns", jobject5.Value<JArray>("__columns").DeepClone()));
						list.Add(new KeyValuePair<int, JObject>(jobject5.Value<int>("__table_index"), jobject2));
					}
					else if (fileInfo2.Name == "__PROPS__.json")
					{
						if (CDBTool.<>o__9.<>p__4 == null)
						{
							CDBTool.<>o__9.<>p__4 = CallSite<Func<CallSite, object, JObject>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(JObject), typeof(CDBTool)));
						}
						Func<CallSite, object, JObject> target2 = CDBTool.<>o__9.<>p__4.Target;
						CallSite <>p__2 = CDBTool.<>o__9.<>p__4;
						if (CDBTool.<>o__9.<>p__3 == null)
						{
							CDBTool.<>o__9.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeepClone", null, typeof(CDBTool), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						jobject3 = target2(<>p__2, CDBTool.<>o__9.<>p__3.Target(CDBTool.<>o__9.<>p__3, arg2));
					}
					else
					{
						if (CDBTool.<>o__9.<>p__6 == null)
						{
							CDBTool.<>o__9.<>p__6 = CallSite<Func<CallSite, object, JObject>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(JObject), typeof(CDBTool)));
						}
						Func<CallSite, object, JObject> target3 = CDBTool.<>o__9.<>p__6.Target;
						CallSite <>p__3 = CDBTool.<>o__9.<>p__6;
						if (CDBTool.<>o__9.<>p__5 == null)
						{
							CDBTool.<>o__9.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeepClone", null, typeof(CDBTool), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						JObject jobject6 = target3(<>p__3, CDBTool.<>o__9.<>p__5.Target(CDBTool.<>o__9.<>p__5, arg2));
						int num3 = -1;
						string text = jobject6.Value<string>("__separator_group_Name");
						if (list2.Count > 0 && !string.IsNullOrEmpty(text))
						{
							if (list2.Count == 0 || text == "")
							{
							}
							int num4 = 0;
							while (num4 < list2.Count && list2[num4].name != text)
							{
								num4++;
							}
							num3 = list2[num4].id;
						}
						for (int k = 0; k < list2.Count; k++)
						{
							if (list2[k].id > num3)
							{
								list2[k].pushLine();
							}
							else if (list2[k].id == num3)
							{
								string b2 = jobject6.Value<string>("__separator_group_Name");
								if (list2[k].name != b2)
								{
									list2[k].pushLine();
								}
							}
						}
						int num5 = jobject6.Value<int>("__original_Index");
						list3.Add(new KeyValuePair<long, int>((long)num5 << 32 | (long)(num3 + 1), list4.Count));
						list4.Add(jobject6);
						jobject6.Remove("__separator_group_ID");
						jobject6.Remove("__separator_group_Name");
						jobject6.Remove("__original_Index");
					}
				}
				list3.Sort((KeyValuePair<long, int> a, KeyValuePair<long, int> b) => a.Key.CompareTo(b.Key));
				for (int l = 0; l < list3.Count; l++)
				{
					jarray2.Add(list4[list3[l].Value]);
				}
				list2.Sort((Separator a, Separator b) => a.lineIndex.CompareTo(b.lineIndex));
				JArray jarray4 = new JArray();
				JProperty content4 = new JProperty("separatorTitles", jarray4);
				jobject3.AddFirst(content4);
				foreach (Separator separator3 in list2)
				{
					if (separator3.lineIndex > -1)
					{
						jarray3.Add(separator3.lineIndex);
						jarray4.Add(separator3.name);
					}
				}
				jobject2.Add(content2);
				jobject2.Add(content3);
				jobject2.Add(new JProperty("props", jobject3));
			}
			list.Sort((KeyValuePair<int, JObject> a, KeyValuePair<int, JObject> b) => a.Key.CompareTo(b.Key));
			for (int m = 0; m < list.Count; m++)
			{
				jarray.Add(list[m].Value);
			}
			jobject.Add("compress", false);
			jobject.Add("customTypes", new JArray());
			Directory.CreateDirectory(new FileInfo(_destCDBPath).Directory.FullName);
			string cdbjobjectAsString = CDBTool.GetCDBJObjectAsString(jobject);
			this.WriteContentAsync(_destCDBPath, cdbjobjectAsString).Wait();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002E58 File Offset: 0x00001058
		public static string GetCDBJObjectAsString(JObject _root)
		{
			StringWriter stringWriter = new StringWriter();
			CastleJsonTextWriter castleJsonTextWriter = new CastleJsonTextWriter(stringWriter)
			{
				Formatting = Formatting.Indented,
				Indentation = 1,
				IndentChar = '\t'
			};
			new JsonSerializer().Serialize(castleJsonTextWriter, _root);
			string result = stringWriter.ToString().Replace("\r", "");
			castleJsonTextWriter.Close();
			stringWriter.Close();
			return result;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002EB8 File Offset: 0x000010B8
		public void BuildDiffCDB(string _referenceCDBPath, string _inputDirPath, string _outputDirPath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
			string fullName = directoryInfo.FullName;
			this.Expand(_referenceCDBPath, fullName);
			Dictionary<string, CDBTool.Test> dictionary = this.BuildCRCFileMap(directoryInfo);
			DirectoryInfo rootDir = new DirectoryInfo(_inputDirPath);
			Dictionary<string, CDBTool.Test> dictionary2 = this.BuildCRCFileMap(rootDir);
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, CDBTool.Test> keyValuePair in dictionary2)
			{
				CDBTool.Test test;
				if (dictionary.TryGetValue(keyValuePair.Key, out test))
				{
					if (test.i != keyValuePair.Value.i)
					{
						list.Add(keyValuePair.Value.originalFileName);
					}
				}
				else
				{
					list.Add(keyValuePair.Value.originalFileName);
				}
			}
			if (list.Count > 0)
			{
				new DirectoryInfo(_outputDirPath).Create();
				foreach (string text in list)
				{
					FileInfo fileInfo = new FileInfo(Path.Combine(_outputDirPath, this.GetOriginalName(text)));
					Directory.CreateDirectory(fileInfo.Directory.FullName);
					File.Copy(Path.Combine(_inputDirPath, text), fileInfo.FullName, true);
				}
			}
			directoryInfo.Delete(true);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00003020 File Offset: 0x00001220
		private Dictionary<string, CDBTool.Test> BuildCRCFileMap(DirectoryInfo _rootDir)
		{
			int length = _rootDir.FullName.Length;
			Dictionary<string, CDBTool.Test> dictionary = new Dictionary<string, CDBTool.Test>();
			List<DirectoryInfo> list = new List<DirectoryInfo>();
			list.Add(_rootDir);
			Adler32 adler = new Adler32();
			for (int i = 0; i < list.Count; i++)
			{
				foreach (DirectoryInfo item in list[i].GetDirectories())
				{
					list.Add(item);
				}
				foreach (FileInfo fileInfo in list[i].GetFiles())
				{
					string originalName = this.GetOriginalName(fileInfo.FullName.Substring(length + 1));
					try
					{
						JObject jobject = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(fileInfo.FullName));
						int i2;
						if (jobject.Remove("__original_Index"))
						{
							i2 = adler.Make(new MemoryStream(Encoding.UTF8.GetBytes(jobject.ToString())));
						}
						else
						{
							i2 = adler.Make(File.OpenRead(fileInfo.FullName));
						}
						dictionary.Add(originalName, new CDBTool.Test(i2, jobject, fileInfo.FullName.Substring(length + 1)));
					}
					catch (Exception ex)
					{
						Log.Error("Failing to build CRC for file : " + originalName + " from " + fileInfo.FullName, "");
						throw ex;
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00003198 File Offset: 0x00001398
		private string GetOriginalName(string _indexedAndCategorizedName)
		{
			string[] array = _indexedAndCategorizedName.Split(new char[]
			{
				'\\'
			});
			if (array.Length == 0)
			{
				array = _indexedAndCategorizedName.Split(new char[]
				{
					'/'
				});
			}
			List<string> list = new List<string>(array);
			while (list.Count > 2)
			{
				list.RemoveAt(1);
			}
			string text = list[list.Count - 1];
			int num = text.IndexOf("---");
			if (num != -1)
			{
				list[list.Count - 1] = text.Substring(num + 3);
			}
			return Path.Combine(list.ToArray());
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000322C File Offset: 0x0000142C
		private static void WriteContentSync(string _fileName, string _content)
		{
			StreamWriter streamWriter = new FileInfo(_fileName).CreateText();
			streamWriter.Write(_content);
			streamWriter.Close();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00003248 File Offset: 0x00001448
		private async Task WriteContentAsync(string _fileName, string _content)
		{
			FileInfo fileInfo = new FileInfo(_fileName);
			StreamWriter writer = fileInfo.CreateText();
			await writer.WriteAsync(_content);
			writer.Close();
		}

		// Token: 0x04000001 RID: 1
		public const string structureFileName = "__STRUCTURE__.json";

		// Token: 0x04000002 RID: 2
		public const string propertyFileName = "__PROPS__.json";

		// Token: 0x04000003 RID: 3
		public const string columnsNodeName = "__columns";

		// Token: 0x04000004 RID: 4
		public const string tableIndexName = "__table_index";

		// Token: 0x04000005 RID: 5
		public const string separatorGroupIDName = "__separator_group_ID";

		// Token: 0x04000006 RID: 6
		public const string separatorGroupNameName = "__separator_group_Name";

		// Token: 0x04000007 RID: 7
		public const string originalIndexName = "__original_Index";

		// Token: 0x04000008 RID: 8
		public bool bMultiThread = true;

		// Token: 0x02000005 RID: 5
		private class Test
		{
			// Token: 0x06000014 RID: 20 RVA: 0x00003539 File Offset: 0x00001739
			public Test(int _i, JObject _obj, string _originalFileName)
			{
				this.i = _i;
				this.obj = _obj;
				this.originalFileName = _originalFileName;
			}

			// Token: 0x0400000C RID: 12
			public int i;

			// Token: 0x0400000D RID: 13
			public JObject obj;

			// Token: 0x0400000E RID: 14
			public string originalFileName;
		}
	}
}
