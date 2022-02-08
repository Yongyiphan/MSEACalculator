using System.Collections.Generic;
using System.IO;
using Windows.Storage;

using MSEACalculator.CalculationRes;
using MSEACalculator.CharacterRes;

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
        
        public const int MaxCrytalCount = 180;

        public static List<ArcaneSymbol> Symbols { get; set; } = new List<ArcaneSymbol> {
            new ArcaneSymbol
            {
                Name = "Vanishing Journey",
                BaseSymbolGain = 8,
                SubMap = "Reverse City",
                PartyQuestSymbols = 6
            },
            new ArcaneSymbol
            {
                Name = "Chew Chew",
                BaseSymbolGain = 4,
                SubMap = "Yum Yum",
                PartyQuestSymbols = 0,
                PQGainLimit = 15,
                SymbolExchangeRate = 1
            },
            new ArcaneSymbol
            {
                Name = "Lachelein",
                BaseSymbolGain = 8,
                PartyQuestSymbols = 0,
                PQGainLimit = 500,
                SymbolExchangeRate = 30
            },
            new ArcaneSymbol
            {
                Name = "Arcana",
                BaseSymbolGain = 8,
                PartyQuestSymbols = 0,
                PQGainLimit = 30,
                SymbolExchangeRate = 3
            },
            new ArcaneSymbol
            {
                Name = "Moras",
                BaseSymbolGain = 8,
                PartyQuestSymbols = 6
            },
            new ArcaneSymbol
            {
                Name = "Esfera",
                BaseSymbolGain = 8,
                PartyQuestSymbols = 6
            }

        };

        public const int MaxSymbolLvl = 20;
        public static int MaxSymbolExp { get; set; } = CalForm.CalMaxExp(MaxSymbolLvl);
        public static int MaxArcaneForce { get; set; } = CalForm.CalArcaneStatsForce(MaxSymbolLvl, "General")["ArcaneForce"] * Symbols.Count;
        public static string CONN_STRING { get; set; } = $"Filename = {databasePath}";


        public const int minLevel  = 1;
        public const int maxLevel  = 300;




        public static List<string> CategoryAEquips { get; set; } = new List<string>
        {
            "Hat", "Top", "Bottom", "Overall", "Cape", "Ring", "Pendant", "Belt", "Shoulderpad", "Shield" ,"Weapon"
        };
        public static List<string> AccEquips { get; set; } = new List<string>
        {
            "Ring", "Shoulder", "Pendant", "Face Accessory","Eye Accessory", "Earrings", "Belt", "Badge", "Medal", "Emblem", "Heart", "Pocket"
        };

        public static List<string> ArEquips { get; set; } = new List<string>
        {
            "Hat", "Bottom", "Top", "Overall", "Cape", "Gloves", "Shoes"
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


        ///Potential Display Constraints
        //Can only appear 1 time
        //Decent Skills of any kind
        //Invincibility time increase


        //Can only appear 2 time
        //Damage on boss monsters increase
        //Ignore monster DEF increase
        //Item Drop Rate increase
        //Chance to ignore % damage
        //Chance to become invincible
        public static List<string> PotentialGrade { get; set; } = new List<string>
        {
            "Rare","Epic", "Unique","Legendary"
        };

        public static List<string> RepeatOnePot { get; set; } = new List<string>
        {
            "Decent", "Invincibility Time"
        };

        public static List<string> RepeatTwoPot { get; set; } = new List<string>
        {
            "Boss", "Ignore Monster's DEF", "Item", "chance to ignore", "chance to be invincible"
        };

        public static StorageFolder storageFolder { get; set; } = ApplicationData.Current.LocalFolder;

    }
}
