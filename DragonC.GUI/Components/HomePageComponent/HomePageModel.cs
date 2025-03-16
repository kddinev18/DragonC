using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.GUI.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.HomePageComponent
{
    public partial class HomePageModel : ObservableObject
    {
        public ImageSource Icon { get; set; }
        public HomePageModel()
        {
            Icon = TextToImageHelper.CreateImageFromText(
                text: "&#xE70F;", // Unicode icon
                fontFamily: "MaterialAssets",
                fontSize: 64,
                textColor: SKColors.Black,
                backgroundColor: SKColors.Transparent
            );
        }
        [ObservableProperty]
        private int counter = 0;

        [RelayCommand]
        private void Click()
        {
            Counter += 1;
        }
    }
}
