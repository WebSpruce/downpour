using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using downpour.Popups;
using Weather.NET.Models.WeatherModel;

namespace downpour.ViewModels
{
    public class AddCityViewModel : INotifyPropertyChanged
    {
        private string entryCity;
        public string EntryCity
        {
            get { return entryCity; }
            set { 
                if (entryCity != value)
                {
                    entryCity = value;
                    OnPropertyChanged();
                } 
            }
        }
        public ICommand AddCityCommand { get; private set; }
        public ICommand AddCityIconCommand { get; private set; }

        public static AddCityViewModel instance;
        public AddCityViewModel()
        {
            instance = this;

            AddCityCommand = new Command(() => AddCityToList());
            AddCityIconCommand = new Command(() => AddCityByLocation());
        }

        private void AddCityToList()
        {
            if (!MainViewModel.instance.favoriteCities.Contains(EntryCity)){
                MainViewModel.instance.favoriteCities.Add(EntryCity);
            }
            LoadWeather(MainViewModel.instance.favoriteCities);
            AddCity.instance.Close();
        }
        private async void AddCityByLocation()
        {
            var location = await Geolocation.Default.GetLocationAsync();
            string locationInformation = $"{location.Latitude};{location.Longitude}";
            if (!MainViewModel.instance.favoriteCities.Contains(locationInformation))
            {
                MainViewModel.instance.favoriteCities.Add(locationInformation);
            }
            LoadWeather(MainViewModel.instance.favoriteCities);
            AddCity.instance.Close();
        }
        private void LoadWeather(List<string> favCities)
        {
            for(int i=0; i< favCities.Count; i++)
            {
                if (favCities[i].Contains(';'))
                {
                    double[] latitudeAndLongitude = Array.ConvertAll(favCities[i].Split(';'), Double.Parse);
                    MainViewModel.instance.GetCurrentWeather(latitudeAndLongitude);
                    MainViewModel.instance.GetForecastToday(latitudeAndLongitude);
                    MainViewModel.instance.GetForecastNextDays(latitudeAndLongitude);
                }
                else
                {
                    MainViewModel.instance.GetCurrentWeather(favCities[i]);
                    MainViewModel.instance.GetForecastToday(favCities[i]);
                    MainViewModel.instance.GetForecastNextDays(favCities[i]);
                }
                
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
