using System;
using System.Collections.Generic;

namespace VPE
{
	/// <summary>Základní sdílená funkcionalita pro tabulky.</summary>
	public class Settings_Base
	{
		internal List<byte> Single_TablesToBytes(List<Table> tables)
		{
			List<byte> result = new();
			foreach (Table table in tables)
			{
				result.AddRange(Single_TableToBytes(table));
			}
			return result;
		}

		internal List<byte> Double_TablesToBytes(List<Table> tables)
		{
			List<byte> result = new();
			foreach (Table table in tables)
			{
				result.AddRange(Double_TableToBytes(table));
			}
			return result;
		}

		internal List<byte> Single_TableToBytes(Table t)
		{
			List<byte> result = new();
			result.Add(t.IsPaired ? (byte)1 : (byte)0);
			result.AddRange(BitConverter.GetBytes(t.Idx));
			foreach (ushort item in t.MainTable)
			{
				result.Add((byte)item);
			}
			return result;
		}

		internal List<byte> Double_TableToBytes(Table t)
		{
			List<byte> result = new();
			result.Add(t.IsPaired ? (byte)1 : (byte)0);
			result.AddRange(BitConverter.GetBytes(t.Idx));
			foreach (ushort item in t.MainTable)
			{
				result.AddRange(BitConverter.GetBytes(item));
			}
			return result;
		}

		internal List<Table> Single_TablesFromBytes(byte[] set, ref int pozition, int tableCount, ushort lim)
		{
			List<Table> result = new ();
			for (int i = 0; i < tableCount; i++)
			{
				result.Add(Single_TableFromBytes(set, ref pozition, lim));
			}
			return result;
		}

		internal List<Table> Double_TablesFromBytes(byte[] set, ref int pozition, int tableCount, ushort lim)
		{
			List<Table> result = new ();
			for (int i = 0; i < tableCount; i++)
			{
				result.Add(Double_TableFromBytes(set, ref pozition, lim));
			}
			return result;
		}

		internal Table Single_TableFromBytes(byte[] set, ref int pozition, ushort lim)
		{
			Table t = new();
			t.IsPaired = set[pozition] == 1;
			pozition++;
			t.Idx = BitConverter.ToUInt16(set, pozition);
			pozition += 2;
			for (ushort j = 0; j < lim; j++)
			{
				t.MainTable.Add(set[pozition]);
				pozition++;
			}
			return t;
		}

		internal Table Double_TableFromBytes(byte[] set, ref int pozition, ushort lim)
		{
			Table t = new();
			t.IsPaired = set[pozition] == 1;
			pozition++;
			t.Idx = BitConverter.ToUInt16(set, pozition);
			pozition += 2;
			for (ushort j = 0; j < lim; j++)
			{
				t.MainTable.Add(BitConverter.ToUInt16(set, pozition));
				pozition += 2;
			}
			return t;
		}
	}
	public class Settings : Settings_Base
	{
		public List<Table> Rotors { get; private set; } = new();
		public ushort[] Pozitions { get; set; }
		public List<Table> Swaps { get; private set; } = new();
		public Table Reflector { get; set; }
		public ushort ConstShift { get; set; }
		public ushort VarShift { get; set; }
		public ushort RandCharSeed { get; set; }

		public Settings ()
		{

		}

		public Settings (byte[] file)
		{
			if (file == null)
			{
				return;
			}
			if (file.Length == 0)
			{
				return;
			}
			if (file[0] == 1)
			{
				Double_ByteDecode(file);
			}
			else
			{
				Single_ByteDecode(file);
			}
		}

		public byte[] ToBytes ()
		{
			List<byte> result = new(65536);
			bool size = Codepage.Limit <= 256;
			result.Add(size ? (byte)0 : (byte)1); // Pseudoverzovací číslo: pokud tam je maximálně 256 znaků, budu ukládat čísla z tabulek jako byte, pokud jich je víc, bude to jako ushort.
			if (size)
			{
				Double_ByteConv(ref result);
			}
			else
			{
				Single_ByteConv(ref result);
			}
			return result.ToArray();
		}
		
