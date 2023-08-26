using CommunityToolkit.Maui.Views;
using downpour.Popups;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Weather.NET;
using Weather.NET.Enums;
using Weather.NET.Models.WeatherModel;

namespace downpour.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string currentTemperature;
        public string CurrentTemperature
        {
            get { return currentTemperature; }
            set
            {
                if (currentTemperature != value)
                {
                    currentTemperature = value;
                    OnPropertyChanged();
                }
            }
        }
        private string currentCityName;
        public string CurrentCityName
        {
            get { return currentCityName; }
            set
            {
                if (currentCityName != value)
                {
                    currentCityName = value;
                    OnPropertyChanged();
                }
            }
        }
        private string currentFullDate;
        public string CurrentFullDate
        {
            get { return currentFullDate; }
            set
            {
                if (currentFullDate != value)
                {
                    currentFullDate = value;
                    OnPropertyChanged();
                }
            }
        }
        private string currentDayName;
        public string CurrentDayName
        {
            get { return currentDayName; }
            set
            {
                if (currentDayName != value)
                {
                    currentDayName = value;
                    OnPropertyChanged();
                }
            }
        }
        private string currentWeatherImage;
        public string CurrentWeatherImage
        {
            get { return currentWeatherImage; }
            set
            {
                if (currentWeatherImage != value)
                {
                    currentWeatherImage = value;
                    OnPropertyChanged();
                }
            }
        }
        private string humidity;
        public string Humidity
        {
            get { return humidity; }
            set
            {
                if (humidity != value)
                {
                    humidity = value;
                    OnPropertyChanged();
                }
            }
        }
        private string wind;
        public string Wind
        {
            get { return wind; }
            set
            {
                if (wind != value)
                {
                    wind = value;
                    OnPropertyChanged();
                }
            }
        }
        private string hpa;
        public string HPA
        {
            get { return hpa; }
            set
            {
                if (hpa != value)
                {
                    hpa = value;
                    OnPropertyChanged();
                }
            }
        }
        private string currentDate;
        public string CurrentDate
        {
            get { return currentDate; }
            set
            {
                if (currentDate != value)
                {
                    currentDate = value;
                    OnPropertyChanged();
                }
            }
        }
        private string dayName;
        public string DayName
        {
            get { return dayName; }
            set
            {
                if (dayName != value)
                {
                    dayName = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<string> favoriteCities = new List<string>();

        public static MainViewModel instance;
        public MainViewModel()
        {
            instance = this;
            try
            {
                Trace.WriteLine($"fav: {favoriteCities.Count}");
                if (favoriteCities.Count < 1)
                {
                    MainPage.instance.ShowPopup(new AddCity());
                }
            }
            catch(Exception ex)
            {
                Trace.WriteLine($"connection and first city error: {ex}");
            }
        }

        public async void WeatherAPIConnection(List<string> favCities)
        {
            try
            {
                WeatherClient weatherClient = new WeatherClient("d7475cfcb03771e74f09ee5319181305");
                List<WeatherModel> forecastsForCity = await weatherClient.GetForecastAsync(favCities[0], 8, Measurement.Metric, Language.English);

                WeatherModel currentWeather = await weatherClient.GetCurrentWeatherAsync(favCities[0], Measurement.Metric);

                CurrentCityName = currentWeather.CityName;
                CurrentTemperature = ((int)currentWeather.Main.Temperature).ToString();
                Humidity = $"Humidity: {(int)currentWeather.Main.HumidityPercentage}%";
                Wind = $"Wind: {currentWeather.Wind.Speed.ToString("#.#")}m/s";
                HPA = $"{currentWeather.Main.AtmosphericPressure}hPa";
                CurrentDate = $"{currentWeather.AnalysisDate.Day}/{currentWeather.AnalysisDate.Month}/{currentWeather.AnalysisDate.Year}";
                DayName = $"{currentWeather.AnalysisDate.DayOfWeek}";
                Trace.WriteLine($"temp text: {CurrentTemperature} - {CurrentCityName}");

                Trace.WriteLine($"forecasts test: {forecastsForCity[0].Weather[0].Description}");
                Trace.WriteLine($"forecasts test: {forecastsForCity[0].Weather[0].Title}");
                Trace.WriteLine($"Temperatures: {currentWeather.Main.Temperature} - {currentWeather.Main.TemperatureMin} - {currentWeather.Main.TemperatureMax}");
                Trace.WriteLine($"weather test: {currentWeather.Weather.Capacity}");
                Trace.WriteLine($"wind test: {currentWeather.Wind.Gust} - {currentWeather.Wind.Speed} - {currentWeather.Wind.Direction}");
                //Trace.WriteLine($"rain test: {currentWeather.Rain.PastHourVolume} - {currentWeather.Rain.Past3HoursVolume}");
                Trace.WriteLine($"timezone test: {currentWeather.Timezone}");
                Trace.WriteLine($"main test: {currentWeather.Main.HumidityPercentage} - {currentWeather.Main.AtmosphericPressure}");
            }
            catch(Exception ex)
            {
                Trace.WriteLine($"API connection error: {ex}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
