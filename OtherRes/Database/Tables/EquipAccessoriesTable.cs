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
                    "STR int," +
                    "DEX int," +
                    "INT int," +
                    "LUK int," +
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

                        insertCMD.Parameters.AddWithValue("@STR", Aitem.BaseStats.STR);
                        insertCMD.Parameters.AddWithValue("@DEX", Aitem.BaseStats.DEX);
                        insertCMD.Parameters.AddWithValue("@INT", Aitem.BaseStats.INT);
                        insertCMD.Parameters.AddWithValue("@LUK", Aitem.BaseStats.LUK);
                        insertCMD.Parameters.AddWithValue("@AllStat", Aitem.BaseStats.AllStat);
                        insertCMD.Parameters.AddWithValue("@HP", Aitem.BaseStats.HP);
                        insertCMD.Parameters.AddWithValue("@MP", Aitem.BaseStats.MP);
                        insertCMD.Parameters.AddWithValue("@DEF", Aitem.BaseStats.DEF);
                        insertCMD.Parameters.AddWithValue("@ATK", Aitem.BaseStats.ATK);
                        insertCMD.Parameters.AddWithValue("@MATK", Aitem.BaseStats.MATK);
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
                        equip.BaseStats.STR = Convert.ToInt32(temp[6]);
                        equip.BaseStats.DEX = Convert.ToInt32(temp[7]);
                        equip.BaseStats.INT = Convert.ToInt32(temp[8]);
                        equip.BaseStats.LUK = Convert.ToInt32(temp[9]);
                        equip.BaseStats.AllStat = Convert.ToInt32(temp[10]);


                        equip.BaseStats.HP = temp[11];
                        equip.BaseStats.MP = temp[12];
                        equip.BaseStats.DEF = Convert.ToInt32(temp[13]);
                        equip.BaseStats.ATK = Convert.ToInt32(temp[14]);
                        equip.BaseStats.MATK = Convert.ToInt32(temp[15]);

                        equip.BaseStats.SPD = Convert.ToInt32(temp[16]);
                        equip.BaseStats.JUMP = Convert.ToInt32(temp[17]);
                        equip.BaseStats.IED = Convert.ToInt32(temp[18]);


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

                string selectQuery = "SELECT * FROM EquipAccessoriesData";

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

                            equipModel.BaseStats.STR = reader.GetInt32(5);
                            equipModel.BaseStats.DEX = reader.GetInt32(6);
                            equipModel.BaseStats.INT = reader.GetInt32(7);
                            equipModel.BaseStats.LUK = reader.GetInt32(8);
                            equipModel.BaseStats.AllStat = reader.GetInt32(9);
    
                            if (reader.GetString(10).Contains('%'))
                            {
                                equipModel.BaseStats.HP = reader.GetString(10);

                            }
                            else
                            {
                                equipModel.BaseStats.MaxHP = Convert.ToInt32(reader.GetString(10));
                            }

                            if (reader.GetString(11).Contains('%'))
                            {
                                equipModel.BaseStats.MP = reader.GetString(11);
                            }
                            else
                            {
                                equipModel.BaseStats.MaxMP = Convert.ToInt32(reader.GetString(11));
                            }
                            equipModel.BaseStats.DEF = reader.GetInt32(12);
                            equipModel.BaseStats.ATK = reader.GetInt32(13);
                            equipModel.BaseStats.MATK = reader.GetInt32(14);

                            equipModel.BaseStats.SPD = reader.GetInt32(15);
                            equipModel.BaseStats.JUMP = reader.GetInt32(16);
                            equipModel.BaseStats.IED = reader.GetInt32(17);

                            accModel.Add(equipModel);
                        }
                    }
                }


            }



            return accModel;
        }

    }
}
