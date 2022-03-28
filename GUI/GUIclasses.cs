﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
	public class C_UC_Table : INotifyPropertyChanged
	{
		private ushort PozitionV;
		private ushort TableV;
        public ushort Pozition
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

        public ushort Table
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
}