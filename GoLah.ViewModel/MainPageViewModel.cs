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

        private ArrivalBusService _selectedBusService;

        private LoadingStates _loadingState = LoadingStates.Loading;

        #endregion

        #region Properties

        public LoadingStates LoadingState
        {
            get { return _loadingState; }
            set { Set(ref _loadingState, value); }
        }

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

        #region Constructor

        public MainPageViewModel()
        {
            LoadData();
        }


        #endregion

        #region Methods

        private async void LoadData()
        {
            try
            {
                var repo = new LtaDataRepository();
                await repo.GetBusStopsAsync();
                await repo.GetBusRoutesAsync();
                LoadingState = LoadingStates.Loaded;
            }
            catch
            {
                LoadingState = LoadingStates.Error;
            }
        }

        /// <summary>
        /// Update arrival bus services based on the bus stop code.
        /// </summary>
        private async void UpdateArrivalBusServices()
        {
            //if (!string.IsNullOrEmpty(BusStopCode))
            //{
            //    var repository = new LtaDataRepository();
            //    ArrivalBusServices = new ObservableCollection<ArrivalBusService>(
            //        await repository.GetNextBusAsync(BusStopCode));
            //}
            //else
            //{
            //    ArrivalBusServices.Clear();
            //}

            //SelectedBusService = null;
        }

        #endregion

        public enum LoadingStates
        {
            Loading,
            Loaded,
            Error
        }
    }
}
