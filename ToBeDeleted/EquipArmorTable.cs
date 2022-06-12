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
    public class EquipArmorTable : BaseDBTable, ITableUpload
    {
        public List<EquipCLS> EquipList { get; set; }

        private string[] ArmorTableSpec = { "(" +
                    "EquipSet string," +
                    "ClassType string," +
                    "EquipSlot string," +
                    "EquipLevel int," +
                    "STR int," +
                    "DEX int," +
                    "INT int," +
                    "LUK int," +
                    "AllStat int," +
                    "HP int," +
                    "MP int," +
                    "DEF int," +
                    "ATK int," +
                    "MATK int," +
                    "SPD int," +
                    "JUMP int," +
                    "IED int," +
                    "PRIMARY KEY (EquipSet, ClassType, EquipSlot)" +
                    ");" };

        public EquipArmorTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
            TableParameters = ArmorTableSpec[0];
        }

        public async void RetrieveData()
        {
            EquipList = await GetArmorCSVAsync();
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
                        insertCMD.Parameters.AddWithValue("@EquipSet", equipItem.EquipSet);
                        insertCMD.Parameters.AddWithValue("@ClassType", equipItem.ClassType);
                        insertCMD.Parameters.AddWithValue("@EquipSlot", equipItem.EquipSlot);
                        insertCMD.Parameters.AddWithValue("@EquipLevel", equipItem.EquipLevel);
                        insertCMD.Parameters.AddWithValue("@STR", equipItem.BaseStats.STR);
                        insertCMD.Parameters.AddWithValue("@DEX", equipItem.BaseStats.DEX);
                        insertCMD.Parameters.AddWithValue("@INT", equipItem.BaseStats.INT);
                        insertCMD.Parameters.AddWithValue("@LUK", equipItem.BaseStats.LUK);

                        insertCMD.Parameters.AddWithValue("@AllStat", equipItem.BaseStats.AllStat);

                        insertCMD.Parameters.AddWithValue("@HP", equipItem.BaseStats.MaxHP);
                        insertCMD.Parameters.AddWithValue("@MP", equipItem.BaseStats.MaxMP);
                        insertCMD.Parameters.AddWithValue("@DEF", equipItem.BaseStats.DEF);
                        insertCMD.Parameters.AddWithValue("@ATK", equipItem.BaseStats.ATK);
                        insertCMD.Parameters.AddWithValue("@MATK", equipItem.BaseStats.MATK);
                        insertCMD.Parameters.AddWithValue("@SPD", equipItem.BaseStats.SPD);
                        insertCMD.Parameters.AddWithValue("@JUMP", equipItem.BaseStats.JUMP);
                        insertCMD.Parameters.AddWithValue("@IED", equipItem.BaseStats.IED);

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



        public static async Task<List<EquipCLS>> GetArmorCSVAsync()
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
                        equip.EquipSet = temp[1];
                        equip.ClassType = temp[2];
                        equip.EquipSlot = temp[3];
                        equip.EquipLevel = Convert.ToInt32(temp[4]);
                        equip.BaseStats.STR = Convert.ToInt32(temp[5]);
                        equip.BaseStats.DEX = Convert.ToInt32(temp[6]);
                        equip.BaseStats.INT = Convert.ToInt32(temp[7]);
                        equip.BaseStats.LUK = Convert.ToInt32(temp[8]);

                        equip.BaseStats.AllStat = Convert.ToInt32(temp[9]);
                        equip.BaseStats.MaxHP = Convert.ToInt32(temp[10]);
                        equip.BaseStats.MaxMP = Convert.ToInt32(temp[11]);
                        equip.BaseStats.DEF = Convert.ToInt32(temp[12]);
                        equip.BaseStats.ATK = Convert.ToInt32(temp[13]);
                        equip.BaseStats.MATK = Convert.ToInt32(temp[14]);
                        equip.BaseStats.SPD = Convert.ToInt32(temp[15]);
                        equip.BaseStats.JUMP = Convert.ToInt32(temp[16]);
                        equip.BaseStats.IED = Convert.ToInt32(temp[17]);

                        equipList.Add(equip);

                    }
                }
            }
            return equipList;

        }

        


    }
}
