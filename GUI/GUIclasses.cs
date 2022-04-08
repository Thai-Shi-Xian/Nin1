using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
	public class C_UC_Table : INotifyPropertyChanged
	{
		private string PozitionV;
		private List<ushort> TableV = new();
		public string Pozition
		{
			get
			{
				return PozitionV;
			}
			set
			{
				if (PozitionV != value)
				{
					PozitionV = value;
					OnPropertyChanged("Pozition");
				}
			}
		}

		public List<ushort> Table
		{
			get
			{
				return TableV;
			}
			set
			{
				if (TableV != value)
				{
					TableV = value;
					OnPropertyChanged("Table");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string info)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(info));
			}
		}
	}

	public class C_VPE_Sett : INotifyPropertyChanged
	{
		private List<ushort> SwapsV = new();
		private List<ushort> ReflsV = new();
		private string ConstShiftStrV;
		private string VarShiftStrV;
		private string RandCharFreqStrV;
		private string RotorGenCountStrV;
		private string SwapGenCountStrV;
		private string ReflGenCountStrV;
		public string ConstShiftStr
		{
			get
			{
				return ConstShiftStrV;
			}
			set
			{
				if (ConstShiftStrV != value)
				{
					ConstShiftStrV = value;
					OnPropertyChanged("ConstShiftStr");
				}
			}
		}
		public ushort? ConstShiftNum
		{
			get
			{
				if (ushort.TryParse(ConstShiftStrV, out ushort shiftNum))
				{
					return shiftNum;
				}
				else
				{
					return null; // Neplatné číslo.
				}
			}
		}
		public string VarShiftStr
		{
			get
			{
				return VarShiftStrV;
			}
			set
			{
				if (VarShiftStrV != value)
				{
					VarShiftStrV = value;
					OnPropertyChanged("VarShiftStr");
				}
			}
		}
		public ushort? VarShiftNum
		{
			get
			{
				if (ushort.TryParse(VarShiftStrV, out ushort shiftNum))
				{
					return shiftNum;
				}
				else
				{
					return null; // Neplatné číslo.
				}
			}
		}
		public string RandCharFreqStr
		{
			get
			{
				return RandCharFreqStrV;
			}
			set
			{
				if (RandCharFreqStrV != value)
				{
					RandCharFreqStrV = value;
					OnPropertyChanged("RandCharFreqStr");
				}
			}
		}
		public ushort? RandCharFreqNum
		{
			get
			{
				if (ushort.TryParse(RandCharFreqStrV, out ushort shiftNum))
				{
					return shiftNum;
				}
				else
				{
					return null; // Neplatné číslo.
				}
			}
		}
		public string RotorGenCountStr
		{
			get
			{
				return RotorGenCountStrV;
			}
			set
			{
				if (RotorGenCountStrV != value)
				{
					RotorGenCountStrV = value;
					OnPropertyChanged("RotorGenCountStr");
				}
			}
		}
		public ushort? RotorGenCountNum
		{
			get
			{
				if (ushort.TryParse(RotorGenCountStrV, out ushort shiftNum))
				{
					return shiftNum;
				}
				else
				{
					return null; // Neplatné číslo.
				}
			}
		}
		public string SwapGenCountStr
		{
			get
			{
				return SwapGenCountStrV;
			}
			set
			{
				if (SwapGenCountStrV != value)
				{
					SwapGenCountStrV = value;
					OnPropertyChanged("SwapGenCountStr");
				}
			}
		}
		public ushort? SwapGenCountNum
		{
			get
			{
				if (ushort.TryParse(SwapGenCountStrV, out ushort shiftNum))
				{
					return shiftNum;
				}
				else
				{
					return null; // Neplatné číslo.
				}
			}
		}
		public string ReflGenCountStr
		{
			get
			{
				return ReflGenCountStrV;
			}
			set
			{
				if (ReflGenCountStrV != value)
				{
					ReflGenCountStrV = value;
					OnPropertyChanged("ReflGenCountStr");
				}
			}
		}
		public ushort? ReflGenCountNum
		{
			get
			{
				if (ushort.TryParse(ReflGenCountStrV, out ushort shiftNum))
				{
					return shiftNum;
				}
				else
				{
					return null; // Neplatné číslo.
				}
			}
		}
		public List<ushort> Swaps
		{
			get
			{
				return SwapsV;
			}
			set
			{
				if (SwapsV != value)
				{
					SwapsV = value;
					OnPropertyChanged("Swaps");
				}
			}
		}
		public List<ushort> Refls
		{
			get
			{
				return ReflsV;
			}
			set
			{
				if (ReflsV != value)
				{
					ReflsV = value;
					OnPropertyChanged("Refls");
				}
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string info)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(info));
			}
		}
	}
}