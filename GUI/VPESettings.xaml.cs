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
		public C_VPE_Sett DataFromGUI = new();
		public VPESettings (ref VPE_VM VModel)
		{
			InitializeComponent ();
			VPE = VModel;
		}
		#region GUI eventy
		private void B_Submit_Click (object sender, RoutedEventArgs e)
		{ // ToDo: This whole method is just one big to do.
			
			Close ();
		}

		private void B_GenRotors_Click (object sender, RoutedEventArgs e)
		{
			if (DataFromGUI.RotorGenCountNum is not null)
			{
				VPE?.GenerateRotors(DataFromGUI.RotorGenCountNum.Value);
				foreach (UC_Table rotor in SP_Rotors.Children)
				{
					rotor.UpdateTableList(VPE.Rotors);
				}
			}
		}

		private void B_GenRefl_Click (object sender, RoutedEventArgs e)
		{
			VPE?.GenerateReflector ();
		}

		private void B_GenSwaps_Click (object sender, RoutedEventArgs e)
		{
			VPE?.GenerateSwaps ();
		}

		private void B_Rotors_Add_Click(object sender, RoutedEventArgs e)
		{
			TablesInGUI = Convert.ToUInt16(SP_Rotors.Children.Count);
			if (TablesInGUI < TablesMax)
			{
				UC_Table table = new()
				{
					Name = "UCT_" + (TablesInGUI - 1).ToString(),
				};
				SP_Rotors.Children.Add(table);
				B_Rotors_Add.IsEnabled = TablesInGUI < TablesMax;
			}
		}

		private void B_Rotors_Remove_Click(object sender, RoutedEventArgs e)
		{
			if (SP_Rotors.Children.Count > 0)
			{
				SP_Rotors.Children.RemoveAt(SP_Rotors.Children.Count - 1);
				B_Rotors_Remove.IsEnabled = SP_Rotors.Children.Count > 0;
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
			VPE?.SaveAll();
		}

		private void B_Load_Click(object sender, RoutedEventArgs e)
		{
			VPE.LoadAll();
		}

		private void B_LoadMerge_Click(object sender, RoutedEventArgs e)
		{
			VPE.LoadAndMerge();
		}

		private void B_GenerateConstShift(object sender, RoutedEventArgs e)
		{
			VPE?.GenerateRandNum();
		}

		private void B_GenerateVarShift(object sender, RoutedEventArgs e)
		{
			VPE?.GenerateRandNum();
		}

		private void B_RandChars(object sender, RoutedEventArgs e)
		{
			VPE?.GenerateRandNum();
		}
		#endregion
	}
}