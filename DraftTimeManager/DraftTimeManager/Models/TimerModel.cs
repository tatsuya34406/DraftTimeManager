using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using DraftTimeManager.Interfaces;
using Xamarin.Forms;
using PropertyChanged;

namespace DraftTimeManager.Models
{
    public class TimerModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<double> countList = new List<double>() { 40, 35, 30, 25, 25, 20, 20, 15, 10, 10, 5, 5, 5, 5 };
        private List<double> intervalList = new List<double>() { 30, 40 };
        private int pickMax = 14;
        private int packMax = 3;
        private bool endFlg = false;

        public int Pack { get; set; }
        public int Pick { get; set; }
        public double Time { get; set; }

        public string PickCount => IsInterval ? "Check picked card." : $"Pack {Pack}, Pick {Pick}";
        public string TimeCount => $"{Time:00}"; 

        public bool IsInterval { get; set; }
        public bool IsBtnEnabled { get; set; }
        public bool IsTimerStart { get; set; }

        public TimerModel()
        {
            Initialize();
        }

        public void Initialize()
        {
            Pack = 1;
            Pick = 1;
            Time = countList.First();
            IsInterval = false;
            IsBtnEnabled = true;
            IsTimerStart = false;
        }

        public bool TimeMove(double timeunit)
        {
            Time -= timeunit;

            if (Time <= 0)
            {
                if (Pack >= packMax && Pick >= pickMax)
                {
                    DependencyService.Get<ITextToSpeech>().Speak($"Draft is over. Please build your deck.");
                    Initialize();
                    return false;
                }

                if (IsInterval)
                {
                    IsInterval = false;
                    Time = countList.First();
                    DependencyService.Get<ITextToSpeech>().Speak($"Draft pack {Pack}. {Time} seconds.");
                    return true;
                }

                if (Pick < pickMax)
                {
                    Pick++;
                    Time = countList.ElementAt(Pick - 1);
                    DependencyService.Get<ITextToSpeech>().Speak($"Draft. {Time} seconds.");
                }
                else
                {
                    Pick = 1;
                    Pack++;
                    IsInterval = true;
                    Time = intervalList.ElementAt(Pack - 2);
                    DependencyService.Get<ITextToSpeech>().Speak($"Check picked card. {Time} seconds.");
                }
            }
            else if((Time <= 5 && Pick <= 10) || Time <= 3)
            {
                DependencyService.Get<ITextToSpeech>().Speak($"{Time}");
            }

            return true;
        }

        public void TimerStart()
        {
            if (this.IsTimerStart) return;

            Initialize();
            IsBtnEnabled = false;
            IsTimerStart = true;
            var timeunit = 1;

            DependencyService.Get<ITextToSpeech>().Speak($"Draft Start. {countList.First()} seconds.");
            Device.StartTimer(
                TimeSpan.FromSeconds(timeunit),
                () =>
                {
                    if (endFlg) return false;
                    return this.TimeMove(timeunit);
                });
        }

        public void TimerEnd()
        {
            endFlg = true;
        }
    }
}
