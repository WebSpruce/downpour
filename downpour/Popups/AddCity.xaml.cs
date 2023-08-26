using CommunityToolkit.Maui.Views;
using downpour.ViewModels;

namespace downpour.Popups;

public partial class AddCity : Popup
{
	public static AddCity instance;
	public AddCity()
	{
		InitializeComponent();
		instance = this;
		BindingContext = new AddCityViewModel();
	}
}