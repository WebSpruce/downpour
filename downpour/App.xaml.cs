namespace downpour;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
    protected override Window CreateWindow(IActivationState activationState)
    {
        var windows = base.CreateWindow(activationState);

        windows.Height = 1000;
        windows.Width = 900;
        windows.X = 700;
        windows.Y = 50;
        return windows;
    }
}
