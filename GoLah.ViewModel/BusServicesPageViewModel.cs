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
                LoadDataAsync();
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

        private async void LoadDataAsync()
        {
            // Filter and add stops for each bus route
            var routes = await new LtaDataRepository<BusRoute>().QueryAsync().ConfigureAwait(false);
            var routestops = await new LtaDataRepository<BusRouteStop>().QueryAsync().ConfigureAwait(false);
            routes.ToList().ForEach(x => x.BusStopCodes =
                routestops.Where(s => s.ServiceNo == x.ServiceNo && s.Direction == x.Direction)
                .OrderBy(s => s.StopSequence)
                .Select(s => s.BusStopCode)
                .ToList());
            AllBusServices = new ObservableCollection<BusService>(await MergeBusRoutesToBusServiceAsync(routes));
            SelectedBusService = AllBusServices.First();
        }


        /// <summary>
        /// Massage the bus routes to bus service.
        /// </summary>
        /// <param name="_cachedBusRoutes"></param>
        /// <returns></returns>
        private async Task<IEnumerable<BusService>> MergeBusRoutesToBusServiceAsync(IEnumerable<BusRoute> busRoutes)
        {
            var allStops = await new LtaDataRepository<BusStop>().QueryAsync();

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
                        Stops = r.BusStopCodes == null? new List<BusStop>() : allStops.Where(s => r.BusStopCodes.Contains(s.Code)).ToList(),
                        Timing = new BusTiming
                        {
                            EveningOffPeakFrequency = r.EveningOffPeakFrequency + " mins",
                            EveningPeakFrequency = r.EveningPeakFrequency + " mins",
                            MorningOffpeakFrequency = r.MorningOffpeakFrequency + " mins",
                            MorningPeakFrequency = r.MorningPeakFrequency + " mins"
                        }

                    }).ToArray()
                });
            }

            services.Sort();
            return services;
        }

    }
}
