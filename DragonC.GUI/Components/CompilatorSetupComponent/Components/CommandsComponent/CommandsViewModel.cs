using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.CommandsComponent.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.CompilatorSetupComponent.Components.CommandsComponent
{
    public partial class CommandsViewModel : ObservableObject
    {
        public ObservableCollection<CommandModel> Commands { get; set; } = [
            new CommandModel()
            {
                MachineCode = "01",
                Name = "Name",
            },
            new CommandModel()
            {
                MachineCode = "02",
                Name = "Write",
            },
            new CommandModel()
            {
                MachineCode = "03",
                Name = "Delete",
            },
        ];


        [RelayCommand]
        private void Add()
        {
            Commands.Add(new CommandModel()
            {
                MachineCode = "",
                Name = "",
            });
        }

        [RelayCommand]
        private void Delete(CommandModel command)
        {
            Commands.Remove(command);
        }
    }
}
