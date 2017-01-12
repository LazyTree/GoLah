using GoLah.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace GoLah.Apps.Converter
{
    class BusStopConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is string) || string.IsNullOrEmpty(value.ToString()))
            {
                return string.Empty;
            }

            var repo = new LtaDataRepository();

            return repo.GetBusStopByCode(value.ToString())?.Description ?? value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
