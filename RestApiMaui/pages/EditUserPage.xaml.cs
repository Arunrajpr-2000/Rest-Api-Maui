using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace RestApiMaui
{
    public partial class EditUserPage : ContentPage
    {
        private MainPage _mainPage;
        private HttpClient _client;
        private string _baseUrl = "https://67adb0343f5a4e1477dea6e9.mockapi.io/users";
        private Users _user;

        public EditUserPage(MainPage mainPage, Users user)
        {
            InitializeComponent();
            _client = new HttpClient();
            _user = user;
            _mainPage = mainPage;

            // Set the existing name in the Entry field
            UserNameEntry.Text = _user.name;
            UserAgeEntry.Text = _user.age.ToString();
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

            if (!int.TryParse(UserAgeEntry.Text, out int userAge))
            {
                await DisplayAlert("Error", "Please enter a valid age", "OK");
                return;
            }


            _user.name = UserNameEntry.Text;
            _user.age = userAge;

            string json = JsonSerializer.Serialize(_user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{_baseUrl}/{_user.id}", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success", "User updated successfully", "OK");
                _mainPage.GetAllUsersCommand.Execute(this);
                await Navigation.PopAsync(); // Go back to previous page
            }
            else
            {
                await DisplayAlert("Error", "Failed to update user", "OK");
            }
        }
    }
}
