using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinesApp.Model
{
   
    public class Wine
    {
        public int WineId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Variety { get; set; }
        public string Tasting { get; set; }
        public string Pairing { get; set; }
        public decimal Price { get; set; }

        //lo utilizo para devolver la clave primerai para poder actualizar y eliminar:
        public override int GetHashCode()
        {
            return WineId;
        }
    }
}
