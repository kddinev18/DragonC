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

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasCompilationError))]
        private string compilationErrors = "";
        public bool HasCompilationError => CompilationErrors != "";

        public HighLevelCommandsViewModel()
        {
            Commands.Add(new HighLevelCommandModel(true)
            {
                CommandName = "AddCommand",
                FileName = "HighLevelCommand_a0ffc30316b143fa97000a113209b60a.cs",
                ProjectPath = @"C:\Users\kdinev\AppData\Local\Temp\DragonCPluginTemp",
                FullFilePath = @"C:\Users\kdinev\AppData\Local\Temp\DragonCPluginTemp\HighLevelCommand_a0ffc30316b143fa97000a113209b60a.cs"
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
            try
            {
                ICommandPluginProjectService commandPlugin = new CommandPluginProjectService();
                CompilatorService.Instance.CompilatorData.HighLevelCommands = commandPlugin.LoadHighLevelCommands(CompilatorService.Instance.CompilatorData);
                CompilationErrors = "";
            }
            catch (NullReferenceException ex)
            {
                CompilationErrors = "No commands or formal grammar saved";
            }
            catch (Exception ex)
            {
                CompilationErrors = ex.Message;
            }
        }

        [RelayCommand]
        private void Delete(HighLevelCommandModel comamnd)
        {
            Commands.Remove(comamnd);
        }
    }
}
