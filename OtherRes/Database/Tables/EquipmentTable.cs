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

        List<string> EquipTableNames = new List<string> { "EquipArmorData", "EquipAccessoriesData", "EquipWeaponData", "EquipSecondaryData" };

        //EquipArmorData
        private string[] ArmorTableSpec = { "(" +
                "EquipSlot string, " +
                "ClassType string," +
                "EquipName string," +
                "EquipSet  string," +
                "EquipLevel int," +
                "STR int," +
                "DEX int," +
                "LUK int," +
                "INT int," +
                "MaxHP int," +
                "MaxMP int," +
                "DEF int" +
                "WATK int," +
                "MATK int," +
                "IED int, " +
                "Speed int," +
                "Jump int," +
                "Upgrades int," +
                "PRIMARY KEY (EquipSet, ClassType, EquipSlot)" +
                    ");" };
        //EquipAccessoriesData
        private string[] AccessoriesTableSpec = { "(" +
                "EquipSlot  string,"+
                "ClassType  string,	"+
                "EquipName	string,"+
                "EquipSet   string,"+
                "Category	string,"+
                "EquipLevel	string,"+

                "STR int,"+
                "DEX int,"+
                "INT int,"+
                "LUK int,"+
                "AllStats int,"+
                "MaxHP int,"+
                "MaxMP int,"+
                "DEF int,"+
                "WATK int,"+
                "MATK int,"+
                "IED int,"+
                "Speed int,"+
                "Jump int,"+
                "Upgrades int,"+
                "Rank int," +
                "PRIMARY KEY (EquipName, EquipSet, ClassType, EquipSlot) " +
                ");" };


        private string[] AndroidSpec = { "("+
                "EquipSlot string," +
                "EquipName   string," +
                "Category string," +
                "EquipLevel   int," +
                "Rank int," +
                "PRIMARY KEY (EquipSlot, EquipName)" +
                ");" };

        private string[] MedalSpec = { "(" +
                "EquipSlot  string," +
                "ClassType	string," +
                "EquipName  string," +
                "EquipSet	string," +
                "Category	string," +
                "EquipLevel	int," +

                "STR        int," +
                "DEX        int," +
                "INT        int," +
                "LUK        int," +
                "AllStats   int," +
                "MaxHP	    int," +
                "MaxMP	    int," +
                "DEF        int," +
                "WATK       int," +
                "MATK       int," +
                "IED        int," +
                "BD         int," +
                "Speed	    int," +
                "Jump       int," +
                "PRIMARY KEY (EquipSlot, ClassType, EquipName, EquipSet) " +
                ");" };


        private string[] MainWeapTableSpec = { "(" +
                "EquipSlot string," +
                "EquipSet string," +
                "EquipName string," +
                "WeaponType string," +
                "EquipLevel int," +
                "STR int," +
                "DEX int," +
                "INT int," +
                "LUK int," +
                "MaxHP int," +
                "DEF int," +
                "WATK int," +
                "MATK int," +
                "AttackSpeed int," +
                "BD int," +
                "IED int," +
                "Speed int," +
                "Upgrades int," +
                "PRIMARY KEY (EquipSet, WeaponType) " +
                ");" };

        private string[] SecWeapTableSpec = { "(" +
                "EquipSlot string," +
                "ClassName string," +
                "EquipName string," +
                "WeaponType string," +
                "EquipLevel int," +
                "STR int," +
                "DEX int," +
                "INT int," +
                "LUK int," +
                "AllStats int," +
                "MaxHP int," +
                "MaxMP int," +
                "DEF int," +
                "WATK int," +
                "MATK int," +
                "AttackSpeed int," +
                "MaxDF int," +
                "PRIMARY KEY (ClassName, EquipName, WeaponType, EquipLevel) " +
                ");" };

        public EquipmentTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {

            List<string> EquipTableNames = new List<string> { "EquipArmorData", "EquipAccessoriesData", "EquipWeaponData", "EquipSecondaryData" };
            switch (TableName)
            {
                case "EquipArmorData":
                    TablePara = ArmorTableSpec[0];
                    break;
                case "EquipAccessoriesData":
                    TablePara = AccessoriesTableSpec[0];
                    break;
                case "EquipAndroidData":
                    TablePara = AndroidSpec[0];
                    break;
                case "EquipMedalData":
                    TablePara = MedalSpec[0];
                    break;

                case "EquipWeaponData":
                    TablePara = MainWeapTableSpec[0];
                    break;
                case "EquipSecondaryData":
                    TablePara = SecWeapTableSpec[0];
                    break;
            }

        }

        public async void RetrieveData()
        {
            switch (TableName)
            {
                case "EquipArmorData":
                    EquipList = await GetArmorCSVAsync();
                    break;

                case "EquipAccessoriesData":
                    EquipList = await GetAccessoriesCSVAsync();
                    break;
                case "EquipAndroidData":
                    EquipList = await GetAndroidCSVAsync();
                    break;
                case "EquipMedalData":
                    EquipList = await GetMedalCSVAsync();
                    break;

                case "EquipWeaponData":
                    EquipList = await GetWeaponCSVAsync();
                    break;
                case "EquipSecondaryData":
                    EquipList = await GetSecondaryCSVAsync();
                    break;
            }

        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if (ComFunc.IsOpenConnection(connection))
            {

                switch (TableName)
                {
                    case "EquipArmorData":
                        UploadArmor(connection, transaction);
                        break;
                    case "EquipAccessoriesData":
                        UploadAccessories(connection, transaction);
                        break;
                    case "EquipAndroidData":
                        UploadAndroid(connection, transaction);
                        break;
                    case "EquipMedalData":
                        UploadMedal(connection, transaction);
                        break;

                    case "EquipWeaponData":
                        UploadWeapon(connection, transaction);
                        break;
                    case "EquipSecondaryData":
                        UploadSecondary(connection, transaction);
                        break;
                }
            }
        }


        //Method to get Armor Data
        private static async Task<List<EquipCLS>> GetArmorCSVAsync()
        {
            List<EquipCLS> equipList = new List<EquipCLS>();

            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.EquipmentPath + "ArmorData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    int counter = 0;
                    foreach (string equipItem in result.Skip(1))
                    {
                        if (equipItem == "")
                        {
                            return equipList;
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
                        equip.BaseStats.LUK = Convert.ToInt32(temp[8]);
                        equip.BaseStats.INT = Convert.ToInt32(temp[9]);

                        equip.BaseStats.MaxHP = Convert.ToInt32(temp[10]);
                        equip.BaseStats.MaxMP = Convert.ToInt32(temp[11]);
                        equip.BaseStats.DEF = Convert.ToInt32(temp[12]);
                        equip.BaseStats.ATK = Convert.ToInt32(temp[13]);
                        equip.BaseStats.MATK = Convert.ToInt32(temp[14]);
                        equip.BaseStats.IED = Convert.ToInt32(temp[15]);
                        equip.BaseStats.SPD = Convert.ToInt32(temp[16]);
                        equip.BaseStats.JUMP = Convert.ToInt32(temp[17]);
                        equip.BaseStats.NoUpgrades = Convert.ToInt32(temp[18]);

                        equipList.Add(equip);

                    }
                }
            }
            return equipList;

        }
        private void UploadArmor(SqliteConnection connection, SqliteTransaction transaction)
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
                    insertCMD.Parameters.AddWithValue("@EquipName", equipItem.EquipName);
                    insertCMD.Parameters.AddWithValue("@EquipSet", equipItem.EquipSet);
                    insertCMD.Parameters.AddWithValue("@EquipLevel", equipItem.EquipLevel);

                    insertCMD.Parameters.AddWithValue("@STR", equipItem.BaseStats.STR);
                    insertCMD.Parameters.AddWithValue("@DEX", equipItem.BaseStats.DEX);
                    insertCMD.Parameters.AddWithValue("@LUK", equipItem.BaseStats.INT);
                    insertCMD.Parameters.AddWithValue("@INT", equipItem.BaseStats.LUK);
                    insertCMD.Parameters.AddWithValue("@MaxHP", equipItem.BaseStats.MaxHP);
                    insertCMD.Parameters.AddWithValue("@MaxMP", equipItem.BaseStats.MaxMP);
                    insertCMD.Parameters.AddWithValue("@DEF", equipItem.BaseStats.DEF);
                    insertCMD.Parameters.AddWithValue("@WATK", equipItem.BaseStats.ATK);
                    insertCMD.Parameters.AddWithValue("@MATK", equipItem.BaseStats.MATK);
                    insertCMD.Parameters.AddWithValue("@IED", equipItem.BaseStats.IED);
                    insertCMD.Parameters.AddWithValue("@Speed", equipItem.BaseStats.SPD);
                    insertCMD.Parameters.AddWithValue("@Jump", equipItem.BaseStats.JUMP);
                    insertCMD.Parameters.AddWithValue("@Upgrades", equipItem.BaseStats.NoUpgrades);

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

        //Method to get Acc Data
        private async Task<List<EquipCLS>> GetAccessoriesCSVAsync()
        {
            List<EquipCLS> AccessoriesList = new List<EquipCLS>();

            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.EquipmentPath + "AccessoryData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    foreach (string accItem in result.Skip(1))
                    {
                        if (accItem == "")
                        {
                            return AccessoriesList;
                        }
                
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
                        equip.BaseStats.AllStat = Convert.ToInt32(temp[11]);
                        equip.BaseStats.MaxHP = Convert.ToInt32(temp[12]);
                        equip.BaseStats.MaxMP = Convert.ToInt32(temp[13]);
                        equip.BaseStats.DEF = Convert.ToInt32(temp[14]);
                        equip.BaseStats.ATK = Convert.ToInt32(temp[15]);
                        equip.BaseStats.MATK = Convert.ToInt32(temp[16]);
                        equip.BaseStats.IED = Convert.ToInt32(temp[17]);
                        equip.BaseStats.SPD = Convert.ToInt32(temp[18]);
                        equip.BaseStats.JUMP = Convert.ToInt32(temp[19]);
                        equip.BaseStats.NoUpgrades = Convert.ToInt32(temp[20]);

                        equip.Rank = Convert.ToInt32(temp[21]);



                        AccessoriesList.Add(equip);


                    }
                }
            }


            return AccessoriesList;
        }
        private void UploadAccessories(SqliteConnection connection, SqliteTransaction transaction)
        {
            string insertAcc = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);

            using (SqliteCommand insertCMD = new SqliteCommand(insertAcc, connection, transaction))
            {
                foreach (EquipCLS Aitem in EquipList)
                {

                    ErrorCounter++;
                    insertCMD.Parameters.Clear();
                    insertCMD.Parameters.AddWithValue("@EquipSlot", Aitem.EquipSlot);
                    insertCMD.Parameters.AddWithValue("@ClassType",Aitem.ClassType);
                    insertCMD.Parameters.AddWithValue("@EquipName",Aitem.EquipName);
                    insertCMD.Parameters.AddWithValue("@EquipSet",Aitem.EquipSet);
                    insertCMD.Parameters.AddWithValue("@Category",Aitem.Category);
                    insertCMD.Parameters.AddWithValue("@EquipLevel",Aitem.EquipLevel);

                    insertCMD.Parameters.AddWithValue("@STR",Aitem.BaseStats.STR);
                    insertCMD.Parameters.AddWithValue("@DEX",Aitem.BaseStats.DEX);
                    insertCMD.Parameters.AddWithValue("@INT",Aitem.BaseStats.INT);
                    insertCMD.Parameters.AddWithValue("@LUK",Aitem.BaseStats.LUK);
                    insertCMD.Parameters.AddWithValue("@AllStats",Aitem.BaseStats.AllStat);
                    insertCMD.Parameters.AddWithValue("@MaxHP",Aitem.BaseStats.MaxHP);
                    insertCMD.Parameters.AddWithValue("@MaxMP",Aitem.BaseStats.MaxMP);
                    insertCMD.Parameters.AddWithValue("@DEF",Aitem.BaseStats.DEF);
                    insertCMD.Parameters.AddWithValue("@WATK",Aitem.BaseStats.ATK);
                    insertCMD.Parameters.AddWithValue("@MATK",Aitem.BaseStats.MATK);
                    insertCMD.Parameters.AddWithValue("@IED",Aitem.BaseStats.IED);
                    insertCMD.Parameters.AddWithValue("@Speed",Aitem.BaseStats.SPD);
                    insertCMD.Parameters.AddWithValue("@Jump",Aitem.BaseStats.JUMP);
                    insertCMD.Parameters.AddWithValue("@Upgrades",Aitem.BaseStats.NoUpgrades);
                    insertCMD.Parameters.AddWithValue("@Rank", Aitem.Rank);

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

        //Method to get Android Data

        private async Task<List<EquipCLS>> GetAndroidCSVAsync()
        {
            List<EquipCLS> AndroiList = new List<EquipCLS>();

            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.EquipmentPath + "AndroidData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    foreach (string accItem in result.Skip(1))
                    {
                        if (accItem == "")
                        {
                            return AndroiList;
                        }
                
                        var temp = accItem.Split(",");

                        EquipCLS equip = new EquipCLS();
                        equip.EquipSlot = temp[1];
                        equip.EquipName = temp[2];
                        equip.Category = temp[3];
                        equip.EquipLevel = Convert.ToInt32(temp[4]);
                        equip.Rank = Convert.ToInt32(temp[5]);
                        
                        AndroiList.Add(equip);
                    }
                }
            }
            return AndroiList;
        }

        private void UploadAndroid(SqliteConnection connection, SqliteTransaction transaction)
        {
            string insertAcc = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);

            using (SqliteCommand insertCMD = new SqliteCommand(insertAcc, connection, transaction))
            {
                foreach (EquipCLS Aitem in EquipList)
                {

                    ErrorCounter++;
                    insertCMD.Parameters.Clear();
                    insertCMD.Parameters.AddWithValue("@EquipSlot", Aitem.EquipSlot);
                    insertCMD.Parameters.AddWithValue("@EquipName", Aitem.EquipName);
                    insertCMD.Parameters.AddWithValue("@Category", Aitem.Category);
                    insertCMD.Parameters.AddWithValue("@EquipLevel", Aitem.EquipLevel);
                    insertCMD.Parameters.AddWithValue("@Rank", Aitem.Rank);

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
        
        //Method to get Medal Data
        private async Task<List<EquipCLS>> GetMedalCSVAsync()
        {
            List<EquipCLS> AndroiList = new List<EquipCLS>();

            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.EquipmentPath + "AccessoriesData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    foreach (string accItem in result.Skip(1))
                    {
                        if (accItem == "")
                        {
                            return AndroiList;
                        }
                        var temp = accItem.Split(",");

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
                        equip.BaseStats.AllStat = Convert.ToInt32(temp[11]);
                        equip.BaseStats.MaxHP = Convert.ToInt32(temp[12]);
                        equip.BaseStats.MaxMP = Convert.ToInt32(temp[13]);
                        equip.BaseStats.DEF = Convert.ToInt32(temp[14]);
                        equip.BaseStats.ATK = Convert.ToInt32(temp[15]);
                        equip.BaseStats.MATK = Convert.ToInt32(temp[16]);
                        equip.BaseStats.IED = Convert.ToInt32(temp[17]);
                        equip.BaseStats.BD = Convert.ToInt32(temp[18]);
                        equip.BaseStats.SPD = Convert.ToInt32(temp[19]);
                        equip.BaseStats.JUMP = Convert.ToInt32(temp[20]);
                        

                        
                        AndroiList.Add(equip);
                    }
                }
            }
            return AndroiList;
        }

        private void UploadMedal(SqliteConnection connection, SqliteTransaction transaction)
        {
            string insertAcc = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);

            using (SqliteCommand insertCMD = new SqliteCommand(insertAcc, connection, transaction))
            {
                foreach (EquipCLS Aitem in EquipList)
                {

                    ErrorCounter++;
                    insertCMD.Parameters.Clear();
                    insertCMD.Parameters.AddWithValue("EquipSlot", Aitem.EquipSlot);
                    insertCMD.Parameters.AddWithValue("ClassType", Aitem.ClassType);
                    insertCMD.Parameters.AddWithValue("EquipName", Aitem.EquipName);
                    insertCMD.Parameters.AddWithValue("EquipSet", Aitem.EquipSet);
                    insertCMD.Parameters.AddWithValue("Category", Aitem.Category);
                    insertCMD.Parameters.AddWithValue("EquipLevel", Aitem.EquipLevel);

                    insertCMD.Parameters.AddWithValue("STR", Aitem.BaseStats.STR);
                    insertCMD.Parameters.AddWithValue("DEX", Aitem.BaseStats.DEX);
                    insertCMD.Parameters.AddWithValue("@INT", Aitem.BaseStats.INT);
                    insertCMD.Parameters.AddWithValue("@LUK", Aitem.BaseStats.LUK);
                    insertCMD.Parameters.AddWithValue("@AllStats", Aitem.BaseStats.AllStat);
                    insertCMD.Parameters.AddWithValue("@MaxHP", Aitem.BaseStats.MaxHP);
                    insertCMD.Parameters.AddWithValue("@MaxMP", Aitem.BaseStats.MaxMP);
                    insertCMD.Parameters.AddWithValue("@DEF", Aitem.BaseStats.DEF);
                    insertCMD.Parameters.AddWithValue("@WATK", Aitem.BaseStats.ATK);
                    insertCMD.Parameters.AddWithValue("@MATK", Aitem.BaseStats.MATK);
                    insertCMD.Parameters.AddWithValue("@IED", Aitem.BaseStats.IED);
                    insertCMD.Parameters.AddWithValue("@BD", Aitem.BaseStats.BD);
                    insertCMD.Parameters.AddWithValue("Speed", Aitem.BaseStats.SPD);
                    insertCMD.Parameters.AddWithValue("@Jump", Aitem.BaseStats.JUMP);


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


        //Method to get Main Weapon Data
        private static async Task<List<EquipCLS>> GetWeaponCSVAsync()
        {
            List<EquipCLS> WeaponList = new List<EquipCLS>();

            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.EquipmentPath + "WeaponData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    foreach (string weapItem in result.Skip(1))
                    {
                        if (weapItem == "")
                        {
                            return WeaponList;
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
                        equip.BaseStats.ATKSPD = Convert.ToInt32(temp[14]);
                        equip.BaseStats.BD = Convert.ToInt32(temp[15]);
                        equip.BaseStats.IED = Convert.ToInt32(temp[16]);
                        equip.BaseStats.SPD = Convert.ToInt32(temp[17]);
                        equip.BaseStats.NoUpgrades = Convert.ToInt32(temp[19]);

                        WeaponList.Add(equip);
                    }
                }
            }
            return WeaponList;


        }

        private void UploadWeapon(SqliteConnection connection, SqliteTransaction transaction)
        {
            string insertWeap = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
            using (SqliteCommand insertCMD = new SqliteCommand(insertWeap, connection, transaction))
            {
                foreach (EquipCLS Aitem in EquipList)
                {
                    ErrorCounter++;
                    insertCMD.Parameters.Clear();


                    insertCMD.Parameters.AddWithValue("@EquipSlot", Aitem.EquipSlot);
                    insertCMD.Parameters.AddWithValue("@EquipSet", Aitem.EquipSet);
                    insertCMD.Parameters.AddWithValue("@EquipName", Aitem.EquipName);
                    insertCMD.Parameters.AddWithValue("@WeaponType", Aitem.WeaponType);
                    insertCMD.Parameters.AddWithValue("@EquipLevel", Aitem.EquipLevel);
                    insertCMD.Parameters.AddWithValue("@STR", Aitem.BaseStats.STR);
                    insertCMD.Parameters.AddWithValue("@DEX", Aitem.BaseStats.DEX);
                    insertCMD.Parameters.AddWithValue("@INT", Aitem.BaseStats.INT);
                    insertCMD.Parameters.AddWithValue("@LUK", Aitem.BaseStats.LUK);
                    insertCMD.Parameters.AddWithValue("@MaxHP", Aitem.BaseStats.MaxHP);
                    insertCMD.Parameters.AddWithValue("@DEF", Aitem.BaseStats.DEF);
                    insertCMD.Parameters.AddWithValue("@WATK", Aitem.BaseStats.ATK);
                    insertCMD.Parameters.AddWithValue("@MATK", Aitem.BaseStats.MATK);
                    insertCMD.Parameters.AddWithValue("@AttackSpeed", Aitem.BaseStats.ATKSPD);
                    insertCMD.Parameters.AddWithValue("@BD", Aitem.BaseStats.BD);
                    insertCMD.Parameters.AddWithValue("@IED", Aitem.BaseStats.IED);
                    insertCMD.Parameters.AddWithValue("@Speed", Aitem.BaseStats.SPD);
                    insertCMD.Parameters.AddWithValue("@Upgrades", Aitem.BaseStats.NoUpgrades);
                    try
                    {
                        insertCMD.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        Console.WriteLine(ErrorCounter.ToString());
                    }
                }

            }
        }


        //Method to get Sec Weapon Data
        public static async Task<List<EquipCLS>> GetSecondaryCSVAsync()
        {
            List<EquipCLS> WeaponList = new List<EquipCLS>();

            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.EquipmentPath + "SecondaryData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    foreach (string weapItem in result.Skip(1))
                    {
                        if (weapItem == "")
                        {
                            return WeaponList;
                        }

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
                        equip.BaseStats.ATKSPD = Convert.ToInt32(temp[16]);
                        equip.BaseStats.MaxDF = Convert.ToInt32(temp[17]);


                        WeaponList.Add(equip);

                    }
                }
            }
            return WeaponList;
        }

        public void UploadSecondary(SqliteConnection connection, SqliteTransaction transaction)
        {
            string insertSec = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
            using (SqliteCommand insertCMD = new SqliteCommand(insertSec, connection, transaction))
            {
                foreach (EquipCLS Aitem in EquipList)
                {
                    ErrorCounter++;
                    insertCMD.Parameters.Clear();
                    insertCMD.Parameters.AddWithValue("@EquipSlot", Aitem.EquipSlot);
                    insertCMD.Parameters.AddWithValue("@ClassName", Aitem.ClassType);
                    insertCMD.Parameters.AddWithValue("@EquipName", Aitem.EquipName);
                    insertCMD.Parameters.AddWithValue("@WeaponType", Aitem.WeaponType);
                    insertCMD.Parameters.AddWithValue("@EquipLevel", Aitem.EquipLevel);
                    insertCMD.Parameters.AddWithValue("@STR", Aitem.BaseStats.STR);
                    insertCMD.Parameters.AddWithValue("@DEX", Aitem.BaseStats.DEX);
                    insertCMD.Parameters.AddWithValue("@INT", Aitem.BaseStats.INT);
                    insertCMD.Parameters.AddWithValue("@LUK", Aitem.BaseStats.LUK);
                    insertCMD.Parameters.AddWithValue("@AllStats", Aitem.BaseStats.AllStat);
                    insertCMD.Parameters.AddWithValue("@MaxHP", Aitem.BaseStats.MaxHP);
                    insertCMD.Parameters.AddWithValue("@MaxMP", Aitem.BaseStats.MaxMP);
                    insertCMD.Parameters.AddWithValue("@DEF", Aitem.BaseStats.DEF);
                    insertCMD.Parameters.AddWithValue("@WATK", Aitem.BaseStats.ATK);
                    insertCMD.Parameters.AddWithValue("@MATK", Aitem.BaseStats.MATK);
                    insertCMD.Parameters.AddWithValue("@AttackSpeed", Aitem.BaseStats.ATKSPD);
                    insertCMD.Parameters.AddWithValue("@MaxDF", Aitem.BaseStats.MaxDF);

                    try
                    {
                        insertCMD.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        Console.WriteLine(ErrorCounter.ToString());
                    }
                }

            }
        }


    }
}
