using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace RestApiMaui
{
    public partial class AddUserPage : ContentPage
    {
        private HttpClient _client;
        private string _baseUrl = "https://67adb0343f5a4e1477dea6e9.mockapi.io/users";

        public AddUserPage()
        {
            InitializeComponent();
            _client = new HttpClient();
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserNameEntry.Text))
            {
                await DisplayAlert("Error", "Please enter a name", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(UserAgeEntry.Text))
            {
                await DisplayAlert("Error", "Please enter a age", "OK");
                return;
            }

            // Ensure age is converted to an integer
            if (!int.TryParse(UserAgeEntry.Text, out int userAge))
            {
                await DisplayAlert("Error", "Age must be a valid number", "OK");
                return;
            }


            var user = new
            {
                name = UserNameEntry.Text,
                avatar = $"https://fakeimg.pl/350x200/?text={UserNameEntry.Text}",
                age = userAge
            };
            string json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_baseUrl, content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success", "User added successfully", "OK");
                UserNameEntry.Text = string.Empty; // Clear input after success
                UserAgeEntry.Text = string.Empty;
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Failed to add user", "OK");
            }
        }
    }
}
