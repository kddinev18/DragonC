using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.GUI.Components.HighLevelCommandsComponent.Models;
using DragonC.GUI.Services;
using DragonC.HLCC.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.HighLevelCommandsComponent
{
    public partial class HighLevelCommandsViewModel : ObservableObject
    {
        public ObservableCollection<HighLevelCommandModel> Commands { get; set; } = [];

        public HighLevelCommandsViewModel()
        {
            Commands.Add(new HighLevelCommandModel(true)
            {
                FileName = "HighLevelCommand_f65816b6c0ca4494b9fb03a67b534e41.cs",
                ProjectPath = @"C:\Users\User\AppData\Local\Temp\DragonCPluginTemp",
                FullFilePath = @"C:\Users\User\AppData\Local\Temp\DragonCPluginTemp\HighLevelCommand_f65816b6c0ca4494b9fb03a67b534e41.cs"
            });
        }

        [RelayCommand]
        private void Add()
        {
            Commands.Add(new HighLevelCommandModel());
        }

        [RelayCommand]
        private void Edit(HighLevelCommandModel command)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", $"/c \"code {command.ProjectPath}\"")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = false
            };

            using (var process = new Process())
            {
                process.StartInfo = processInfo;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();
            }
        }

        [RelayCommand]
        private void Submit()
        {
            ICommandPluginProjectService commandPlugin = new CommandPluginProjectService();
            CompilatorService.Instance.CompilatorData.HighLevelCommands = commandPlugin.LoadHighLevelCommands(CompilatorService.Instance.CompilatorData);
        }

        [RelayCommand]
        private void Delete(HighLevelCommandModel comamnd)
        {
            Commands.Remove(comamnd);
        }
    }
}
