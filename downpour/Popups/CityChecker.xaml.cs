using CommunityToolkit.Maui.Views;
using downpour.OtherClasses;
using downpour.ViewModels;
using System.Diagnostics;

namespace downpour.Popups;

public partial class CityChecker : Popup
{
	public static CityChecker instance;
	public CityChecker()
	{
		InitializeComponent();
		instance = this;
		BindingContext = new CityCheckerViewModel();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		CityCheckerViewModel.instance.Swipped();
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
		CityCheckerViewModel.instance.Selected();
    }
}