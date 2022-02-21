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
    public class EquipSecWeaponTable : BaseDBTable, ITableUpload
    {
        public List<EquipCLS> CurrentList { get; set; }
        public EquipSecWeaponTable(string TableName, string TablePara) : base(TableName, TablePara)
        {
        }

        public async void RetrieveData()
        {

            CurrentList = await ImportCSV.GetSecondaryCSVAsync();
        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if (ComFunc.IsOpenConnection(connection))
            {
                string insertSec = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                using (SqliteCommand insertCMD = new SqliteCommand(insertSec, connection, transaction))
                {
                    foreach (EquipCLS model in CurrentList)
                    {
                        ErrorCounter++;
                        insertCMD.Parameters.Clear();
                        insertCMD.Parameters.AddWithValue("@ClassName", model.ClassType);
                        insertCMD.Parameters.AddWithValue("@EquipName", model.EquipName);
                        insertCMD.Parameters.AddWithValue("@WeaponType", model.WeaponType);
                        insertCMD.Parameters.AddWithValue("@EquipLevel", model.EquipLevel);
                        
                        insertCMD.Parameters.AddWithValue("@MainStat", model.BaseStats.MS);
                        insertCMD.Parameters.AddWithValue("@SecStat", model.BaseStats.SS);
                        insertCMD.Parameters.AddWithValue("@AllStat", model.BaseStats.AllStat);


                        insertCMD.Parameters.AddWithValue("@ATK", model.BaseStats.ATK);
                        insertCMD.Parameters.AddWithValue("@MATK", model.BaseStats.MATK);
                        insertCMD.Parameters.AddWithValue("@DEF", model.BaseStats.DEF);
                        insertCMD.Parameters.AddWithValue("@HP", model.BaseStats.HP);
                        insertCMD.Parameters.AddWithValue("@MP", model.BaseStats.MP);
                        insertCMD.Parameters.AddWithValue("@ATKSPD", model.BaseStats.ATKSPD);


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
}
