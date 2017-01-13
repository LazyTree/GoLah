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
        private BusRoutine _selectedBusService;
        private bool _isBusServiceInfoReady = false;

        public BusServicesPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                _allBusServices = new ObservableCollection<BusService>
                {
                    new BusService
                    {
                        ServiceNo = "112e",
                        Routines = new BusRoutine[]
                        {
                            new BusRoutine
                            {
                                ServiceNo = "112e",
                                Category = BusServiceCategory.FlatFee,
                                Operator ="SBS",
                                OriginCode = "123",
                                DestinationCode = "456"
                            }
                        }
                    },

                    new BusService
                    {
                        ServiceNo = "857",
                        Routines = new BusRoutine[]
                        {
                            new BusRoutine
                            {
                                ServiceNo = "857",
                                Category = BusServiceCategory.Express,
                                Operator ="SMART",
                                OriginCode = "123",
                                DestinationCode = "456"
                            }
                        }
                    },

                    new BusService
                    {
                        ServiceNo = "858",
                        Routines = new BusRoutine[]
                        {
                            new BusRoutine
                            {
                                ServiceNo = "858",
                                Category = BusServiceCategory.Feeder,
                                Operator ="SMRT",
                                OriginCode = "123",
                                DestinationCode = "456"
                            }
                        }
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

        public BusRoutine SelectedBusService
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

            AllBusServices = new ObservableCollection<BusService>(repository.CachedBusServices);

            IsBusServiceInfoReady = true;
        }
    }
}
