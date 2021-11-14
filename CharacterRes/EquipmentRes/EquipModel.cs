using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class EquipModel
    {
        public string EquipSet { get; set; }
        public string JobType { get; set; }
        public string EquipSlot { get; set; }

        //BASE STATS
        public int MS { get; set; }
        public int SS { get; set; }
        public int DEF { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int SPD { get; set; }
        public int JUMP { get; set; }
        public int ATK { get; set; }
        public int MATK { get; set; }

        //ADDITIONAL BASE STAT FOR WEAPONS
        public int IED { get; set; }
        public int BD { get; set; }
        public int ATKSPD { get; set; }
        public int DMG { get; set; }

        //FLAME STATS
        public int AllStat { get; set; }
        public int STR { get; set; }
        public int DEX { get; set; }
        public int LUK { get; set; }
        public int INT { get; set; }

        //SCROLLING
        public int Slots { get; set; }
        public string scroll { get; set; }
        public int lvl { get; set; }

        public EquipModel() { }



        //FOR INIT FROM DATABASE
        public EquipModel(string equipset, string job, string equipslot, int ms, int ss, int hp, int mp, int atk, int matk, int def, int spd, int jump, int ied)
        {
            this.EquipSet = equipset;
            this.JobType = job;
            this.EquipSlot = equipslot;
            this.MS = ms;
            this.SS = ss;
            this.HP = hp;
            this.MP = mp;
            this.ATK = atk;
            this.MATK = matk;
            this.DEF = def;
            this.SPD = spd;
            this.JUMP = jump;
            this.IED = ied;
        }

        //RETRIEVE FROM DATABASE
        public EquipModel(string equipslot, int ms, int ss, int hp, int mp, int atk, int matk, int def, int spd, int jump, int ied)
        {
            this.EquipSlot = equipslot;
            this.MS = ms;
            this.SS = ss;
            this.HP = hp;
            this.MP = mp;
            this.ATK = atk;
            this.MATK = matk;
            this.DEF = def;
            this.SPD = spd;
            this.JUMP = jump;
            this.IED = ied;
        }


        //FOR FLAME TRACK ARMOR
        public EquipModel(string equipSet, string equipSlot,
            int str = 0, int dex = 0, int luk = 0, int INT = 0, int AS = 0,
            int hp = 0, int mp = 0, int def = 0, int spd = 0, int j = 0,
            int atk = 0, int bd = 0, int ied = 0, int dmg = 0)
        {
            this.EquipSet = equipSet;
            this.EquipSlot = equipSlot;
            this.STR = str;
            this.DEX = dex;
            this.LUK = luk;
            this.INT = INT;
            this.AllStat = AS;
            this.HP = hp;
            this.MP = mp;
            this.DEF = def;
            this.SPD = spd;
            this.JUMP = j;
            this.ATK = atk;
            this.BD = bd;
            this.IED = ied;
            this.DMG = dmg;
        }

        
    
    }
}
