using CommunityToolkit.Maui.Views;
using downpour.OtherClasses;
using downpour.Popups;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
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
        private List<ForecastItem> forecast;
        public List<ForecastItem> Forecast
        {
            get { return forecast; }
            set
            {
                forecast = value;
                OnPropertyChanged();
            }
        }
        private List<ForecastItem> forecastNextDays;
        public List<ForecastItem> ForecastNextDays
        {
            get { return forecastNextDays; }
            set
            {
                forecastNextDays = value;
                OnPropertyChanged();
            }
        }

        public List<favouriteCities> favoriteCities = new List<favouriteCities>();

        public ICommand AddCityIconCommand { get; private set; }
        public ICommand ShowCitiesCommand { get; private set; }

       

        public static MainViewModel instance;
        public MainViewModel()
        {
            instance = this;
            AddCityIconCommand = new Command(ShowAddCityPopup);
            ShowCitiesCommand = new Command(ShowSavedCitiesPopup);
            try
            {
                WeatherAPIConnection();
                getFavCities();
                
            }
            catch(Exception ex)
            {
                Trace.WriteLine($"connection and first city error: {ex}");
            }
        }
        private async void ShowAddCityPopup()
        {
            await App.Current.MainPage.ShowPopupAsync(new AddCity());
        }
        private async void ShowSavedCitiesPopup()
        {
            await App.Current.MainPage.ShowPopupAsync(new CityChecker());
        }
        private async void getFavCities()
        {
            favoriteCities = await App.Database.GetAllFavouriteCities();

            Trace.WriteLine($"fav count: {favoriteCities.Count}");
            if (favoriteCities.Count < 1)
            {
                ShowAddCityPopup();
            }
            else
            {
                ShowSavedCitiesPopup();
            }

        }
        private WeatherClient weatherClient;
        public void WeatherAPIConnection()
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
        public async void LoadWeather(favouriteCities favCities)
        {
            try
            {

                    if (favCities.CityOrLocation.Contains(';'))
                    {
                        double[] latitudeAndLongitude = Array.ConvertAll(favCities.CityOrLocation.Split(';'), Double.Parse);
                        GetForecastNextDays(latitudeAndLongitude);
                        GetForecastToday(latitudeAndLongitude);
                        GetCurrentWeather(latitudeAndLongitude);
                    }
                    else
                    {
                        GetForecastNextDays(favCities.CityOrLocation);
                        GetForecastToday(favCities.CityOrLocation);
                        GetCurrentWeather(favCities.CityOrLocation);
                    }

                
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Loading Weather error: {ex}");
                await App.Database.DeleteCityAsync(favCities);
                await MainPage.instance.DisplayAlert("Error", $"The location was not available.", "Close Information");
            }

        }
        public async void GetCurrentWeather(string favCities)
        {
            try
            {
                WeatherModel currentWeather = await weatherClient.GetCurrentWeatherAsync(favCities, Measurement.Metric);
                weatherInformation(currentWeather);
            }
            catch(Exception ex)
            {
                Trace.WriteLine($"get current weather error: {ex}");
                await MainPage.instance.DisplayAlert("Error",$"Current Weather error.", "Close Information");
            }
        }
        public async void GetCurrentWeather(double[] latitudeAndLongitude)
        {
            try
            {
                Trace.WriteLine($"location: {latitudeAndLongitude[0]} - {latitudeAndLongitude[1]}");
                WeatherModel currentWeather = await weatherClient.GetCurrentWeatherAsync(latitudeAndLongitude[0], latitudeAndLongitude[1], Measurement.Metric);
                weatherInformation(currentWeather);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"get current weather location error: {ex}");
                await MainPage.instance.DisplayAlert("Error", $"Current Weather error.", "Close Information");
            }
        }
        private void weatherInformation(WeatherModel currentWeather)
        {
            CurrentCityName = currentWeather.CityName;
            CurrentTemperature = ((int)currentWeather.Main.Temperature).ToString();
            MinTemperature = ((int)currentWeather.Main.TemperatureMin).ToString();
            MaxTemperature = ((int)currentWeather.Main.TemperatureMax).ToString();
            Humidity = $"Humidity: {(int)currentWeather.Main.HumidityPercentage}%";
            Wind = $"Wind: {currentWeather.Wind.Speed.ToString("#.#")}m/s";
            HPA = $"{currentWeather.Main.AtmosphericPressure}hPa";
            CurrentDate = $"{currentWeather.AnalysisDate.Day}/{currentWeather.AnalysisDate.Month}/{currentWeather.AnalysisDate.Year}";
            DayName = $"{currentWeather.AnalysisDate.DayOfWeek}";
            CurrentWeatherImage = GetWeatherIcon(currentWeather.Weather[0].Title);
            
        }
        public async void GetForecastToday(string favCities)
        {
            try
            {
                List<ForecastItem> forecastItems = new List<ForecastItem>();
                List<WeatherModel> forecastsForCity = await weatherClient.GetForecastAsync(favCities, 8, Measurement.Metric, Language.English);
                foreach (var item in forecastsForCity)
                {
                    item.Main.Temperature = (int)item.Main.Temperature;
                    forecastItems.Add(new ForecastItem { WeatherModel = item, WeatherIconPath = GetWeatherIcon(item.Weather[0].Title) });

                }
                Forecast = forecastItems;

            }
            catch (Exception ex)
            {
                Trace.WriteLine($"get forecast error: {ex}");
                await MainPage.instance.DisplayAlert("Error", $"Today weather forecast error.", "Close Information");
            }
        }
        public async void GetForecastToday(double[] latitudeAndLongitude)
        {
            try
            {
                List<ForecastItem> forecastItems = new List<ForecastItem>();
                List<WeatherModel> forecastsForCity = await weatherClient.GetForecastAsync(latitudeAndLongitude[0], latitudeAndLongitude[1], 8, Measurement.Metric, Language.English);
                foreach (var item in forecastsForCity)
                {
                    item.Main.Temperature = (int)item.Main.Temperature;
                    forecastItems.Add(new ForecastItem { WeatherModel = item, WeatherIconPath = GetWeatherIcon(item.Weather[0].Title) });
                }
                Forecast = forecastItems;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"get forecast location error: {ex}");
                await MainPage.instance.DisplayAlert("Error", $"Today weather forecast error.", "Close Information");
            }
        }
        public async void GetForecastNextDays(string favCities)
        {
            try
            {
                List<ForecastItem> forecastItems = new List<ForecastItem>();

                List<WeatherModel> forecastForDays = await weatherClient.GetForecastAsync(favCities, 100, Measurement.Metric, Language.English);
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
                for (int i = 0; i < forecastOnly15PM.Count - 1; i++)
                {
                    int day = forecastOnly15PM[i].AnalysisDate.Day;
                    if (day != forecastOnly15PM[i + 1].AnalysisDate.Day)
                    {
                        forecastOnly15PM[i].Main.Temperature = (int)forecastOnly15PM[i].Main.Temperature;
                        forecastForNextDays.Add(forecastOnly15PM[i]);
                    }

                }
                foreach (var item in forecastForNextDays)
                {
                    item.Main.Temperature = (int)item.Main.Temperature;
                    forecastItems.Add(new ForecastItem { WeatherModel = item, WeatherIconPath = GetWeatherIcon(item.Weather[0].Title) });
                }
                ForecastNextDays = forecastItems;
            }
            catch(Exception ex)
            {
                Trace.WriteLine($"get forecast for next days error: {ex}");
                await MainPage.instance.DisplayAlert("Error", $"Forecast for next days error.", "Close Information");
            }
            

        }
        public async void GetForecastNextDays(double[] latitudeAndLongitude)
        {
            try
            {
                List<ForecastItem> forecastItems = new List<ForecastItem>();
                List<WeatherModel> forecastForDays = await weatherClient.GetForecastAsync(latitudeAndLongitude[0], latitudeAndLongitude[1], 100, Measurement.Metric, Language.English);
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
                for (int i = 0; i < forecastOnly15PM.Count - 1; i++)
                {
                    int day = forecastOnly15PM[i].AnalysisDate.Day;
                    if (day != forecastOnly15PM[i + 1].AnalysisDate.Day)
                    {
                        forecastOnly15PM[i].Main.Temperature = (int)forecastOnly15PM[i].Main.Temperature;
                        forecastForNextDays.Add(forecastOnly15PM[i]);
                    }
                }
                foreach (var item in forecastForNextDays)
                {
                    item.Main.Temperature = (int)item.Main.Temperature;
                    forecastItems.Add(new ForecastItem { WeatherModel = item, WeatherIconPath = GetWeatherIcon(item.Weather[0].Title) });
                }

                ForecastNextDays = forecastItems;
            }
            catch(Exception ex)
            {
                Trace.WriteLine($"get forecast for next days location error: {ex}");
                await MainPage.instance.DisplayAlert("Error", $"Forecast for next days error.", "Close Information");
            }
        }
        private string GetWeatherIcon(string title)
        {
            string path = string.Empty;
            switch (title)
            {
                case "Clear": { path = "Resources/Images/sun.png"; break; }
                case "Clouds": { path = "Resources/Images/cloudy.png"; break; }
                case "Mist  ": 
                case "Haze": 
                case "Dust": 
                case "Fog": 
                case "Sand": 
                case "Tornado": 
                case "Ash": 
                case "Squall": 
                case "Smoke": { path = "Resources/Images/fog.png"; break; }
                case "Snow": { path = "Resources/Images/snow.png"; break; }
                case "Rain": { path = "Resources/Images/rainy.png"; break; }
                case "Drizzle": { path = "Resources/Images/rain.png"; break; }
                case "Thunderstorm": { path = "Resources/Images/thunder.png"; break; }
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
