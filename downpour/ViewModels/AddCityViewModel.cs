using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using downpour.OtherClasses;
using downpour.Popups;
using Weather.NET.Models.WeatherModel;
using static System.Net.WebRequestMethods;

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

        private async void AddCityToList()
        {
            try
            {
                bool contains = false;
                List<favouriteCities> fTemp = await App.Database.GetAllFavouriteCities();
                foreach (var item in fTemp)
                {
                    if (item.CityOrLocation.Contains(EntryCity))
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                {
                    await App.Database.SaveCityAsync(new favouriteCities { CityOrLocation = EntryCity });
                    fTemp.Add(new favouriteCities { CityOrLocation = EntryCity });
                    MainViewModel.instance.LoadWeather(fTemp.Find(x => x.CityOrLocation == EntryCity));
                }
                else
                {
                    await MainPage.instance.DisplayAlert("Error", $"The city or location is already on your favourite list.", "Close Information");
                }

                AddCity.instance.Close();
            }
            catch(Exception ex)
            {
                Trace.WriteLine($"Add City To List error: {ex}");
                await MainPage.instance.DisplayAlert("Error", $"Add City To List error.", "Close Information");
            }
}
        private async void AddCityByLocation()
        {
            try
            {
                var location = await Geolocation.Default.GetLocationAsync();
                string locationInformation = $"{location.Latitude};{location.Longitude}";

                List<favouriteCities> fTemp = await App.Database.GetAllFavouriteCities();
                bool contains = false;
                foreach (var item in fTemp)
                {
                    if (item.CityOrLocation.Contains(locationInformation))
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                {
                    await App.Database.SaveCityAsync(new favouriteCities { CityOrLocation = locationInformation });
                    fTemp.Add(new favouriteCities { CityOrLocation = locationInformation });
                    MainViewModel.instance.LoadWeather(fTemp.Find(x=>x.CityOrLocation == locationInformation));
                }
                else
                {
                    await MainPage.instance.DisplayAlert("Error", $"The city or location is already on your favourite list.", "Close Information");
                }
                AddCity.instance.Close();
            }
            catch(Exception ex)
            {
                Trace.WriteLine($"Add City by location error: {ex}");
                await MainPage.instance.DisplayAlert("Error", $"Add City by location error.", "Close Information");
            }
}
        


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