		private void Single_ByteConv(ref List<byte> set)
		{
			set.Add((byte)Codepage.Limit);
			set.AddRange(BitConverter.GetBytes(Rotors.Count));
			set.AddRange(Single_TablesToBytes(Rotors));
			foreach (ushort poz in Pozitions)
			{
				set.Add((byte)poz);
			}
			set.AddRange(Single_TableToBytes(Reflector));
			set.AddRange(BitConverter.GetBytes(Swaps.Count));
			set.AddRange(Single_TablesToBytes(Swaps));
			set.Add((byte)ConstShift);
			set.Add((byte)VarShift);
			set.AddRange(BitConverter.GetBytes(RandCharSeed));
		}

		private void Double_ByteConv(ref List<byte> set)
		{
			set.AddRange(BitConverter.GetBytes(Codepage.Limit));
			set.AddRange(BitConverter.GetBytes(Rotors.Count));
			set.AddRange(Double_TablesToBytes(Rotors));
			foreach (ushort poz in Pozitions)
			{
				set.AddRange(BitConverter.GetBytes(poz));
			}
			set.AddRange(Double_TableToBytes(Reflector));
			set.AddRange(BitConverter.GetBytes(Swaps.Count));
			set.AddRange(Double_TablesToBytes(Swaps));
			set.AddRange(BitConverter.GetBytes(ConstShift));
			set.AddRange(BitConverter.GetBytes(VarShift));
			set.AddRange(BitConverter.GetBytes(RandCharSeed));
		}

		private void Single_ByteDecode(byte[] set)
		{
			int pozition = 1;
			ushort lim = set[pozition];
			pozition++;
			int tableCount = BitConverter.ToInt32(set, pozition);
			pozition += 4;
			Rotors.AddRange(Single_TablesFromBytes(set, ref pozition, tableCount, lim));
			Pozitions = new ushort[tableCount];
			for (int i = 0; i < tableCount; i++)
			{
				Pozitions[i] = set[pozition];
				pozition++;
			}
			Reflector = Single_TableFromBytes(set, ref pozition, lim);
			tableCount = BitConverter.ToInt32(set, pozition);
			pozition += 4;
			Swaps.AddRange(Single_TablesFromBytes(set, ref pozition, tableCount, lim));
			ConstShift = set[pozition];
			pozition++;
			VarShift = set[pozition];
			pozition++;
			RandCharSeed = BitConverter.ToUInt16(set, pozition);
		}

		private void Double_ByteDecode(byte[] set)
		{
			int pozition = 1;
			ushort lim = BitConverter.ToUInt16(set, pozition);
			pozition += 2;
			int tableCount = BitConverter.ToInt32(set, pozition);
			pozition += 4;
			Rotors.AddRange(Double_TablesFromBytes(set, ref pozition, tableCount, lim));
			Pozitions = new ushort[tableCount];
			for (int i = 0; i < tableCount; i++)
			{
				Pozitions[i] = BitConverter.ToUInt16(set, pozition);
				pozition += 2;
			}
			Reflector = Double_TableFromBytes(set, ref pozition, lim);
			tableCount = BitConverter.ToInt32(set, pozition);
			pozition += 4;
			Swaps.AddRange(Double_TablesFromBytes(set, ref pozition, tableCount, lim));
			ConstShift = BitConverter.ToUInt16(set, pozition);
			pozition += 2;
			VarShift = BitConverter.ToUInt16(set, pozition);
			pozition += 2;
			RandCharSeed = BitConverter.ToUInt16(set, pozition);
		}
	}
	/// <summary>Ukládá množiny tabulek.</summary>
	public class SettingsStorage : Settings_Base
	{
		public List<Table> Swaps { get; private set; } = new();
		public List<Table> Rotors { get; private set; } = new();
		public List<Table> Reflectors { get; private set; } = new();

		public SettingsStorage ()
		{

		}

