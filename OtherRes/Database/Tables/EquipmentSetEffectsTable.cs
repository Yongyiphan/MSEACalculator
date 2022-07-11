using Microsoft.Data.Sqlite;
using MSEACalculator.CalculationRes;
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
    public class EquipmentSetEffectsTable : BaseDBTable, ITableUpload
    {
        List<SetEffectCLS> CurrentList = new List<SetEffectCLS>();
        public EquipmentSetEffectsTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
                   }

        public async void RetrieveData()
        {
            (List<SetEffectCLS>, string) Result;

            switch (TableName)
            {
                case "SetEffectAt":
                    Result = await GetSetEffectCSVAsync("EquipSetData.csv", "PRIMARY KEY (EquipSet, ClassType, SetAt)");
                    CurrentList = Result.Item1;
                    TableParameters = Result.Item2;
                    break;
                case "SetEffectCul":
                    Result = await GetSetEffectCSVAsync("EquipSetCulData.csv", "PRIMARY KEY (EquipSet, ClassType, SetAt)");
                    CurrentList = Result.Item1;
                    TableParameters = Result.Item2;
                    break;
            }

        }

        private static async Task<(List<SetEffectCLS>, string)> GetSetEffectCSVAsync(string FileName, string TableKey)
        {
            List<SetEffectCLS> CurrentList = new List<SetEffectCLS>();
            List<string> result = await ComFunc.CSVStringAsync(GVar.EquipmentPath, FileName);
            string tableSpec = "";

            int counter = 0;
            foreach (string setAt in result)
            {
                if (setAt == "")
                {
                    return (CurrentList, tableSpec);
                }
                if (counter == 0)
                {
                    tableSpec = ComFunc.TableSpecStringBuilder(TableColNames.SetEffectCN, setAt, TableKey);
                    counter += 1;
                    continue;
                }

                //Critical Damage,Damage,All Skills, Damage Against Normal Monsters,Abnormal Status Resistance

                var temp = setAt.Split(",");
                counter += 1;
                SetEffectCLS equip = new SetEffectCLS();
                equip.EquipSet = temp[1];
                equip.ClassType = temp[2];
                equip.SetAt = Convert.ToInt32(temp[3]);

                equip.SetEffect.STR = Convert.ToInt32(temp[4]);
                equip.SetEffect.DEX = Convert.ToInt32(temp[5]);
                equip.SetEffect.INT = Convert.ToInt32(temp[6]);
                equip.SetEffect.LUK = Convert.ToInt32(temp[7]);
                equip.SetEffect.AllStat = Convert.ToInt32(temp[8]);
                equip.SetEffect.MaxHP = Convert.ToInt32(temp[9]);
                equip.SetEffect.MaxMP = Convert.ToInt32(temp[10]);
                equip.SetEffect.HP = temp[11];
                equip.SetEffect.MP = temp[12];
                equip.SetEffect.DEF = Convert.ToInt32(temp[13]);
                equip.SetEffect.ATK = Convert.ToInt32(temp[14]);
                equip.SetEffect.MATK = Convert.ToInt32(temp[15]);
                equip.SetEffect.IED = Convert.ToInt32(temp[16]);
                equip.SetEffect.BD = Convert.ToInt32(temp[17]);
                equip.SetEffect.CDMG = Convert.ToInt32(temp[18]);
                equip.SetEffect.DMG = Convert.ToInt32(temp[19]);
                equip.SetEffect.AllSkills = temp[20].Contains(" ") ? Convert.ToInt32(temp[20].Split(' ').First()) : Convert.ToInt32(temp[20]);
                equip.SetEffect.NDMG = Convert.ToInt32(temp[21]);
                equip.SetEffect.StatusRes = Convert.ToInt32(temp[22]);


                CurrentList.Add(equip);

            }
            return (CurrentList, tableSpec);

        }
        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {

            string insertEquip = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
            using (SqliteCommand insertCMD = new SqliteCommand(insertEquip, connection, transaction))
            {
                foreach (SetEffectCLS equipItem in CurrentList)
                {
                    ErrorCounter++;
                    insertCMD.Parameters.Clear();
                    insertCMD.Parameters.AddWithValue("@ClassType", equipItem.ClassType);
                    insertCMD.Parameters.AddWithValue("@EquipSet", equipItem.EquipSet);
                    insertCMD.Parameters.AddWithValue("@SetAt", equipItem.SetAt);


                    insertCMD.Parameters.AddWithValue("@STR", equipItem.SetEffect.STR);
                    insertCMD.Parameters.AddWithValue("@DEX", equipItem.SetEffect.DEX);
                    insertCMD.Parameters.AddWithValue("@INT", equipItem.SetEffect.INT);
                    insertCMD.Parameters.AddWithValue("@LUK", equipItem.SetEffect.LUK);
                    insertCMD.Parameters.AddWithValue("@AllStats", equipItem.SetEffect.AllStat);
                    insertCMD.Parameters.AddWithValue("@MaxHP", equipItem.SetEffect.MaxHP);
                    insertCMD.Parameters.AddWithValue("@MaxMP", equipItem.SetEffect.MaxMP);
                    insertCMD.Parameters.AddWithValue("@PercMaxHP", equipItem.SetEffect.DEF);
                    insertCMD.Parameters.AddWithValue("@PercMaxMP", equipItem.SetEffect.DEF);
                    insertCMD.Parameters.AddWithValue("@DEF", equipItem.SetEffect.DEF);
                    insertCMD.Parameters.AddWithValue("@WATK", equipItem.SetEffect.ATK);
                    insertCMD.Parameters.AddWithValue("@MATK", equipItem.SetEffect.MATK);
                    insertCMD.Parameters.AddWithValue("@BD", equipItem.SetEffect.BD);
                    insertCMD.Parameters.AddWithValue("@IED", equipItem.SetEffect.IED);
                    insertCMD.Parameters.AddWithValue("@CDMG", equipItem.SetEffect.CDMG);
                    insertCMD.Parameters.AddWithValue("@DMG", equipItem.SetEffect.DMG);
                    insertCMD.Parameters.AddWithValue("@NDMG", equipItem.SetEffect.NDMG);
                    insertCMD.Parameters.AddWithValue("@AllSkills", equipItem.SetEffect.AllSkills);
                    insertCMD.Parameters.AddWithValue("@StatusResist", equipItem.SetEffect.StatusRes);

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
