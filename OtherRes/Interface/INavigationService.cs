using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Interface
{
    public interface INavigationService
    {
        void Navigate(Type type, INPCObject ViewModels);
    }
}
