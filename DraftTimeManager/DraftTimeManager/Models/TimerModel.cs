using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace DraftTimeManager.Models
{
    public class TimerModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int pack;
        public int pick;
        public double time;
        private bool isInterval;
        private bool isBtnEnabled;
        public List<double> countList;
        public List<double> intervalList;

        protected int pickMax = 14;
        protected int packMax = 3;

        public int Pack
        {
            get { return this.pack; }
            set
            {
                if(this.pack != value)
                {
                    this.pack = value;
                    OnPropertyChanged("PickCount");
                }
            }
        }
        public int Pick
        {
            get { return this.pick; }
            set
            {
                if (this.pick != value)
                {
                    this.pick = value;
                    OnPropertyChanged("PickCount");
                }
            }
        }
        public double Time
        {
            get { return this.time; }
            set
            {
                if (this.time != value)
                {
                    this.time = value;
                    OnPropertyChanged("TimeCount");
                }
            }
        }

        public string PickCount
        {
            get
            {
                return IsInterval 
                    ? "Check picked card."
                    : $"Pack {Pack}, Pick {Pick}";
            }
        }
        public string TimeCount { get { return $"{time:00}"; } }

        public bool IsInterval
        {
            get { return isInterval; }
            set
            {
                if (this.isInterval != value)
                {
                    this.isInterval = value;
                    OnPropertyChanged("PickCount");
                }
            }
        }

        public bool IsBtnEnabled
        {
            get { return this.isBtnEnabled; }
            set
            {
                if (this.isBtnEnabled != value)
                {
                    this.isBtnEnabled = value;
                    OnPropertyChanged("IsBtnEnabled");
                }
            }
        }

        public TimerModel()
        {
            countList = new List<double>() { 40, 35, 30, 25, 25, 20, 20, 15, 10, 10, 5, 5, 5, 5 };
            intervalList = new List<double>() { 30, 40 };
            Initialize();
        }

        public void Initialize()
        {
            Pack = 1;
            Pick = 1;
            Time = countList.First();
            IsInterval = false;
            isBtnEnabled = true;
        }

        public bool TimeMove(double timeunit)
        {
            Time -= timeunit;

            if (Time <= 0)
            {
                if (Pack >= packMax && Pick >= pickMax)
                {
                    IsBtnEnabled = true;
                    return false;
                }

                if (IsInterval)
                {
                    IsInterval = false;
                    Time = countList.First();
                    return true;
                }

                if (pick < pickMax)
                {
                    Pick++;
                    Time = countList.ElementAt(pick - 1);
                }
                else
                {
                    Pick = 1;
                    Pack++;
                    IsInterval = true;
                    Time = intervalList.ElementAt(pack - 2);
                }
            }

            return true;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
