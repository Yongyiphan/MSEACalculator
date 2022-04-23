using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes
{
    public class CheckTypes : IValidateTypes
    {
        public bool NotInt(string value)
        {
            if (int.TryParse(value, out int result) == false && value != String.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool NotNull(object item)
        {
            if(item != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
