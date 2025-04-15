using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonC.Domain.API.Common;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DragonC.GUI.Components.LogInPageComponent
{
    public partial class LogInPageModel : ObservableObject
    {
        [ObservableProperty]
        private string userName = "test@abv.bg";
        [ObservableProperty]
        private string password = "123Asd@1";

        [RelayCommand]
        public async Task LogIn()
        {
            using (HttpClient client = new HttpClient())
            {
                string json = System.Text.Json.JsonSerializer.Serialize(new LogInDTO()
                {
                    Email = userName,
                    Password = password
                });

                HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(Path.Combine(Consts.APIUrl, "Auth", "login"), content);
                if (response.IsSuccessStatusCode)
                {
                    CurrentUserData.Token = (string)JsonNode.Parse(await response.Content.ReadAsStringAsync())["token"];
                    await Shell.Current.GoToAsync("projects");
                }
            }
        }
        [RelayCommand]
        public async Task Register()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client
                    .PostAsJsonAsync(Path.Combine(Consts.APIUrl, "Auth", "register"), new LogInDTO()
                    {
                        Email = userName,
                        Password = password
                    });
            }
        }
    }
}
