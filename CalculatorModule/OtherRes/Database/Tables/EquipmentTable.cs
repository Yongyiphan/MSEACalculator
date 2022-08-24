using Microsoft.Data.Sqlite;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class EquipmentTable : BaseDBTable, ITableUpload
    {

        public List<EquipCLS> EquipList { get; set; }

        public EquipmentTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {

            //List<string> EquipTableNames = new List<string> { "EquipArmorData", "EquipAccessoriesData", "EquipWeaponData", "EquipSecondaryData" };
            //switch (TableName)
            //{
            //    case "EquipArmorData":
            //        TablePara = ArmorTableSpec[0];
            //        break;
            //    case "EquipAccessoriesData":
            //        TablePara = AccessoriesTableSpec[0];
            //        break;
            //    case "EquipAndroidData":
            //        TablePara = AndroidSpec[0];
            //        break;
            //    case "EquipMedalData":
            //        TablePara = MedalSpec[0];
            //        break;

            //    case "EquipWeaponData":
            //        TablePara = MainWeapTableSpec[0];
            //        break;
            //    case "EquipSecondaryData":
            //        TablePara = SecWeapTableSpec[0];
            //        break;
            //}

        }

        public async void RetrieveData()
        {
            (List<EquipCLS>, string) Result;
            try
            {
                switch (TableName)
                {
                    case "EquipArmorData":
                        Result = await GetArmorCSVAsync(FileName: "ArmorData.csv", TableKey: "PRIMARY KEY (EquipSet, ClassType, EquipName, EquipSlot)");
                        EquipList = Result.Item1;
                        TableParameters = Result.Item2;
                        break;
                    case "EquipAccessoriesData":
                        Result = await GetAccessoriesCSVAsync(FileName:"AccessoryData.csv", TableKey: "PRIMARY KEY (EquipName, EquipSet, ClassName, EquipSlot)");
                        EquipList = Result.Item1;
                        TableParameters = Result.Item2;
                        break;

                    case "EquipAndroidData":
                        Result = await GetAndroidCSVAsync(FileName: "AndroidData.csv", TableKey: "PRIMARY KEY (EquipSlot, EquipName)");
                        EquipList = Result.Item1;
                        TableParameters = Result.Item2;
                        break;
                    case "EquipMedalData":
                        Result = await GetMedalCSVAsync(FileName: "MedalData.csv", TableKey: "PRIMARY KEY (EquipSlot, ClassName, EquipName, EquipSet)");
                        EquipList = Result.Item1;
                        TableParameters = Result.Item2;
                        break;


                    case "EquipWeaponData":
                        Result = await GetWeaponCSVAsync(FileName: "WeaponData.csv", TableKey: "All");
                        EquipList = Result.Item1;
                        TableParameters = Result.Item2;
                        break;
                    case "EquipSecondaryData":
                        Result = await GetSecondaryCSVAsync(FileName: "SecondaryData.csv",TableKey: "PRIMARY KEY (ClassName, EquipName, WeaponType, EquipLevel)");
                        EquipList = Result.Item1;
                        TableParameters = Result.Item2;
                        break;
                }

            }
            catch (Exception E)
            {
                Console.WriteLine(E);
            }

        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if (ComFunc.IsOpenConnection(connection))
            {

                string insertEquip = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                using (SqliteCommand insertCMD = new SqliteCommand(insertEquip, connection, transaction))
                {
                    foreach (EquipCLS equipItem in EquipList)
                    {
                        ErrorCounter++;
                        insertCMD.Parameters.Clear();
                        insertCMD.Parameters.AddWithValue("@EquipSlot", equipItem.EquipSlot);
                        insertCMD.Parameters.AddWithValue("@ClassType", equipItem.ClassType);
                        insertCMD.Parameters.AddWithValue("@ClassName", equipItem.ClassType);
                        insertCMD.Parameters.AddWithValue("@EquipName", equipItem.EquipName);
                        insertCMD.Parameters.AddWithValue("@EquipSet", equipItem.EquipSet);
                        insertCMD.Parameters.AddWithValue("@EquipLevel", equipItem.EquipLevel);
                        insertCMD.Parameters.AddWithValue("@Category", equipItem.Category);


                        insertCMD.Parameters.AddWithValue("@WeaponType", equipItem.WeaponType);
                        insertCMD.Parameters.AddWithValue("@AtkSpd", equipItem.BaseStats.ATKSPD);

                        insertCMD.Parameters.AddWithValue("@STR", equipItem.BaseStats.STR);
                        insertCMD.Parameters.AddWithValue("@DEX", equipItem.BaseStats.DEX);
                        insertCMD.Parameters.AddWithValue("@INT", equipItem.BaseStats.INT);
                        insertCMD.Parameters.AddWithValue("@LUK", equipItem.BaseStats.LUK);

                        insertCMD.Parameters.AddWithValue("@MaxHP", equipItem.BaseStats.MaxHP);
                        insertCMD.Parameters.AddWithValue("@MaxMP", equipItem.BaseStats.MaxMP);
                        insertCMD.Parameters.AddWithValue("@PercMaxHP", equipItem.BaseStats.HP);
                        insertCMD.Parameters.AddWithValue("@PercMaxMP", equipItem.BaseStats.MP);
                        insertCMD.Parameters.AddWithValue("@MaxDF", equipItem.BaseStats.MaxDF);
                        insertCMD.Parameters.AddWithValue("@DEF", equipItem.BaseStats.DEF);
                        insertCMD.Parameters.AddWithValue("@WATK", equipItem.BaseStats.ATK);
                        insertCMD.Parameters.AddWithValue("@MATK", equipItem.BaseStats.MATK);
                        insertCMD.Parameters.AddWithValue("@AllStats", equipItem.BaseStats.AllStat);
                        insertCMD.Parameters.AddWithValue("@BD", equipItem.BaseStats.BD);
                        insertCMD.Parameters.AddWithValue("@IED", equipItem.BaseStats.IED);
                        insertCMD.Parameters.AddWithValue("@Speed", equipItem.BaseStats.SPD);
                        insertCMD.Parameters.AddWithValue("@Jump", equipItem.BaseStats.JUMP);
                        insertCMD.Parameters.AddWithValue("@Upgrades", equipItem.BaseStats.NoUpgrades);
                        insertCMD.Parameters.AddWithValue("@Rank", equipItem.Rank);

                        try
                        {
                            insertCMD.ExecuteNonQuery();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ErrorCounter.ToString());
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }

            }
        }


        //Method to get Armor Data
        private async Task<(List<EquipCLS>, string)> GetArmorCSVAsync(string FileName, string TableKey)
        {
            List<EquipCLS> equipList = new List<EquipCLS>();

            List<string> result = await ComFunc.CSVStringAsync(GVar.EquipmentPath, FileName);
            string tableSpec = "";
            int counter = 0;
            foreach (string equipItem in result)
            {
                if (equipItem == "")
                {
                    return (equipList, tableSpec);
                }
                if (counter == 0)
                {
                    tableSpec = ComFunc.TableSpecStringBuilder(TableColNames.EquipmentCN, equipItem, TableKey);
                    counter += 1;
                    continue;
                }


                var temp = equipItem.Split(",");
                counter += 1;
                EquipCLS equip = new EquipCLS();

                equip.EquipSlot = temp[1];
                equip.ClassType = temp[2];
                equip.EquipName = temp[3];
                equip.EquipSet  = temp[4];
                equip.EquipLevel= Convert.ToInt32(temp[5]);


                equip.BaseStats.STR = Convert.ToInt32(temp[6]);
                equip.BaseStats.DEX = Convert.ToInt32(temp[7]);
                equip.BaseStats.INT = Convert.ToInt32(temp[8]);
                equip.BaseStats.LUK = Convert.ToInt32(temp[9]);

                equip.BaseStats.MaxHP = Convert.ToInt32(temp[10]);
                equip.BaseStats.MaxMP = Convert.ToInt32(temp[11]);
                equip.BaseStats.DEF = Convert.ToInt32(temp[12]);
                equip.BaseStats.ATK = Convert.ToInt32(temp[13]);
                equip.BaseStats.MATK = Convert.ToInt32(temp[14]);
                equip.BaseStats.IED = Convert.ToInt32(temp[15].Trim('%'));
                equip.BaseStats.SPD = Convert.ToInt32(temp[16]);
                equip.BaseStats.JUMP = Convert.ToInt32(temp[17]);
                equip.BaseStats.NoUpgrades = Convert.ToInt32(temp[18]);

                try
                {
                equipList.Add(equip);

                }
                catch (Exception)
                {

                }

            }
            return (equipList, tableSpec);

        }

        //Method to get Acc Data
        private async Task<(List<EquipCLS>, string)> GetAccessoriesCSVAsync(string FileName, string TableKey)
        {
            List<EquipCLS> AccessoriesList = new List<EquipCLS>();
            List<string> result = await ComFunc.CSVStringAsync(GVar.EquipmentPath, FileName);
            int counter = 0;
            string tableSpec = "";
            try
            {
                foreach (string accItem in result)
                {
                    if (accItem == "")
                    {
                        return (AccessoriesList, tableSpec);
                    }
                    if (counter == 0)
                    {
                        tableSpec = ComFunc.TableSpecStringBuilder(TableColNames.EquipmentCN, accItem, TableKey);
                        counter += 1;
                        continue;
                    }

                    counter += 1;
                    var temp = accItem.Split(",");

                    EquipCLS equip = new EquipCLS();
                    equip.EquipSlot = temp[1];
                    equip.ClassType = temp[2];
                    equip.EquipName = temp[3];
                    equip.EquipSet  = temp[4];
                    equip.Category = temp[5];
                    equip.EquipLevel = Convert.ToInt32(temp[6]);

                    equip.BaseStats.STR = Convert.ToInt32(temp[7]);
                    equip.BaseStats.DEX = Convert.ToInt32(temp[8]);
                    equip.BaseStats.INT = Convert.ToInt32(temp[9]);
                    equip.BaseStats.LUK = Convert.ToInt32(temp[10]);
                    equip.BaseStats.MaxHP = Convert.ToInt32(temp[11]);
                    equip.BaseStats.MaxMP = Convert.ToInt32(temp[12]);
                    equip.BaseStats.HP = temp[13];
                    equip.BaseStats.MP = temp[14];
                    equip.BaseStats.DEF = Convert.ToInt32(temp[15]);
                    equip.BaseStats.ATK = Convert.ToInt32(temp[16]);
                    equip.BaseStats.MATK = Convert.ToInt32(temp[17]);
                    equip.BaseStats.IED = Convert.ToInt32(temp[18].Trim('%'));
                    equip.BaseStats.SPD = Convert.ToInt32(temp[19]);
                    equip.BaseStats.JUMP = Convert.ToInt32(temp[20]);
                    equip.BaseStats.NoUpgrades = Convert.ToInt32(temp[21]);

                    equip.Rank = Convert.ToInt32(temp[21]);



                    AccessoriesList.Add(equip);


                }
            }
            catch (Exception E)
            {
                Console.WriteLine(E);
            }


            return (AccessoriesList, tableSpec);

        }
        //Method to get Android Data

        private async Task<(List<EquipCLS>, string)> GetAndroidCSVAsync(string FileName, string TableKey)
        {
            List<EquipCLS> AndroiList = new List<EquipCLS>();

            List<string> result = await ComFunc.CSVStringAsync(GVar.EquipmentPath, FileName);
            int counter = 0;
            string tableSpec = "";
            foreach (string accItem in result)
            {
                if (accItem == "")
                {
                    return (AndroiList, tableSpec);
                }
                if (counter == 0)
                {
                    tableSpec = ComFunc.TableSpecStringBuilder(TableColNames.EquipmentCN, accItem, TableKey);
                    counter += 1;
                    continue;
                }
                counter += 1;

                var temp = accItem.Split(",");

                EquipCLS equip = new EquipCLS();
                try
                {
                    equip.EquipSlot = temp[1];
                    equip.EquipName = temp[2];
                    equip.Category = temp[3];
                    equip.EquipLevel = Convert.ToInt32(temp[4]);
                                        
                    equip.Rank = temp[5].Contains(" ") ? Convert.ToInt32(temp[5].Split(' ').Last()) : Convert.ToInt32(temp[5]);

                    AndroiList.Add(equip);
                }

                catch (Exception E)
                {
                    Console.WriteLine(E);
                }
            }

            return (AndroiList, tableSpec);
        }

        //Method to get Medal Data
        private async Task<(List<EquipCLS>, string)> GetMedalCSVAsync(string FileName, string TableKey)
        {
            List<EquipCLS> AndroiList = new List<EquipCLS>();
            List<string> result = await ComFunc.CSVStringAsync(GVar.EquipmentPath, FileName);
            int counter = 0;
            string tableSpec = "";
            try
            {
                foreach (string accItem in result)
                {
                    if (accItem == "")
                    {
                        return (AndroiList, tableSpec);
                    }
                    if (counter == 0)
                    {
                        tableSpec = ComFunc.TableSpecStringBuilder(TableColNames.EquipmentCN, accItem, TableKey);
                        counter += 1;
                        continue;
                    }

                    var temp = accItem.Split(",");
                    counter +=1;

                    EquipCLS equip = new EquipCLS();
                    equip.EquipSlot = temp[1];
                    equip.ClassType = temp[2];
                    equip.EquipName = temp[3];
                    equip.EquipSet = temp[4];
                    equip.Category = temp[5];
                    equip.EquipLevel = Convert.ToInt32(temp[6]);

                    equip.BaseStats.STR = Convert.ToInt32(temp[7]);
                    equip.BaseStats.DEX = Convert.ToInt32(temp[8]);
                    equip.BaseStats.INT = Convert.ToInt32(temp[9]);
                    equip.BaseStats.LUK = Convert.ToInt32(temp[10]);
                    equip.BaseStats.MaxHP = Convert.ToInt32(temp[11]);
                    equip.BaseStats.MaxMP = Convert.ToInt32(temp[12]);
                    equip.BaseStats.DEF = Convert.ToInt32(temp[13]);
                    equip.BaseStats.ATK = Convert.ToInt32(temp[14]);
                    equip.BaseStats.MATK = Convert.ToInt32(temp[15]);
                    equip.BaseStats.IED = Convert.ToInt32(temp[16].Trim('%'));
                    equip.BaseStats.BD = Convert.ToInt32(temp[17].Trim('%'));
                    equip.BaseStats.SPD = Convert.ToInt32(temp[18]);
                    equip.BaseStats.JUMP = Convert.ToInt32(temp[19]);



                    AndroiList.Add(equip);
                }

            }
            catch (Exception E)
            {
                Console.WriteLine(E);
            }

            return (AndroiList, tableSpec);
        }

        //Method to get Main Weapon Data
        private async Task<(List<EquipCLS>, string)> GetWeaponCSVAsync(string FileName, string TableKey)
        {
            List<EquipCLS> WeaponList = new List<EquipCLS>();
            List<string> result = await ComFunc.CSVStringAsync(GVar.EquipmentPath, FileName);
            string tableSpec = "";
            int counter = 0;
            foreach (string weapItem in result)
            {
                if (weapItem == "")
                {
                    return (WeaponList, tableSpec);
                }
                if (counter == 0)
                {
                    tableSpec = ComFunc.TableSpecStringBuilder(TableColNames.EquipmentCN, weapItem, TableKey);
                    counter += 1;
                    continue;
                }
                var temp = weapItem.Split(",");

                EquipCLS equip = new EquipCLS();

                equip.EquipSlot = temp[1];
                equip.EquipSet = temp[2];
                equip.EquipName = temp[3];
                equip.WeaponType = temp[4];
                //Ignore Beast Tamer's Weapon
                if (equip.WeaponType == "Scepter")
                {
                    continue;
                }
                equip.EquipLevel = Convert.ToInt32(temp[5]);

                equip.BaseStats.STR = Convert.ToInt32(temp[6]);
                equip.BaseStats.DEX = Convert.ToInt32(temp[7]);
                equip.BaseStats.INT = Convert.ToInt32(temp[8]);
                equip.BaseStats.LUK = Convert.ToInt32(temp[9]);
                equip.BaseStats.MaxHP = Convert.ToInt32(temp[10]);
                equip.BaseStats.DEF = Convert.ToInt32(temp[11]);
                equip.BaseStats.ATK = Convert.ToInt32(temp[12]);
                equip.BaseStats.MATK = Convert.ToInt32(temp[13]);

                equip.BaseStats.BD = Convert.ToInt32(temp[15].Trim('%'));
                equip.BaseStats.IED = Convert.ToInt32(temp[16].Trim('%'));
                equip.BaseStats.SPD = Convert.ToInt32(temp[17]);
                equip.BaseStats.NoUpgrades = Convert.ToInt32(temp[19]);

                try
                {

                    WeaponList.Add(equip);
                }
                catch (Exception)
                {

                }
            }
            return (WeaponList, tableSpec);


        }

        //Method to get Sec Weapon Data
        public  async Task<(List<EquipCLS>, string)> GetSecondaryCSVAsync(string FileName, string TableKey)
        {
            List<EquipCLS> WeaponList = new List<EquipCLS>();
            List<string> result = await ComFunc.CSVStringAsync(GVar.EquipmentPath, FileName);
            string tableSpec = "";
            int counter = 0;
            try
            {
                foreach (string weapItem in result)
                {
                    if (weapItem == "")
                    {
                        return (WeaponList, tableSpec);
                    }
                    if (counter == 0)
                    {
                        tableSpec = ComFunc.TableSpecStringBuilder(TableColNames.EquipmentCN, weapItem, TableKey);
                        counter += 1;
                        continue;
                    }

                    counter += 1;
                    var temp = weapItem.Split(",");

                    EquipCLS equip = new EquipCLS();

                    equip.EquipSlot = temp[1];
                    equip.ClassType = temp[2];
                    equip.EquipName = temp[3];
                    equip.WeaponType = temp[4];
                    equip.EquipLevel = Convert.ToInt32(temp[5]);
                    equip.BaseStats.STR = Convert.ToInt32(temp[6]);
                    equip.BaseStats.DEX = Convert.ToInt32(temp[7]);
                    equip.BaseStats.INT = Convert.ToInt32(temp[8]);
                    equip.BaseStats.LUK = Convert.ToInt32(temp[9]);
                    equip.BaseStats.AllStat = Convert.ToInt32(temp[10]);
                    equip.BaseStats.MaxHP = Convert.ToInt32(temp[11]);
                    equip.BaseStats.MaxMP = Convert.ToInt32(temp[12]);
                    equip.BaseStats.DEF = Convert.ToInt32(temp[13]);
                    equip.BaseStats.ATK = Convert.ToInt32(temp[14]);
                    equip.BaseStats.MATK = Convert.ToInt32(temp[15]);
                    equip.BaseStats.ATKSPD = temp[16].Contains('(') ? Convert.ToInt32(temp[16].Split('(').Last().Replace(')', ' ')) : Convert.ToInt32(temp[16]);
                    equip.BaseStats.MaxDF = Convert.ToInt32(temp[17]);


                    WeaponList.Add(equip);

                }

            }
            catch (Exception E)
            {
                Console.WriteLine(E);
            }
            return (WeaponList, tableSpec);
        }



        //DB RETRIEVAL
        public static Dictionary<string, List<EquipCLS>> GetEquipmentDB()
        {
            Dictionary<string, List<EquipCLS>> EquipList = new Dictionary<string, List<EquipCLS>>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();
                SqliteCommand selectCMD = new SqliteCommand();
                selectCMD.Connection = dbCon;
                selectCMD.CommandText = "SELECT* FROM EquipArmorData; ";
                using (SqliteDataReader result = selectCMD.ExecuteReader())
                {
                    int ErrorCounter = 0;
                    while (result.Read())
                    {
                        ErrorCounter++;
                        string EquipSlot = result.GetString(0);

                        EquipCLS citem = new EquipCLS();
                        citem.EquipSlot = EquipSlot;
                        citem.ClassType = result.GetString(1);
                        citem.EquipName = result.GetString(2);
                        citem.EquipSet = result.GetString(3);
                        citem.EquipLevel = result.GetInt32(4);

                        citem.BaseStats.STR = result.GetInt32(5);
                        citem.BaseStats.DEX = result.GetInt32(6);
                        citem.BaseStats.INT = result.GetInt32(7);
                        citem.BaseStats.LUK = result.GetInt32(8);
                        citem.BaseStats.MaxHP = result.GetInt32(9);
                        citem.BaseStats.MaxMP = result.GetInt32(10);
                        citem.BaseStats.DEF = result.GetInt32(11);
                        citem.BaseStats.ATK = result.GetInt32(12);
                        citem.BaseStats.MATK = result.GetInt32(13);
                        citem.BaseStats.IED = result.GetInt32(14);
                        citem.BaseStats.SPD = result.GetInt32(15);
                        citem.BaseStats.JUMP = result.GetInt32(16);
                        citem.BaseStats.NoUpgrades = result.GetInt32(17);

                        try
                        {
                            if (EquipList.ContainsKey(EquipSlot))
                            {
                                EquipList[EquipSlot].Add(citem);
                                continue;
                            }
                            EquipList.Add(EquipSlot, new List<EquipCLS>() { citem });
                        }
                        catch (Exception E)
                        {
                            Console.WriteLine(E.Message);
                        }
                    }

                }
;
                selectCMD.CommandText = "SELECT* FROM EquipAccessoriesData;";
                using (SqliteDataReader reader = selectCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string EquipSlot = reader.GetString(0);
                        EquipCLS citem = new EquipCLS();
                        citem.EquipSlot = EquipSlot;
                        citem.ClassType = reader.GetString(1);
                        citem.EquipName = reader.GetString(2);
                        citem.EquipSet = reader.GetString(3);
                        citem.Category = reader.GetString(4);
                        citem.EquipLevel = reader.GetInt32(5);

                        citem.BaseStats.STR = reader.GetInt32(6);
                        citem.BaseStats.DEX = reader.GetInt32(7);
                        citem.BaseStats.INT = reader.GetInt32(8);
                        citem.BaseStats.LUK = reader.GetInt32(9);
                        citem.BaseStats.MaxHP = reader.GetInt32(10);
                        citem.BaseStats.MaxMP = reader.GetInt32(11);
                        citem.BaseStats.HP = reader.GetString(12);
                        citem.BaseStats.MP = reader.GetString(13);
                        citem.BaseStats.DEF = reader.GetInt32(14);
                        citem.BaseStats.ATK = reader.GetInt32(15);
                        citem.BaseStats.MATK = reader.GetInt32(16);
                        citem.BaseStats.IED = reader.GetInt32(17);
                        citem.BaseStats.SPD = reader.GetInt32(18);
                        citem.BaseStats.JUMP = reader.GetInt32(19);
                        citem.BaseStats.NoUpgrades = reader.GetInt32(20);
                        citem.BaseStats.Rank = reader.GetInt32(21);

                        if (EquipList.ContainsKey(EquipSlot))
                        {
                            EquipList[EquipSlot].Add(citem);
                            continue;
                        }
                        EquipList.Add(EquipSlot, new List<EquipCLS>() { citem });

                    }
                }
                selectCMD.CommandText = "SELECT* FROM EquipMedalData;";
                using (SqliteDataReader reader = selectCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string EquipSlot = reader.GetString(0);
                        EquipCLS citem = new EquipCLS();
                        citem.EquipSlot = EquipSlot;
                        citem.ClassType = reader.GetString(1);
                        citem.EquipName = reader.GetString(2);
                        citem.EquipSet = reader.GetString(3);
                        citem.Category = reader.GetString(4);
                        citem.EquipLevel = reader.GetInt32(5);

                        citem.BaseStats.STR = reader.GetInt32(6);
                        citem.BaseStats.DEX = reader.GetInt32(7);
                        citem.BaseStats.INT = reader.GetInt32(8);
                        citem.BaseStats.LUK = reader.GetInt32(9);
                        citem.BaseStats.MaxHP = reader.GetInt32(10);
                        citem.BaseStats.MaxMP = reader.GetInt32(11);
                        citem.BaseStats.DEF = reader.GetInt32(12);
                        citem.BaseStats.ATK = reader.GetInt32(13);
                        citem.BaseStats.MATK = reader.GetInt32(14);
                        citem.BaseStats.IED = reader.GetInt32(15);
                        citem.BaseStats.BD = reader.GetInt32(16);
                        citem.BaseStats.SPD = reader.GetInt32(17);
                        citem.BaseStats.JUMP = reader.GetInt32(18);

                        if (EquipList.ContainsKey(EquipSlot))
                        {
                            EquipList[EquipSlot].Add(citem);
                            continue;
                        }
                        EquipList.Add(EquipSlot, new List<EquipCLS>() { citem });

                    }
                }
                selectCMD.CommandText = "SELECT* FROM EquipAndroidData;";
                using (SqliteDataReader reader = selectCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string EquipSlot = reader.GetString(0);
                        EquipCLS citem = new EquipCLS();
                        citem.EquipSlot = EquipSlot;
                        citem.EquipName = reader.GetString(1);
                        citem.Category = reader.GetString(2);
                        citem.EquipLevel = reader.GetInt32(3);
                        citem.Rank = reader.GetInt32(4);


                        if (EquipList.ContainsKey(EquipSlot))
                        {
                            EquipList[EquipSlot].Add(citem);
                            continue;
                        }
                        EquipList.Add(EquipSlot, new List<EquipCLS>() { citem });

                    }
                }

                selectCMD.CommandText = "SELECT* FROM EquipSecondaryData;";
                using (SqliteDataReader reader = selectCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string EquipSlot = reader.GetString(0);
                        EquipCLS citem = new EquipCLS();
                        citem.EquipSlot = EquipSlot;
                        citem.ClassType = reader.GetString(1);
                        citem.EquipName = reader.GetString(2);
                        citem.WeaponType = reader.GetString(3);
                        citem.EquipLevel = reader.GetInt32(4);

                        citem.BaseStats.STR = reader.GetInt32(5);
                        citem.BaseStats.DEX = reader.GetInt32(6);
                        citem.BaseStats.INT = reader.GetInt32(7);
                        citem.BaseStats.LUK = reader.GetInt32(8);
                        citem.BaseStats.AllStat = reader.GetInt32(9);
                        citem.BaseStats.MaxHP = reader.GetInt32(10);
                        citem.BaseStats.MaxMP = reader.GetInt32(11);
                        citem.BaseStats.DEF = reader.GetInt32(12);
                        citem.BaseStats.ATK = reader.GetInt32(13);
                        citem.BaseStats.MATK = reader.GetInt32(14);
                        citem.BaseStats.ATKSPD = reader.GetInt32(15);
                        citem.BaseStats.MaxDF = reader.GetInt32(16);


                        if (EquipList.ContainsKey(EquipSlot))
                        {
                            EquipList[EquipSlot].Add(citem);
                            continue;
                        }
                        EquipList.Add(EquipSlot, new List<EquipCLS>() { citem });

                    }
                }

                selectCMD.CommandText = "SELECT* FROM EquipWeaponData;";
                using (SqliteDataReader reader = selectCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string EquipSlot = reader.GetString(0);
                        EquipCLS citem = new EquipCLS();
                        citem.EquipSlot = EquipSlot;
                        citem.EquipSet = reader.GetString(1);
                        citem.EquipName = reader.GetString(2);
                        citem.WeaponType = reader.GetString(3);
                        citem.EquipLevel = reader.GetInt32(4);

                        citem.BaseStats.STR = reader.GetInt32(5);
                        citem.BaseStats.DEX = reader.GetInt32(6);
                        citem.BaseStats.INT = reader.GetInt32(7);
                        citem.BaseStats.LUK = reader.GetInt32(8);
                        citem.BaseStats.MaxHP = reader.GetInt32(9);
                        citem.BaseStats.DEF = reader.GetInt32(10);
                        citem.BaseStats.ATK = reader.GetInt32(11);
                        citem.BaseStats.MATK = reader.GetInt32(12);
                        citem.BaseStats.ATKSPD = reader.GetInt32(13);
                        citem.BaseStats.BD = reader.GetInt32(14);
                        citem.BaseStats.IED = reader.GetInt32(15);
                        citem.BaseStats.SPD = reader.GetInt32(16);
                        citem.BaseStats.NoUpgrades = reader.GetInt32(17);


                        if (EquipList.ContainsKey(EquipSlot))
                        {
                            EquipList[EquipSlot].Add(citem);
                            continue;
                        }
                        EquipList.Add(EquipSlot, new List<EquipCLS>() { citem });

                    }
                }


            }


            return EquipList;
        }
       


    }
}
