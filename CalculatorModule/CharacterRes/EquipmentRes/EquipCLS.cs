using MSEACalculator.CalculationRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class EquipCLS
    {
        //For Oz/ Event Rings
        public string Category { get; set; }
        public int Rank { get; set; } = 0;

        public string EquipName { get; set; }
        public string EquipSet { get; set; }
        public string ClassType { get; set; }
        public string WeaponType { get; set; }
        public string MainStat { get; set; }

        //BASE SLOT TYPE
        public string EquipSlot { get; set; }

        //SLOT POSITION i.e RING 1, RING 2
        public string EquipListSlot { get; set; }
        public int EquipLevel { get; set; }
        public string StatType { get; set; } //PERC OR FLAT 
        public int SlotCount { get; set; } = 0;


        public EquipStatsCLS BaseStats { get; set; } = new EquipStatsCLS();

        public bool IsSpellTraced { get; set; } = false;
        public int SpellTracePerc { get; set; }

        public EquipStatsCLS ScrollStats { get; set; } = new EquipStatsCLS();


        public EquipStatsCLS FlameStats { get; set; } = new EquipStatsCLS();

        public int MPotGrade { get; set; } = 0;
        public Dictionary<string, PotentialStatsCLS> MainPot { get; set; } = new Dictionary<string, PotentialStatsCLS>
        {
            {"First", new PotentialStatsCLS() },
            {"Second", new PotentialStatsCLS() },
            {"Third", new PotentialStatsCLS() },
        };

        public int APotGrade { get; set; } = 0;
        public Dictionary<string, PotentialStatsCLS> AddPot { get; set; } = new Dictionary<string, PotentialStatsCLS>
        {
            {"First", new PotentialStatsCLS() },
            {"Second", new PotentialStatsCLS() },
            {"Third", new PotentialStatsCLS() },
        };


        public int StarForce { get; set; } = 0;
        public EquipStatsCLS StarforceStats { get; set; } = new EquipStatsCLS();

        //DEFAULT CONSTRUCTOR
        public EquipCLS() { }

        public override bool Equals(object obj)
        {
            List<bool> CheckCondition = new List<bool>();
            if (obj == null)
            {
                return false;
            }
            else
            {
                if (obj is EquipCLS)
                {
                    EquipCLS cObj = (EquipCLS)obj;
                    List<string> ManualCheckList = new List<string>() { "BaseStats", "ScrollStats", "FlameStats" };
                    foreach (PropertyInfo prop in GetType().GetProperties())
                    {
                        if (ManualCheckList.Contains(prop.Name))
                        {
                            continue;
                        }
                        var cValue = prop.GetValue(this);
                        var nValue = cObj.GetType().GetProperty(prop.Name).GetValue(obj);
                        if(cValue == null || nValue == null)
                        {

                            CheckCondition.Add(nValue == cValue ? true : false);
                        }
                        else
                        {
                            CheckCondition.Add(nValue.Equals(cValue) ? true : false);
                        }

                    }

                    CheckCondition.Add(ScrollStats.Equals(cObj.ScrollStats) ? true : false);
                    CheckCondition.Add(FlameStats.Equals(cObj.FlameStats) ? true : false);

                    //test.Add(new HashSet<PotentialStats>(MainPot).Equals(new HashSet<PotentialStats>(((EquipModel)obj).MainPot)) ? "true" : "false");
                    CheckCondition.Add(MainPot.Values.ToList().Select(x => x.PotID).ToList().SequenceEqual(cObj.MainPot.Values.ToList().Select(x => x.PotID).ToList()) ? true : false);
                    CheckCondition.Add(AddPot.Values.ToList().Select(x => x.PotID).ToList().SequenceEqual(cObj.AddPot.Values.ToList().Select(x => x.PotID).ToList()) ? true : false);


                }
            }

            return CheckCondition.Contains(false) ? false : true;
        }

        public EquipCLS ShallowCopy()
        {
            return (EquipCLS)this.MemberwiseClone();
        }

        public EquipCLS DeepCopy()
        {
            EquipCLS copy = (EquipCLS) MemberwiseClone();
            copy.BaseStats = BaseStats;
            copy.ScrollStats = ScrollStats;
            copy.FlameStats = FlameStats;
            copy.MainPot = MainPot;
            copy.AddPot = AddPot;
            copy.StarforceStats = StarforceStats;

            return copy;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public void InitBaseEquipStat()
        {
            //To update equip with proper values as per class
            //EquipModel equipModel = new EquipModel();

            //Update from Main/Sec Stats to Stat Values STR|DEX|...
            //Keep MS/SS property
            int AS = BaseStats.AllStat;
            if (AS > 0)
            {
                BaseStats.STR = AS;
                BaseStats.DEX = AS;
                BaseStats.INT = AS;
                BaseStats.LUK = AS;
                BaseStats.AllStat = 0;
            }
        }
       

        public void UpdateFromDB(EquipCLS UpdatedEquip)
        {
            EquipListSlot =  UpdatedEquip?.EquipListSlot;
            StarForce = UpdatedEquip.StarForce;
            if (UpdatedEquip.SpellTracePerc > 0)
            {
                IsSpellTraced = true;
            }
            SpellTracePerc = UpdatedEquip.SpellTracePerc;
            SlotCount = UpdatedEquip.SlotCount;
            ScrollStats = UpdatedEquip.ScrollStats;
            FlameStats = UpdatedEquip.FlameStats;
            MPotGrade = UpdatedEquip.MPotGrade;
            MainPot = UpdatedEquip.MainPot;
            APotGrade = UpdatedEquip.APotGrade;
            AddPot = UpdatedEquip.AddPot;
        }
     }
}
