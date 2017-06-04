using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinesApp.ViewModels;
using WinesApp.Views;

namespace WinesApp.Service
{
    public class Navigationservice
    {
        public async Task Navigate(string pageName)
        {
            switch (pageName)
            {
                case "NewWineView":

                    //aqui consumo el singleton, cuando el ingrese aui ya el objeto esta generado y ya se puede bindiar:
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.NewWine = new NewWineViewModel();

                    await App.Current.MainPage.Navigation.PushAsync(new NewWineView());
                    break;
                default:
                    break;
            }


        }

        public async Task BackViews()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
