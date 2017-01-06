using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.Model
{
    public enum Status
    {
        InOperation,
        NotInOperation
    }

    public enum Operator
    {
        SBST,
        SMRT
    }

    public enum Load
    {
        SeatsAvailable,
        StandingAvailable,
        LimitedStanding
    }

    public enum Feature
    {
        WheelChairAccessible,
        None
    }
}
