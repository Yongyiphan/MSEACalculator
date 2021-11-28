using MSEACalculator.CharacterRes.EquipmentRes;
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

        public static string DBName { get; set; } = "Maplestory.db";
        public static string databasePath { get; set; } = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);

        public static int minLevel { get; set; } = 1;
        public static int maxLevel { get; set; } = 300;


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
