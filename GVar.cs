using MSEACalculator.CalculationRes;
using System.Collections.Generic;
using System.IO;
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


    //STRING NAMING CONVERSION FOR REUSE
    //ACCESSORIES => Accessory
    //ARMOR => Armor
    //WEAPON => Weapon
    //SECONDARY => Secondary

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
        public static string CONN_STRING { get; set; } = $"Filename = {databasePath}";
        
        
        
        /// <summary>
        /// STATIC / CONSTANT VARIABLES
        /// </summary>

        




        
        


        //STAR FORCE
        public static List<string> CategoryAEquips { get; set; } = new List<string>
        {
            "Hat", "Top", "Bottom", "Overall", "Cape", "Ring", "Pendant", "Belt", "Shoulderpad", "Shield" ,"Weapon"
        };

        
        public static Dictionary<string, List<string>> EnhanceRestriction { get; set; } = new Dictionary<string, List<string>>()
        {
            {"Scroll", new List<string>
            { "Medal", "Pocket", "Badge", "Secondary", "Emblem"} }, //Event Rings

            {"Flame", new List<string>
            { "Medal", "Badge", "Ring", "Shoulder", "Heart", "Emblem", "Secondary"}},

             {"Potential", new List<string>
            { "Medal", "Pocket", "Badge"}},//Critical Ring, Onyx Ring, Oz Ring


            {"Starforce", new List<string>
            {"Medal", "Pocket", "Badge", "Emblem", "Secondary"} } //Event Rings, Oz Rings
        };


        //TOTAL 20 UNIQUE SLOTS + 3 EXTRA RING + 1 EXTRA PENDANT = 24 SLOTS
        //EQUIPMENT SLOTS BEGIN

        //12 UNIQUE SLOTS IN ACC EQUIPS
        public static List<string> AccEquips { get; set; } = new List<string>
        {
            "Ring", "Pocket",
            "Pendant", "Belt",
            "Face Accessory", "Eye Accessory",
            "Earrings", "Shoulder",
            "Emblem", "Medal", "Badge", "Heart"
        };

        //6 SLOT (TOP, BTM) / 5 SLOTS (OVERALLS)
        public static List<string> ArmorEquips { get; set; } = new List<string>
        {
            "Hat",
            "Bottom", "Top", "Overall",
            "Cape", "Gloves", "Shoes"
        };

        public static List<string> GrpByName { get; set; } = new List<string>
        {

        };


        //+ WEAPON
        //+ SECONDARY/ SHIELD

        //EQUIPMENT SLOTS END

        public static List<string> MainStats { get; set; } = new List<string>
        {
            "STR", "DEX", "INT", "LUK"
        };

        public static List<string> BaseStatTypes { get; set; } = new List<string> {
            "STR","DEX","INT","LUK",
            "MaxHP","MaxMP","DEF","SPD","JUMP",
            "ATK","MATK"
        };



        public static List<string> SpecialStatType { get; set; } = new List<string>
        {
            "BD","DMG", "AllStat", "HP" , "MP"
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

        public static List<string> RepeatOneMPot { get; set; } = new List<string>
        {
            "Decent", "Invincibility Time"
        };

        public static List<string> RepeatTwoMPot { get; set; } = new List<string>
        {
            "Boss", "Ignore Monster's DEF", "Item", "chance to ignore", "chance to be invincible"
        };

        public static List<string> RepeatTwoAPot { get; set; } = new List<string>
        {
            "Boss", "Ignore Monster's DEF", "Item"
        };

        public static StorageFolder storageFolder { get; set; } = ApplicationData.Current.LocalFolder;

    }
}
