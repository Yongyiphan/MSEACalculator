﻿using System;
using System.Data;
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
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;

namespace MSEACalculator
{
    public class ComFunc
    {
        public static Dictionary<string, string> EquipSlot  = DBRetrieve.GetEquipSlotDB();
        public static List<string> PotentialGrade = GVar.PotentialGrade;

        public static bool IsOpenConnection(SqliteConnection connection)
        {
            return connection.State == ConnectionState.Open ? true : false;
        }
        public static string InsertSQLStringBuilder(string TableName, string TablePara)
        {
            List<string> ToRemove = new List<string>() { "int", "string", "(", ");", "NOT NULL","double", "nvarchar" };
            StringBuilder SB = new StringBuilder(TablePara);
            StringBuilder Result = new StringBuilder("INSERT INTO " + TableName + " ( ");
            foreach(string Type in ToRemove)
            {
                SB.Replace(Type, "");
            }
            string[] deliSplit = SB.ToString().Split(",");
            foreach(string deli in deliSplit)
            {
                if (deli.Contains("PRIMARY") || deli.Contains("FOREIGN"))
                {
                    break;
                }
                Result.Append(String.Format("{0},", deli.Trim()));

            }
            Result = new StringBuilder(Result.ToString().TrimEnd(','));
            Result.Append(") VALUES ( ");

            foreach (string deli in deliSplit)
            {
                if (deli.Contains("PRIMARY") || deli.Contains("FOREIGN"))
                {
                    break;
                }
                Result.Append(String.Format("@{0},", deli.Trim()));

            }
            Result = new StringBuilder(Result.ToString().TrimEnd(','));
            Result.Append(");");
            return Result.ToString();

        }

