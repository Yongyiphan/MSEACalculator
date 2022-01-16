using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using MSEACalculator.BossRes;
using MSEACalculator.StarforceRes;
using MSEACalculator.EventRes;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.UI.Popups;
using Windows.UI.Core;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes.Database;

namespace MSEACalculator
{
    public class CommonFunc
    {
        public static Dictionary<string, string> EquipSlot { get; set; } = DBRetrieve.GetEquipSlotDB();


        public static int SpellTraceTier(EquipModel selectedEquip)
        {
            int tier = 0;
            if(selectedEquip.EquipLevel < 75)
            {
                tier = 1;
            }
            else if(selectedEquip.EquipLevel >= 75 && selectedEquip.EquipLevel < 115)
            {
                tier = 2;
            }
            else if(selectedEquip.EquipLevel > 114)
            {
                tier = 3;
            }

            return tier;
        }

        public static string returnUnionRank(string charName,int lvl)
        {
            string rank = "";

            if(charName == "Zero")
            {
                if(lvl >= 130 && lvl < 160)
                {
                    rank = "B";
                }
                else if(lvl >=160 && lvl < 180)
                {
                    rank = "A";
                }
                else if(lvl >=180 && lvl < 200)
                {
                    rank = "S";
                }
                else if(lvl >=200 && lvl < 250)
                {
                    rank = "SS";
                }
                else if(lvl >= 250)
                {
                    rank = "SSS";
                }
            }
            else
            {
                if (lvl >= 60 && lvl < 100)
                {
                    rank = "B";
                }
                else if (lvl >= 100 && lvl < 140)
                {
                    rank = "A";
                }
                else if (lvl >= 140 && lvl < 200)
                {
                    rank = "S";
                }
                else if (lvl >= 200 && lvl < 250)
                {
                    rank = "SS";
                }
                else if (lvl >= 250)
                {
                    rank = "SSS";
                }

            }


            return rank;
        }

        public static async void errorDia(string message)
        {
            var errorDia = new MessageDialog(message);

            await errorDia.ShowAsync();
           
        }

        public static EquipModel updateBaseStats(Character character, EquipModel baseEquip)
        {
            //To update equip with proper values as per class
            //EquipModel equipModel = new EquipModel();

            //Update from Main/Sec Stats to Stat Values STR|DEX|...
            //Keep MS/SS property
            int mainStat = baseEquip.BaseStats.MS, secStat = baseEquip.BaseStats.SS, AS = baseEquip.BaseStats.AllStat;
            if(AS > 0)
            {
                baseEquip.BaseStats.STR = AS;
                baseEquip.BaseStats.DEX = AS;
                baseEquip.BaseStats.INT = AS;
                baseEquip.BaseStats.LUK = AS;
                baseEquip.BaseStats.AllStat = 0;
            }
            else
            {
                switch (character.MainStat)
                {
                    case "STR":
                        baseEquip.BaseStats.STR = mainStat;
                        baseEquip.BaseStats.DEX = secStat;
                        break;
                    case "DEX":
                        baseEquip.BaseStats.DEX = mainStat;
                        baseEquip.BaseStats.STR = secStat;
                        break;
                    case "INT":
                        baseEquip.BaseStats.INT = mainStat;
                        baseEquip.BaseStats.LUK = secStat;
                        break;
                    case "LUK":
                        baseEquip.BaseStats.LUK = mainStat;
                        //CADENA    DUAL BLADE
                        if (character.SecStat == "SPECIAL")
                        {
                            baseEquip.BaseStats.STR = secStat;
                        }
                        baseEquip.BaseStats.DEX = secStat;
                        break;
                    case "HP":
                        break;
                    //XENON
                    case "SPECIAL":
                        baseEquip.BaseStats.STR = mainStat;
                        baseEquip.BaseStats.DEX = mainStat;
                        baseEquip.BaseStats.LUK = mainStat;
                        break;

                }
            }

            return baseEquip;
        }

        public static EquipStatsModel recordToProperty(EquipStatsModel RM, Dictionary<string, int> record)
        {
            foreach(var R in record.Keys)
            {
                switch (R)
                {
                    case "STR":
                        RM.STR = record[R];
                        break;
                    case "DEX":
                        RM.DEX = record[R];
                        break;
                    case "INT":
                        RM.INT = record[R];
                        break;
                    case "LUK":
                        RM.LUK = record[R];
                        break;
                    case "ATK":
                        RM.ATK = record[R];
                        break;
                    case "MATK":
                        RM.MATK = record[R];
                        break;
                    case "HP":
                        RM.HP = record[R];
                        break;
                    case "MP":
                        RM.MP = record[R];
                        break;
                    case "DEF":
                        RM.DEF = record[R];
                        break;
                    case "SPD":
                        RM.SPD = record[R];
                        break;
                    case "JUMP":
                        RM.JUMP = record[R];
                        break;

                    case "AllStat":
                        RM.AllStat = record[R];
                        break;
                    case "SpecialHP":
                        RM.SpecialHP = record[R].ToString();
                        break;
                    case "SpecialMP":
                        RM.SpecialMP = record[R].ToString();
                        break;
                    case "BD":
                        RM.BD = record[R];
                        break;
                    case "DMG":
                        RM.DMG = record[R];
                        break;

                    default:
                        break;
                }
            }


            return RM;
        }
        
