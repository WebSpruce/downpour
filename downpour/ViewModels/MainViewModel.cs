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
        private string minTemperature;
        public string MinTemperature
        {
            get { return minTemperature; }
            set
            {
                if (minTemperature != value)
                {
                    minTemperature = value;
                    OnPropertyChanged();
                }
            }
        }
        private string maxTemperature;
        public string MaxTemperature
        {
            get { return maxTemperature; }
            set
            {
                if (maxTemperature != value)
                {
                    maxTemperature = value;
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
        private string iconPath;
        public string IconPath
        {
            get { return iconPath; }
            set
            {
                if (iconPath != value)
                {
                    iconPath = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<WeatherModel> forecast;
        public List<WeatherModel> Forecast
        {
            get { return forecast; }
            set
            {
                if (forecast != value)
                {
                    forecast = value;
                    OnPropertyChanged();
                }
            }
        }
        private List<WeatherModel> forecastNextDays;
        public List<WeatherModel> ForecastNextDays
        {
            get { return forecastNextDays; }
            set
            {
                if (forecastNextDays != value)
                {
                    forecastNextDays = value;
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
                WeatherAPIConnection();
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
        private WeatherClient weatherClient;
        public async void WeatherAPIConnection()
        {
            try
            {
                weatherClient = new WeatherClient("d7475cfcb03771e74f09ee5319181305");
            }
            catch(Exception ex)
            {
                Trace.WriteLine($"API connection error: {ex}");
            }
        }
        public async void GetCurrentWeather(List<string> favCities)
        {
            try
            {
                WeatherModel currentWeather = await weatherClient.GetCurrentWeatherAsync(favCities[0], Measurement.Metric);

                CurrentCityName = currentWeather.CityName;
                CurrentTemperature = ((int)currentWeather.Main.Temperature).ToString();
                MinTemperature = ((int)currentWeather.Main.TemperatureMin).ToString();
                MaxTemperature = ((int)currentWeather.Main.TemperatureMax).ToString();
                Humidity = $"Humidity: {(int)currentWeather.Main.HumidityPercentage}%";
                Wind = $"Wind: {currentWeather.Wind.Speed.ToString("#.#")}m/s";
                HPA = $"{currentWeather.Main.AtmosphericPressure}hPa";
                CurrentDate = $"{currentWeather.AnalysisDate.Day}/{currentWeather.AnalysisDate.Month}/{currentWeather.AnalysisDate.Year}";
                DayName = $"{currentWeather.AnalysisDate.DayOfWeek}";
                Trace.WriteLine($"icon: {currentWeather.Weather[0].Title} - {GetWeatherIcon(currentWeather.Weather[0].Title)}");
                CurrentWeatherImage = GetWeatherIcon(currentWeather.Weather[0].Title);
                //rain
            }
            catch(Exception ex)
            {
                Trace.WriteLine($"get current weather error: {ex}");
            }
        }
        public async void GetForecastToday(List<string> favCities)
        {
            try
            {
                List<WeatherModel> forecastsForCity = await weatherClient.GetForecastAsync(favCities[0], 8, Measurement.Metric, Language.English);
                foreach (var item in forecastsForCity)
                {
                    item.Main.Temperature = (int)item.Main.Temperature;
                    IconPath = GetWeatherIcon(item.Weather[0].Title);
                }

                Forecast = forecastsForCity;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"get forecast error: {ex}");
            }
        }
        public async void GetForecastNextDays(List<string> favCities)
        {
            List<WeatherModel> forecastForDays = await weatherClient.GetForecastAsync(favCities[0], 60, Measurement.Metric, Language.English);
            List<WeatherModel> forecastForNextDays = new List<WeatherModel>();
            List<WeatherModel> forecastOnly15PM = new List<WeatherModel>();
            for (int i = 0; i < forecastForDays.Count - 1; i++)
            {
                TimeSpan time = new TimeSpan(15, 00, 00);
                if (forecastForDays[i].AnalysisDate.TimeOfDay == time)
                {
                    forecastOnly15PM.Add(forecastForDays[i]);
                }
            }
            for (int i=0; i< forecastOnly15PM.Count-1; i++)
            {
                int day = forecastOnly15PM[i].AnalysisDate.Day; 
                if (day != forecastOnly15PM[i + 1].AnalysisDate.Day)
                {
                    forecastOnly15PM[i].Main.Temperature = (int)forecastOnly15PM[i].Main.Temperature;
                    forecastForNextDays.Add(forecastOnly15PM[i]);
                }
                
            }

            ForecastNextDays = forecastForNextDays;

        }
        private string GetWeatherIcon(string title)
        {
            string path = string.Empty;
            switch (title)
            {
                case "Clear": { path = "Resources/Images/weather_icons/sun.png"; break; }
                case "Clouds": { path = "Resources/Images/weather_icons/cloudy.png"; break; }
                case "Mist  ": 
                case "Haze": 
                case "Dust": 
                case "Fog": 
                case "Sand": 
                case "Tornado": 
                case "Ash": 
                case "Squall": 
                case "Smoke": { path = "Resources/Images/weather_icons/fog.png"; break; }
                case "Snow": { path = "Resources/Images/weather_icons/snow.png"; break; }
                case "Rain": { path = "Resources/Images/weather_icons/rainy.png"; break; }
                case "Drizzle": { path = "Resources/Images/weather_icons/rain.png"; break; }
                case "Thunderstorm": { path = "Resources/Images/weather_icons/thunder.png"; break; }
            }
            return path;
            
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