        public static int SpellTraceTier(EquipCLS selectedEquip)
        {
            int tier = 0;
            if(selectedEquip.EquipLevel < 75)
            {
                return tier = 1;
            }
            else if(selectedEquip.EquipLevel >= 75 && selectedEquip.EquipLevel < 115)
            {
                return tier = 2;
            }
            else if(selectedEquip.EquipLevel > 114)
            {
                if (selectedEquip.EquipSlot == "Gloves")
                {
                    return tier = 2;
                }
                return tier = 3;
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

        public static async void ErrorDia(string message)
        {
            var errorDia = new MessageDialog(message);

            await errorDia.ShowAsync();
           
        }

        public static EquipCLS UpdateBaseStats(CharacterCLS character, EquipCLS baseEquip)
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

        public static EquipStatsCLS RecordToProperty(EquipStatsCLS RM, Dictionary<string, int> record)
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
        
        public static Dictionary<string,int> PropertyToRecord(EquipStatsCLS RM, Dictionary<string, int> record)
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
            record["BMDG"] = RM.BD;
            record["IED"] = RM.IED;
            record["DMG"] = RM.DMG;

            return record;
        }


        public static EquipCLS FindEquip(ReadOnlyCollection<EquipCLS> FindingList, CharacterCLS SCharacter, string Slot, string ESet)
        {
            

            EquipCLS returnedEquip = new EquipCLS();

            
            if (GVar.AccEquips.Contains(Slot))
            {
                foreach(EquipCLS equip in FindingList)
                {
                    if (equip.EquipSlot == Slot && (equip.EquipSet == ESet || equip.EquipName == ESet))
                    {
                        if (Slot == "Shoulder")
                        {
                            if (equip.ClassType == SCharacter.ClassType || equip.ClassType == "Any")
                            {
                                return returnedEquip =  equip;
                            }
                        }
                        
                        return returnedEquip =  equip;
                    }
                }
            }
            else if (GVar.ArmorEquips.Contains(Slot))
            {
                foreach (EquipCLS equip in FindingList)
                {
                    if (equip.EquipSet ==  ESet && equip.EquipSlot == Slot && equip.ClassType == SCharacter.ClassType)
                    {
                        return returnedEquip =  equip;
                    }
                }
            }
            else
            {
                switch (EquipSlot[Slot])
                {
                    case "Weapon":
                        foreach (EquipCLS equip in FindingList)
                        {
                            if (equip.WeaponType == SCharacter?.CurrentMainWeapon && equip.EquipSet == ESet)
                            {
                                returnedEquip =  equip;
                                returnedEquip.EquipSlot = "Weapon";
                                return returnedEquip;
                            }
                        }
                        break;
                    case "Secondary":
                        foreach (EquipCLS equip in FindingList)
                        {
                            if (equip.EquipName == ESet)
                            {
                                if (equip.ClassType == SCharacter.ClassName || equip.ClassType == SCharacter.ClassType)
                                {
                                    returnedEquip =  equip;
                                    if (equip.WeaponType.Contains("Demon Aegis")){
                                        returnedEquip.EquipSlot = "Demon Aegis";
                                    }
                                    else if (equip.WeaponType.Contains("Soul Ring"))
                                    {
                                        returnedEquip.EquipSlot = "Soul Rings";
                                    }
                                    else
                                    {
                                        returnedEquip.EquipSlot = "Secondary";
                                    }

                                    return returnedEquip;
                                }
                            }
                        }
                        break;
                }
            }

            return null;
        }



        public static List<string> FilterBy(string slotCat, CharacterCLS Character, string eSlot, ReadOnlyCollection<EquipCLS> SearchList)
        {
            List<string> result = new List<string>();
            string format = slotCat == "Accessory" || slotCat == "Secondary" ? "Name" : "Set";
            
            switch (format)
            {
                case "Set":
                    foreach (var item in SearchList)
                    {
                        if (eSlot == "Weapon")
                        {
                            foreach (string weap in Character.MainWeapon)
                            {
                                if (item.WeaponType == weap)
                                {
                                    if (!result.Contains(item.EquipSet))
                                    {
                                        result.Add(item.EquipSet);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (item.EquipSlot == eSlot && (item.ClassType == Character.ClassType ||item.ClassType == "Any"))
                            {
                                if (!result.Contains(item.EquipSet))
                                {
                                    result.Add(item.EquipSet);
                                }
                            }
                        }
                    }
                    break;
                case "Name":
                    foreach (var item in SearchList)
                    {
                        if (eSlot == "Secondary")
                        {
                            foreach (string weap in Character.SecondaryWeapon)
                            {
                                if (weap == "Shield")
                                {
                                    if (item.WeaponType == weap && item.ClassType == Character.ClassType)
                                    {
                                        if (!result.Contains(item.EquipName))
                                        {
                                            result.Add(item.EquipName);
                                        }
                                    }
                                }
                                else
                                {
                                    if (item.WeaponType == weap)
                                    {
                                        if (!result.Contains(item.EquipName))
                                        {
                                            result.Add(item.EquipName);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (item.EquipSlot == eSlot && (item.ClassType == Character.ClassType ||item.ClassType == "Any"))
                            {
                                if (!result.Contains(item.EquipName))
                                {
                                    result.Add(item.EquipName);
                                }
                            }
                        }
                    }
                    break;
            }


            return result;
        }
        public static Dictionary<string, Dictionary<int, Dictionary<int, ScrollingModelCLS>>> SpellTraceDict { get; set; } = new Dictionary<string, Dictionary<int, Dictionary<int, ScrollingModelCLS>>>
        {
            ["Armor"] = new Dictionary<int, Dictionary<int, ScrollingModelCLS>>
            {
                [1] = new Dictionary<int, ScrollingModelCLS>
                {
                    [100] = new ScrollingModelCLS{
                        MainStat = 1,
                        HP = 5,
                        DEF =1
                    },
                    [70] = new ScrollingModelCLS{
                        MainStat= 2,
                        HP = 15,
                        DEF =2
                    },
                    [30] = new ScrollingModelCLS{MainStat= 3, HP = 30, DEF =4 }
                },
                [2] = new Dictionary<int, ScrollingModelCLS>
                {
                    [100] = new ScrollingModelCLS { MainStat = 2, HP = 20, DEF = 4 },
                    [70] = new ScrollingModelCLS { MainStat = 3, HP = 40, DEF = 4},
                    [30] = new ScrollingModelCLS { MainStat = 5, HP = 70,DEF = 7}
                },
                [3] = new Dictionary<int, ScrollingModelCLS>
                {
                    [100] = new ScrollingModelCLS { MainStat = 3, HP = 30, DEF = 3 },
                    [70] = new ScrollingModelCLS { MainStat = 4, HP = 70, DEF = 5 },
                    [30] = new ScrollingModelCLS { MainStat = 7, HP = 120, DEF = 10 }
                }
            },
            ["Gloves"] = new Dictionary<int, Dictionary<int, ScrollingModelCLS>>
            {
                [1] = new Dictionary<int, ScrollingModelCLS>
                {
                    [100] = new ScrollingModelCLS { DEF = 3},
                    [70] = new ScrollingModelCLS { ATK = 1},
                    [30] = new ScrollingModelCLS { ATK = 2}
                },
                [2] = new Dictionary<int, ScrollingModelCLS>
                {
                    [100] = new ScrollingModelCLS { ATK = 1},
                    [70] = new ScrollingModelCLS { ATK = 2},
                    [30] = new ScrollingModelCLS { ATK = 3}
                }
            },
            ["Accessory"] = new Dictionary<int, Dictionary<int, ScrollingModelCLS>>
            {
                [1] = new Dictionary<int, ScrollingModelCLS>
                {
                    [100] = new ScrollingModelCLS { MainStat = 1},
                    [70] = new ScrollingModelCLS { MainStat = 2},
                    [30] = new ScrollingModelCLS { MainStat = 3}
                },
                [2] = new Dictionary<int, ScrollingModelCLS>
                {
                    [100] = new ScrollingModelCLS { MainStat = 1},
                    [70] = new ScrollingModelCLS { MainStat = 2},
                    [30] = new ScrollingModelCLS { MainStat = 4}
                },
                [3] = new Dictionary<int, ScrollingModelCLS>
                {
                    [100] = new ScrollingModelCLS { MainStat = 2},
                    [70] = new ScrollingModelCLS { MainStat = 3},
                    [30] = new ScrollingModelCLS { MainStat =5}
                }
            },
            ["Heart"] = new Dictionary<int, Dictionary<int, ScrollingModelCLS>>
            {
                [1] = new Dictionary<int, ScrollingModelCLS>
                {
                    [100] = new ScrollingModelCLS { ATK = 1},
                    [70] = new ScrollingModelCLS { ATK = 2},
                    [30] = new ScrollingModelCLS { ATK = 3}
                },
                [2] = new Dictionary<int, ScrollingModelCLS>
                {
                    [100] = new ScrollingModelCLS { ATK = 2},
                    [70] = new ScrollingModelCLS { ATK = 3},
                    [30] = new ScrollingModelCLS { ATK = 5}
                }
        ,
                [3] = new Dictionary<int, ScrollingModelCLS>
                {
                    [100] = new ScrollingModelCLS { ATK = 3},
                    [70] = new ScrollingModelCLS { ATK = 4},
                    [30] = new ScrollingModelCLS { ATK = 7}
                }
            },
            ["Weapon"] = new Dictionary<int, Dictionary<int, ScrollingModelCLS>>
            {
                [1] = new Dictionary<int, ScrollingModelCLS>
                {
                    [100] = new ScrollingModelCLS { ATK = 1},
                    [70] = new ScrollingModelCLS { ATK = 2},
                    [30] = new ScrollingModelCLS { MainStat = 1, ATK = 3},
                    [15] = new ScrollingModelCLS { MainStat = 2, ATK = 5}
                },
                [2] = new Dictionary<int, ScrollingModelCLS>
                {
                    [100] = new ScrollingModelCLS { ATK = 2},
                    [70] = new ScrollingModelCLS { MainStat = 1, ATK = 3},
                    [30] = new ScrollingModelCLS { MainStat = 2, ATK = 5},
                    [15] = new ScrollingModelCLS { MainStat = 3, ATK = 7}
                }
        ,
                [3] = new Dictionary<int, ScrollingModelCLS>
                {
                    [100] = new ScrollingModelCLS { MainStat = 1, ATK = 3},
                    [70] = new ScrollingModelCLS { MainStat = 2, ATK = 5},
                    [30] = new ScrollingModelCLS { MainStat = 3, ATK = 7},
                    [15] = new ScrollingModelCLS { MainStat = 4, ATK = 9}
                }
            }

        };

        public static string ReturnRingPend(string ESlot)
        {
            try
            {
                if (EquipSlot[ESlot] == "Ring" || EquipSlot[ESlot] == "Pendant")
                {
                    return EquipSlot[ESlot];
                }
            }
            catch(KeyNotFoundException)
            {
                return ESlot;
            }
            
            return ESlot;
        }


        //RETURNS TABLE TO FIND EQUIPS FROM
        public static string ReturnSetCat(string selectedESlot)
        {
            string eslot = ReturnRingPend(selectedESlot);

            if (GVar.AccEquips.Contains(eslot))
            {
                return "Accessory";
            }
            else if (GVar.ArmorEquips.Contains(eslot))
            {
                return "Armor";
            }
            else
            {
                return eslot;
            }
        }


        //RETURN SCROLL TABLE TO FOLLOW
        public static string ReturnScrollCat(string selectedESlot)
        {
            string eslot = ReturnRingPend(selectedESlot);
            if (GVar.AccEquips.Contains(eslot))
            {
                if (eslot ==  "Shoulder")
                {
                    return "Armor";
                }
                else if (eslot == "Heart")
                {
                    return "Heart";
                }
                return "Accessory";
            }
            else if (GVar.ArmorEquips.Contains(eslot))
            {
                if(eslot == "Gloves")
                {
                    return "Gloves";
                }

                return "Armor";
            }
            else
            {
                return eslot;
            }

        }





        //public static bool notNULL(object obj)
        //{
        //    if(obj != null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsInt(string number)
        //{
        //    if(int.TryParse(number, out int result) == false && number != String.Empty)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
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
