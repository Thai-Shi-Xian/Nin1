using System;
using System.Collections.Generic;

namespace VPE
{
	public class Settings
	{
		public ushort[] Positions { get; set; }
		public List<Table> Swaps { get; private set; } = new();
		public ushort Shift { get; set; }
		public List<Table> Tables { get; private set; } = new();
		public Table Reflector { get; set; }
	}

	public class SettingsStorage
	{
		public List<Table> Swaps { get; private set; } = new();
		public List<Table> Tables { get; private set; } = new();
		public List<Table> Reflectors { get; private set; } = new();
		public List<ushort> Shifts { get; private set; } = new();
		
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
			int poz = 2, count;
			Table t;
			for (int i = 0; i < 3; i++) // Skupiny tabulek.
			{
				count = BitConverter.ToInt32 (file, poz);
				poz += 4;
				for (int j = 0; j < count; i++) // Tabulky ve skupině.
				{
					t = new();
					int tableLen = BitConverter.ToInt32(file, poz);
					poz += 4;
					for (int k = 0; k < tableLen; k++) // Položky tabulky.
					{
						t.MainTable.Add(BitConverter.ToUInt16(file, poz));
						poz += 2;
					}
					switch(i)
					{
						case 0:
							Tables.Add(t);
							break;
						case 1:
							Reflectors.Add(t);
							break;
						case 2:
							Swaps.Add(t);
							break;
					}
				}
			}
			count = BitConverter.ToInt32(file, poz);
			poz += 4;
			for (int i = 0; i < count; i++)
			{
				Shifts.Add(BitConverter.ToUInt16(file, poz));
				poz += 2;
			}
		}

		public byte[] ToBytes()
		{
			List<byte> result = new(65536);
			result.AddRange(BitConverter.GetBytes(Codepage.Limit)); // Tolik položek bude mít každá tabulka.
			result.AddRange(BitConverter.GetBytes(Tables.Count)); // Počet tabulek.
			result.AddRange(TablesToBytes(Tables));
			result.AddRange(BitConverter.GetBytes(Reflectors.Count)); // Počet reflektorů.
			result.AddRange(TablesToBytes(Reflectors));
			result.AddRange(BitConverter.GetBytes(Swaps.Count)); // Počet swapů.
			result.AddRange(TablesToBytes(Swaps));
			result.AddRange(BitConverter.GetBytes(Shifts.Count)); // Počet posunů.
			foreach (ushort item in Shifts)
			{
				result.AddRange(BitConverter.GetBytes(item));
			}
			return result.ToArray();
		}

		public void Merge(SettingsStorage settingsStorage)
		{
			Swaps.AddRange(settingsStorage?.Swaps);
			Tables.AddRange(settingsStorage?.Tables);
			Reflectors.AddRange(settingsStorage?.Reflectors);
			Shifts.AddRange(settingsStorage?.Shifts);
		}

		public Settings Select (List<ushort> tables, List<ushort> pozitions, List<ushort> swaps, int refl, ushort shift)
		{
			Settings s = new()
			{
				Reflector = Reflectors[refl],
				Shift = shift,
			};
			foreach (ushort i in tables)
			{
				s.Tables.Add(Tables[i]);
			}
			foreach (ushort i in swaps)
			{
				s.Swaps.Add(Tables[i]);
			}
			s.Positions = pozitions.ToArray();
			return s;
		}
		
		private List<byte> TablesToBytes (List<Table> tables)
		{
			List<byte> result = new();
			foreach (Table table in tables)
			{
				result.AddRange(BitConverter.GetBytes(table.MainTable.Count));
				foreach (ushort item in table.MainTable)
				{
					result.AddRange(BitConverter.GetBytes(item));
				}
			}
			return result;
		}
	}
}