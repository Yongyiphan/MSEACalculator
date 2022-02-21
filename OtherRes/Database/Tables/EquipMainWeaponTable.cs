using Microsoft.Data.Sqlite;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSEACalculator.CharacterRes.EquipmentRes;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class EquipMainWeaponTable : BaseDBTable, ITableUpload
    {
        public List<EquipCLS> CurrentList { get; set; }
        public EquipMainWeaponTable(string TableName, string TablePara) : base(TableName, TablePara)
        {
        }
        public async void RetrieveData()
        {
            
            CurrentList = await ImportCSV.GetWeaponCSVAsync();
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
                        insertCMD.Parameters.AddWithValue("@HP", model.BaseStats.HP);
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
    }
}
