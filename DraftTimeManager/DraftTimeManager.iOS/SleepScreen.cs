using System;
using System.Collections.Generic;
using System.IO;

using Xamarin.Forms;
using DraftTimeManager.iOS;
using DraftTimeManager.Interfaces;
using UIKit;

[assembly: Dependency(typeof(SleepScreen))]
namespace DraftTimeManager.iOS
{
    public class SleepScreen : ISleepScreen
    {
        public void SleepDisabled()
        {
            UIApplication.SharedApplication.IdleTimerDisabled = true;
        }

        public void SleepEnabled()
        {
            UIApplication.SharedApplication.IdleTimerDisabled = false;
        }
    }
}
