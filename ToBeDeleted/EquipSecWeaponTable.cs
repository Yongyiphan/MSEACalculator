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
    public class EquipSecWeaponTable : BaseDBTable, ITableUpload
    {
        public List<EquipCLS> CurrentList { get; set; }

        private string[] SecWeapTableSpec = { "(" +
                    "ClassName string," +
                    "EquipName string," +
                    "WeaponType string," +
                    "EquipLevel int," +
                    "STR int," +
                    "DEX int," +
                    "INT int," +
                    "LUK int," +    
                    "ATK int," +
                    "MATK int," +
                    "AllStat int," +
                    "DEF int," +
                    "HP int," +
                    "MP int," +
                    "ATKSPD int," +
                    "PRIMARY KEY (ClassName, EquipName, WeaponType, EquipLevel) " +
                    ");" };


        public EquipSecWeaponTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
            TableParameters = SecWeapTableSpec[0];

        }

        public async void RetrieveData()
        {

            CurrentList = await GetSecondaryCSVAsync();
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

                        insertCMD.Parameters.AddWithValue("@STR", model.BaseStats.STR);
                        insertCMD.Parameters.AddWithValue("@DEX", model.BaseStats.DEX);
                        insertCMD.Parameters.AddWithValue("@INT", model.BaseStats.INT);
                        insertCMD.Parameters.AddWithValue("@LUK", model.BaseStats.LUK);
                        insertCMD.Parameters.AddWithValue("@AllStat", model.BaseStats.AllStat);


                        insertCMD.Parameters.AddWithValue("@ATK", model.BaseStats.ATK);
                        insertCMD.Parameters.AddWithValue("@MATK", model.BaseStats.MATK);
                        insertCMD.Parameters.AddWithValue("@DEF", model.BaseStats.DEF);
                        insertCMD.Parameters.AddWithValue("@HP", model.BaseStats.MaxHP);
                        insertCMD.Parameters.AddWithValue("@MP", model.BaseStats.MaxMP);
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

        public static async Task<List<EquipCLS>> GetSecondaryCSVAsync()
        {
            List<EquipCLS> WeaponList = new List<EquipCLS>();

            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.EquipmentPath + "SecondaryWeapData.csv");

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
                        equip.ClassType = temp[1];
                        equip.WeaponType = temp[2];
                        equip.EquipName = temp[3];
                        equip.EquipLevel = Convert.ToInt32(temp[4]);
                        equip.BaseStats.STR =Convert.ToInt32(temp[5]);
                        equip.BaseStats.DEX =Convert.ToInt32(temp[6]);
                        equip.BaseStats.INT =Convert.ToInt32(temp[7]);
                        equip.BaseStats.LUK =Convert.ToInt32(temp[8]);
                        equip.BaseStats.AllStat = Convert.ToInt32(temp[9]);
                        equip.BaseStats.MaxHP = Convert.ToInt32(temp[10]);
                        equip.BaseStats.MaxMP = Convert.ToInt32(temp[11]);
                        equip.BaseStats.DEF = Convert.ToInt32(temp[12]);
                        equip.BaseStats.ATK = Convert.ToInt32(temp[13]);
                        equip.BaseStats.MATK = Convert.ToInt32(temp[14]);
                        equip.BaseStats.ATKSPD = Convert.ToInt32(temp[15]);


                        WeaponList.Add(equip);


                    }
                }
            }


            return WeaponList;


        }

       
    }
}
