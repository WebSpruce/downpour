using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.NET.Models.WeatherModel;

namespace downpour.OtherClasses
{
    public class ForecastItem: INotifyPropertyChanged
    {
        private WeatherModel _weatherModel;
        public WeatherModel WeatherModel
        {
            get { return _weatherModel; }
            set
            {
                _weatherModel = value;
                OnPropertyChanged(nameof(WeatherModel));
            }
        }
        private string _weatherIconPath;
        public string WeatherIconPath
        {
            get { return _weatherIconPath; }
            set
            {
                _weatherIconPath = value;
                OnPropertyChanged(nameof(WeatherIconPath));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
