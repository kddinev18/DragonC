﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Cmponents.NonTerminalsView.Models;
using DragonC.GUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Models
{
    public partial class FormalRuleModel : ObservableObject
    {
        [ObservableProperty]
        private NonTerminalModel start;

        public ObservableCollection<FormalRuleVariantModel> FormalRuleVariants { get; set; } = new ObservableCollection<FormalRuleVariantModel>();
    }
}
