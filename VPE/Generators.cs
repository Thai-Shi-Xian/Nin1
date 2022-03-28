using System;
using System.Collections.Generic;
using System.Linq;

namespace VPE
{
	public class Generators
	{
		private Random R;
		private ushort Limit;
		/// <summary>Vytvoří novou instanci generátoru.</summary>
		/// <param name="Lim">Maximální hodnota (excluding!), po k</param>
		/// <param name="Seed">Seed pro vytvoření náhodného generátoru.</param>
		public Generators(ushort Lim, long Seed = 0)
		{
			Limit = Lim;
			UpdateSeed(Seed);
		}

		public void UpdateSeed(long NewSeed)
		{
			DateTime Now;
			if (NewSeed == 0)
			{
				Now = DateTime.Now;
			}
			else
			{
				Now = DateTime.FromBinary(NewSeed);
			}
			ulong ms = (ulong)(Now.Millisecond + Now.Second * 1000 + Now.Minute * 60000) + (ulong)(Now.Hour * 3600000) + (ulong)((Now.DayOfYear - 1) * 86400000) + (ulong)(Now.Year * 365.25 * 86400000);
			int remainder = (int)(ms % int.MaxValue);
			R = new Random(remainder);
		}
		/// <summary>Vygeneruje tabulku.</summary>
		/// <param name="index">Index tabulky, použit jako pseudonázev.</param>
		/// <returns>Výsledná tabulka.</returns>
		public Table GenerateTable(ushort index)
		{
			Table T = new()
			{
				Idx = index,
			};
			List<ushort> Remains = new();
			for (ushort u = 0; u < Limit; u++)
			{
				Remains.Add(u);
			}
			int RandIndex;
			ushort Selected;
			for (int i = 0; i < Limit; i++)
			{
				if (i < Limit - 2)
				{
					RandIndex = R.Next(Remains.Count);
				}
				else
				{
					RandIndex = 0;
				}
				Selected = Remains[RandIndex];
				T.MainTable.Add(Selected);
				Remains.Remove(Selected);
			}
			return T;
		}
		/// <summary>Vygeneruje párovou tabulku, na reflektory.</summary>
		/// <param name="index">Index tabulky, použit jako pseudonázev.</param>
		/// <returns>Výsledná tabulka.</returns>
		public Table GeneratePairs(ushort index)
		{
			Table T = new()
			{
				Idx = index,
			};
			T.IsPaired = true;
			ushort[] Temp = new ushort[Codepage.Limit];
			List<ushort> Remains = new();
			for (ushort u = 0; u < Limit; u++)
			{
				Remains.Add(u);
			}
			int RandIndex;
			ushort SelA, SelB;
			while (Remains.Count > 0)
			{
				RandIndex = R.Next(Remains.Count);
				SelA = Remains[RandIndex];
				Remains.RemoveAt(RandIndex);
				RandIndex = R.Next(Remains.Count);
				SelB = Remains[RandIndex];
				Remains.RemoveAt(RandIndex);
				Temp[SelA] = SelB;
				Temp[SelB] = SelA;
			}
			T.MainTable = Temp.ToList();
			return T;
		}
		/// <summary>Vygeneruje párovou tabulku, kde nejsou všechny hodnoty. Na swapy.</summary>
		/// <param name="index">Index tabulky, použit jako pseudonázev.</param>
		/// <param name="fillPortion">Jaký podíl záměn má být vyplněn? Nevyplněné položky nebudou zaměňovány.</param>
		/// <returns>Výsledná tabulka.</returns>
		public Table GeneratePairsWithSkips(ushort index, double fillPortion = 0.65234375)
		{
			Table T = new()
			{
				Idx = index,
			};
			T.IsPaired = true;
			ushort[] Temp = new ushort[Codepage.Limit];
			List<ushort> Remains = new();
			for (ushort u = 0; u < Limit; u++)
			{
				Remains.Add(u);
			}
			int RandIndex;
			ushort SelA, SelB;
			while (Remains.Count > 0)
			{
				RandIndex = R.Next(Remains.Count);
				SelA = Remains[RandIndex];
				Remains.RemoveAt(RandIndex);
				RandIndex = R.Next(Remains.Count);
				SelB = Remains[RandIndex];
				Remains.RemoveAt(RandIndex);
				if (R.NextDouble() <= fillPortion)
				{
					
					Temp[SelA] = SelB;
					Temp[SelB] = SelA;
				}
			}
			T.MainTable = Temp.ToList();
			return T;
		}
		/// <summary></summary>
		/// <returns>Náhodné číslo v limitu.</returns>
		public ushort GenerateNum()
		{
			return Convert.ToUInt16(R.Next(Limit));
		}
	}
}