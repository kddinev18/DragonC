using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.ProjectsPageComponent.Models
{
    public partial class ProjectModel : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }

        [RelayCommand]
        private async Task OpenProject()
        {
            CurrentUserData.SelectedProjectId = Id;
            await Shell.Current.GoToAsync("home");
        }
    }
}
