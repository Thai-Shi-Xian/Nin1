using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VPE;

namespace GUI
{
	/// <summary>Interakční logika pro UC_Table.xaml</summary>
	public partial class UC_Table : UserControl
	{
		private readonly Generators Generator = new(Codepage.Limit, DateTime.Now.Ticks);

		public ushort Pozition
		{
			get
			{
				return GetPozition();
			}
			set
			{
				Representation.Pozition = value.ToString();
			}
		}
		public ushort SelTable { get; private set; }

		public C_UC_Table Representation = new();
		public UC_Table()
		{
			InitializeComponent();
		}

		private ushort GetPozition()
		{
			if (ushort.TryParse (Representation.Pozition, out ushort poz))
			{
				return poz;
			}
			else
			{
				return ushort.MaxValue;
			}
		}
		
		public ushort GenerateRandNum()
		{
			Generator.UpdateSeed(DateTime.Now.Ticks);
			return Generator.GenerateNum();
		}
		public void UpdateTableList (List<uint> tables)
		{
			Representation.Rotors.Clear();
			Representation.Rotors = tables;
		}

		private void B_Rand_Shift_Click (object sender, RoutedEventArgs e)
		{
			Representation.Pozition = GenerateRandNum().ToString();
		}
	}
}