		public SettingsStorage(byte[] file)
		{
			if (file == null)
			{
				return;
			}
			if (file.Length == 0)
			{
				return;
			}
			if (file[0] == 1)
			{
				Double_ByteDecode(file);
			}
			else
			{
				Single_ByteDecode(file);
			}
		}

		public byte[] ToBytes()
		{
			List<byte> result = new(65536);
			bool size = Codepage.Limit <= 256;
			result.Add(size ? (byte)0 : (byte)1); // Pseudoverzovací číslo: pokud tam je maximálně 256 znaků, budu ukládat čísla z tabulek jako byte, pokud jich je víc, bude to jako ushort.
			if (size)
			{
				Double_ByteConv(ref result);
			}
			else
			{
				Single_ByteConv(ref result);
			}
			return result.ToArray();
		}

		private void Single_ByteConv(ref List<byte> set)
		{
			set.Add((byte)Codepage.Limit);
			set.AddRange(BitConverter.GetBytes(Rotors.Count));
			set.AddRange(Single_TablesToBytes(Rotors));
			set.AddRange(BitConverter.GetBytes(Reflectors.Count));
			set.AddRange(Single_TablesToBytes(Reflectors));
			set.AddRange(BitConverter.GetBytes(Swaps.Count));
			set.AddRange(Single_TablesToBytes(Swaps));
		}

		private void Double_ByteConv(ref List<byte> set)
		{
			set.AddRange(BitConverter.GetBytes(Codepage.Limit));
			set.AddRange(BitConverter.GetBytes(Rotors.Count));
			set.AddRange(Double_TablesToBytes(Rotors));
			set.AddRange(BitConverter.GetBytes(Reflectors.Count));
			set.AddRange(Double_TablesToBytes(Reflectors));
			set.AddRange(BitConverter.GetBytes(Swaps.Count));
			set.AddRange(Double_TablesToBytes(Swaps));
		}

		private void Single_ByteDecode(byte[] set)
		{
			int pozition = 1;
			ushort lim = set[pozition];
			pozition++;
			int tableCount = BitConverter.ToInt32(set, pozition);
			pozition += 4;
			Rotors.AddRange(Single_TablesFromBytes(set, ref pozition, tableCount, lim));
			tableCount = BitConverter.ToInt32(set, pozition);
			pozition += 4;
			Reflectors.AddRange(Single_TablesFromBytes(set, ref pozition, tableCount, lim));
			tableCount = BitConverter.ToInt32(set, pozition);
			pozition += 4;
			Swaps.AddRange(Single_TablesFromBytes(set, ref pozition, tableCount, lim));
		}

		private void Double_ByteDecode(byte[] set)
		{
			int pozition = 1;
			ushort lim = BitConverter.ToUInt16(set, pozition);
			pozition += 2;
			int tableCount = BitConverter.ToInt32(set, pozition);
			pozition += 4;
			Rotors.AddRange(Double_TablesFromBytes(set, ref pozition, tableCount, lim));
			tableCount = BitConverter.ToInt32(set, pozition);
			pozition += 4;
			Reflectors.AddRange(Double_TablesFromBytes(set, ref pozition, tableCount, lim));
			tableCount = BitConverter.ToInt32(set, pozition);
			pozition += 4;
			Swaps.AddRange(Double_TablesFromBytes(set, ref pozition, tableCount, lim));
		}

		public void Merge(SettingsStorage settingsStorage)
		{
			Swaps.AddRange(settingsStorage?.Swaps);
			Rotors.AddRange(settingsStorage?.Rotors);
			Reflectors.AddRange(settingsStorage?.Reflectors);
		}

		public Settings Select (List<ushort> tables, List<ushort> swaps, int refl)
		{
			Settings s = new()
			{
				Reflector = Reflectors[refl],
			};
			foreach (ushort i in tables)
			{
				s.Rotors.Add(Rotors[i]);
			}
			foreach (ushort i in swaps)
			{
				s.Swaps.Add(Rotors[i]);
			}
			return s;
		}
	}
}