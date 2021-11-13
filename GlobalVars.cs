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
        public static string databasePath { get; set; } = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Maplestory.db");


        //public static List<string> EquipmentSlots { get; set; } = new List<string>() 
        //{
        //    "Ring1","Ring2", "Ring3", "Ring4", 
        //    "Pendant1", "Pendant2",
        //    "Face Accesssory", "EyeDecor","EarRing", "Badge","Medal", "Belt","Heart","Pocket",
        //    "Weapon","Emblem","Secondary",
        //    "Hat","Top", "Bottom","Shoulder", "Gloves", "Cape", "Shoes"
        //};

        public static Dictionary<string, string> EquipmentDict { get; set; } = new Dictionary<string, string>()
        {
            {"Ring1", "Ring" },{"Ring2", "Ring" },{"Ring3", "Ring" },{"Ring4", "Ring" },
            {"Pendant1", "Pendant" },{"Pendant2", "Pendant" },
            {"Face Accessory", "Misc" },{"Eye Decor", "Misc" },{"Ear Ring", "Misc" },{"Badge", "Misc" },{"Medal", "Misc" },{"Belt", "Misc" },{"Heart", "Misc" },{"Pocket", "Misc" },{"Shoulder", "Misc"},
            {"Weapon", "Weapon" },{"Secondary", "Weapon" },{"Emblem", "Weapon" },
            {"Hat", "Armor" },{"Top", "Armor" },{"Bottom", "Armor" },{"Overall", "Armor" },{"Gloves", "Armor" },{"Cape", "Armor" },{"Shoes", "Armor" }

        };

        public static StorageFolder storageFolder { get; set; } = ApplicationData.Current.LocalFolder;


        public static List<string> ArmorSet { get; set; } = DatabaseAccess.GetAllArmorDB().Select(x => x.EquipSet).ToList().Distinct().ToList();



        
    }
}
