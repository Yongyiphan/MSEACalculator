﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.UnionRes
{
    public class UnionCLS
    {

        public string Effect { get; set; }
        public string EffectType { get; set; }
        public int RankB { get; set; }
        public int RankA { get; set; }
        public int RankS { get; set; }
        public int RankSS { get; set; }
        public int RankSSS { get; set; }

        public UnionCLS()
        {

        }

        public UnionCLS(string Stat, string ST, int B, int A, int S, int SS, int SSS)
        {
            this.Effect = Stat;
            this.EffectType = ST;
            this.RankB = B;
            this.RankA = A;
            this.RankS = S;
            this.RankSS = SS;
            this.RankSSS = SSS;

        }

    }
}
