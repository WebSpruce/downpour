using CommunityToolkit.Maui.Views;
using downpour.OtherClasses;
using downpour.Popups;
using System.Collections.Generic;
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
        public ICommand AddCityIconCommand { get; private set; }

        public static CityCheckerViewModel instance;
        public CityCheckerViewModel() 
        {
            instance = this;
            getCities();

            SelectedCommand = new Command(Selected);
            SwippedCommand = new Command(Swipped);
            AddCityIconCommand = new Command(async () => { CityChecker.instance.Close();  await App.Current.MainPage.ShowPopupAsync(new AddCity());}) ;
        }
        public async void getCities()
        {
            Cities = await App.Database.GetAllFavouriteCities();
        }
        public void Selected()
        {
            MainViewModel.instance.LoadWeather(SelectedCity);
            CityChecker.instance.Close();
        }
        public async void Swipped()
        {
            await App.Database.DeleteCityAsync(SelectedCity);
            CityChecker.instance.Close();
            await App.Current.MainPage.ShowPopupAsync(new AddCity());
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
