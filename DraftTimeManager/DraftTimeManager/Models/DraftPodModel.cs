using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using DraftTimeManager.Interfaces;
using DraftTimeManager.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PropertyChanged;

namespace DraftTimeManager.Models
{
    public class DraftPodModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        int minPlayer = 4;
        int maxPlayer = 8;
        int tempGuest = 0;

        public ObservableCollection<int> PlayerNumbers { get; set; }
        public ObservableCollection<int> GuestNumbers { get; set; }

        public int SelectPlayerNumber { get; set; }
        public int SelectGuestNumber { get; set; }

        public DraftPodModel()
        {
            SelectPlayerNumber = 8;
            SelectGuestNumber = 8;
            PlayerNumbers = new ObservableCollection<int>(Enumerable.Range(minPlayer, maxPlayer - (minPlayer - 1)).Reverse());
            GuestNumbers = new ObservableCollection<int>(Enumerable.Range(1, maxPlayer).Reverse());
        }

        public void SetGuestList()
        {
            tempGuest = SelectGuestNumber;
            GuestNumbers = new ObservableCollection<int>(Enumerable.Range(1, SelectPlayerNumber).Reverse());
        }

        public void SetGuest()
        {
            if (SelectGuestNumber != 0) return;
            SelectGuestNumber = SelectPlayerNumber < tempGuest ? SelectPlayerNumber : tempGuest;
        }
    }
}
