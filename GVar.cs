﻿using System.Collections.Generic;
using System.IO;
using Windows.Storage;

using MSEACalculator.OtherRes;

namespace MSEACalculator
{
    //NAMING CONVENTION
    //STATIC DATA TABLE
    //CLASSNAME <- WINDARCHER, KANNA....
    //CLASSTYPE <-  WARRIOR, MAGE, BOWMAN, THIEF, PIRATE
    //ENTRY TYPE <- DAILY, WEEKLY, MONTHLY

    //EDITABLE DATA TABLE
    //CHARNAME <- WINDARCHER, KANNA....

    public static class GVar
    {
        /// <summary>
        /// COMMON PATHS
        /// </summary>
        public static string DBName { get; set; } = "Maplestory.db";
        public static string databasePath { get; set; } = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);

        public const string CharacterPath = @"\DefaultData\CharacterData\";
        public const string EquipmentPath = @"\DefaultData\EquipmentData\";
        public const string CalculationsPath = @"\DefaultData\CalculationsData\";



        /// <summary>
        /// STATIC / CONSTANT VARIABLES
        /// </summary>
        public const int MaxCrytalCount = 120;
        public const int MaxSymbolLvl = 20;
        public static int MaxSymbolExp { get; set; } = CalculationFormulas.CalMaxExp(MaxSymbolLvl);
        public static string CONN_STRING { get; set; } = $"Filename = {databasePath}";


        public const int minLevel  = 1;
        public const int maxLevel  = 300;




        public static List<string> CategoryAEquips { get; set; } = new List<string>
        {
            "Hat", "Top", "Bottom", "Overall", "Cape", "Ring", "Pendant", "Belt", "Shoulderpad", "Shield" ,"Weapon"
        };

        public static List<string> BaseStatTypes { get; set; } = new List<string> {
            "STR","DEX","INT","LUK",
            "HP","MP","DEF","SPD","JUMP",
            "ATK","MATK"
        };

        public static List<string> SpecialStatType { get; set; } = new List<string>
        {
            "BD","DMG", "AllStat"
        };

        public static List<string> PotentialGrade { get; set; } = new List<string>
        {
            "Rare","Epic", "Unique","Legendary"
        };


        public static StorageFolder storageFolder { get; set; } = ApplicationData.Current.LocalFolder;



        

        
    }
}