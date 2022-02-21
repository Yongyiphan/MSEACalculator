using Microsoft.Data.Sqlite;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class EquipArmorTable : BaseDBTable, ITableUpload
    {
        public List<EquipCLS> EquipList { get; set; }

        public EquipArmorTable(string TableName, string TablePara) : base(TableName, TablePara)
        {
            
        }

        public async void RetrieveData()
        {
            EquipList = await ImportCSV.GetArmorCSVAsync();
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
                        insertCMD.Parameters.AddWithValue("@MainStat", equipItem.BaseStats.MS);
                        insertCMD.Parameters.AddWithValue("@SecStat", equipItem.BaseStats.SS);
                        insertCMD.Parameters.AddWithValue("@AllStat", equipItem.BaseStats.AllStat);

                        insertCMD.Parameters.AddWithValue("@HP", equipItem.BaseStats.HP);
                        insertCMD.Parameters.AddWithValue("@MP", equipItem.BaseStats.MP);
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
    }
}
