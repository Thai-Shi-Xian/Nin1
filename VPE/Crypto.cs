using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VPE
{
	public class Crypto
	{
		private Settings Sett;

		public Crypto(Settings S)
		{
			Sett = S;
		}
		/// <summary>Zašifruje text.</summary>
		/// <param name="Text">Text na zašifrování.</param>
		/// <returns>Zašifrovaný text.</returns>
		public string Encypt(string Text)
		{
			List<ushort> Working = ConvertToNums(Text); // Pøevod textu na èísla.
			AddRandomChars(ref Working); // Pøidávám náhodné znaky, co se budou šifrovat.
			OrderShift(ref Working); // Provede jednoduchý posun celé sady podle poøadí znaku v sadì.
			Swap(ref Working); // Prohodí znaky podle (èásteènì vyplnìné) tabulky.
			Scramble(ref Working); // Zamíchá znaky postupnì dle tabulek, odrazí a pak zpìtnì dle tabulek.
			Swap(ref Working);
			ConstantShift(ref Working); // Posune každý znak o konstantu.
			VariableShift(ref Working); // Posune každý znak o promìnné èíslo závislé na seedu a poøadí.
			//AddRandomChars(ref Working);
			return ConvertToString(Working); // Pøevede èísla na text.
		}
		/// <summary>Dešifruje text.</summary>
		/// <param name="Text">Zašifrovaný text.</param>
		/// <returns>Dešifrovaný text.</returns>
		public string Decypt(string Text)
		{
			List<ushort> Working = ConvertToNums(Text);
			RemoveRandomChars(ref Working);
			UnOrderShift(ref Working);
			Unswap(ref Working);
			Unscramble(ref Working);
			Unswap(ref Working);
			UnConstantShift(ref Working);
			UnVariableShift(ref Working);
			//RemoveRandomChars(ref Working); // Pøidávám náhodné znaky, nešifrované.
			return ConvertToString(Working);
		}
		/// <summary>Zkonvertuje textovou zprávu na èíselnou reprezentaci podle tabulky znakù.</summary>
		/// <param name="Text">Textová zpráva.</param>
		/// <returns>Èíselná reprezentace.</returns>
		private List<ushort> ConvertToNums(string Text)
		{
			List<ushort> Result = new(Text.Length);
			foreach (char C in Text)
			{
				if (C == '\r')
				{ // Beru rovnou \r\n.
					Result.Add((ushort)Codepage.Letters.IndexOf("\r\n"));
					continue;
				}
				if (C == '\n')
				{ // Ignoruji, \n o samotì nemá význam.
					continue;
				}
				string Letter = Convert.ToString(C);
				int index = Codepage.Letters.IndexOf(Letter);
				if (index >= 0)
				{
					Result.Add((ushort)index);
				}
				else
				{ // Pokud znak neznám, pøeskakuji ho.
					continue;
				}
			}
			return Result;
		}
		/// <summary>Pøevede sadu èísel na text.</summary>
		/// <param name="Numbers">Sada èísel.</param>
		/// <returns>Text.</returns>
		private string ConvertToString(List<ushort> Numbers)
		{
			StringBuilder SB = new();
			foreach (ushort Num in Numbers)
			{
				SB.Append(Codepage.Letters[Num]);
			}
			return SB.ToString();
		}
		/// <summary>Prohodí èísla postupnì podle všech tabulek.</summary>
		/// <param name="Numbers">Èísla na prohození.</param>
		private void Swap(ref List<ushort> Numbers)
		{
			for (int i = 0; i < Numbers.Count; i++)
			{
				foreach (Table T in Sett.Swaps)
				{
					if (T.FindValueUsingIndex(Numbers[i]) < ushort.MaxValue - 10) // Error kódy.
					{
						ushort swapped = T.FindValueUsingIndex(Numbers[i]);
						Numbers[i] = swapped == (ushort.MaxValue - 1) ? Numbers[i] : swapped; // Pokud tam tahle hodnota není, neprohazuji.
					}
					else
					{
						continue;
					}
				}
			}
		}
		/// <summary>Zvrátí prohození èísel, zpìtnì.</summary>
		/// <param name="Numbers">Èísla na odprohození.</param>
		private void Unswap(ref List<ushort> Numbers)
		{
			for (int j = Sett.Swaps.Count - 1; j >= 0; j--)
			{
				for (int i = Numbers.Count - 1; i >= 0; i--)
				{
					if (Sett.Swaps[j].FindIndexUsingValue(Numbers[i]) != ushort.MaxValue)
					{
						ushort swapped = Sett.Swaps[j].FindIndexUsingValue(Numbers[i]);
						Numbers[i] = swapped == (ushort.MaxValue - 1) ? Numbers[i] : swapped;
					}
					else
					{
						continue;
					}
				}
			}
		}
		/// <summary>Zamíchá èísla podle tabulek.</summary>
		/// <param name="Numbers">Èísla na zamíchání.</param>
		private void Scramble(ref List<ushort> Numbers)
		{
			ushort[] Positions = Sett.Pozitions;
			for (int i = 0; i < Numbers.Count; i++)
			{
				Numbers[i] = ForwardScramble(Numbers[i], Positions); // Dopøedný prùchod.
				Numbers[i] = Reflect(Numbers[i], Positions.Last()); // Odraz s prohozením.
				Numbers[i] = BackwardScramble(Numbers[i], Positions); // Zpìtný prùchod.
				int Sum = Numbers[i] + Positions[0];
				Numbers[i] = (ushort)(Sum % Codepage.Limit);
				Increment(ref Positions);
			}
		}
		/// <summary>Odzamíchá èísla podle tabulek.</summary>
		/// <param name="Numbers">Èísla na odzamíchání.</param>
		private void Unscramble(ref List<ushort> Numbers)
		{
			ushort[] Positions = Sett.Pozitions;
			for (int i = 0; i < Numbers.Count; i++)
			{
				Numbers[i] = BackwardScramble(Numbers[i], Positions);
				Numbers[i] = ReflectBack(Numbers[i], Positions.Last());
				Numbers[i] = ForwardScramble(Numbers[i], Positions);
				int Sum = Numbers[i] + Positions[0];
				Numbers[i] = (ushort)(Sum % Codepage.Limit);
				Increment(ref Positions);
			}
		}
		/// <summary>Posune èísla podle posunového parametru.</summary>
		/// <param name="Numbers">Èísla na posunutí.</param>
		private void ConstantShift(ref List<ushort> Numbers)
		{
			for (int i = 0; Numbers.Count > 0; i++)
			{
				Numbers[i] += Sett.ConstShift;
				Numbers[i] %= Codepage.Limit;
			}
		}
		/// <summary>Posune èísla zpìt podle posunového parametru.</summary>
		/// <param name="Numbers">Èísla na posunutí zpìt.</param>
		private void UnConstantShift(ref List<ushort> Numbers)
		{
			for (int i = 0; Numbers.Count > 0; i++)
			{
				Numbers[i] -= Sett.ConstShift;
				Numbers[i] %= Codepage.Limit;
			}
		}
		/// <summary>Posune èísla podle poøadí.</summary>
		/// <param name="Numbers">Èísla na posunutí.</param>
		private void OrderShift(ref List<ushort> Numbers)
		{
			for (int i = 0; i < Numbers.Count; i++)
			{
				Numbers[i] = (ushort)((Numbers[i] + i) % Codepage.Limit);
			}
		}
		/// <summary>Posune èísla zpìt podle poøadí.</summary>
		/// <param name="Numbers">Èísla na posunutí zpìt.</param>
		private void UnOrderShift(ref List<ushort> Numbers)
		{
			for (int i = 0; i < Numbers.Count; i++)
			{
				Numbers[i] = (ushort)((Numbers[i] - i) % Codepage.Limit);
			}
		}
		/// <summary>Provede promìnný posun èísel.</summary>
		/// <param name="Numbers">Èísla na posunutí.</param>
		private void VariableShift (ref List<ushort> Numbers)
		{
			uint v, c = Sett.ConstShift > (Codepage.Limit / 2) ? Sett.ConstShift : (uint)(Codepage.Limit - Sett.ConstShift);
			for (int i = 0; i < Numbers.Count; i++)
			{
				v = Numbers[i] + (uint)(Sett.VarShift * (i % c));
				Numbers[i] = (ushort)(v % Codepage.Limit);
			}
		}
		/// <summary>Provede promìnný posun zpìt èísel.</summary>
		/// <param name="Numbers">Èísla na posunutí zpìt.</param>
		private void UnVariableShift(ref List<ushort> Numbers)
		{
			uint v, c = Sett.ConstShift > (Codepage.Limit / 2) ? Sett.ConstShift : (uint)(Codepage.Limit - Sett.ConstShift);
			for (int i = 0; i < Numbers.Count; i++)
			{
				v = Numbers[i] - (uint)(Sett.VarShift * (i % c));
				Numbers[i] = (ushort)(v % Codepage.Limit);
			}
		}
		/// <summary>Dopøedný prùchod 1 znaku všemi tabulkami.</summary>
		/// <param name="Number">Znak.</param>
		/// <param name="Positions">Pozice v tabulkách.</param>
		/// <returns>Znak po prùchodu všemi tabulkami.</returns>
		private ushort ForwardScramble(ushort Number, ushort[] Positions)
		{
			for (int i = 0; i < Sett.Rotors.Count; i++)
			{
				int Sum = i == 0 ? Number + Positions[i] : Number + (Positions[i] - Positions[i - 1]); // Kalkulace pozice po vstupu do tabulky.
				ushort Remain = (ushort)(Sum % Codepage.Limit); // Zùstání v rozsahu.
				Number = Sett.Rotors[i].FindValueUsingIndex(Remain); // Prùchod tabulkou.
			}
			return Number;
		}
		/// <summary>Odrazí znak dle tabulky.</summary>
		/// <param name="Number">Znak.</param>
		/// <param name="LastPosition">Pozice v poslední tabulce.</param>
		/// <returns>Odražený znak.</returns>
		private ushort Reflect(ushort Number, ushort LastPosition)
		{
			int Sum = Number + LastPosition;
			ushort Remain = (ushort)(Sum % Codepage.Limit);
			ushort Counterpart = Sett.Reflector.FindValueUsingIndex(Remain);
			return Counterpart;
		}
		/// <summary>Zpìtnì odrazí znak dle tabulky.</summary>
		/// <param name="Number">Znak.</param>
		/// <param name="LastPosition">Pozice v poslední tabulce.</param>
		/// <returns>Odražený znak.</returns>
		private ushort ReflectBack(ushort Number, ushort LastPosition)
		{
			int Sum = Number + LastPosition;
			ushort Remain = (ushort)(Sum % Codepage.Limit);
			ushort Counterpart = Sett.Reflector.FindIndexUsingValue(Remain);
			return Counterpart;
		}
		/// <summary>Zpìtný prùchod 1 znaku všemi tabulkami.</summary>
		/// <param name="Number">Znak.</param>
		/// <param name="Positions">Pozice v tabulkách.</param>
		/// <returns>Znak po prùchodu všemi tabulkami.</returns>
		private ushort BackwardScramble(ushort Number, ushort[] Positions)
		{
			for (int i = Sett.Rotors.Count - 1; i >= 0; i--)
			{
				int Sum = i == (Sett.Rotors.Count - 1) ? Number + Positions[i] : Number + (Positions[i] - Positions[i + 1]);
				ushort Remain = (ushort)(Sum % Codepage.Limit);
				Number = Sett.Rotors[i].FindIndexUsingValue(Remain);
			}
			return Number;
		}
		/// <summary>Inkrementuje èísla v poli. Nejnižší index znaèí nejvyšší øád.</summary>
		/// <param name="Numbers">Pole èísel.</param>
		private void Increment(ref ushort[] Numbers)
		{
			int Last = Numbers.Length - 1;
			Numbers[Last]++;
			for (int i = Last; i >= 0; i--)
			{
				if (Numbers[i] >= Codepage.Limit)
				{
					Numbers[i] = 0;
					if (i > 0)
					{
						Numbers[i - 1]++;
					}
				}
			}
		}
		/// <summary>Pøidá náhodné znaky do sady.</summary>
		/// <param name="Numbers">Sada.</param>
		private void AddRandomChars(ref List<ushort> Numbers)
		{
			Generators Gen = new (Codepage.Limit, DateTime.Now.Ticks);
			
		}
		/// <summary>Odebere náhodné znaky ze sady.</summary>
		/// <param name="Numbers">Sada.</param>
		private void RemoveRandomChars(ref List<ushort> Numbers)
		{
			
		}
		/// <summary>Generuje èísla podle zadané eliptické køivky. Y^2 = X^3 - AX + B. Èíslo pak modulo dìlí a posouvá.</summary>
		/// <param name="seed">Pøedchozí hodnota X.</param>
		/// <param name="A">A parametr rovnice.</param>
		/// <param name="B">B parametr rovnice.</param>
		/// <param name="from">Minimální hodnota (posun).</param>
		/// <param name="count">Èíslo na modulo dìlení.</param>
		private uint ElipticNumGen  (uint seed, uint A, uint B, uint from, uint count)
		{
			double yp = Math.Sqrt(Math.Pow(seed, 3) - A * seed + B);
			double yn = -yp;
			double slopep = (3 * Math.Pow(seed, 2) - A) / (2 * Math.Sqrt(Math.Pow(seed, 3) - A * seed + B));
			double slopen = -slopep;

			return 0;
		}
	}
}