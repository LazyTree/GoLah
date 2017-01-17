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
    public class BusServicesPageViewModel : ViewModelBase
    {
        private ObservableCollection<BusService> _allBusServices = new ObservableCollection<BusService>();
        private BusService _selectedBusService;
        private bool _isBusServiceInfoReady = false;

        public BusServicesPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                _allBusServices = new ObservableCollection<BusService>
                {
                    new BusService
                    {
                        ServiceNo = "112e"
                    },

                    new BusService
                    {
                        ServiceNo = "857"
                    },

                    new BusService
                    {
                        ServiceNo = "858"
                    }
                };
            }
            else
            {
                LoadData();
            }
        }

        public bool IsBusServiceInfoReady
        {
            get
            {
                return _isBusServiceInfoReady;
            }
            set
            {
                Set(ref _isBusServiceInfoReady, value);
            }
        }

        public ObservableCollection<BusService> AllBusServices
        {
            get
            {
                return _allBusServices;
            }
            set
            {
                Set(ref _allBusServices, value);
            }
        }

        public BusService SelectedBusService
        {
            get { return _selectedBusService; }
            set
            {
                Set(ref _selectedBusService, value);
            }
        }

        private void LoadData()
        {
            IsBusServiceInfoReady = false;

            var repository = new LtaDataRepository();

            AllBusServices = new ObservableCollection<BusService>(MergeBusRoutesToBusService(repository.CachedRoutes));

            IsBusServiceInfoReady = true;

            SelectedBusService = AllBusServices.First();
        }


        /// <summary>
        /// Massage the bus routes to bus service.
        /// </summary>
        /// <param name="_cachedBusRoutes"></param>
        /// <returns></returns>
        private IEnumerable<BusService> MergeBusRoutesToBusService(List<BusRoute> busRoutes)
        {
            var repository = new LtaDataRepository();

            var allStops = repository.CachedBusStops;

            var groups = busRoutes.GroupBy(x => x.ServiceNo);

            Random rnd = new Random();

            List<BusService> services = new List<BusService>();
            foreach (var group in groups)
            {
                services.Add( new BusService
                {
                    ServiceNo = group.Key,
                    Operator = group.First().Operator,
                    LoopDescription = group.First().LoopDescription,
                    Directions = group.Select(r => new BusDirection
                    {
                        Direction = r.Direction,
                        Origin = allStops.Where(s => s.Code == r.OriginCode).FirstOrDefault()?.Description,
                        Destination = allStops.Where(s => s.Code == r.DestinationCode).FirstOrDefault()?.Description,
                        //Stops = r.BusStopCodes == null? new List<BusStop>() : allStops.Where(s => r.BusStopCodes.Contains(s.Code)).ToList(),
                        Stops = new List<BusStop>
                        {
                            allStops[rnd.Next(0, allStops.Count() - 1)],
                            allStops[rnd.Next(0, allStops.Count() - 1)],
                            allStops[rnd.Next(0, allStops.Count() - 1)],
                            allStops[rnd.Next(0, allStops.Count() - 1)],
                            allStops[rnd.Next(0, allStops.Count() - 1)]
                        },
                        Timing = new BusTiming
                        {
                            EveningOffPeakFrequency = r.EveningOffPeakFrequency,
                            EveningPeakFrequency = r.EveningPeakFrequency,
                            MorningOffpeakFrequency = r.MorningOffpeakFrequency,
                            MorningPeakFrequency = r.MorningPeakFrequency
                        }

                    }).ToArray()
                });
            }

            services.Sort();
            return services;
        }

    }
}
