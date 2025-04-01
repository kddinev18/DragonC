using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.GUI.Components.HighLevelCommandsComponent.Models;
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
        private void Delete(HighLevelCommandModel comamnd)
        {
            Commands.Remove(comamnd);
        }
    }
}
