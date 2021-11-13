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

        //FLAME STATS
        public int FAllStat { get; set; }
        public int FSTR { get; set; }
        public int FDEX { get; set; }
        public int FLUK { get; set; }
        public int FINT { get; set; }
        public int FDEF { get; set; }
        public int FHP { get; set; }
        public int FMP { get; set; }
        public int FSPD { get; set; }
        public int FJUMP { get; set; }


        public int FATK { get; set; }
        public int FIED { get; set; }
        public int FBD { get; set; }

        //SCROLLING
        public int Slots { get; set; }
        public string scroll { get; set; }

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
    }
}
