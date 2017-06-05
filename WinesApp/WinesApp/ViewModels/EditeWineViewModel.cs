using System;
using System.Collections.Generic;
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
    public class EditeWineViewModel : Wine , INotifyPropertyChanged
    {
        #region Atributes
        private DialogService dialogService;
        private ApiService apiService;
        private Navigationservice navigationservice;

        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties

        public bool IsEnabled
        {
            set
            {
                if (isEnabled != null)
                {
                    isEnabled = value;
                    OnPropertyChanged();
                }
            }

            get { return isEnabled; }
        }


        public bool IsRunning
        {
            set
            {
                if (isRunning != null)
                {
                    isRunning = value;
                    OnPropertyChanged();
                }
            }

            get { return isRunning; }
        }
        #endregion

        #region Commands
        public ICommand SaveWineCommand
        {
            get { return  new RelayCommand(SaveWine);}
        }

        private async void SaveWine()
        {
            if (string.IsNullOrEmpty(Type))
            {
                await dialogService.ShowMessage("Error", "You must enter a Type Wine.");
                return;
            }

            if (string.IsNullOrEmpty(Name))
            {
                await dialogService.ShowMessage("Error", "You must enter a Name Wine.");
                return;
            }

            if (string.IsNullOrEmpty(Variety))
            {
                await dialogService.ShowMessage("Error", "You must enter a Variety.");
                return;
            }

            if (string.IsNullOrEmpty(Tasting))
            {
                await dialogService.ShowMessage("Error", "You must enter a  Tasting.");
                return;
            }

            if (string.IsNullOrEmpty(Pairing))
            {
                await dialogService.ShowMessage("Error", "You must enter a Pairing.");
                return;
            }

            if (Price <= 0)
            {
                await dialogService.ShowMessage("Error", "You must enter a number grather than zero in Price.");
                return;
            }

            //aqui ya utilizo el apiservice que invoca al metodo put (actualizar)
            IsRunning = true;
            IsEnabled = false;

            var response = await apiService.Put("http://winesbackend20170603020054.azurewebsites.net", "/api", "/Wines", this);//this = al objeto wine(all atributos)

            IsRunning = false;
            IsEnabled = true;

            //aqui, preguntos si no problama:
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            await  navigationservice.BackViews();

        }

        public ICommand DeleteWineCommand
        {
            get { return  new RelayCommand(DeleteWine);}
        }

        

        private void DeleteWine()
        {
            throw new NotImplementedException();
        }

      

        #endregion

        #region Construct
        public EditeWineViewModel(Wine wine)
        {

            dialogService = new DialogService();
            apiService    = new ApiService();
            navigationservice = new Navigationservice();

            Name = wine.Name;
            Pairing = wine.Pairing;
            Price = wine.Price;
            Tasting = wine.Tasting;
            Type = wine.Type;
            Variety = wine.Variety;
            WineId = wine.WineId;

            IsEnabled = true;

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
