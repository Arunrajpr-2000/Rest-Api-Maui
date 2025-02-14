namespace RestApiMaui;

using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using Microsoft.Maui.Controls;

public partial class MainPage : ContentPage
{
	HttpClient client;
	JsonSerializerOptions _serializerOptions;
	string baseUrl = "https://67adb0343f5a4e1477dea6e9.mockapi.io";

	public ObservableCollection<Users> users { get; set; } = new ObservableCollection<Users>();

	public MainPage()
	{
		InitializeComponent();
		client = new HttpClient();
		_serializerOptions = new JsonSerializerOptions { WriteIndented = true };

		BindingContext = this; // Bind the page to itself
	}

	public ICommand GetAllUsersCommand => new Command(async () =>
	{
		try
		{
			var url = $"{baseUrl}/users/";
			var response = await client.GetAsync(url);

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var data = JsonSerializer.Deserialize<List<Users>>(content, _serializerOptions);

				if (data != null)
				{
					users.Clear();
					foreach (var user in data)
					{
						users.Add(user);
					}
				}
			}
			else
			{
				await DisplayAlert("Error", "Failed to fetch users", "OK");
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "OK");
		}
	});

	
	private void onAddUserButton(object sender, EventArgs e) 
	{
		var addUserPage = new AddUserPage();
        Navigation.PushAsync(addUserPage);
		// Refresh user list when returning
        addUserPage.Disappearing += (s, args) => GetAllUsersCommand.Execute(null);

	}

	private void onEditUserButton(object sender, EventArgs e)
	{
		if (sender is Button button && button.BindingContext is Users user)
		{
			Navigation.PushAsync(new EditUserPage(this, user));
		}
	}


	public ICommand DeleteUserCommand => new Command<Users>(async (user) =>
	{
		if (user == null) return;

		var url = $"{baseUrl}/users/{user.id}/";
		var response = await client.DeleteAsync(url);

		if (response.IsSuccessStatusCode)
		{
			users.Remove(user);
		}
	});

	// public ICommand AddUserCommand => new Command(async () =>
	// {
	// 	await Navigation.PushAsync(new AddUserPage());
	// });

	// public ICommand UpdateUserCommand => new Command<Users>(async (user) =>
	// {
	// 	if (user == null) return;
	// 	await Navigation.PushAsync(new EditUserPage(user));
	// });

}
