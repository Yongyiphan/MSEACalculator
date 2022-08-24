using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CalculationRes
{
    public class StarforceCLS
    {
        public int SFLevel { get; set; } = 0;
        public int MinLvl { get; set; } = 0;
        public int MaxLvl { get; set; } = 300;



        public int JobStat { get; set; } = 0; //MAIN + SEC STAT
        public int NonWeapVDef { get; set; } = 0; //NON WEAPON VISIBLE DEF
        public int OverallVDef { get; set; } = 0; //(OVERALL**) VISIBE DEF
        public int CatAMaxHP { get; set; } = 0; //CAT A (GVAR) MAX HP
        public int WeapMaxMP { get; set; } = 0; //WEAPON MAX MP

       
        
        //WEAPON VISIBLE ATK / MATK
        public int WeapVATK { get; set; } = 0;
        public int WeapVMATK { get; set; } = 0;


        //SHOE
        public int SJump { get; set; } = 0;
        public int SSpeed { get; set; } = 0;


        //GLOVES
        public int GloveVATK { get; set; } = 0;
        public int GloveVMATK { get; set; } = 0;

        //VISIBLE STAT
        public int VStat { get; set; } = 0; 
        public int VDef { get; set; } = 0;
        public int VATK { get; set; } = 0;

        //NON WEAPON VISIBLE ATK / MATK
        public int NonWeapATK { get; set; } = 0;
        public int NonWeapMATK { get; set; } = 0;



        //DEPENDS ON LEVEL RANK
        public int LevelRank { get; set; } = 0;
        public Dictionary<(int, int), int> VDefL { get; set; } = new Dictionary<(int, int), int>();
        public Dictionary<(int, int), int> VStatL { get; set; } = new Dictionary<(int, int), int>();
        public Dictionary<(int,int), int> NonWeapVATKL { get; set; } = new Dictionary<(int, int), int>();
        public Dictionary<(int,int), int> NonWeapVMATKL { get; set; } = new Dictionary<(int, int), int>();
        public Dictionary<(int,int), int> WeapVATKL { get; set; } = new Dictionary<(int, int), int>();
        public Dictionary<(int,int), int> WeapVMATKL { get; set; } = new Dictionary<(int, int), int>();


      
        //public int MainStat { get; set; } = 0;
        //public List<int> MainStatL { get; set; } = new List<int>();
        //public int NonWeapDef { get; set; } = 0;
        //public int OverallDef { get; set; } = 0;
        //public int MaxHP { get; set; } = 0;
        //public int MaxMP { get; set; } = 0;
        //public int Speed { get; set; } = 0;
        //public int Jump { get; set; } = 0;
        //public int GloveAtk { get; set; } = 0;
        //public int NonWATK { get; set; } = 0;
        //public List<int> NonWATKL { get; set; } = new List<int>();
        //public int WATK { get; set; } = 0;
        //public List<int> WATKL { get; set; } = new List<int>();



        public StarforceCLS() { }

    }
}
