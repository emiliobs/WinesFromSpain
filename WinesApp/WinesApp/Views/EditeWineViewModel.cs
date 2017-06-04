using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinesApp.Model;

namespace WinesApp.Views
{
    public class EditeWineViewModel : Wine
    {
        #region Construct
        public EditeWineViewModel(Wine wine)
        {
            Name = wine.Name;
            Pairing = wine.Pairing;
            Price = wine.Price;
            Tasting = wine.Tasting;
            Type = wine.Type;
            Variety = wine.Variety;
            WineId = wine.WineId;
            
        }

        #endregion
    }
}
