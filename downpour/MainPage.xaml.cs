using downpour.ViewModels;

namespace downpour;

public partial class MainPage : ContentPage
{
	public static MainPage instance;
	public MainPage()
	{
		InitializeComponent();
		instance = this;
		BindingContext = new MainViewModel();
	}

}

