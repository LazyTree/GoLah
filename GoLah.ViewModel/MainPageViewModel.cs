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
    /// <summary>
    /// View model for the main page.
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        #region Fields

        private const int BUS_STOP_CODE_LENGTH = 5;

        private string _busStopCode = string.Empty;

        private ObservableCollection<ArrivalBusService> _arrivalBusServices
            = new ObservableCollection<ArrivalBusService>();

        private ArrivalBusService _selectedBusService;

        #endregion

        #region Properties

        /// <summary>
        /// Bus stop code to check for bus arrival.
        /// </summary>
        public string BusStopCode
        {
            get
            {
                return _busStopCode;
            }
            set
            {
                Set(ref _busStopCode, value.Trim());

                UpdateArrivalBusServices();
            }
        }

        /// <summary>
        /// Bus stop code length.
        /// </summary>
        public int BusStopCodeLength
        {
            get
            {
                return BUS_STOP_CODE_LENGTH;
            }
        }

        /// <summary>
        /// Arrival bus services for the given <see cref="BusStopCode"/>
        /// </summary>
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

        /// <summary>
        /// Selected bus service.
        /// This should be one of the item in <see cref="ArrivalBusServices"/>
        /// </summary>
        public ArrivalBusService SelectedBusService
        {
            get
            {
                return _selectedBusService;
            }
            set
            {
                Set(ref _selectedBusService, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Update arrival bus services based on the bus stop code.
        /// </summary>
        private async void UpdateArrivalBusServices()
        {
            if (!string.IsNullOrEmpty(BusStopCode))
            {
                var repository = new LtaDataRepository();
                ArrivalBusServices = new ObservableCollection<ArrivalBusService>(
                    await repository.GetNextBusAsync(BusStopCode));
            }
            else
            {
                ArrivalBusServices.Clear();
            }

            SelectedBusService = null;
        }

        #endregion
    }
}
