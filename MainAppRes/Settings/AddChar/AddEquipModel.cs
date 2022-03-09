using MSEACalculator.OtherRes.Database;
using System.Collections.Generic;
using System.Linq;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.CharacterRes;
using System.Collections.ObjectModel;
using MSEACalculator.CalculationRes;
using MSEACalculator.OtherRes.Database.Tables;

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
            {"Armor", EquipArmorTable.GetAllArmorDB().AsReadOnly() },
            {"Accessory", EquipAccessoriesTable.GetAllAccessoriesDB().AsReadOnly()},
            {"Weapon", EquipMainWeaponTable.GetAllWeaponDB().AsReadOnly()},
            {"Secondary",EquipSecWeaponTable.GetAllSecondaryDB().AsReadOnly()}
        };
        public List<PotentialStatsCLS> AllPotDict { get => PotentialTable.GetAllPotentialDB(); }
        public List<PotentialStatsCLS> AllBonusPotDict { get => PotentialTable.GetAllBonusPotentialDB(); }

        public Dictionary<string, string> EquipSlot { get; set; } = EquipSlotTable.GetEquipSlotDB();
        public List<string> FlameStatsTypes { get; set; } = GVar.BaseStatTypes.Concat(GVar.SpecialStatType).ToList();

        public Dictionary<string, ReadOnlyCollection<StarforceCLS>> StarforceStore { get; } = new Dictionary<string, ReadOnlyCollection<StarforceCLS>>
        {
            {"Basic",StarForceTable.GetAllStarforceDB().AsReadOnly()},
            {"Superior",StarForceTable.GetAllSuperiorStarforceDB().AsReadOnly()}
        };


        public List<string> XenonClassType { get; } = new List<string>()
        {
            "Pirate", "Thief"
        };

    }
}
