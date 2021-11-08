using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.UnionRes
{
    public class UnionModel
    {

        public string Stat { get; set; }
        public string StatType { get; set; }
        public int RankB { get; set; }
        public int RankA { get; set; }
        public int RankS { get; set; }
        public int RankSS { get; set; }
        public int RankSSS { get; set; }

        public UnionModel()
        {

        }

        public UnionModel(string Stat, string ST, int B, int A, int S, int SS, int SSS)
        {
            this.Stat = Stat;
            this.StatType = ST;
            this.RankB = B;
            this.RankA = A;
            this.RankS = S;
            this.RankSS = SS;
            this.RankSSS = SSS;

        }

    }
}
