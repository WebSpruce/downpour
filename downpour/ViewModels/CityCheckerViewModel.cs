using downpour.OtherClasses;
using downpour.Popups;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace downpour.ViewModels
{
    public class CityCheckerViewModel : INotifyPropertyChanged
    {
        private List<favouriteCities> cities;
        public List<favouriteCities> Cities
        {
            get { return cities;}
            set
            {
                if (cities != value)
                {
                    cities = value;
                    OnPropertyChanged();
                }
            }
        }
        private favouriteCities selectedCity;
        public favouriteCities SelectedCity
        {
            get { return selectedCity; }
            set
            {
                if (selectedCity != value)
                {
                    selectedCity = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand SelectedCommand { get; private set; }
        public ICommand SwippedCommand { get; private set; }

        public static CityCheckerViewModel instance;
        public CityCheckerViewModel() 
        {
            instance = this;
            Cities = MainViewModel.instance.favoriteCities;

            SelectedCommand = new Command(Selected);
            SwippedCommand = new Command(Swipped);
        }
        public void Selected()
        {
            Trace.WriteLine($"selected city: {SelectedCity.CityOrLocation}");
            MainViewModel.instance.LoadWeather(SelectedCity);
            CityChecker.instance.Close();
        }
        public async void Swipped()
        {
            Trace.WriteLine($"swipped city: {SelectedCity.CityOrLocation}");
            await App.Database.DeleteCityAsync(SelectedCity);
            Cities = MainViewModel.instance.favoriteCities;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