        public static Dictionary<string,int> propertyToRecord(EquipStatsModel RM, Dictionary<string, int> record)
        {
            record.Clear();
            record["STR"] = RM.STR;
            record["DEX"] = RM.DEX;
            record["INT"] = RM.INT;
            record["LUK"] = RM.LUK;
            record["ATK"] = RM.ATK;
            record["MATK"] = RM.MATK;
            record["DEF"] = RM.DEF;
            record["HP"] = RM.HP;
            record["MP"] = RM.MP;
            record["SPD"] = RM.SPD;
            record["JUMP"] = RM.JUMP;
            record["AllSTAT"] = RM.AllStat;

            return record;
        }


        public static EquipModel FindEquip(List<EquipModel> FindingList, Character SCharacter, string Slot, string ESet)
        {
            List<string> accList =  new List<string>() { "Ring", "Pendant", "Emblem", "Accessory", "Misc"};

            EquipModel returnedEquip = new EquipModel();

            
            if (accList.Contains(Slot))
            {
                foreach(EquipModel equip in FindingList)
                {
                    if (equip.EquipSlot == Slot && equip.EquipSet == ESet)
                    {
                        if (Slot == "Shoulder")
                        {
                            if (equip.ClassType == SCharacter.ClassType)
                            {
                                return returnedEquip =  equip;
                            }
                        }
                        return returnedEquip =  equip;
                    }
                }
            }
            else
            {
                switch (EquipSlot[Slot])
                {
                    case "Weapon":
                        foreach (EquipModel equip in FindingList)
                        {
                            if (equip.WeaponType == SCharacter?.CurrentMainWeapon && equip.EquipSet == ESet)
                            {
                                return returnedEquip =  equip;
                            }
                        }
                        break;
                    case "Secondary":
                        foreach (EquipModel equip in FindingList)
                        {
                            if (equip.EquipName == ESet)
                            {
                                if (equip.ClassType == SCharacter.Faction || equip.ClassType == SCharacter.ClassType)
                                {
                                    return returnedEquip =  equip;
                                }
                            }
                        }
                        break;
                    case "Heart":
                        break;
                    case "Armor":
                        foreach(EquipModel equip in FindingList)
                        {
                            if (equip.EquipSet ==  ESet && equip.EquipSlot == Slot && equip.ClassType == SCharacter.ClassType)
                            {
                                return returnedEquip =  equip;
                            }
                        }
                        break;
                }
            }

            return returnedEquip;
        }

        

        public static Dictionary<string, Dictionary<int, Dictionary<int, ScrollingModel>>> SpellTraceDict { get; set; } = new Dictionary<string, Dictionary<int, Dictionary<int, ScrollingModel>>>
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

        public string returnEquipType(string ESlot)
        {
            if (EquipSlot[ESlot] == "Ring" || EquipSlot[ESlot] == "Pendant")
            {
                return EquipSlot[ESlot];
            }
            return ESlot;
        }

        //public static async Task<List<EventRecords>> retrieveEventJson()
        //{
        //    List<EventRecords> eventList;

        //    string jsonFP = Path.Combine(ApplicationData.Current.LocalFolder.Path, @"DefaultData\Event.json");

        //    if (!File.Exists(jsonFP))
        //    {
        //        //if file dont exist create a blank file
        //        EventRecords tempR = new EventRecords();
        //        tempR.startDate = DateTime.Today;
        //        tempR.endDate = DateTime.Today;
        //        tempR.eventDuration = 0;
        //        tempR.availableDays = new Dictionary<string, bool>
        //        {
        //            {"Monday",false},
        //            {"Tuesday",false},
        //            {"Wednesday",false},
        //            {"Thursday",false},
        //            {"Friday",false},
        //            {"Saturday",false},
        //            {"Sunday",false}
        //        };


        //        List<EventRecords> temprecordList = new List<EventRecords>() { tempR}; 

        //        await DatabaseAccess.writetoEventJson("init", temprecordList);
        //        //Thread.Sleep(3000);
        //        using (FileStream fileStream = File.OpenRead(jsonFP))
        //        {
        //            eventList = await JsonSerializer.DeserializeAsync<List<EventRecords>>(fileStream);
        //        }

        //    }    

        //    using (FileStream fileStream = File.OpenRead(jsonFP))
        //    {
        //        eventList = await JsonSerializer.DeserializeAsync<List<EventRecords>>(fileStream);
        //    }



        //    return eventList;
        //}


    }
}
