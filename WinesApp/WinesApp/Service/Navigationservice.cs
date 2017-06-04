using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinesApp.Model;
using WinesApp.ViewModels;
using WinesApp.Views;

namespace WinesApp.Service
{
    public class Navigationservice
    {

        //metodo navigación genericos;
        public async Task Navigate(string pageName)
        {
            var mainViewModel = MainViewModel.GetInstance();

            switch (pageName)
            {
                case "NewWineView":    
                    //aqui consumo el singleton, cuando el ingrese aui ya el objeto esta generado y ya se puede bindiar:    
                    mainViewModel.NewWine = new NewWineViewModel();                
                    await App.Current.MainPage.Navigation.PushAsync(new NewWineView());
                    break;
               default:
                    break;
            }


        }


        //Método navegación con parametros:
        public async Task EditWine(Wine wine)
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EditWine = new EditeWineViewModel(wine);
            await App.Current.MainPage.Navigation.PushAsync(new EditWineView());
        }

        public async Task BackViews()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
