using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
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
        [EnumMember(Value = "NIGHT SERVICE")]
        NightService,
        [EnumMember(Value = "NIGHT RIDER")]
        NightRider,
        TwoTierFlatFee,
        [EnumMember(Value = "INTRA-TOWN")]
        IntraTown,
        FlatFee
    }
}
