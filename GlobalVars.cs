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
        private static string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Maplestory.db");

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

        public static List<string> EquipmentSlots { get; set; } = new List<string>() 
        {
            "Ring1","Ring2", "Ring3", "Ring4", 
            "Pendant1", "Pendant2",
            "Face Accesssory", "EyeDecor","EarRing", "Badge","Medal", "Belt","Heart","Pocket",
            "Weapon","Emblem","Secondary",
            "Hat","Top", "Bottom","Shoulder", "Gloves", "Cape", "Shoes"
        };

        public static StorageFolder storageFolder { get; set; } = ApplicationData.Current.LocalFolder;
    }
}
