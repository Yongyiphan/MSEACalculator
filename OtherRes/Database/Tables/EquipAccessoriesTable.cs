using Microsoft.Data.Sqlite;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class EquipAccessoriesTable : BaseDBTable, ITableUpload
    {
        public List<EquipCLS> EquipList { get; set; }

        private string[] AccessoriesTableSpec = { "(" +
                    "EquipName string," +
                    "EquipSet string," +
                    "ClassType  string," +
                    "EquipSlot string," +
                    "EquipLevel int," +
                    "MainStat int," +
                    "SecStat int," +
                    "AllStat int," +
                    "HP string," +
                    "MP string," +
                    "DEF int," +
                    "ATK int," +
                    "MATK int," +
                    "SPD int," +
                    "JUMP int," +
                    "IED int," +
                    "PRIMARY KEY (EquipName, EquipSet, ClassType, EquipSlot) " +
                    ");" };
        public EquipAccessoriesTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
            TableParameters =  AccessoriesTableSpec[0];
        }


        public async void RetrieveData()
        {
            EquipList =  await GetAccessoriesCSVAsync();
        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if (ComFunc.IsOpenConnection(connection))
            {
                string insertAcc = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);

                using (SqliteCommand insertCMD = new SqliteCommand(insertAcc, connection, transaction))
                {
                    foreach (EquipCLS Aitem in EquipList)
                    {
                        ErrorCounter++;
                        insertCMD.Parameters.Clear();
                        insertCMD.Parameters.AddWithValue("@EquipName", Aitem.EquipName);
                        insertCMD.Parameters.AddWithValue("@EquipSet", Aitem.EquipSet);
                        insertCMD.Parameters.AddWithValue("@ClassType", Aitem.ClassType);
                        insertCMD.Parameters.AddWithValue("@EquipSlot", Aitem.EquipSlot);
                        insertCMD.Parameters.AddWithValue("@EquipLevel", Aitem.EquipLevel);

                        insertCMD.Parameters.AddWithValue("@MainStat", Aitem.BaseStats.MS);
                        insertCMD.Parameters.AddWithValue("@SecStat", Aitem.BaseStats.SS);
                        insertCMD.Parameters.AddWithValue("@AllStat", Aitem.BaseStats.AllStat);
                        insertCMD.Parameters.AddWithValue("@HP", Aitem.BaseStats.SpecialHP);
                        insertCMD.Parameters.AddWithValue("@MP", Aitem.BaseStats.SpecialMP);
                        insertCMD.Parameters.AddWithValue("@DEF", Aitem.BaseStats.FlatDEF);
                        insertCMD.Parameters.AddWithValue("@ATK", Aitem.BaseStats.FlatATK);
                        insertCMD.Parameters.AddWithValue("@MATK", Aitem.BaseStats.FlatMATK);
                        insertCMD.Parameters.AddWithValue("@SPD", Aitem.BaseStats.SPD);
                        insertCMD.Parameters.AddWithValue("@JUMP", Aitem.BaseStats.JUMP);
                        insertCMD.Parameters.AddWithValue("@IED", Aitem.BaseStats.IED);

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


        public async Task<List<EquipCLS>> GetAccessoriesCSVAsync()
        {
            List<EquipCLS> AccessoriesList = new List<EquipCLS>();

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
                            return AccessoriesList;
                        }

                        var temp = accItem.Split(",");

                        EquipCLS equip = new EquipCLS();
                        equip.EquipName = temp[1];
                        equip.EquipSet = temp[2];
                        equip.ClassType = temp[3];
                        equip.EquipSlot = temp[4];

                        equip.EquipLevel = Convert.ToInt32(temp[5]);
                        equip.BaseStats.MS = Convert.ToInt32(temp[6]);
                        equip.BaseStats.SS = Convert.ToInt32(temp[7]);
                        equip.BaseStats.AllStat = Convert.ToInt32(temp[8]);

                        equip.BaseStats.SpecialHP = temp[9];
                        equip.BaseStats.SpecialMP = temp[10];
                        equip.BaseStats.FlatDEF = Convert.ToInt32(temp[11]);
                        equip.BaseStats.FlatATK = Convert.ToInt32(temp[12]);
                        equip.BaseStats.FlatMATK = Convert.ToInt32(temp[13]);

                        equip.BaseStats.SPD = Convert.ToInt32(temp[14]);
                        equip.BaseStats.JUMP = Convert.ToInt32(temp[15]);
                        equip.BaseStats.IED = Convert.ToInt32(temp[16]);


                        AccessoriesList.Add(equip);


                    }
                }
            }


            return AccessoriesList;
        }

        public static List<EquipCLS> GetAllAccessoriesDB()
        {
            List<EquipCLS> accModel = new List<EquipCLS>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM AccessoriesData";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EquipCLS equipModel = new EquipCLS();
                            equipModel.EquipName = reader.GetString(0);
                            equipModel.EquipSet = reader.GetString(1);
                            equipModel.ClassType = reader.GetString(2);
                            equipModel.EquipSlot = reader.GetString(3);
                            equipModel.EquipLevel = reader.GetInt32(4);

                            equipModel.BaseStats.MS = reader.GetInt32(5);
                            equipModel.BaseStats.SS = reader.GetInt32(6);
                            equipModel.BaseStats.AllStat = reader.GetInt32(7);

                            equipModel.BaseStats.SpecialHP = reader.GetString(8);
                            equipModel.BaseStats.SpecialMP = reader.GetString(9);
                            equipModel.BaseStats.FlatDEF = reader.GetInt32(10);
                            equipModel.BaseStats.FlatATK = reader.GetInt32(11);
                            equipModel.BaseStats.FlatMATK = reader.GetInt32(12);

                            equipModel.BaseStats.SPD = reader.GetInt32(13);
                            equipModel.BaseStats.JUMP = reader.GetInt32(14);
                            equipModel.BaseStats.IED = reader.GetInt32(15);

                            accModel.Add(equipModel);
                        }
                    }
                }


            }



            return accModel;
        }

    }
}
