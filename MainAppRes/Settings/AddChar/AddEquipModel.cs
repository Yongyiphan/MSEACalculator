using MSEACalculator.OtherRes.Database;
using System.Collections.Generic;
using System.Linq;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.CharacterRes;

namespace MSEACalculator.MainAppRes.Settings.AddChar
{
    public class AddEquipModel
    {
        
        public List<EquipModel> AllArmorList { get => DBRetrieve.GetAllArmorDB(); }
        public List<EquipModel> AllAccList { get => DBRetrieve.GetAllAccessoriesDB(); }
        public List<EquipModel> AllWeapList { get => DBRetrieve.GetAllWeaponDB(); }
        public List<EquipModel> AllSecList { get => DBRetrieve.GetAllSecondaryDB(); }


        public Dictionary<string, string> EquipSlot { get; set; } = DBRetrieve.GetEquipSlotDB();
        public List<string> FlameStatsTypes { get; set; } = GVar.BaseStatTypes.Concat(GVar.SpecialStatType).ToList();

        public List<PotentialStats> AllPotDict { get => DBRetrieve.GetAllPotential(); }

        public List<string> AccGrp { get; } = new List<string>
        {
            "Accessory", "Ring", "Pendant"
        };

        public List<Character> AllCharList { get; set; }


        


        
    }
}
