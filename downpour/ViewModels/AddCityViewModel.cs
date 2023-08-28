using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using downpour.Popups;

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

        public static AddCityViewModel instance;
        public AddCityViewModel()
        {
            instance = this;

            AddCityCommand = new Command(() => AddCityToList());
        }

        private void AddCityToList()
        {
            MainViewModel.instance.favoriteCities.Add(EntryCity);
            MainViewModel.instance.GetCurrentWeather(MainViewModel.instance.favoriteCities);
            MainViewModel.instance.GetForecastToday(MainViewModel.instance.favoriteCities);
            MainViewModel.instance.GetForecastNextDays(MainViewModel.instance.favoriteCities);
            AddCity.instance.Close();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
