using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DraftTimeManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Timer : ContentView
    {
        public Timer()
        {
            InitializeComponent();
        }
    }
}