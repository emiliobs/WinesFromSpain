using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using WinesApp.Annotations;
using WinesApp.Model;
using WinesApp.Service;

namespace WinesApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Atributtes

        private ApiService apiService;
        private Navigationservice navigationservice;
        private DialogService dialogService;

        private bool isRefreshing;

        #endregion

        #region Properties

        public ObservableCollection<WineItemViewModel> Wines { get; set; }
        public NewWineViewModel NewWine { get; set; }

        public bool IsRefreshing
        {

            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;

                    OnPropertyChanged();
                }
            }
            get { return isRefreshing; }
        }

        #endregion

        #region Singleton

        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }

        #endregion

        #region Constructor

        public MainViewModel()
        {

            //singleton:
            instance = this;// la instancia soy yo:

            //Service
            apiService = new ApiService();
            navigationservice = new Navigationservice();
            dialogService = new DialogService();

            //View Models;
            Wines = new ObservableCollection<WineItemViewModel>();
            //la idea es crarlo cuando se vaya a utilizar, patron singleton:
            //NewWine = new NewWineViewModel();

            //Load Data:
            //LoadWines();//ya lo estoy invocando desde el commands en llamado desde el codiback de la wineview.xaml:

        }

        #endregion

        #region Commands
        public ICommand AddWineCommand
        {
            get { return  new RelayCommand(AddWine);}
        }

        public ICommand RefresWinehCommand
        {
            get { return  new RelayCommand(Refresh);}
        }

        private void Refresh()
        {
            IsRefreshing = true;

            LoadWines();

            IsRefreshing = false;

        }


        private async void AddWine()
        {
            await navigationservice.Navigate("NewWineView");
        }

        #endregion

        #region Methods
        private async void LoadWines()
        {
            //aqui ya uso el  apiservice instanciado, con sus metodos y parametros:
            var response = await apiService.Get<Wine>("http://winesbackend20170603020054.azurewebsites.net", "/api", "/Wines");

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            //aqui atomiso el método para mejor practicas de programación:
            ReloadWines((List<Wine>)response.Result);

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
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


    }
}
