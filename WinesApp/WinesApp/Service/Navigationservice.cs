using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
