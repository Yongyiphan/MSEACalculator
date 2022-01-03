using MSEACalculator.OtherRes.Database;
using MSEACalculator.MainAppRes.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class AddEquipModel
    {
        
        public List<EquipModel> AllArmorList { get => DBRetrieve.GetAllArmorDB(); }
        public List<EquipModel> AllAccList { get => DBRetrieve.GetAllAccessoriesDB(); }
        public List<EquipModel> AllWeapList { get => DBRetrieve.GetAllWeaponDB(); }


        public Dictionary<string, string> EquipSlot { get; set; } = DBRetrieve.GetEquipSlotDB();
        public List<string> FlameStatsTypes { get; set; } = GVar.BaseStatTypes.Concat(GVar.SpecialStatType).ToList();

        

        public List<string> AccGrp { get; } = new List<string>
        {
            "Accessory", "Ring", "Pendant"
        };

        public List<Character> AllCharList { get; set; }


        


        public Dictionary<string, Dictionary<int, Dictionary<int, ScrollingModel>>> SpellTraceDict { get; set; } = new Dictionary<string, Dictionary<int, Dictionary<int, ScrollingModel>>>
        {
            ["Armor"] = new Dictionary<int, Dictionary<int, ScrollingModel>>
            {
                [1] = new Dictionary<int, ScrollingModel>
                {
                    [100] = new ScrollingModel(1, 5, 1),
                    [70] = new ScrollingModel(2, 15, 2),
                    [30] = new ScrollingModel(3, 30, 4)
                },
                [2] = new Dictionary<int, ScrollingModel>
                {
                    [100] = new ScrollingModel(2, 20, 2),
                    [70] = new ScrollingModel(3, 40, 4),
                    [30] = new ScrollingModel(5, 70, 7)
                },
                [3] = new Dictionary<int, ScrollingModel>
                {
                    [100] = new ScrollingModel(3, 30, 3),
                    [70] = new ScrollingModel(4, 70, 5),
                    [30] = new ScrollingModel(7, 120, 10)
                }
            },
            ["Gloves"] = new Dictionary<int, Dictionary<int, ScrollingModel>>
            {
                [1] = new Dictionary<int, ScrollingModel>
                {
                    [100] = new ScrollingModel(0, 0, 3),
                    [70] = new ScrollingModel(1),
                    [30] = new ScrollingModel(2)
                },
                [2] = new Dictionary<int, ScrollingModel>
                {
                    [100] = new ScrollingModel(1),
                    [70] = new ScrollingModel(2),
                    [30] = new ScrollingModel(3)
                }
            },
            ["Accessory"] = new Dictionary<int, Dictionary<int, ScrollingModel>>
            {
                [1] = new Dictionary<int, ScrollingModel>
                {
                    [100] = new ScrollingModel(1, 0, 0),
                    [70] = new ScrollingModel(2, 0, 0),
                    [30] = new ScrollingModel(3, 0, 0)
                },
                [2] = new Dictionary<int, ScrollingModel>
                {
                    [100] = new ScrollingModel(1, 0, 0),
                    [70] = new ScrollingModel(2, 0, 0),
                    [30] = new ScrollingModel(4, 0, 0)
                },
                [3] = new Dictionary<int, ScrollingModel>
                {
                    [100] = new ScrollingModel(2, 0, 0),
                    [70] = new ScrollingModel(3, 0, 0),
                    [30] = new ScrollingModel(5, 0, 0)
                }
            },
            ["Heart"] = new Dictionary<int, Dictionary<int, ScrollingModel>>
            {
                [1] = new Dictionary<int, ScrollingModel>
                {
                    [100] = new ScrollingModel(1),
                    [70] = new ScrollingModel(2),
                    [30] = new ScrollingModel(3)
                },
                [2] = new Dictionary<int, ScrollingModel>
                {
                    [100] = new ScrollingModel(2),
                    [70] = new ScrollingModel(3),
                    [30] = new ScrollingModel(5)
                }
        ,
                [3] = new Dictionary<int, ScrollingModel>
                {
                    [100] = new ScrollingModel(3),
                    [70] = new ScrollingModel(4),
                    [30] = new ScrollingModel(7)
                }
            },
            ["Weapon"] = new Dictionary<int, Dictionary<int, ScrollingModel>>
            {
                [1] = new Dictionary<int, ScrollingModel>
                {
                    [100] = new ScrollingModel(1),
                    [70] = new ScrollingModel(2),
                    [30] = new ScrollingModel(1, 3),
                    [15] = new ScrollingModel(2, 5)
                },
                [2] = new Dictionary<int, ScrollingModel>
                {
                    [100] = new ScrollingModel(2),
                    [70] = new ScrollingModel(1, 3),
                    [30] = new ScrollingModel(2, 5),
                    [15] = new ScrollingModel(3, 7)
                }
        ,
                [3] = new Dictionary<int, ScrollingModel>
                {
                    [100] = new ScrollingModel(1, 3),
                    [70] = new ScrollingModel(2, 5),
                    [30] = new ScrollingModel(3, 7),
                    [15] = new ScrollingModel(4, 9)
                }
            }

        };

    }
}
