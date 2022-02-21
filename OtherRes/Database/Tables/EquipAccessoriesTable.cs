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
    public class EquipAccessoriesTable : BaseDBTable, ITableUpload
    {
        public EquipAccessoriesTable(string TableName, string TablePara) : base(TableName, TablePara)
        {
        }

        public List<EquipCLS> EquipList { get; set; }

        public async void RetrieveData()
        {
            EquipList =  await ImportCSV.GetAccessoriesCSVAsync();
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
    }
}
