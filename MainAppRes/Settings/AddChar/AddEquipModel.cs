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

        //Create Factory to generate Dictionary Content
        public IReadOnlyDictionary<string, ReadOnlyCollection<EquipCLS>> AllEquipStore { get; } = new Dictionary<string, ReadOnlyCollection<EquipCLS>>
        {
            //{"Armor", DBRetrieve.GetArmorDB().AsReadOnly() },
            //{"Accessory", DBRetrieve.GetAccessoriesDB().AsReadOnly()},
            //{"Weapon", DBRetrieve.GetWeaponDB().AsReadOnly()},
            //{"Secondary",DBRetrieve.GetSecondaryDB().AsReadOnly()}
        };
        private Dictionary<string, Dictionary<string, List<PotentialStatsCLS>>> _AllPotDict = DBRetrieve.GetAllPotentialDB();
        private Dictionary<string, Dictionary<string, List<PotentialStatsCLS>>> _AllBonusPotDict = DBRetrieve.GetAllBonusPotentialDB();
        public Dictionary<string, Dictionary<string, List<PotentialStatsCLS>>> AllPotDict { get => _AllPotDict; }
        public Dictionary<string, Dictionary<string, List<PotentialStatsCLS>>> AllBonusPotDict { get => _AllBonusPotDict; }

        public Dictionary<string, string> EquipSlot { get; set; } = DBRetrieve.GetEquipSlotDB();
        public List<string> FlameStatsTypes { get; set; } = GVar.BaseStatTypes.Concat(GVar.SpecialStatType).ToList();

        public Dictionary<string, ReadOnlyCollection<StarforceCLS>> StarforceStore { get; } = new Dictionary<string, ReadOnlyCollection<StarforceCLS>>
        {
            {"Basic",DBRetrieve.GetAllStarforceDB().AsReadOnly()},
            {"Superior",DBRetrieve.GetAllSuperiorStarforceDB().AsReadOnly()}
        };



        public List<string> XenonClassType { get; } = new List<string>()
        {
            "Pirate", "Thief"
        };

    }
}
