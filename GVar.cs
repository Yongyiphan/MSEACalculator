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



        public static Dictionary<string, Dictionary<string, string>> NameReplacementType = new Dictionary<string, Dictionary<string, string>>()
        {
            {"ClassName", new Dictionary<string, string>() { {"Rename", "ClassName" },{"Type", "string"} } },
            {"Faction" , new Dictionary<string, string>() { {"Rename", "Faction" },{"Type", "string"} } },
            {"ClassType", new Dictionary<string, string>() { {"Rename", "ClassType" },{"Type", "string"} } },
            {"MainStat", new Dictionary<string, string>() { {"Rename", "MainStat" },{"Type", "string"} } },
            {"SecStat", new Dictionary<string, string>() { {"Rename", "SecStat" },{"Type", "string"} } },
            {"UnionEffect", new Dictionary<string, string>() { {"Rename", "UEffect" },{"Type", "string"} } },
            {"UnionStatType", new Dictionary<string, string>() { {"Rename", "UEffectType" },{"Type", "string"} } },
            {"Weapon", new Dictionary<string, string>() { {"Rename", "Weapon" },{"Type", "string"} } },
            {"Secondary", new Dictionary<string, string>() { {"Rename", "Secondary" },{"Type", "string"} } },
            {"Effect", new Dictionary<string, string>() { {"Rename", "Effect" },{"Type", "string"} } },
            {"EffectType", new Dictionary<string, string>() { {"Rename", "EffectType" },{"Type", "string"} } },
            {"EquipSlot", new Dictionary<string, string>() { {"Rename", "EquipSlot" },{"Type", "string"} } },
            {"EquipName", new Dictionary<string, string>() { {"Rename", "EquipName" },{"Type", "string"} } },
            {"EquipSet", new Dictionary<string, string>() { {"Rename", "EquipSet" },{"Type", "string"} } },
            {"Category", new Dictionary<string, string>() { {"Rename", "Category" },{"Type", "string"} } },
            {"EquipLevel", new Dictionary<string, string>() { {"Rename", "EquipLevel" },{"Type", "int"} } },
            //Ignore "Special Function" Col from AndroidData
            {"Set At", new Dictionary<string, string>() { {"Rename", "SetAt" },{"Type", "string"} } },
            {"WeaponType", new Dictionary<string, string>() { {"Rename", "WeaponType" },{"Type", "string"} } },
           

            {"B", new Dictionary<string, string>() { {"Rename", "B" },{"Type", "int"} } },
            {"A", new Dictionary<string, string>() { {"Rename", "A" },{"Type", "int"} } },
            {"S", new Dictionary<string, string>() { {"Rename", "S" },{"Type", "int"} } },
            {"SS", new Dictionary<string, string>() { {"Rename", "SS" },{"Type", "int"} } },
            {"SSS", new Dictionary<string, string>() { {"Rename", "SSS" },{"Type", "int"} } },
            {"STR", new Dictionary<string, string>() { {"Rename", "STR" },{"Type", "int"} } },
            {"DEX", new Dictionary<string, string>() { {"Rename", "DEX" },{"Type", "int"} } },
            {"INT", new Dictionary<string, string>() { {"Rename", "INT" },{"Type", "int"} } },
            {"LUK", new Dictionary<string, string>() { {"Rename", "LUK" },{"Type", "int"} } },
            {"All Stats", new Dictionary<string, string>() { {"Rename", "AllStats" },{"Type", "int"} } },
            {"Max HP", new Dictionary<string, string>() { {"Rename", "MaxHP" },{"Type", "int"} } },
            {"Max MP", new Dictionary<string, string>() { {"Rename", "MaxMP" },{"Type", "int"} } },
            {"Perc Max HP", new Dictionary<string, string>() { {"Rename", "PercMaxHP" },{"Type", "int"} } },
            {"Perc Max MP", new Dictionary<string, string>() { {"Rename", "PercMaxMP" },{"Type", "int"} } },
            {"Defense", new Dictionary<string, string>() { {"Rename", "DEF" },{"Type", "int"} } },
            {"Weapon Attack", new Dictionary<string, string>() { {"Rename", "WATK" },{"Type", "int"} } },
            {"Magic Attack", new Dictionary<string, string>() { {"Rename", "MATK" },{"Type", "int"} } },
            {"Ignored Enemy Defense", new Dictionary<string, string>() { {"Rename", "IED" },{"Type", "int"} } },
            {"Boss Damage", new Dictionary<string, string>() { {"Rename", "BD" },{"Type", "int"} } },
            {"Critical Damage", new Dictionary<string, string>() { {"Rename", "CDMG" },{"Type", "int"} } },
            {"Damage", new Dictionary<string, string>() { {"Rename", "DMG" },{"Type", "int"} } },
            {"All Skills"  , new Dictionary<string, string>() { {"Rename", "AllSkills" },{"Type", "int"} } },
            {"Damage Against Normal Monsters"  , new Dictionary<string, string>() { {"Rename", "NDMG" },{"Type", "int"} } },
            {"Abnormal Status Resistance"  , new Dictionary<string, string>() { {"Rename", "StatusResist" },{"Type", "int"} } },
            {"Movement Speed"  , new Dictionary<string, string>() { {"Rename", "Speed" },{"Type", "int"} } },
            {"Jump"  , new Dictionary<string, string>() { {"Rename", "Jump" },{"Type", "int"} } },
            {"Number of Upgrades"  , new Dictionary<string, string>() { {"Rename", "Upgrades" },{"Type", "int"} } },
            {"Rank"  , new Dictionary<string, string>() { {"Rename", "Rank" },{"Type", "int"} } },
            {"Attack Speed"  , new Dictionary<string, string>() { {"Rename", "AtkSpd" },{"Type", "int"} } },
            {"Max DF"  , new Dictionary<string, string>() { {"Rename", "MaxDF" },{"Type", "int"} } },



            {"Slot"  , new Dictionary<string, string>() { {"Rename", "EquipSlot" },{"Type", "string"} } },
            {"Type"  , new Dictionary<string, string>() { {"Rename", "PotentialGroup" },{"Type", "string"} } },
            {"Grade"  , new Dictionary<string, string>() { {"Rename", "Grade" },{"Type", "string"} } },
            {"Prime"  , new Dictionary<string, string>() { {"Rename", "Prime" },{"Type", "string"} } },
            {"DisplayStat"  , new Dictionary<string, string>() { {"Rename", "DisplayStat" },{"Type", "string"} } },
            {"Stat"  , new Dictionary<string, string>() { {"Rename", "Stat" },{"Type", "string"} } },
            {"StatT"  , new Dictionary<string, string>() { {"Rename", "StatType" },{"Type", "string"} } },

            {"MinLvl", new Dictionary<string, string>() { {"Rename", "MinLvl" },{"Type", "int"} } },
            {"MaxLvl", new Dictionary<string, string>() { {"Rename", "MaxLvl" },{"Type", "int"} } },
            {"Stat value", new Dictionary<string, string>() { {"Rename", "StatValue" },{"Type", "int"} } },
            {"Chance", new Dictionary<string, string>() { {"Rename", "Chance" },{"Type", "int"} } },
            {"Duration", new Dictionary<string, string>() { {"Rename", "Duration" },{"Type", "int"} } },
            {"Reflect damage", new Dictionary<string, string>() { {"ReflectDmg", "EquipName" },{"Type", "int"} } },
            {"Tick", new Dictionary<string, string>() { {"Rename", "Tick" },{"Type", "int"} } },
            

            {"CubeType", new Dictionary<string, string>() { {"Rename", "CubeType" },{"Type", "string"} } },
            {"Initial", new Dictionary<string, string>() { {"Rename", "Initial" },{"Type", "string"} } },
            {"In-game cube", new Dictionary<string, string>() { {"Rename", "GameCube" },{"Type", "string"} } },
            {"Cash cube", new Dictionary<string, string>() { {"Rename", "CashCube" },{"Type", "string"} } }
           

        };

        public static string f = "";



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
