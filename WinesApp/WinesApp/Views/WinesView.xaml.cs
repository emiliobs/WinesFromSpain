using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinesApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WinesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WinesView : ContentPage
    {
        public WinesView()
        {
            InitializeComponent();

            //aqui refrezco de forma imediata cuando regrese de la newwineview:
            var mainViewMain = MainViewModel.GetInstance();
            Appearing += (sender, args) =>
            {
                mainViewMain.RefresWinehCommand.Execute((this));
            };


        }
    }
}