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

        private string _busStopCode = string.Empty;
        private ObservableCollection<ArrivalBusService> _arrivalBusServices = new ObservableCollection<ArrivalBusService>();

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

        #endregion

        #region Methods

        /// <summary>
        /// Update arrival bus services based on the bus stop code.
        /// </summary>
        private async void UpdateArrivalBusServices()
        {
            ArrivalBusServices.Clear();

            if (BusStopCode != null)
            {
                var repository = new LtaDataRepository();
                ArrivalBusServices = new ObservableCollection<ArrivalBusService>(
                    await repository.GetNextBusAsync(BusStopCode));
            }
        }

        #endregion
    }
}
