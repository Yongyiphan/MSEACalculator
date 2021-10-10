using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MSEACalculator
{
    public static class GlobalVars
    {
        public static string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Maplestory.db");

        public static string databasePath
        {
            get 
            { 
                return path;
            }
            set
            {
                path = value;
            }
        }

    }
}
