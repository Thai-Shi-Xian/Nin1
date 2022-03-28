using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VPE;

namespace GUI
{
	/// <summary>
	/// Interakční logika pro VPESettings.xaml
	/// </summary>
	public partial class VPESettings : Window
	{
		private readonly VPE_VM VPE;
		private ushort TablesInGUI = 0, SwapsInGUI = 0;
		private const ushort TablesMax = 20, SwapsMax = 20; // Kolik tam může být maximálně tabulek a swapů, v GUI.
		public VPESettings (ref VPE_VM VModel)
		{
			InitializeComponent ();
			VPE = VModel;
		}
		#region GUI eventy
		private void B_Submit_Click (object sender, RoutedEventArgs e)
		{ // ToDo: This whole method is just one big to do.
			List<ushort> tables = new();
			List<ushort> pozitions = new();
			foreach (UC_Table table in SP_Tables.Children)
			{
				tables.Add (table.SelTable);
				pozitions.Add(table.Pozition);
			}
			List<ushort> swaps = new();
			foreach (ComboBox swap in SP_Swaps.Children)
			{
				swaps.Add ((ushort)swap.SelectedItem);
			}
			ushort reflector = (ushort)CB_Reflector.SelectedItem;// ToDo: make it safe/working!
			ushort shift = ushort.Parse(TB_Shift.Text); // ToDo: make it safe!
			VPE?.SelectSettings(tables, pozitions, swaps, reflector, shift);
			Close ();
		}

		private void B_GenTables_Click (object sender, RoutedEventArgs e)
		{
			VPE?.GenerateTables ();
		}

		private void B_GenRefl_Click (object sender, RoutedEventArgs e)
		{
			VPE?.GenerateReflector ();
		}

		private void B_GenSwaps_Click (object sender, RoutedEventArgs e)
		{
			VPE?.GenerateSwaps ();
		}

		private void B_Tables_Add_Click(object sender, RoutedEventArgs e)
		{
			TablesInGUI = Convert.ToUInt16(SP_Tables.Children.Count);
			if (TablesInGUI < TablesMax)
			{
				UC_Table table = new()
				{
					Name = "UCT_" + (TablesInGUI - 1).ToString(),
				};
				SP_Tables.Children.Add(table);
				B_Tables_Add.IsEnabled = TablesInGUI < TablesMax;
			}
		}

		private void B_Tables_Remove_Click(object sender, RoutedEventArgs e)
		{
			if (SP_Tables.Children.Count > 0)
			{
				SP_Tables.Children.RemoveAt(SP_Tables.Children.Count - 1);
				B_Tables_Remove.IsEnabled = SP_Tables.Children.Count > 0;
			}
		}

		private void B_Swaps_Add_Click(object sender, RoutedEventArgs e)
		{
			SwapsInGUI = Convert.ToUInt16(SP_Swaps.Children.Count);
			if (SwapsInGUI < SwapsMax)
			{
				ComboBox swap = new()
				{
					Name = "CB_Swap_" + (SwapsInGUI - 1).ToString(),
				};
				SP_Swaps.Children.Add(swap);
				B_Swaps_Add.IsEnabled = SwapsInGUI < SwapsMax;
			}
		}

		private void B_Swaps_Remove_Click(object sender, RoutedEventArgs e)
		{
			if (SP_Swaps.Children.Count > 0)
			{
				SP_Swaps.Children.RemoveAt(SP_Swaps.Children.Count - 1);
				B_Swaps_Remove.IsEnabled = SP_Swaps.Children.Count > 0;
			}
		}

		private void B_Save_Click(object sender, RoutedEventArgs e)
		{
			VPE?.Save();
		}

		private void B_Load_Click(object sender, RoutedEventArgs e)
		{
			VPE.Load();
		}

		private void B_LoadMerge_Click(object sender, RoutedEventArgs e)
		{
			VPE.LoadAndMerge();
		}

		private void B_Add_Shift(object sender, RoutedEventArgs e)
		{
			VPE.Add_Shift();

		}

		private void B_Substract_Shift(object sender, RoutedEventArgs e)
		{
			VPE.Substract_Shift();
		}
		#endregion
	}
}