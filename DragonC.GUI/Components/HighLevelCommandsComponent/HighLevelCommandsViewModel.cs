using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.GUI.Components.HighLevelCommandsComponent.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.HighLevelCommandsComponent
{
    public partial class HighLevelCommandsViewModel : ObservableObject
    {
        public ObservableCollection<HighLevelCommandModel> Commands { get; set; } = [];

        [RelayCommand]
        private void Add()
        {
            Commands.Add(new HighLevelCommandModel()
            {
                CommandName = ""
            });
        }

        [RelayCommand]
        private void Delete(HighLevelCommandModel comamnd)
        {
            Commands.Remove(comamnd);
        }
    }
}
