using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.Domain.Compilator;
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
                Name = "IMM_TO_REGT",
                MachineCode = "01000101"
            },
            new CommandModel()
            {
                Name = "REGT_TO_REG1",
                MachineCode = "01110000"
            },
            new CommandModel()
            {
                Name = "REG1_TO_REGT",
                MachineCode = "01000110"
            },
            new CommandModel()
            {
                Name = "ADD",
                MachineCode = "10000100"
            },
            new CommandModel()
            {
                Name = "REG3_TO_OUT",
                MachineCode = "01011110"
            },
            new CommandModel()
            {
                Name = "REG3_TO_REGT",
                MachineCode = "01011101"
            },
            new CommandModel()
            {
                Name = "REG2_TO_REGT",
                MachineCode = "01010101"
            },
            new CommandModel()
            {
                Name = "REGT_TO_REG2",
                MachineCode = "01110001"
            },
            new CommandModel()
            {
                Name = "GO_TO",
                MachineCode = "11000100",
                IsConditional = true
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
