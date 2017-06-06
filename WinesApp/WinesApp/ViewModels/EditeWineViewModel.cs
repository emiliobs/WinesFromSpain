using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using WinesApp.Annotations;
using WinesApp.Classes;
using WinesApp.Model;
using WinesApp.Service;
using Xamarin.Forms;

namespace WinesApp.ViewModels
{
    public class EditeWineViewModel : Wine , INotifyPropertyChanged
    {
        #region Atributes
        private DialogService dialogService;
        private ApiService apiService;
        private Navigationservice navigationservice;
        private ImageSource imageSource;
        private MediaFile file;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties

        public ImageSource ImageSource
        {
            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    OnPropertyChanged();
                }
            }

            get { return imageSource; }
        }

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

        public ICommand TakePictureCommand
        {
            get { return new RelayCommand(TakePicture);}
        }

        private async void TakePicture()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await dialogService.ShowMessage("No Camera", ":( NO Camara available.");
            }

            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                Directory = "Sample",
                Name = "test.jpg",
                PhotoSize = PhotoSize.Small,
            });

            IsRunning = true;

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();

                    return stream;
                });
            }

            IsRunning = false;
        }

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

            var imageArray = FilesHelper.ReadFully(file.GetStream());
            file.Dispose();

            var wine = new Wine()
            {
                Name = Name,
                Price = Price,
                Type = Type,
                Variety = Variety,
                Tasting = Tasting,
                Pairing = Pairing,
                ImageArray = imageArray,
                Image = Image,
                WineId = WineId,
                

            };

            //aqui ya utilizo el apiservice que invoca al metodo put (actualizar)
            IsRunning = true;
            IsEnabled = false;

            var response = await apiService.Put("http://winesbackend20170603020054.azurewebsites.net", "/api", "/Wines", wine);//this = al objeto wine(all atributos)

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

       
        private async void DeleteWine()
        {
            var answer = await dialogService.ShowConfirm("Confirm", "Are you sure to delte this?");

            if (!answer)
            {
               return; 
            }

            //si estru true borro el dato:
            //aqui ya utilizo el apiservice que invoca al metodo put (actualizar)
            IsRunning = true;
            IsEnabled = false;

            var response = await apiService.Delete("http://winesbackend20170603020054.azurewebsites.net", "/api", "/Wines", this);//this = al objeto wine(all atributos)

            IsRunning = false;
            IsEnabled = true;

            //aqui, preguntos si no problama:
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            await navigationservice.BackViews();
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
            Image = wine.Image;

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
