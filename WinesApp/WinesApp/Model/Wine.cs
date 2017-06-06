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

        public byte [] ImageArray { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(Image))
                {
                    return "ic_launcher.png";
                }

                return $"http://winesbackend20170603020054.azurewebsites.net{Image.Substring(1)}";
            }
        }

        
        //lo utilizo para devolver la clave primerai para poder actualizar y eliminar:
        public override int GetHashCode()
        {
            return WineId;
        }
    }
}
