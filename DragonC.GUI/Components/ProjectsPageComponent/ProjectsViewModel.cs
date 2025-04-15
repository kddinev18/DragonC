using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.Domain.API;
using DragonC.Domain.API.Common;
using DragonC.Domain.API.Filters;
using DragonC.GUI.Components.ProjectsPageComponent.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.ProjectsPageComponent
{
    public partial class ProjectsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string projectName;
        private int _page = 1;
        public ObservableCollection<ProjectModel> Projects { get; set; } = [];

        public async Task Load(string projectName = "")
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", CurrentUserData.Token);

                string json = System.Text.Json.JsonSerializer.Serialize(new PagedCollection<ProjectFilters>()
                {
                    Page = _page,
                    PageSize = 10,
                    Filters = new ProjectFilters()
                    {
                        ProjectName = projectName,
                        AuthorUserName = ""
                    }
                });

                HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client
                    .PostAsync(Path.Combine(Consts.APIUrl, "Projects", "getAllPaged"), content);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string responseJson = await response.Content.ReadAsStringAsync();
                    List<ProjectDTO> projects = JsonSerializer.Deserialize<List<ProjectDTO>>(responseJson, options);
                    Projects.Clear();
                    foreach (var project in projects)
                    {

                        Projects.Add(new ProjectModel()
                        {
                            Id = project.Id.Value,
                            Name = project.Name
                        });
                    }
                }
            }
        }

        [RelayCommand]
        private async Task Prev()
        {
            if (_page == 1)
            {
                return;
            }
            _page--;

            await Load();
        }

        [RelayCommand]
        private async Task Next()
        {
            if (Projects.Count < 10)
            {
                return;
            }
            _page++;

            await Load();
        }

        [RelayCommand]
        private async Task Search()
        {
            _page = 1;
            await Load(ProjectName);
        }

        [RelayCommand]
        private async Task Create()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", CurrentUserData.Token);

                var response = await client
                    .PostAsJsonAsync(Path.Combine(Consts.APIUrl, "Projects", "create"),
                    new ProjectDTO()
                    {
                        Name = ProjectName,
                        FileName = "Null",
                        FileData = new byte[] { 0, 1, 0 }
                    });

                if (response.IsSuccessStatusCode)
                {
                    CurrentUserData.SelectedProjectId = int.Parse(await response.Content.ReadAsStringAsync());
                    await Shell.Current.GoToAsync("home");
                }
            }
        }
    }
}
