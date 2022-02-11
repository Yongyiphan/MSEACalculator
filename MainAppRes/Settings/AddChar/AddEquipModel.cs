﻿using MSEACalculator.OtherRes.Database;
using System.Collections.Generic;
using System.Linq;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.CharacterRes;

namespace MSEACalculator.MainAppRes.Settings.AddChar
{
    public class AddEquipModel
    {
        
        public List<EquipCLS> AllArmorList { get => DBRetrieve.GetAllArmorDB(); }
        public List<EquipCLS> AllAccList { get => DBRetrieve.GetAllAccessoriesDB(); }
        public List<EquipCLS> AllWeapList { get => DBRetrieve.GetAllWeaponDB(); }
        public List<EquipCLS> AllSecList { get => DBRetrieve.GetAllSecondaryDB(); }


        public Dictionary<string, string> EquipSlot { get; set; } = DBRetrieve.GetEquipSlotDB();
        public List<string> FlameStatsTypes { get; set; } = GVar.BaseStatTypes.Concat(GVar.SpecialStatType).ToList();

        public List<PotentialStatsCLS> AllPotDict { get => DBRetrieve.GetAllPotential(); }

        public List<string> AccGrp { get; } = new List<string>
        {
            "Accessory", "Ring", "Pendant"
        };

        public List<CharacterCLS> AllCharList { get; set; }


        


        
    }
}
