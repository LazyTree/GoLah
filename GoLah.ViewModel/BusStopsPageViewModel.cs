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
        private ObservableCollection<BusStop> _allBusStops;
        private BusStop _selectedBusStop;
        private ObservableCollection<ArrivalBusService> _arrivalBusServices = new ObservableCollection<ArrivalBusService>();

        public BusStopsPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                _allBusStops = new ObservableCollection<BusStop>
                {
                    new BusStop
                    {
                        Code = "11111",
                        Description = "BusStop1"
                    },
                    new BusStop
                    {
                        Code = "22222",
                        Description = "BusStop2"
                    },
                    new BusStop
                    {
                        Code = "33333",
                        Description = "BusStop3"
                    },
                };
            }
            else
            {
                GetBusStopsAsync();
            }
        }

        public ObservableCollection<BusStop> AllBusStops
        {
            get
            {
                return _allBusStops;
            }
            set
            {
                Set(ref _allBusStops, value);
            }
        }

        public BusStop SelectedBusStop
        {
            get { return _selectedBusStop; }
            set
            {
                GetNextBusAsync(value.Code);
                Set(ref _selectedBusStop, value);
            }
        }
        public ObservableCollection<ArrivalBusService> ArrivalBusServices
        {
            get
            {
                return _arrivalBusServices;
            }
            set
            {
                Set(ref _arrivalBusServices, value);
            }
        }


        private async void GetNextBusAsync(string code)
        {
            var repo = new LtaDataRepositoryBase<ArrivalBusService>();
            ArrivalBusServices = new ObservableCollection<ArrivalBusService>(await repo.QueryAsync(true, code));
        }

        private async void GetBusStopsAsync()
        {
            AllBusStops = new ObservableCollection<BusStop>(await new LtaDataRepository<BusStop>().QueryAsync());
            SelectedBusStop = AllBusStops.FirstOrDefault();
        }
    }
}
