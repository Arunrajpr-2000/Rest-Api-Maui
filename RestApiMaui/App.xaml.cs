using RestApiMaui;

namespace RestApiMaui;


public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new MainPage())
		{
			BarBackgroundColor = Color.FromArgb("#512BD4"),
			BarTextColor = Colors.White
		};
	}
}