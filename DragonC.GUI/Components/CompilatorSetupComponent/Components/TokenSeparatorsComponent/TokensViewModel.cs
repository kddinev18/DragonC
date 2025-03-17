using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.TokenSeparatorsComponent.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.CompilatorSetupComponent.Components.TokenSeparatorsComponent
{
    public partial class TokensViewModel : ObservableObject
    {
        public ObservableCollection<TokenModel> Tokens { get; set; } = [
            new TokenModel()
            {
                Separator = ";"
            },
            new TokenModel()
            {
                Separator = ":"
            }
        ];


        [RelayCommand]
        private void Add()
        {
            Tokens.Add(new TokenModel()
            {
                Separator = "",
            });
        }

        [RelayCommand]
        private void Delete(TokenModel token)
        {
            Tokens.Remove(token);
        }
    }
}
