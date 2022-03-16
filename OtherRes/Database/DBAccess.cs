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

        public static bool insertCharTrack(CharacterCLS character)
        {
            bool insertPassed;

            string insertQueryStr = "INSERT INTO TrackCharacter (CharName, UnionRank, Level, Starforce ) VALUES (@CN, @UR, @Lvl, @SF)";

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                try
                {
                    using (SqliteCommand insertCMD = new SqliteCommand(insertQueryStr, dbCon))
                    {
                        insertCMD.Parameters.AddWithValue("@CN", character.ClassName);
                        insertCMD.Parameters.AddWithValue("@UR", character.UnionRank);
                        insertCMD.Parameters.AddWithValue("@Lvl", character.Level);
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

        public static bool InsertCharWithEquip(CharacterCLS Character)
        {

            bool insertPassed = false;

            using (SqliteConnection con = new SqliteConnection(GVar.CONN_STRING))
            {
                con.Open();
                using (SqliteTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        //RECORD BASE CHARACTER
                        string insertTrackChar = "INSERT INTO TrackCharacter VALUES (@CharName, @UnionRank, @Level, @Starforce);";

                        using(SqliteCommand insertCMD = new SqliteCommand(insertTrackChar, con, trans))
                        {
                            insertCMD.Parameters.AddWithValue("@CharName", Character.ClassName);
                            insertCMD.Parameters.AddWithValue("@UnionRank", Character.UnionRank);
                            insertCMD.Parameters.AddWithValue("@Level", Character.Level);
                            insertCMD.Parameters.AddWithValue("@Starforce", Character.Starforce);
                            insertCMD.ExecuteNonQuery();    
                        }

                        //RECORD MAIN AND SEC WEAPONS
                        if(!string.IsNullOrEmpty(Character.CurrentMainWeapon) || !string.IsNullOrEmpty(Character.CurrentSecondaryWeapon))
                        {
                            string insertCurrentWeapon = "INSERT INTO TrackCharWeapons VALUES (@CharName, @Main, @Sec)";
                            using(SqliteCommand cmd = new SqliteCommand(insertCurrentWeapon, con, trans))
                            {
                                cmd.Parameters.AddWithValue("@CharName", Character.ClassName);
                                cmd.Parameters.AddWithValue("@Main", Character.CurrentMainWeapon);
                                cmd.Parameters.AddWithValue("@Sec", Character.CurrentSecondaryWeapon);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        //START RECODRING CHARACTER EQUIPMENT

                        if (Character.EquipmentList.Values.ToList().Count > 0)
                        {
                            string insertEquipTrack = "INSERT INTO TrackCharEquip VALUES (@CharName, @ClassType, @EquipSlot, @EquipSet);";

                            string insertEquipScoll = "INSERT INTO TrackCharEquipScroll VALUES  (@CharName, @ClassType, @EquipSlot, @EquipSet, @SlotCount, @ScrollPerc, @STR, @DEX, @INT, @LUK, @HP, @MP, @DEF, @ATK, @MATK, @SPD, @JUMP);";

                            string insertEquipFlame = "INSERT INTO TrackCharEquipFlame VALUES  (@CharName, @ClassType, @EquipSlot, @EquipSet, @STR, @DEX, @INT, @LUK, @HP, @MP, @DEF, @ATK, @MATK, @SPD, @JUMP, @ALLSTAT, @BD, @DMG);";

                            string insertEquipPot = "INSERT INTO TrackCharEquipPot VALUES (@CharName, @ClassType, @EquipSlot, @EquipSet, @MGrade, @M1, @M2, @M3, @AGrade, @A1, @A2, @A3);";

                            foreach (EquipCLS equip in Character.EquipmentList.Values)
                            {
                                SqliteCommand cmd = new SqliteCommand();

                                cmd.CommandText = insertEquipTrack;
                                cmd.Connection = con;
                                cmd.Transaction = trans;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@CharName", Character.ClassName);
                                cmd.Parameters.AddWithValue("@ClassType", equip.ClassType);
                                cmd.Parameters.AddWithValue("@EquipSlot", equip.EquipListSlot);
                                if (equip.EquipListSlot == "Secondary" || GVar.AccEquips.Contains(equip.EquipSlot))
                                {
                                    cmd.Parameters.AddWithValue("@EquipSet", equip.EquipName);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@EquipSet", equip.EquipSet);
                                }
                                cmd.ExecuteNonQuery();

                                cmd.CommandText = insertEquipScoll;
                                cmd.Parameters.AddWithValue("@SlotCount", equip.SlotCount);
                                cmd.Parameters.AddWithValue("@ScrollPerc", equip.SpellTracePerc);
                                cmd.Parameters.AddWithValue("@STR", equip.ScrollStats.STR);
                                cmd.Parameters.AddWithValue("@DEX", equip.ScrollStats.DEX);
                                cmd.Parameters.AddWithValue("@INT", equip.ScrollStats.INT);
                                cmd.Parameters.AddWithValue("@LUK", equip.ScrollStats.LUK);
                                cmd.Parameters.AddWithValue("@HP", equip.ScrollStats.MaxHP);
                                cmd.Parameters.AddWithValue("@MP", equip.ScrollStats.MaxMP);
                                cmd.Parameters.AddWithValue("@DEF", equip.ScrollStats.DEF);
                                cmd.Parameters.AddWithValue("@ATK", equip.ScrollStats.ATK);
                                cmd.Parameters.AddWithValue("@MATK", equip.ScrollStats.MATK);
                                cmd.Parameters.AddWithValue("@SPD", equip.ScrollStats.SPD);
                                cmd.Parameters.AddWithValue("@JUMP", equip.ScrollStats.JUMP);

                                cmd.ExecuteNonQuery();

                                cmd.CommandText = insertEquipFlame;
                                cmd.Parameters["@STR"].Value = equip.FlameStats.STR;
                                cmd.Parameters["@DEX"].Value = equip.FlameStats.DEX;
                                cmd.Parameters["@INT"].Value= equip.FlameStats.INT;
                                cmd.Parameters["@LUK"].Value = equip.FlameStats.LUK;
                                cmd.Parameters["@HP"].Value = equip.FlameStats.LUK;
                                cmd.Parameters["@MP"].Value = equip.FlameStats.MaxHP;
                                cmd.Parameters["@DEF"].Value = equip.FlameStats.MaxMP;
                                cmd.Parameters["@ATK"].Value = equip.FlameStats.ATK;
                                cmd.Parameters["@MATK"].Value = equip.FlameStats.MATK;
                                cmd.Parameters["@SPD"].Value= equip.FlameStats.SPD;
                                cmd.Parameters["@JUMP"].Value= equip.FlameStats.JUMP;
                                cmd.Parameters.AddWithValue("@ALLSTAT", equip.FlameStats.AllStat);
                                cmd.Parameters.AddWithValue("@BD", equip.FlameStats.BD);
                                cmd.Parameters.AddWithValue("@DMG", equip.FlameStats.DMG);

                                cmd.ExecuteNonQuery();


                                cmd.CommandText = insertEquipPot;
                                cmd.Parameters.AddWithValue("@MGrade", equip.MPotGrade);
                                cmd.Parameters.AddWithValue("@M1", equip.MainPot["First"].PotID);
                                cmd.Parameters.AddWithValue("@M2", equip.MainPot["Second"].PotID);
                                cmd.Parameters.AddWithValue("@M3", equip.MainPot["Third"].PotID);
                                cmd.Parameters.AddWithValue("@AGrade", equip.APotGrade);
                                cmd.Parameters.AddWithValue("@A1", equip.AddPot["First"].PotID);
                                cmd.Parameters.AddWithValue("@A2", equip.AddPot["Second"].PotID);
                                cmd.Parameters.AddWithValue("@A3", equip.AddPot["Third"].PotID);

                                cmd.ExecuteNonQuery();

                                cmd.Dispose();

                            }


                        } 
                        trans.Commit();

                        insertPassed = true;
                    }
                    catch (SqliteException)
                    {

                        trans.Rollback();
                        insertPassed=false;
                    }
                }
            }



            return insertPassed;
        }


        public static List<BossCLS> getCharBossList(string character)
        {


            List<BossCLS> bossList = new List<BossCLS>();

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

                            BossCLS tempBoss = new BossCLS();

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

        public static bool insertCharTEquip(EquipCLS selectedEquip)
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

        public static bool deleteCharT(CharacterCLS character)
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
