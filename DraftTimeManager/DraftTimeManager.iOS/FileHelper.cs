using System;
using System.Collections.Generic;
using System.IO;

using Xamarin.Forms;
using DraftTimeManager.iOS;
using DraftTimeManager.Interfaces;

[assembly: Dependency(typeof(FileHelper))]
namespace DraftTimeManager.iOS
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, filename);
        }
    }
}
