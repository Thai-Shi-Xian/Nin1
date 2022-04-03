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

	public class VPE_Sett : INotifyPropertyChanged
	{
		private string ShiftStrV;
		private ushort ShiftNumV;
		public string ShiftStr
		{
			get
			{
				return ShiftStrV;
			}
			set
			{
				if (ShiftStrV != value)
				{
					ShiftStrV = value;
					OnPropertyChanged("ShiftStr");
				}
			}
		}
		public ushort ShiftNum
		{
			get
			{
				if (ushort.TryParse(ShiftStrV, out ushort shiftNum))
				{
					return shiftNum;
				}
				else
				{
					return ushort.MaxValue;
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