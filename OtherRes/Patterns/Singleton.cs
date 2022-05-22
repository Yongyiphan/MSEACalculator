using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Patterns
{
    public sealed class Singleton
    {
        public static Singleton Instance { get; private set; }

        private GVarFlex _GFlex;

        private Singleton()
        {
            _GFlex = new GVarFlex();
        }
        public GVarFlex GFlex { get { return _GFlex; } }

        static Singleton() { Instance = new Singleton(); }
    }
}
