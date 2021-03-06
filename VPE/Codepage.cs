using System;
using System.Collections.Generic;

namespace VPE
{
	public static class Codepage
	{
		/// <summary>Znaková sada.</summary>
		public static readonly List <string> Letters = new List<string>()
		{
			" ",
			"A",
			"Á",
			"Ä",
			"B",
			"C",
			"Č",
			"D",
			"Ď",
			"E",
			"É",
			"Ě",
			"Ë",
			"F",
			"G",
			"H",
			"I",
			"Í",
			"Ï",
			"J",
			"K",
			"L",
			"Ł",
			"M",
			"N",
			"Ň",
			"O",
			"Ó",
			"Ö",
			"P",
			"Q",
			"R",
			"Ř",
			"S",
			"Š",
			"T",
			"Ť",
			"U",
			"Ú",
			"Ů",
			"Ü",
			"V",
			"W",
			"X",
			"Y",
			"Ý",
			"Ÿ",
			"Z",
			"Ž",
			"a",
			"á",
			"ä",
			"b",
			"c",
			"č",
			"d",
			"ď",
			"e",
			"é",
			"ě",
			"ë",
			"f",
			"g",
			"h",
			"i",
			"í",
			"ï",
			"j",
			"k",
			"l",
			"ł",
			"m",
			"n",
			"ň",
			"o",
			"ó",
			"ö",
			"p",
			"q",
			"r",
			"ř",
			"s",
			"š",
			"ß",
			"t",
			"ť",
			"u",
			"ú",
			"ů",
			"ü",
			"v",
			"w",
			"x",
			"y",
			"ý",
			"ÿ",
			"z",
			"ž",
			"0",
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			",",
			".",
			"-",
			"?",
			"!",
			"_",
			":",
			";",
			"(",
			")",
			"[",
			"]",
			"{",
			"}",
			"<",
			">",
			"–",
			"—",
			"…",
			"„",
			"“",
			"/",
			"\\",
			"|",
			"\'",
			"\"",
			"%",
			"‰",
			"=",
			"+",
			"*",
			"^",
			"@",
			"$",
			"#",
			"€",
			"§",
			"°",
			"÷",
			"×",
			"©",
			"®",
			"≠",
			"≈",
			"←",
			"↑",
			"→",
			"↓",
			"α",
			"β",
			"γ",
			"δ",
			"μ",
			"π",
			"Ω",
			"∆",
			"∏",
			"∑",
			"Φ",
			"Λ",
			"\t",
			"\r\n",
		};
		private static ushort LimitV = Convert.ToUInt16(Letters.Count);
		/// <summary>Počet znaků v tabulce.</summary>
		public static ushort Limit
		{
			get
			{
				return LimitV;
			}
		}
	}
}