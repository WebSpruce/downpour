using CommunityToolkit.Maui.Views;
using downpour.ViewModels;

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

    private async void Button_Delete(object sender, EventArgs e)
    {
		CityCheckerViewModel.instance.Swipped();
    }

    private void Button_Show(object sender, EventArgs e)
    {
		CityCheckerViewModel.instance.Selected();
    }
}