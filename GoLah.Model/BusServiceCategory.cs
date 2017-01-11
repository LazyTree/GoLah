using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        NightService,
        NightRider,
        TwoTierFlatFee,
        IntraTown,
        FlatFee_1_00,
        FlatFee_1_80,
        FlatFee_2_00,
        FlatFee_2_70,
        FlatFee_3_50,
        FlatFee_4_00,
        FlatFee_4_20,
        FlatFee_4_50,
        FlatFee_5_00
    }
}
