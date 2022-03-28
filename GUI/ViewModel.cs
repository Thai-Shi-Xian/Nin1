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
		private Settings Selected;
		private Crypto C;
		private ushort TableCNTR = 0, ReflCNTR = 0, SwapCNTR = 0, Shift = 0;

		public void GenerateTables(uint count = 10)
		{
			for (uint i = 0; i < count; i++)
			{
				Generator.UpdateSeed(DateTime.Now.Ticks);
				SS.Tables.Add(Generator.GenerateTable(TableCNTR));
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

		internal void SelectSettings(List<ushort> tables, List<ushort> pozitions, List<ushort> swaps, ushort reflector, ushort shift)
		{
			Selected = SS.Select(tables, pozitions, swaps, reflector, shift);
		}

		public void Load()
		{
			SS = FileHandling.ReadAll(GetPath());
		}

		public void LoadAndMerge()
		{
			SS.Merge(FileHandling.ReadAll(GetPath()));
		}

		public void Save()
		{
			string folder = GetFolder(GetPath());
			if (folder != "N/A")
			{
				FileHandling.Save(SS, folder);
			}
		}

		public void Add_Shift()
		{
			if (Shift == (Codepage.Limit - 1))
			{
				Shift = 0;
			}
			else
			{
				Shift++;
			}
		}

		public void Substract_Shift()
		{
			if (Shift == 0)
			{
				Shift = (ushort)(Codepage.Limit - 1);
			}
			else
			{
				Shift--;
			}
		}

		internal string Encrypt(string inText)
		{
			C = new(Selected);
			return C?.Encypt(inText);
		}

		internal string Decrypt(string inText)
		{
			C = new(Selected);
			return C?.Decypt(inText);
		}

		private string GetPath()
		{
			string path = "N/A";
			OpenFileDialog OFD = new();
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