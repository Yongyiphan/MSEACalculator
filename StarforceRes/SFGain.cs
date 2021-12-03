using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.StarforceRes
{
    public class SFGain
    {
        public int StarForceLevel { get; set; }
        public int MainStat { get; set; }
        public List<int> MainStatL { get; set; }
        public int NonWeapDef { get; set; }
        public int OverallDef { get; set; }
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }
        public int Speed { get; set; }
        public int Jump { get; set; }
        public int GloveAtk { get; set; }
        public int NonWATK { get; set; }
        public List<int> NonWATKL { get; set; }
        public int WATK { get; set; }
        public List<int> WATKL { get; set; }



        public SFGain() { }
        public SFGain(int SF, int MStat, int NWDEF, int ODef, int MHP, int MMP, int Spd, int Jump, int GloveAtk, int NWATK, int WAtk = 0)
        {
            this.StarForceLevel = SF;
            this.MainStat = MStat;
            this.NonWeapDef = NWDEF;
            this.OverallDef = ODef;
            this.MaxHP = MHP;
            this.MaxMP = MMP;
            this.Speed = Spd;
            this.Jump = Jump;
            this.GloveAtk = GloveAtk;
            this.NonWATK = NWATK;
            this.WATK = WAtk;
        }
        public SFGain(int SF, List<int> MSL, int NWDEF, int ODef, int MHP, int MMP, int Spd, int Jump, int GloveAtk, List<int> NWATKL, List<int> WAtkL)
        {
            this.StarForceLevel = SF;
            this.MainStatL = MSL;
            this.NonWeapDef = NWDEF;
            this.OverallDef = ODef;
            this.MaxHP = MHP;
            this.MaxMP = MMP;
            this.Speed = Spd;
            this.Jump = Jump;
            this.GloveAtk = GloveAtk;
            this.NonWATKL = NWATKL;
            this.WATKL = WAtkL;

        }

    }
}
