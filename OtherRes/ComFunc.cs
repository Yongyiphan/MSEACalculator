using Microsoft.Data.Sqlite;
using MSEACalculator.CharacterRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.OtherRes.Database;
using MSEACalculator.OtherRes.Database.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;

namespace MSEACalculator
{
    public class ComFunc
    {
        public static Dictionary<string, string> EquipSlot  = EquipSlotTable.GetEquipSlotDB();
        public static List<string> PotentialGrade = GVar.PotentialGrade;

        public static bool IsOpenConnection(SqliteConnection connection)
        {
            return connection.State == ConnectionState.Open ? true : false;
        }

        public static async Task<List<string>> CSVStringAsync(string Location, string FileName)
        {
            StorageFile charTable = await GVar.storageFolder.GetFileAsync(Location + FileName);
            List<string> CSVAsStringList = new List<string>();
            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;
            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    CSVAsStringList = text.Split("\r\n").ToList();
                    
                }
            }

            return CSVAsStringList;
        }


        public static string TableSpecStringBuilder(Dictionary<string, Dictionary<string, string>> Replacement, string ColName, string constraints)
        {
            try
            {
                StringBuilder sb = new StringBuilder("(");

                List<string> IgnoreStings = new List<string>() { "Functions", "Knockback", "1 in" };
                List<string> SplitComma = ColName.Split(",").ToList();

                List<string> TPara = new List<string>();
                List<string> TConstraints = new List<string>();

                foreach (string name in SplitComma)
                {
                    if (String.IsNullOrWhiteSpace(name) || IgnoreStings.Any(s => name.Contains(s)))
                    {
                        continue;
                    }
                    //sb.Append(GVar.NameReplacementType[name]["Rename"] + " " + GVar.NameReplacementType[name]["Type"] + ",");
                    TPara.Add(Replacement[name]["Rename"] + " " + Replacement[name]["Type"]);
                    if (constraints == "All")
                    {
                        //cb.Append(GVar.NameReplacementType[name]["Rename"]);
                        //if (name != SplitComma.Last())
                        //{
                        //    cb.Append(",");
                        //}
                        TConstraints.Add(Replacement[name]["Rename"]);
                    }
                }

                sb.Append(String.Join(",", TPara));
                sb.Append(",");

                //Table Constraint Addon
                if (constraints == "All")
                {
                    sb.Append("PRIMARY KEY (");
                    sb.Append(String.Join(",", TConstraints));
                    sb.Append(")");
                }
                else
                {
                    sb.Append(constraints);
                }
                sb.Append(");");

                string Result = sb.ToString();

                return Result;

            }
            catch (Exception E)
            {
                Console.WriteLine(E);
                return "";
            }
        }
        public static string InsertSQLStringBuilder(string TableName, string TablePara)
        {
            List<string> ToRemove = new List<string>() { "int", "string", "(", ");", "NOT NULL","double", "nvarchar" };
            //StringBuilder SB = new StringBuilder(TablePara);
            StringBuilder SB = new StringBuilder();
            List<string> commaSplit = TablePara.Split(",").ToList();
            List<string> TypeRemoved = new List<string>();
            foreach(string split in commaSplit)
            {
                if (split.Contains("PRIMARY") || split.Contains("FOREIGN"))
                {
                    break;
                }

                string[] NameType = split.Trim().Split(" ");
                TypeRemoved.Add(NameType[0].Replace("(", "")); 
            }
            
            StringBuilder Result = new StringBuilder("INSERT INTO " + TableName + " ( ");
            Result.Append(string.Join(",", TypeRemoved));
            Result.Append(") VALUES ( ");

            foreach (string deli in TypeRemoved)
            {
                Result.Append(String.Format("@{0},", deli));

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

        public static string ReturnUnionRank(string charName,int lvl)
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
        public static int ReturnSFLevelRank(int lvl, string type = "Basic")
        {
            Func<int, int, int, bool> IB =  (level, start, end) => {

                
                return level >= start && level <= end ? true : false;
            };
            int result = 0;
            if(type == "Superior")
            {
                if (IB(lvl, 0, 77))
                {
                    result = 9;
                }
                else if (IB(lvl, 78, 87))
                {
                    result = 8;
                }
                else if (IB(lvl, 88, 97))
                {
                    result = 7;
                }
                else if (IB(lvl, 98, 107))
                {
                    result = 6;
                }
                else if (IB(lvl, 108, 117))
                {
                    result = 5;
                }
                else if (IB(lvl, 118, 127))
                {
                    result = 4;
                }
                else if (IB(lvl, 128, 137))
                {
                    result = 3;
                }
                else if (IB(lvl, 138, 149))
                {
                    result = 2;
                }
                else if (lvl >= 150)
                {
                    result = 1;
                }
            }
            else
            {
                if (IB(lvl, 128, 137))
                {
                    result = 5;
                }
                else if (IB(lvl, 138, 149))
                {
                    result = 4;
                }
                else if (IB(lvl, 150, 159))
                {
                    result = 3;
                }
                else if (IB(lvl, 160, 199))
                {
                    result = 2;
                }
                else if (lvl >= 200)
                {
                    result = 1;
                }
            }

            return result;
        }


        public static async void ErrorDia(string message)
        {
            var errorDia = new MessageDialog(message);

            await errorDia.ShowAsync();
           
        }

        
        public static List<string> FilterBy(string slotCat, CharacterCLS Character, string eSlot, ReadOnlyCollection<EquipCLS> SearchList, string XenonClass = "")
        {
            List<string> result = new List<string>();
            string format = slotCat == "Accessory" || slotCat == "Secondary" ? "Name" : "Set";

            string ClassType = Character.ClassName == "Xenon" ? XenonClass : Character.ClassType;

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
                            if (item.EquipSlot == eSlot)
                            {
                                if (item.ClassType == ClassType ||item.ClassType == "Any")
                                {
                                    if (!result.Contains(item.EquipSet))
                                    {
                                        result.Add(item.EquipSet);
                                    }
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
                                    if (item.WeaponType == weap && item.ClassType == ClassType)
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
                            if (item.EquipSlot == eSlot && (item.ClassType == ClassType ||item.ClassType == "Any"))
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

        public static EquipCLS FindEquip(ReadOnlyCollection<EquipCLS> FindingList, CharacterCLS SCharacter, string Slot, string ESet, string XenonClass = "")
        {
           

            EquipCLS returnedEquip = new EquipCLS();
            string ClassType = SCharacter.ClassName == "Xenon" ? XenonClass : SCharacter.ClassType;
            
            if (GVar.AccEquips.Contains(Slot))
            {
                foreach(EquipCLS equip in FindingList)
                {
                    if (equip.EquipSlot == Slot && (equip.EquipSet == ESet || equip.EquipName == ESet))
                    {
                        if (Slot == "Shoulder")
                        {
                            if (equip.ClassType == ClassType || equip.ClassType == "Any")
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
                    if (equip.EquipSet ==  ESet && equip.EquipSlot == Slot && equip.ClassType == ClassType)
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
                                if (equip.ClassType == SCharacter.ClassName || equip.ClassType == SCharacter.Faction || equip.ClassType == ClassType)
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
                                    returnedEquip.ClassType = ClassType;

                                    return returnedEquip;
                                }
                            }
                        }
                        break;
                }
            }

            return returnedEquip;
        }

        //public static EquipCLS UpdateBaseStats(CharacterCLS character, EquipCLS baseEquip, string XenonClass = "")
        //{
        //    //To update equip with proper values as per class
        //    //EquipModel equipModel = new EquipModel();

        //    //Update from Main/Sec Stats to Stat Values STR|DEX|...
        //    //Keep MS/SS property
        //    int mainStat = baseEquip.BaseStats.MS, secStat = baseEquip.BaseStats.SS, AS = baseEquip.BaseStats.AllStat;
        //    if (AS > 0)
        //    {
        //        baseEquip.BaseStats.STR = AS;
        //        baseEquip.BaseStats.DEX = AS;
        //        baseEquip.BaseStats.INT = AS;
        //        baseEquip.BaseStats.LUK = AS;
        //        baseEquip.BaseStats.AllStat = 0;
        //    }
        //    else
        //    {
        //        if (character.ClassName == "Xenon")
        //        {
        //            switch (XenonClass)
        //            {
        //                case "Pirate":
        //                    baseEquip.BaseStats.STR = mainStat;
        //                    baseEquip.BaseStats.DEX = mainStat;
        //                    break;
        //                case "Thief":
        //                    baseEquip.BaseStats.DEX = mainStat;
        //                    baseEquip.BaseStats.LUK = mainStat;
        //                    break;
        //            }
        //            return baseEquip;
        //        }

        //        switch (character.MainStat)
        //        {
        //            case "STR":
        //                baseEquip.BaseStats.STR = mainStat;
        //                baseEquip.BaseStats.DEX = secStat;
        //                break;
        //            case "DEX":
        //                baseEquip.BaseStats.DEX = mainStat;
        //                baseEquip.BaseStats.STR = secStat;
        //                break;
        //            case "INT":
        //                baseEquip.BaseStats.INT = mainStat;
        //                baseEquip.BaseStats.LUK = secStat;
        //                break;
        //            case "LUK":
        //                baseEquip.BaseStats.LUK = mainStat;
        //                baseEquip.BaseStats.DEX = secStat;
        //                ////CADENA    DUAL BLADE
        //                //if (character.SecStat == "SPECIAL")
        //                //{
        //                //    baseEquip.BaseStats.STR = secStat;
        //                //}
        //                break;
        //            case "HP":
        //                break;

        //        }
        //    }



        //    return baseEquip;
        //}




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

        public static Dictionary<string, string> ReturnMainSecStat(string ClassType)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            switch (ClassType)
            {
                case "Warrior":
                    result.Add("Main", "STR");
                    result.Add("Main", "STR");
                    break;
                case "Bowman":
                    result.Add("Main", "STR");
                    result.Add("Main", "STR");
                    break;
                case "Thief":
                    result.Add("Main", "STR");
                    result.Add("Main", "STR");
                    break;
                case "Magician":
                    result.Add("Main", "STR");
                    result.Add("Main", "STR");
                    break;
                case "Pirate":
                    result.Add("Main", "STR");
                    result.Add("Main", "STR");
                    break;

            }

            return result;
        }
       
       
        
        public static List<(string, string)> ReturnPotGradeKey(int Grade)
        {
            List<(string, string)> result = new List<(string, string)>();
            switch (Grade)
            {
                case 0:
                    result.Add(("Rare", "Non prime"));
                    result.Add(("Rare", "Prime"));
                    break;

                case 1:
                    result.Add(("Rare", "Prime"));
                    result.Add(("Epic", "Prime"));
                    break;

                case 2:
                    result.Add(("Epic", "Prime"));
                    result.Add(("Unique", "Prime"));
                    break;

                case 3:
                    result.Add(("Unique", "Prime"));
                    result.Add(("Legendary", "Prime"));
                    break;

            }


            return new List<(string, string)>();
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
