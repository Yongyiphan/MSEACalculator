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
    //NAMING CONVENTION
    //STATIC DATA TABLE
    //CLASSNAME <- WINDARCHER, KANNA....
    //CLASSTYPE <-  WARRIOR, MAGE, BOWMAN, THIEF, PIRATE
    //ENTRY TYPE <- DAILY, WEEKLY, MONTHLY

    //EDITABLE DATA TABLE
    //CHARNAME <- WINDARCHER, KANNA....

    public static class GlobalVars
    {

        public static string DBName { get; set; } = "Maplestory.db";
        public static string databasePath { get; set; } = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);

        public static string CONN_STRING { get; set; } = $"Filename = {databasePath}";
        public static int minLevel { get; set; } = 1;
        public static int maxLevel { get; set; } = 300;


        public static List<string> BaseStatTypes { get; set; } = new List<string> {
            "STR","DEX","INT","LUK",
            "HP","MP","DEF","SPD","JUMP",
            "ATK","MATK"
        };

        public static List<string> SpecialStatType { get; set; } = new List<string>
        {
            "BD","DMG", "AllStat"
        };
        

        public static StorageFolder storageFolder { get; set; } = ApplicationData.Current.LocalFolder;


        //public static List<string> ArmorSet { get; set; } = DatabaseAccess.GetAllArmorDB().Select(x => x.EquipSet).ToList().Distinct().ToList();

        

        
    }
}
