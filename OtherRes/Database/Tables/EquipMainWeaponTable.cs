using Microsoft.Data.Sqlite;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSEACalculator.CharacterRes.EquipmentRes;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class EquipMainWeaponTable : BaseDBTable, ITableUpload
    {
        public List<EquipCLS> CurrentList { get; set; }

        private string[] MainWeapTableSpec = { "(" +
                    "EquipSet string," +
                    "WeaponType string," +
                    "EquipLevel int," +
                    "ATKSPD int," +
                    "MainStat int," +
                    "SecStat int," +
                    "HP int," +
                    "DEF int," +
                    "ATK int," +
                    "MATK int," +
                    "SPD int," +
                    "BDMG int," +
                    "IED int," +
                    "PRIMARY KEY (EquipSet, WeaponType) " +
                    ");" };

        public EquipMainWeaponTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
            TableParameters = MainWeapTableSpec[0];

        }
        public async void RetrieveData()
        {
            
            CurrentList = await GetWeaponCSVAsync();
        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if (ComFunc.IsOpenConnection(connection))
            {
                
                string insertWeap = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                using (SqliteCommand insertCMD = new SqliteCommand(insertWeap, connection, transaction))
                {
                    foreach (EquipCLS model in CurrentList)
                    {
                        ErrorCounter++;
                        insertCMD.Parameters.Clear();
                        insertCMD.Parameters.AddWithValue("@EquipSet", model.EquipSet);
                        insertCMD.Parameters.AddWithValue("@WeaponType", model.WeaponType);
                        insertCMD.Parameters.AddWithValue("@EquipLevel", model.EquipLevel);
                        insertCMD.Parameters.AddWithValue("@ATKSPD", model.BaseStats.ATKSPD);
                        insertCMD.Parameters.AddWithValue("@MainStat", model.BaseStats.MS);
                        insertCMD.Parameters.AddWithValue("@SecStat", model.BaseStats.SS);
                        insertCMD.Parameters.AddWithValue("@HP", model.BaseStats.MaxHP);
                        insertCMD.Parameters.AddWithValue("@DEF", model.BaseStats.DEF);
                        insertCMD.Parameters.AddWithValue("@ATK", model.BaseStats.ATK);
                        insertCMD.Parameters.AddWithValue("@MATK", model.BaseStats.MATK);
                        insertCMD.Parameters.AddWithValue("@SPD", model.BaseStats.SPD);
                        insertCMD.Parameters.AddWithValue("@BDMG", model.BaseStats.BD);
                        insertCMD.Parameters.AddWithValue("@IED", model.BaseStats.IED);

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

        public static async Task<List<EquipCLS>> GetWeaponCSVAsync()
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
                        equip.EquipSet = temp[1];
                        equip.WeaponType = temp[2];
                        equip.EquipLevel = Convert.ToInt32(temp[3]);
                        equip.BaseStats.ATKSPD = Convert.ToInt32(temp[4]);
                        equip.BaseStats.MS = Convert.ToInt32(temp[5]);
                        equip.BaseStats.SS = Convert.ToInt32(temp[6]);
                        equip.BaseStats.MaxHP = Convert.ToInt32(temp[7]);
                        equip.BaseStats.DEF = Convert.ToInt32(temp[8]);
                        equip.BaseStats.ATK = Convert.ToInt32(temp[9]);
                        equip.BaseStats.MATK = Convert.ToInt32(temp[10]);
                        equip.BaseStats.SPD = Convert.ToInt32(temp[11]);
                        equip.BaseStats.BD = Convert.ToInt32(temp[12]);
                        equip.BaseStats.IED = Convert.ToInt32(temp[13]);




                        WeaponList.Add(equip);


                    }
                }
            }


            return WeaponList;


        }

        public static List<EquipCLS> GetAllWeaponDB()
        {
            List<EquipCLS> weapModel = new List<EquipCLS>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM WeaponData";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EquipCLS equipModel = new EquipCLS();
                            equipModel.EquipSet = reader.GetString(0);
                            equipModel.WeaponType = reader.GetString(1);
                            equipModel.EquipLevel = reader.GetInt32(2);

                            equipModel.BaseStats.ATKSPD = reader.GetInt32(3);

                            equipModel.BaseStats.MS = reader.GetInt32(4);
                            equipModel.BaseStats.SS = reader.GetInt32(5);

                            equipModel.BaseStats.MaxHP = reader.GetInt32(6);
                            equipModel.BaseStats.DEF = reader.GetInt32(7);
                            equipModel.BaseStats.ATK = reader.GetInt32(8);
                            equipModel.BaseStats.MATK = reader.GetInt32(9);

                            equipModel.BaseStats.SPD = reader.GetInt32(10);
                            equipModel.BaseStats.BD = reader.GetInt32(11);
                            equipModel.BaseStats.IED = reader.GetInt32(12);



                            weapModel.Add(equipModel);
                        }
                    }
                }


            }



            return weapModel;
        }


    }
}
