using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPE;
using Factorizator;
using NeueDT;
using DTcalc;
using Microsoft.Win32;

namespace GUI
{
	public class VPE_VM
	{
		private Generators Generator = new(Codepage.Limit, DateTime.Now.Ticks);
		private SettingsStorage SS = new();
		private Settings S;
		private Crypto C;
		private const string VPESS_filter = "VPE Settings Storage files (*.vpess)|*.vpess";
		private const string VPES_filter = "VPE Settings files (*.vpes)|*.vpes";
		private ushort TableCNTR = 0, ReflCNTR = 0, SwapCNTR = 0;
		public List<ushort> Refls = new();
		public List<ushort> Swaps = new();

		public void GenerateRotors(uint count = 10)
		{
			for (uint i = 0; i < count; i++)
			{
				Generator.UpdateSeed(DateTime.Now.Ticks);
				SS.Rotors.Add(Generator.GenerateTable(TableCNTR));
				TableCNTR++;
			}
		}

		public void GenerateReflector(uint count = 2)
		{
			for (uint i = 0; i < count; i++)
			{
				Generator.UpdateSeed(DateTime.Now.Ticks);
				SS.Reflectors.Add(Generator.GeneratePairs(ReflCNTR));
				ReflCNTR++;
			}
		}

		public void GenerateSwaps(uint count = 5)
		{
			for (uint i = 0; i < count; i++)
			{
				Generator.UpdateSeed(DateTime.Now.Ticks);
				SS.Swaps.Add(Generator.GeneratePairsWithSkips(SwapCNTR));
				SwapCNTR++;
			}
		}

		public void SelectSettings(List<ushort> tables, List<ushort> swaps, ushort reflector)
		{
			S = SS.Select(tables, swaps, reflector);
		}

		public void LoadAll()
		{
			SS = FileHandling.ReadAll(OpenFile(VPESS_filter));
		}

		public void LoadAndMerge()
		{
			SS.Merge(FileHandling.ReadAll(OpenFile(VPESS_filter)));
		}

		public void LoadSpecific()
		{
			S = FileHandling.ReadSpecific(OpenFile(VPES_filter));
		}

		public void SaveAll()
		{
			string folder = GetFolder(SaveFile(VPESS_filter));
			if (folder != "N/A")
			{
				FileHandling.Save(SS, folder);
			}
		}

		public void SaveSpecific()
		{
			string folder = GetFolder(SaveFile(VPES_filter));
			if (folder != "N/A")
			{
				FileHandling.Save(SS, folder);
			}
		}

		public ushort GenerateRandNum()
		{
			Generator.UpdateSeed(DateTime.Now.Ticks);
			return Generator.GenerateNum();
		}

		public string Encrypt(string inText)
		{
			C = new(S);
			return C?.Encypt(inText);
		}

		public string Decrypt(string inText)
		{
			C = new(S);
			return C?.Decypt(inText);
		}

		private string OpenFile(string ext)
		{
			string path = "N/A";
			OpenFileDialog OFD = new();
			OFD.Filter = ext;
			bool? dia = OFD.ShowDialog();
			if (dia is not null)
			{
				if (dia.Value)
				{
					path = OFD.FileName;
				}
			}
			return path;
		}

		private string SaveFile(string ext)
		{
			string path = "N/A";
			SaveFileDialog SFD = new();
			SFD.Filter = ext;
			bool? dia = SFD.ShowDialog();
			if (dia is not null)
			{
				if (dia.Value)
				{
					path = SFD.FileName;
				}
			}
			return path;
		}

		private string GetFolder(string path)
		{
			if (path != "N/A")
			{
				int idx = path.LastIndexOf('\\');
				return path[..(idx + 1)];
			}
			return path;
		}
	}

	public class NDT_VM
	{

	}

	public class DTC_VM
	{

	}

	public class Fact_VM
	{

	}
}