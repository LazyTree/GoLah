using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.Model
{
    /// <summary>
    /// Category of the SBS bus service.
    /// </summary>
    public enum BusServiceCategory
    {
        Express,
        Feeder,
        Industrial,
        TownLink,
        Trunk,
        TwoTierFlatFee,
        FlatFee_1_10,
        FlatFee_1_90,
        FlatFee_3_50,
        FlatFee_3_80
    }
}
