using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.Model
{
    public class LtaData
    {
        public virtual string ServiceUrl => $"{GetType().Name}s";
    }
}
