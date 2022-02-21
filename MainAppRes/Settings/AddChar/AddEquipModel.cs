using MSEACalculator.OtherRes.Database;
using System.Collections.Generic;
using System.Linq;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.CharacterRes;

namespace MSEACalculator.MainAppRes.Settings.AddChar
{
    public class AddEquipModel
    {
        //ONLY RETREIVE
        public List<EquipCLS> AllArmorList { get => DBRetrieve.GetAllArmorDB(); }
        public List<EquipCLS> AllAccList { get => DBRetrieve.GetAllAccessoriesDB(); }
        public List<EquipCLS> AllWeapList { get => DBRetrieve.GetAllWeaponDB(); }
        public List<EquipCLS> AllSecList { get => DBRetrieve.GetAllSecondaryDB(); }
        public List<PotentialStatsCLS> AllPotDict { get => DBRetrieve.GetAllPotential(); }
        public Dictionary<string, string> EquipSlot { get; set; } = DBRetrieve.GetEquipSlotDB();
        public List<string> FlameStatsTypes { get; set; } = GVar.BaseStatTypes.Concat(GVar.SpecialStatType).ToList();

    }
}
