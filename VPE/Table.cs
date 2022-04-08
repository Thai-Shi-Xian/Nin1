using System;
using System.Collections.Generic;

namespace VPE
{
	public class Table
	{
		/// <summary>Hlavní tabulka hodnot.</summary>
		public List<ushort> MainTable { get; set; } = new (256);
		/// <summary>Jestli je to tabulka párů. Deafultně předpokládám, že ne.</summary>
		public bool IsPaired { get; set; } = false;
		private string NameV = "";
		public string Name
		{
			get
			{
				if (NameV == "")
				{
					return Idx.ToString();
				}
				else
				{
					return NameV;
				}
			}
			set
			{
				NameV = value;
			}
		}

		public ushort Idx { get; set; }
		/// <summary>Vrátí index hodnoty. Vrací 65535 pokud je tabulka prázdná, 65534 pokud neobsahuje hodnotu.</summary>
		/// <param name="Value">Hodnota.</param>
		/// <returns>Index, případně kód chyby.</returns>
		public ushort FindIndexUsingValue (ushort Value)
		{
			if (MainTable.Count > 0)
			{
				if (MainTable.Contains (Value))
				{
					return (ushort)MainTable.IndexOf(Value);
				}
				else
				{
					return ushort.MaxValue - 1;
				}
			}
			else
			{
				return ushort.MaxValue;
			}
		}
		/// <summary>Vrací hodnotu na základě indexu. Vrací 65535 pokud je tabulka prázdná, 65534 pokud je index větší než počet prvků v tabulce.</summary>
		/// <param name="Index">Index.</param>
		/// <returns>Hodnota.</returns>
		public ushort FindValueUsingIndex (ushort Index)
		{
			if (MainTable.Count > 0)
			{
				if (MainTable.Count - 1 >= Index)
				{
					return MainTable[Index];
				}
				else
				{
					return ushort.MaxValue - 1;
				}
			}
			else
			{
				return ushort.MaxValue;
			}
		}
	}
}