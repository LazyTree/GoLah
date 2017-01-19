using GoLah.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GoLah.Apps.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BusServicesPage : Page
    {
        RandomAccessStreamReference _mapIconStreamReference;

        public BusServicesPage()
        {
            this.InitializeComponent();
            _mapIconStreamReference = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapPin.png"));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Visible;

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            base.OnNavigatedTo(e);
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();

            backRequestedEventArgs.Handled = true;
        }

        private void myMap_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateMap();
        }

        private void lvServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMap();
        }

        private async void UpdateMap()
        {
            Random rnd = new Random();

            myMap.MapElements.Clear();
            myMap.ZoomLevel = 1;

            var accessStatus = await Geolocator.RequestAccessAsync();

            if (accessStatus == GeolocationAccessStatus.Allowed)
            {
                Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 10 };
                Geoposition pos = await geolocator.GetGeopositionAsync();

                myMap.Center = new Geopoint(new BasicGeoposition()
                {
                    Latitude = pos.Coordinate.Latitude,
                    Longitude = pos.Coordinate.Longitude
                });
            }
            else
            {
                myMap.Center = new Geopoint(new BasicGeoposition()
                {
                    Latitude = rnd.Next(0, 89),
                    Longitude = rnd.Next(0, 179)
                });
            }

            var service = lvServices.SelectedItem as BusService;

            if (service == null)
            {
                return;
            }

            var originBusStop = service.Directions.First().Stops.FirstOrDefault();

            myMap.Center = new Geopoint(new BasicGeoposition()
            {
                Latitude = double.Parse(originBusStop?.Latitude),
                Longitude = double.Parse(originBusStop?.Longitude)
            });
            myMap.ZoomLevel = 12;

            foreach (var busStop in service.Directions.First().Stops)
            {

                MapIcon mapIcon = new MapIcon();
                mapIcon.Location = new Geopoint(new BasicGeoposition()
                {
                    Latitude = double.Parse(busStop.Latitude),
                    Longitude = double.Parse(busStop.Longitude)
                }); ;
                mapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
                mapIcon.Title = busStop.RoadName;
                mapIcon.Image = _mapIconStreamReference;
                mapIcon.ZIndex = 0;
                myMap.MapElements.Add(mapIcon);
            }
        }
    }
}
