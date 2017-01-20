using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace GoLah.Model
{
    public class BusService : IComparable
    {
        private BusOperator _operator;
        private ImageSource _operatorImage;

        #region Properties

        public string ServiceNo { get; set; }

        public BusOperator Operator
        {
            get
            {
                return _operator;
            }
            set
            {
                _operator = value;

                string uri = null;
                switch (Operator)
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

                _operatorImage = new BitmapImage(new Uri(uri));
            }
        }

        public BusDirection[] Directions { get; set; }

        public bool IsLoop
        {
            get
            {
                return Directions.Length == 1;
            }
        }

        public string LoopDescription { get; set; }

        public int CompareTo(object obj)
        {
            return ServiceNo.CompareTo((obj as BusService).ServiceNo);
        }

        public ImageSource OperatorImage
        {
            get
            {
                return _operatorImage;
            }
        }
        #endregion
    }
}
