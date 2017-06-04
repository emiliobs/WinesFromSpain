using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using WinesApp.Model;
using WinesApp.Service;
using WinesApp.Views;

namespace WinesApp.ViewModels
{
    public class WineItemViewModel : Wine
    {
        #region Atributtes

        private Navigationservice navigationservice;
        #endregion

        #region Properters


        #endregion

        #region Contructor

        public WineItemViewModel()
        {
            navigationservice = new Navigationservice();
        }
        #endregion

        #region Commands

        public ICommand EditWineCommand
        {
            get { return new RelayCommand(EditWine);}

        }

        
        private  async void EditWine()
        {
            //aqui le envio mis porpios parametros:
            await navigationservice.EditWine(this);
        }

        #endregion

        #region Methods

        #endregion

        #region Events

        #endregion
    }
}
