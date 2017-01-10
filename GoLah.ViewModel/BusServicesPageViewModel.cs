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
                        ServiceNumber = "112e",
                        Category = BusServiceCategory.FlatFee_1_10,
                        Operator ="SBS",
                        OriginCode = "123",
                        DestinationCode = "456"
                    },
                    new BusService
                    {
                        ServiceNumber = "857",
                        Category = BusServiceCategory.Express,
                        Operator ="SMART",
                        OriginCode = "123",
                        DestinationCode = "456"
                    },
                    new BusService
                    {
                        ServiceNumber = "858",
                        Category = BusServiceCategory.Feeder,
                        Operator ="SMRT",
                        OriginCode = "123",
                        DestinationCode = "456"
                    },
                };
            }
            else
            {
                LoadData();
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

        private async void LoadData()
        {
            var repository = new LtaDataRepository();
            AllBusServices = new ObservableCollection<BusService>(
                await repository.GetBusServicesAsync());   
        }
    }
}
