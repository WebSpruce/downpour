using downpour.Data;

namespace downpour;

public partial class App : Application
{
    private static database database2;
    public static database Database
    {
        get
        {
            if (database2 == null)
            {
                database2 = new database(Path.Combine(FileSystem.AppDataDirectory, "weatherFavouriteCities.db"));
            }
            return database2;
        }
    }
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
