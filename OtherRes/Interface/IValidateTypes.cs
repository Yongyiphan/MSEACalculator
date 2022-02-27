using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Interface
{
    public interface IValidateTypes
    {

        bool NotInt(string value);
        bool NotNull(object item);
    }
}
