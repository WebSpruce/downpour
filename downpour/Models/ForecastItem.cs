using System.ComponentModel;
using Weather.NET.Models.WeatherModel;

namespace downpour.Models
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
