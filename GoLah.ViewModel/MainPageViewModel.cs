using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        private string testString = "Lazy3";

        public string TestString
        {
            get { return testString; }
            set
            {
                Set(ref testString, value);
            }
        }

        private string inputString = "Lazy3Lazy3";

        public string InputString
        {
            get { return inputString; }
            set
            {
                Set(ref inputString, value);
            }
        }

    }
}
