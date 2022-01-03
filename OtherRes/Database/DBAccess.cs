using Microsoft.Data.Sqlite;
using MSEACalculator.BossRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.CharacterRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database
{
    public class DBAccess
    {

        public static bool insertCharTrack(Character character)
        {
            bool insertPassed;

            string insertQueryStr = "INSERT INTO CharacterTrack (CharName, UnionRank, Level, Starforce ) VALUES (@CN, @UR, @Lvl, @SF)";

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                try
                {
                    using (SqliteCommand insertCMD = new SqliteCommand(insertQueryStr, dbCon))
                    {
                        insertCMD.Parameters.AddWithValue("@CN", character.ClassName);
                        insertCMD.Parameters.AddWithValue("@UR", character.unionRank);
                        insertCMD.Parameters.AddWithValue("@Lvl", character.level);
                        insertCMD.Parameters.AddWithValue("@SF", character.Starforce);

                        insertCMD.ExecuteNonQuery();
                    }

                    insertPassed = true;
                    return insertPassed;
                }
                catch (SqliteException)
                {
                    return insertPassed = false;
                }
            }
        }

        public static List<Boss> getCharBossList(string character)
        {


            List<Boss> bossList = new List<Boss>();

            string getMesoTrackQuery = @"SELECT " +
                "BossList.BossID," +
                "BossList.BossName, " +
                "BossList.Difficulty," +
                "BossList.EntryType," +
                "BossList.EntryLimit," +
                "BossList.BossCrystal, " +
                "BossList.Meso " +
                "FROM BossList " +
                "INNER JOIN BossMesoGains ON BossMesoGains.BossID = BossList.BossID " +
                "WHERE BossMesoGains.charName = @charName";


            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();
                using (SqliteCommand selectCMD = new SqliteCommand())
                {
                    selectCMD.CommandText = getMesoTrackQuery;
                    selectCMD.Connection = dbCon;
                    selectCMD.Parameters.AddWithValue("@charName", character);

                    using (SqliteDataReader result = selectCMD.ExecuteReader())
                    {
                        while (result.Read())
                        {

                            Boss tempBoss = new Boss();

                            tempBoss.BossID = result.GetInt16(0);
                            tempBoss.BossName = result.GetString(1);
                            tempBoss.Difficulty = result.GetString(2);
                            tempBoss.EntryType = result.GetString(3);
                            tempBoss.Meso = result.GetInt32(6);

                            bossList.Add(tempBoss);
                        }
                    }

                }
            }

            return bossList;
        }

        public static bool insertCharTBossList(string charName, string bossName, int bossID)
        {
            bool insertPassed = false;

            string insertQueryStr = "INSERT INTO BossMesoGains (charName, BossName, BossID) VALUES (@charName, @bossName, @bossID)";

            using (SqliteConnection dbCon = new SqliteConnection($"Filename={GVar.databasePath}"))
            {
                dbCon.Open();
                try
                {
                    using (SqliteCommand insertCMD = new SqliteCommand(insertQueryStr, dbCon))
                    {
                        insertCMD.Parameters.AddWithValue("@charName", charName);
                        insertCMD.Parameters.AddWithValue("@bossName", bossName);
                        insertCMD.Parameters.AddWithValue("@bossID", bossID);

                        insertCMD.ExecuteNonQuery();
                    }


                    insertPassed = true;
                    return insertPassed;
                }
                catch (SqliteException)
                {
                    insertPassed = false;
                    return insertPassed;
                }
            }

        }

        public static bool insertCharTEquip(EquipModel selectedEquip)
        {
            bool insertPassed = false;

            using (SqliteConnection dbCon = new SqliteConnection(GVar.CONN_STRING))
            {
                dbCon.Open();
                using (var transaction = dbCon.BeginTransaction())
                {
                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        insertPassed = false;
                    }
                }
            }

            return insertPassed;
        }

        /// <summary>
        /// Deletion from Database
        /// </summary>
        public static bool deleteCharBossList(string charName, int bossID)
        {
            bool deletePassed = false;
            string deleteQuery = "DELETE FROM BossMesoGains WHERE charName =  @CN AND BossID = @BID";

            using (SqliteConnection dbCon = new SqliteConnection($"Filename ={GVar.databasePath}"))
            {
                dbCon.Open();
                try
                {
                    using (SqliteCommand deleteCMD = new SqliteCommand(deleteQuery, dbCon))
                    {
                        deleteCMD.Parameters.AddWithValue("@CN", charName);
                        deleteCMD.Parameters.AddWithValue("@BID", bossID);

                        deleteCMD.ExecuteNonQuery();
                    }

                    deletePassed = true;
                    return deletePassed;
                }
                catch (SqliteException)
                {
                    deletePassed = false;
                    return deletePassed;
                }
            }

        }

        public static bool deleteCharT(Character character)
        {
            bool deletePassed = false;
            using (SqliteConnection dbCon = new SqliteConnection(GVar.CONN_STRING))
            {

                dbCon.Open();
                string deleteQuery = "DELETE FROM CharacterTrack WHERE CharName = @CN";
                using (var transaction = dbCon.BeginTransaction())
                {
                    try
                    {
                        using (SqliteCommand deleteCMD = new SqliteCommand(deleteQuery, dbCon, transaction))
                        {
                            deleteCMD.Parameters.AddWithValue("@CN", character.ClassName);
                            deleteCMD.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        return deletePassed = true;
                    }
                    catch (SqliteException e)
                    {
                        Console.WriteLine(e.Message);
                        transaction.Rollback();
                        deletePassed = false;
                    }
                }
            }

            return deletePassed;
        }


    }
}
