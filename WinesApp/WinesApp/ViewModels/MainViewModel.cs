using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinesApp.Model;
using WinesApp.Service;

namespace WinesApp.ViewModels
{
    public class MainViewModel
    {
        #region Atributtes

        private ApiService apiService;

        #endregion

        #region Properties

        public ObservableCollection<WineItemViewModel> Wines { get; set; }
        #endregion

        #region Constructor

        public MainViewModel()
        {
            apiService = new ApiService();
            Wines = new ObservableCollection<WineItemViewModel>();
                    
            LoadWines();

        }

        #endregion

        #region Commands

        #endregion

        #region Methods
        private async void LoadWines()
        {
            //aqui ya uso el  apiservice instanciado, con sus metodos y parametros:
            var wines = await apiService.Get<Wine>("http://winesbackend20170603020054.azurewebsites.net", "/api", "/Wines");

            //aqui atomiso el método para mejor practicas de programación:
            ReloadWines(wines);

        }

        private void ReloadWines(List<Wine> wines)
        {
            //aqui limpio la observableColection:
            Wines.Clear();

            foreach (var wine in wines)
            {
               Wines.Add(new WineItemViewModel()
               {
                   Image = wine.Image,
                   Name = wine.Name,
                   Pairing = wine.Pairing,
                   Price = wine.Price,
                   Tasting = wine.Tasting,
                   Type = wine.Type,
                   Variety = wine.Variety,
                   WineId = wine.WineId,
                   
               });  
            }
        }

        #endregion

        #region Events

        #endregion
    }
}
