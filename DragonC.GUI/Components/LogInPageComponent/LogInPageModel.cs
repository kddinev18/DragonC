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

namespace DragonC.GUI.Components.LogInPageComponent
{
    public partial class LogInPageModel : ObservableObject
    {
        [ObservableProperty]
        private string userName;
        [ObservableProperty]
        private string password;

        [RelayCommand]
        public async Task LogIn()
        {
            using (HttpClient client = new HttpClient())
            {
                string json = System.Text.Json.JsonSerializer.Serialize(new LogInDTO()
                {
                    UserName = userName,
                    Password = password
                });

                HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("testasdasdasdasd", content);
                if (response.IsSuccessStatusCode)
                {

                }
                else
                {

                }
            }
        }
        [RelayCommand]
        public async Task Register()
        {
            using (HttpClient client = new HttpClient())
            {
                string json = System.Text.Json.JsonSerializer.Serialize(new LogInDTO()
                {
                    UserName = userName,
                    Password = password
                });

                HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("testasdasdasdasd", content);
                if (response.IsSuccessStatusCode)
                {

                }
                else
                {

                }
            }
        }
    }
}
