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

        private void UpdateMap()
        {
            myMap.MapElements.Clear();
            myMap.ZoomLevel = 1;

            Random rnd = new Random();
            myMap.Center = new Geopoint(new BasicGeoposition()
            {
                Latitude = rnd.Next(0, 89),
                Longitude = rnd.Next(0, 179)
            });

            for (int i = 0; i < 5; i++)
            {
                MapIcon mapIcon = new MapIcon();
                mapIcon.Location = new Geopoint(new BasicGeoposition()
                {
                    Latitude = rnd.Next(0, 89),
                    Longitude = rnd.Next(0, 179)
                });

                mapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
                mapIcon.Title = "YoYoYo! LazyTree!";
                mapIcon.Image = _mapIconStreamReference;
                mapIcon.ZIndex = 0;
                myMap.MapElements.Add(mapIcon);
            }

            //// TODO: check why location infor is null.
            //var service = lvServices.SelectedItem as BusService;

            //if (service == null)
            //{
            //    return;
            //}

            //var originBusStop = service.Directions.First().Stops.First();

            //myMap.Center = new Geopoint(new BasicGeoposition()
            //{
            //    Latitude = originBusStop.Location.Latitude,
            //    Longitude = originBusStop.Location.Longitude
            //});


            //foreach(var busStop in service.Directions.First().Stops)
            //{

            //    MapIcon mapIcon = new MapIcon();
            //    mapIcon.Location = new Geopoint(new BasicGeoposition()
            //    {
            //        Latitude = busStop.Location.Latitude,
            //        Longitude = busStop.Location.Longitude
            //    }); ;
            //    mapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
            //    mapIcon.Title = "YoYoYo!";
            //    mapIcon.Image = _mapIconStreamReference;
            //    mapIcon.ZIndex = 0;
            //    myMap.MapElements.Add(mapIcon);
            //}
        }
    }
}
