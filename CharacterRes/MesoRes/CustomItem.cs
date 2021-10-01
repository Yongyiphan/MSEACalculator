using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.MesoRes
{
    class CustomItem
    {

        public string itemName { get; set; }

        public int quantity { get; set; }

        public int value { get; set; }


        public CustomItem() { }

        public CustomItem(string iName, int quantity, int value)
        {
            this.itemName = iName;
            this.quantity = quantity;
            this.value = value;

        }
    }
}
