using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database
{
    public class TableColNames
    {
        
        public static Dictionary<string, Dictionary<string, string>> CharacterCN { get; set; } = new Dictionary<string, Dictionary<string, string>>()
        {
            {"ClassName", new Dictionary<string, string>() { {"Rename", "ClassName" },{"Type", "string"}} },
            {"Faction" , new Dictionary<string, string>() { {"Rename", "Faction" },{"Type", "string"} } },
            {"ClassType", new Dictionary<string, string>() { {"Rename", "ClassType" },{"Type", "string"} } },
            {"MainStat", new Dictionary<string, string>() { {"Rename", "MainStat" },{"Type", "string"} } },
            {"SecStat", new Dictionary<string, string>() { {"Rename", "SecStat" },{"Type", "string"} } },
            {"UnionEffect", new Dictionary<string, string>() { {"Rename", "UEffect" },{"Type", "string"} } },
            {"UnionStatType", new Dictionary<string, string>() { {"Rename", "UEffectType" },{"Type", "string"} } },

            {"Weapon", new Dictionary<string, string>() { {"Rename", "Weapon" },{"Type", "string"} } },
            {"Secondary", new Dictionary<string, string>() { {"Rename", "Secondary" },{"Type", "string"} } },
        };
        public static Dictionary<string, Dictionary<string, string>> EquipmentCN = new Dictionary<string, Dictionary<string, string>>()
        {
            {"EquipSlot", new Dictionary<string, string>() { {"Rename", "EquipSlot" },{"Type", "string"} } },
            {"EquipName", new Dictionary<string, string>() { {"Rename", "EquipName" },{"Type", "string"} } },
            {"WeaponType", new Dictionary<string, string>() { {"Rename", "WeaponType" },{"Type", "string"} } },

            {"ClassName", new Dictionary<string, string>() { {"Rename", "ClassName" },{"Type", "string"}} },
            {"ClassType", new Dictionary<string, string>() { {"Rename", "ClassType" },{"Type", "string"} } },

            {"Category", new Dictionary<string, string>() { {"Rename", "Category" },{"Type", "string"} } },
            {"EquipSet", new Dictionary<string, string>() { {"Rename", "EquipSet" },{"Type", "string"} } },
            {"EquipLevel", new Dictionary<string, string>() { {"Rename", "EquipLevel" },{"Type", "int"} } },

            {"STR", new Dictionary<string, string>() { {"Rename", "STR" },{"Type", "int"} } },
            {"DEX", new Dictionary<string, string>() { {"Rename", "DEX" },{"Type", "int"} } },
            {"INT", new Dictionary<string, string>() { {"Rename", "INT" },{"Type", "int"} } },
            {"LUK", new Dictionary<string, string>() { {"Rename", "LUK" },{"Type", "int"} } },

            {"All Stats", new Dictionary<string, string>() { {"Rename", "AllStats" },{"Type", "int"} } },
            {"Max HP", new Dictionary<string, string>() { {"Rename", "MaxHP" },{"Type", "int"} } },
            {"Max MP", new Dictionary<string, string>() { {"Rename", "MaxMP" },{"Type", "int"} } },
            {"Perc Max HP", new Dictionary<string, string>() { {"Rename", "PercMaxHP" },{"Type", "int"} } },
            {"Perc Max MP", new Dictionary<string, string>() { {"Rename", "PercMaxMP" },{"Type", "int"} } },
            {"Max DF"  , new Dictionary<string, string>() { {"Rename", "MaxDF" },{"Type", "int"} } },
            {"Defense", new Dictionary<string, string>() { {"Rename", "DEF" },{"Type", "int"} } },
            {"Weapon Attack", new Dictionary<string, string>() { {"Rename", "WATK" },{"Type", "int"} } },
            {"Magic Attack", new Dictionary<string, string>() { {"Rename", "MATK" },{"Type", "int"} } },
            {"Ignored Enemy Defense", new Dictionary<string, string>() { {"Rename", "IED" },{"Type", "int"} } },
            {"Boss Damage", new Dictionary<string, string>() { {"Rename", "BD" },{"Type", "int"} } },

            {"Attack Speed"  , new Dictionary<string, string>() { {"Rename", "AtkSpd" },{"Type", "int"} } },
            {"Movement Speed"  , new Dictionary<string, string>() { {"Rename", "Speed" },{"Type", "int"} } },
            {"Jump"  , new Dictionary<string, string>() { {"Rename", "Jump" },{"Type", "int"} } },

            {"Number of Upgrades"  , new Dictionary<string, string>() { {"Rename", "Upgrades" },{"Type", "int"} } },
            {"Rank"  , new Dictionary<string, string>() { {"Rename", "Rank" },{"Type", "int"} } },
        };


        public static Dictionary<string, Dictionary<string, string>> SetEffectCN = new Dictionary<string, Dictionary<string, string>>()
        {

            {"EquipSet", new Dictionary<string, string>() { {"Rename", "EquipSet" },{"Type", "string"} } },
            {"ClassType", new Dictionary<string, string>() { {"Rename", "ClassType" },{"Type", "string"} } },
            {"Set At", new Dictionary<string, string>() { {"Rename", "SetAt" },{"Type", "string"} } },
            
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



        };

        public static Dictionary<string, Dictionary<string, string>> PotentialCN = new Dictionary<string, Dictionary<string, string>>()
        {
            {"Slot"  , new Dictionary<string, string>() { {"Rename", "EquipSlot" },{"Type", "string"} } },
            {"Grade"  , new Dictionary<string, string>() { {"Rename", "Grade" },{"Type", "string"} } },
            {"Prime"  , new Dictionary<string, string>() { {"Rename", "Prime" },{"Type", "string"} } },
            {"DisplayStat"  , new Dictionary<string, string>() { {"Rename", "DisplayStat" },{"Type", "string"} } },
            {"StatT"  , new Dictionary<string, string>() { {"Rename", "StatType" },{"Type", "string"} } },

            {"MinLvl", new Dictionary<string, string>() { {"Rename", "MinLvl" },{"Type", "int"} } },
            {"MaxLvl", new Dictionary<string, string>() { {"Rename", "MaxLvl" },{"Type", "int"} } },
            {"Stat value", new Dictionary<string, string>() { {"Rename", "StatValue" },{"Type", "string"} } },
            {"Chance", new Dictionary<string, string>() { {"Rename", "Chance" },{"Type", "int"} } },
            {"Tick", new Dictionary<string, string>() { {"Rename", "Tick" },{"Type", "int"} } },


            {"CubeType", new Dictionary<string, string>() { {"Rename", "CubeType" },{"Type", "string"} } },
            {"Initial", new Dictionary<string, string>() { {"Rename", "Initial" },{"Type", "string"} } },
            {"In-game cube", new Dictionary<string, string>() { {"Rename", "GameCube" },{"Type", "string"} } },
            {"Cash cube", new Dictionary<string, string>() { {"Rename", "CashCube" },{"Type", "string"} } },
            {"Probaility", new Dictionary<string, string>() { {"Rename", "Probability" },{"Type", "string"} } },

        };

        public static Dictionary<string, Dictionary<string, string>> StarforceCN = new Dictionary<string, Dictionary<string, string>>()
        {

            {"SFID", new Dictionary<string, string>() { {"Rename", "SFID" },{"Type", "int"} } },

            {"MinLvl", new Dictionary<string, string>() { {"Rename", "MinLvl" },{"Type", "int"} } },
            {"MaxLvl", new Dictionary<string, string>() { {"Rename", "MaxLvl" },{"Type", "int"} } },
            {"Job Stat", new Dictionary<string, string>() { {"Rename", "JobStat" },{"Type", "int"} } },
            {"Non-Weapon Visible DEF", new Dictionary<string, string>() { {"Rename", "NonWeaponVDEF" },{"Type", "int"} } },
            {"Overall Visible DEF", new Dictionary<string, string>() { {"Rename", "OverallVDEF" },{"Type", "int"} } },
            {"Category A Max HP", new Dictionary<string, string>() { {"Rename", "CatAMaxHP" },{"Type", "int"} } },
            {"Weapon Max MP", new Dictionary<string, string>() { {"Rename", "WeaponMaxMP" },{"Type", "int"} } },
            {"Weapon Visible ATT", new Dictionary<string, string>() { {"Rename", "WeaponVATK" },{"Type", "int"} } },
            {"Weapon Visible Magic ATT", new Dictionary<string, string>() { {"Rename", "WeaponVMATK" },{"Type", "int"} } },
            {"Shoes Speed", new Dictionary<string, string>() { {"Rename", "ShoeSpeed" },{"Type", "int"} } },
            {"Shoes Jump", new Dictionary<string, string>() { {"Rename", "ShoeJump" },{"Type", "int"} } },
            {"Gloves Visible ATT", new Dictionary<string, string>() { {"Rename", "GloveVATK" },{"Type", "int"} } },
            {"Gloves Visible Magic ATT", new Dictionary<string, string>() { {"Rename", "GloveVMATK" },{"Type", "int"} } },
            {"Visible Stat", new Dictionary<string, string>() { {"Rename", "VStat" },{"Type", "int"} } },
            {"Non-Weapon Weapon ATT", new Dictionary<string, string>() { {"Rename", "NonWeaponATK" },{"Type", "int"} } },
            {"Non-Weapon Weapon Magic ATT", new Dictionary<string, string>() { {"Rename", "NonWeaponMATK" },{"Type", "int"} } },

            //Superior Item Addons
            {"Visible DEF", new Dictionary<string, string>() { {"Rename", "VDEF" },{"Type", "int"} } },
            {"Visible Stats", new Dictionary<string, string>() { {"Rename", "VStat" },{"Type", "int"} } },
            {"Visible ATT", new Dictionary<string, string>() { {"Rename", "VATK" },{"Type", "int"} } },


            //Success Rates Addons

            {"Title", new Dictionary<string, string>() { {"Rename", "Title" },{"Type", "string"} } },

            {"Attempt", new Dictionary<string, string>() { {"Rename", "Attempt" },{"Type", "int"} } },
            {"Success", new Dictionary<string, string>() { {"Rename", "Success" },{"Type", "string"} } },
            {"Maintain", new Dictionary<string, string>() { {"Rename", "Maintain" },{"Type", "string"} } },
            {"Decrease", new Dictionary<string, string>() { {"Rename", "Decrease" },{"Type", "string"} } },
            {"Destroy", new Dictionary<string, string>() { {"Rename", "Destroy" },{"Type", "string"} } },

            //Star Limit Addons

            {"Max Stars", new Dictionary<string, string>() { {"Rename", "MaxStars" },{"Type", "string"} } },
        };

        public static Dictionary<string, Dictionary<string, string>> UnionCN = new Dictionary<string, Dictionary<string, string>>()
        {

            {"Effect", new Dictionary<string, string>() { {"Rename", "Effect" },{"Type", "string"} } },
            {"EffectType", new Dictionary<string, string>() { {"Rename", "EffectType" },{"Type", "string"} } },

            {"B", new Dictionary<string, string>() { {"Rename", "B" },{"Type", "int"} } },
            {"A", new Dictionary<string, string>() { {"Rename", "A" },{"Type", "int"} } },
            {"S", new Dictionary<string, string>() { {"Rename", "S" },{"Type", "int"} } },
            {"SS", new Dictionary<string, string>() { {"Rename", "SS" },{"Type", "int"} } },
            {"SSS", new Dictionary<string, string>() { {"Rename", "SSS" },{"Type", "int"} } },
        };




        public static Dictionary<string, Dictionary<string, string>> NameReplacementType = new Dictionary<string, Dictionary<string, string>>()
        {
            //Ignore "Special Function" Col from AndroidData
                    };



    }
}
