using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnomalyDeviceLib
{
    public interface IValueContainer
    {
        float Value { get; set; }
        float Deviation { get; set; }
        void Pingback(double value);
    }
}
