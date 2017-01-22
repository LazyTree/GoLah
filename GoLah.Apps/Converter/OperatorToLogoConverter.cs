﻿using GoLah.Model;
using GoLah.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using GoLah.Model;

namespace GoLah.Apps.Converter
{
    class OperatorToLogoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is BusOperator))
            {
                return new BitmapImage();
            }

            var _operator = (BusOperator)value;

            string uri = null;
            switch (_operator)
            { 
                case BusOperator.GAS:
                    uri = "ms-appx:///Images/GAS_50x50.png";
                    break;
                case BusOperator.SBST:
                    uri = "ms-appx:///Images/SBST_50x50.png";
                    break;
                case BusOperator.SMRT:
                    uri = "ms-appx:///Images/SMRT_50x50.png";
                    break;
                case BusOperator.TTS:
                    uri = "ms-appx:///Images/TTS_50x50.png";
                    break;
                default:
                    break;

            }

            var image = new BitmapImage(new Uri(uri));

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    class BusStopConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is string) || string.IsNullOrEmpty(value.ToString()))
            {
                return string.Empty;
            }

            var repo = new LtaDataRepository<BusStop>();
            Func<BusStop, bool> predicate = (x) => x.Code.Equals(value.ToString());
            return repo.GetAsync(predicate).Result?.Description ?? value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}