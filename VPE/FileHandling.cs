using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VPE
{
	public static class FileHandling
	{
		public const string FileExt = ".vpess"; // Very primitive encryption settings storage.
		/// <summary>Uloží třídu úložiště nastavení.</summary>
		/// <param name="what">Třída úložiště nastavení.</param>
		/// <param name="where">Kam – složka!</param>
		public static void Save(SettingsStorage what, string where)
		{
			IEnumerable<string> myFiles = Directory.EnumerateFiles(where, "*" + FileExt);
			string finalFileName;
			if (myFiles.Count() == 0)
			{
				finalFileName = where.EndsWith('\\') ? where + "00000" + FileExt : where + '\\' + "00000" + FileExt;
				WriteAll(what, finalFileName);
				return;
			}
			else
			{
				string fileName;
				if (myFiles.Count() == 1)
				{
					fileName = Path.GetFileName(myFiles.First());
					if (uint.TryParse(fileName.AsSpan(0, fileName.Length - 6), out uint name))
					{
						if (name == 0)
						{
							finalFileName = where.EndsWith('\\') ? where + "00001" + FileExt : where + '\\' + "00001" + FileExt;
							WriteAll(what, finalFileName);
							return;
						}
						else
						{
							finalFileName = where.EndsWith('\\') ? where + "00000" + FileExt : where + '\\' + "00000" + FileExt;
							WriteAll(what, finalFileName);
							return;
						}
					}
				}
				else
				{
					List<uint> fileNums = new();
					foreach (string file in myFiles)
					{
						fileName = Path.GetFileName(file);
						string onlyFileName = fileName.AsSpan(0, fileName.Length - 6).ToString();
						if (uint.TryParse(onlyFileName, out uint name))
						{
							fileNums.Add(name);
						}
					}
					fileNums = fileNums.OrderBy(x => x).ToList();
					for (uint i = 0; i <= fileNums.Max() + 1; i++)
					{
						if (fileNums.Contains(i))
						{
							continue;
						}
						else
						{
							finalFileName = where.EndsWith('\\') ? where + i.ToString("00000") + FileExt : where + '\\' + i.ToString("00000") + FileExt;
							WriteAll(what, finalFileName);
							return;
						}
					}
					finalFileName = where.EndsWith('\\') ? where + fileNums.Max().ToString("00000") + FileExt : where + '\\' + fileNums.Max().ToString("00000") + FileExt;
					WriteAll(what, finalFileName);
					return;
				}
			}
		}

		public static SettingsStorage ReadAll(string path)
		{
			return new SettingsStorage(File.ReadAllBytes(path));
		}

		private static void WriteAll(SettingsStorage storage, string path)
		{
			File.WriteAllBytes(path, storage.ToBytes());
		}
	}
}