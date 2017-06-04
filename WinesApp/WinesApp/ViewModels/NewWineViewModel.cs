using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using WinesApp.Annotations;
using WinesApp.Service;

namespace WinesApp.ViewModels
{
    public class NewWineViewModel : INotifyPropertyChanged
    {
        #region Atributes

        private DialogService dialogService;

        private string type;
        private string name;
        private string variety;
        private string tasting;
        private string pairing;
        private decimal price;

        private bool isRunning;
        private bool isEnable;

       
        #endregion

        #region Properties

        public string Type
        {
            set
            {
                if (type != value)
                {
                    type = value;

                   PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Type"));
                }
            }
            get { return type; }
        }

        public string Name
        {
            set
            {
                if (name != value)
                {
                    name = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
            get { return type; }
        }

        public string Variety
        {
            set
            {
                if (variety != value)
                {
                    variety = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Variety"));
                }
            }
            get { return variety; }
        }

        public string Tasting
        {
            set
            {
                if (tasting != value)
                {
                    tasting = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Tasting"));
                }
            }
            get { return tasting; }
        }

        public string Pairing
        {
            set
            {
                if (pairing != value)
                {
                    pairing = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pairing"));
                }
            }
            get { return pairing; }
        }

        public decimal Price
        {
            set
            {
                if (price != value)
                {
                    price = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Price"));
                }
            }
            get { return price; }
        }

        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get { return isRunning; }
        }

        public bool IsEnable
        {
            set
            {
                if (isEnable != value)
                {
                    isEnable = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnable"));
                }
            }
            get { return isEnable; }
        }



        #endregion

        #region Commands
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

        }


        #endregion

        #region Constructor           
        public NewWineViewModel()
        {
            IsEnable = true;

            dialogService = new DialogService();
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
