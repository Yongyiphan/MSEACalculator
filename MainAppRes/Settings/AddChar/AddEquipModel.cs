using MSEACalculator.OtherRes.Database;
using System.Collections.Generic;
using System.Linq;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.CharacterRes;
using System.Collections.ObjectModel;

namespace MSEACalculator.MainAppRes.Settings.AddChar
{
    public class AddEquipModel
    {
        //ONLY RETREIVE

        //private List<EquipCLS> AllArmorList { get => DBRetrieve.GetAllArmorDB(); }
        //private List<EquipCLS> AllAccList { get => DBRetrieve.GetAllAccessoriesDB(); }
        //private List<EquipCLS> AllWeapList { get => DBRetrieve.GetAllWeaponDB(); }
        //private List<EquipCLS> AllSecList { get => DBRetrieve.GetAllSecondaryDB(); }

        public IReadOnlyDictionary<string, ReadOnlyCollection<EquipCLS>> AllEquipStore { get;} = new Dictionary<string, ReadOnlyCollection<EquipCLS>>
        {
            {"Armor", DBRetrieve.GetAllArmorDB().AsReadOnly() },
            {"Accessory", DBRetrieve.GetAllAccessoriesDB().AsReadOnly()},
            {"Weapon", DBRetrieve.GetAllWeaponDB().AsReadOnly()},
            {"Secondary",DBRetrieve.GetAllSecondaryDB().AsReadOnly()}
        };
        public List<PotentialStatsCLS> AllPotDict { get => DBRetrieve.GetAllPotential(); }
        public List<PotentialStatsCLS> AllBonusPotDict { get => DBRetrieve.GetAllBonusPotential(); }

        public Dictionary<string, string> EquipSlot { get; set; } = DBRetrieve.GetEquipSlotDB();
        public List<string> FlameStatsTypes { get; set; } = GVar.BaseStatTypes.Concat(GVar.SpecialStatType).ToList();



    }
}
