using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoLah.Model;
using GoLah.Services;

namespace GoLah.ViewModel
{
    public class BusStopsPageViewModel : ViewModelBase
    {
        public BusStopsPageViewModel()
        {
            GetBusStopsAsync();
        }

        private async void GetBusStopsAsync()
        {
            await new LtaDataRepository<BusStop>().QueryAsync();
        }
    }
}
