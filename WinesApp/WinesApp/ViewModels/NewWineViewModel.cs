using System.ComponentModel;
using System.Runtime.CompilerServices;
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
    public class NewWineViewModel : Wine, INotifyPropertyChanged
    {
        #region Atributes

        private ImageSource imageSource;
        private MediaFile file;

        private DialogService dialogService;
        private ApiService apiService;
        private Navigationservice navigationservice;

        //private string type;
        //private string name;
        //private string variety;
        //private string tasting;
        //private string pairing;
        //private decimal price;

        private bool isRunning;
        private bool isEnable;


        #endregion

        #region Properties

        //public string Type
        //{
        //    set
        //    {
        //        if (type != value)
        //        {
        //            type = value;

        //           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Type"));
        //        }
        //    }
        //    get { return type; }
        //}

        //public string Name
        //{
        //    set
        //    {
        //        if (name != value)
        //        {
        //            name = value;

        //            OnPropertyChanged();
        //            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
        //        }
        //    }
        //    get { return name; }
        //}

        //public string Variety
        //{
        //    set
        //    {
        //        if (variety != value)
        //        {
        //            variety = value;

        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Variety"));
        //        }
        //    }
        //    get { return variety; }
        //}

        //public string Tasting
        //{
        //    set
        //    {
        //        if (tasting != value)
        //        {
        //            tasting = value;

        //            OnPropertyChanged();
        //            // PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Tasting"));
        //        }
        //    }
        //    get { return tasting; }
        //}

        //public string Pairing
        //{
        //    set
        //    {
        //        if (pairing != value)
        //        {
        //            pairing = value;

        //            OnPropertyChanged();
        //            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pairing"));
        //        }
        //    }
        //    get { return pairing; }
        //}

        //public decimal Price
        //{
        //    set
        //    {
        //        if (price != value)
        //        {
        //            price = value;

        //            OnPropertyChanged();
        //            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Price"));
        //        }
        //    }
        //    get { return price; }
        //}

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

        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;

                    OnPropertyChanged();
                    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get { return isRunning; }
        }

       

        public bool IsEnabled
        {
            set
            {
                if (isEnable != value)
                {
                    isEnable = value;

                    OnPropertyChanged();
                    // PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnable"));
                }
            }
            get { return isEnable; }
        }



        #endregion

        #region Commands

        public ICommand TakePictureCommand
        {
            get { return  new RelayCommand(TakePicture);}
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


        public ICommand NewWineCommand
        {
            get { return new RelayCommand(NewWine); }
        }

      
        private async void NewWine()
        {

            if (string.IsNullOrEmpty(Type))
            {
                await dialogService.ShowMessage("Error","You must enter a Type Wine.");
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

            if (Price <= 0 )
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
               
           };

            IsRunning = true;
            IsEnabled = false;

            var response = await apiService.Post("http://winesbackend20170603020054.azurewebsites.net", "/api", "/Wines", wine);

            //aqui pregunto si si pudo agregar el nuevo dato:
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            //si si lo pudo ingresar lo regreso al la pagina principal:
            await  navigationservice.BackViews();

            IsRunning = false;
            IsEnabled = true;

        }


        #endregion

        #region Constructor           
        public NewWineViewModel()
        {
            IsEnabled = true;

            dialogService = new DialogService();
            apiService = new ApiService();
            navigationservice = new Navigationservice();
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
