using System;
using System.Collections.Generic;
using System.Linq;

using Android.OS;
using Android.Content;
using Xamarin.Forms;
using DraftTimeManager.Droid;
using DraftTimeManager.Interfaces;

[assembly: Dependency(typeof(SleepScreen))]
namespace DraftTimeManager.Droid
{
    public class SleepScreen : ISleepScreen
    {
        PowerManager.WakeLock wl;
        public void SleepDisabled()
        {
            if (wl != null) return;
            PowerManager pm = (PowerManager)(Forms.Context.GetSystemService(Context.PowerService));
            wl = pm.NewWakeLock(WakeLockFlags.ScreenBright | WakeLockFlags.OnAfterRelease, "My Tag");

            wl.Acquire();
        }

        public void SleepEnabled()
        {
            if (wl != null)
            {
                wl.Release();
                wl = null; 
            }
        }
    }
}
