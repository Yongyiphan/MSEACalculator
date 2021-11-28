using MSEACalculator.CharacterRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.MainAppRes.Settings
{
    public class ACTModel
    {


        public Dictionary<string, Character> AllCharDict { get; set; } = DatabaseAccess.GetAllCharDB();
        public Dictionary<string, Character> AllCharTrackDict { get; set; } = DatabaseAccess.GetAllCharTrackDB();
        public List<EquipModel> AllArmorList { get; set; } = DatabaseAccess.GetAllArmorDB();
        public Dictionary<string, string> EquipSlot { get; set; } = DatabaseAccess.GetEquipSlotDB();
        public List<string> FlameStatsTypes { get; set; } = GlobalVars.BaseStatTypes.Concat(GlobalVars.AddStatType).ToList();

        public List<EquipModel> AllAccList { get; set; } = DatabaseAccess.GetAllAccessoriesDB();


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
            ["Accessories"] = new Dictionary<int, Dictionary<int, ScrollingModel>>
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

        public ACTModel()
        {
            //ArmorSets = AllArmorList.Select(x => x.EquipSet).ToList().Distinct().ToList();
            AllCharList = AllCharDict.Values.ToList();
        }

    }
}
