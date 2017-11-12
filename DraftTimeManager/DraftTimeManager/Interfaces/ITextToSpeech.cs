using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftTimeManager.Interfaces
{
    public interface ITextToSpeech
    {
        void Speak(string text);
    }
}
