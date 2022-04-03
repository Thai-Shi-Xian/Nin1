using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GUI
{
	/// <summary>
	/// Interakční logika pro UC_Table.xaml
	/// </summary>
	public partial class UC_Table : UserControl
	{
		private ushort PozitionString { get; set; }
		public ushort Pozition { get; private set; }
		public ushort SelTable { get; private set; }

		private C_UC_Table Representation = new();
		public UC_Table()
		{
			InitializeComponent();
		}

		public void UpdateTableList (List<ushort> tables)
		{
			Representation.Table = tables;
		}

		private void B_Rand_Shift_Click (object sender, RoutedEventArgs e)
		{

		}
	}
}