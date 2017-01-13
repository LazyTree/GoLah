using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GoLah.ViewModel;
using GoLah.Apps.View;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GoLah.Apps
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel Logic => DataContext as MainPageViewModel;

        public MainPage()
        {
            this.InitializeComponent();
            Logic.PropertyChanged += Logic_PropertyChanged;
        }

        private void Logic_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainPageViewModel.LoadingState))
                VisualStateManager.GoToState(this, Logic.LoadingState.ToString(), true);
        }

        private void BtnBusServices_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BusServicesPage));
        }

        private void BtnBusStops_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BusStopsPage));
        }
    }
}
