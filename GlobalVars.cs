using MSEACalculator.CharacterRes.EquipmentRes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Data.Sqlite;

namespace MSEACalculator
{
    public static class GlobalVars
    {
        public static string DBFileName { get; set; } = "Maplestory.db";

        public static string databasePath { get; set; } = Path.Combine(ApplicationData.Current.LocalFolder.Path,DBFileName);
        


        public static List<string> BaseStatTypes { get; set; } = new List<string> {
            "STR","DEX","INT","LUK","ALL",
            "HP","MP","DEF","SPD","JUMP",
            "ATK"
        };

        public static List<string> AddStatType { get; set; } = new List<string>
        {
            "BD","IED","DMG"
        };
        

        public static StorageFolder storageFolder { get; set; } = ApplicationData.Current.LocalFolder;


        //public static List<string> ArmorSet { get; set; } = DatabaseAccess.GetAllArmorDB().Select(x => x.EquipSet).ToList().Distinct().ToList();

        

        
    }
}
