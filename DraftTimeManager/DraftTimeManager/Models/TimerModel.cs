using System;
using System.Collections.Generic;
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
    public class TimerModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<double> countList = new List<double>() { 40, 40, 35, 30, 25, 25, 20, 20, 15, 10, 10, 5, 5, 5, 5 };
        private List<double> intervalList = new List<double>() { 60, 90 };
        private double checktime;
        private bool endFlg = false;

        public int Pack { get; set; }
        public int PackMax { get; }
        public int Pick { get; set; }
        public int PickMax { get; set; }
        public double Time { get; set; }
        public Color TimeColor => Time <= 5 ? Color.FromHex("991845") : Color.FromHex("222");

        public string PickCount => IsInterval ? "Check picked card." : $"Pack {Pack}, Pick {Pick}";
        public string TimeCount => $"{Time:00}"; 

        public bool IsInterval { get; set; }
        public bool IsBtnStartEnabled => !IsTimerStart;
        public bool IsBtnPauseEnabled => IsTimerStart;
        public bool IsBtnResetEnabled => !IsTimerStart;
        public bool IsTimerStart { get; set; }
        public bool IsPackCheck { get; set; }
         
        public ImageSource RotateSource { get; set; }
        public double RotateY => Pack % 2 == 1 ? 0 : 180;
        public double lblOpacity => IsTimerStart ? 0 : 100;

        public TimerModel()
        {
            Initialize();
            PackMax = 3;
            PickMax = 14;
            checktime = 10;
        }

        public void Initialize()
        {
            Pack = 1;
            Pick = 1;
            Time = countList.First();
            IsInterval = false;
            IsTimerStart = false;
            RotateSource = ImageSource.FromResource("DraftTimeManager.Images.rotate.png");
        }

        public void Initialize(Settings setting)
        {
            PickMax = setting.Picks;
            checktime = setting.Pick_Interval;
        }

        public bool TimeMove(double timeunit)
        {
            Time -= timeunit;

            if (Time <= 0)
            {
                if (Pack >= PackMax && Pick >= PickMax)
                {
                    DependencyService.Get<ITextToSpeech>().Speak($"Draft is over. Please build your deck.");
                    Initialize();
                    return false;
                }

                if (IsInterval)
                {
                    IsInterval = false;
                    Pick = 1;
                    Pack++;
                    Time = countList.ElementAt(Pick - 1 + (countList.Count() - PickMax));
                    DependencyService.Get<ITextToSpeech>().Speak($"Draft pack {Pack}. {Time} seconds.");
                    return true;
                }

                if (IsPackCheck)
                {
                    IsPackCheck = false;
                    Pick++;
                    Time = countList.ElementAt(Pick - 1 + (countList.Count() - PickMax));
                    DependencyService.Get<ITextToSpeech>().Speak($"Draft. {Time} seconds.");
                    return true;
                }

                if (Pick < PickMax)
                {
                    IsPackCheck = true;
                    Time = checktime;
                    DependencyService.Get<ITextToSpeech>().Speak($"Check the number of cards.");
                }
                else
                {
                    IsInterval = true;
                    Time = intervalList.ElementAt(Pack - 1);
                    DependencyService.Get<ITextToSpeech>().Speak($"Check picked cards. {Time} seconds.");
                }
            }
            else if (Time <= 3 && countList.ElementAt(Pick - 1 + (countList.Count() - PickMax)) <= 5 && !IsPackCheck)
            {
                DependencyService.Get<ITextToSpeech>().Speak($"{Time}");
            }
            else if (Time <= 5 && countList.ElementAt(Pick - 1 + (countList.Count() - PickMax)) > 5 && !IsPackCheck)
            {
                DependencyService.Get<ITextToSpeech>().Speak($"{Time}");
            }

            return true;
        }

        public void TimerStart()
        {
            if (this.IsTimerStart) return;

            IsTimerStart = true;
            var timeunit = 1;
            DependencyService.Get<ISleepScreen>(DependencyFetchTarget.GlobalInstance).SleepDisabled();

            DependencyService.Get<ITextToSpeech>().Speak($"Draft Start. {Time} seconds.");
            Device.StartTimer(
                TimeSpan.FromSeconds(timeunit),
                () =>
                {
                    if (endFlg) 
                    {
                        IsTimerStart = false;
                        endFlg = false;
                        return false;   
                    }
                    return this.TimeMove(timeunit);
                });
        }

        public void TimerEnd()
        {
            endFlg = true;
            DependencyService.Get<ISleepScreen>(DependencyFetchTarget.GlobalInstance).SleepEnabled();
        }
       
        public void SetPickTime()
        {
            Time = countList.ElementAt(Pick - 1);
        }
    }
}
