using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WinesBackend.Models
{
    public class Wine
    {
        [Key]
        public int WineId { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        
        public string Name { get; set; }

        public string Image { get; set; }

        [Required]
        public string Variety { get; set; }

       
        public string Tasting { get; set; }

       
        public string Pairing { get; set; }

        [Required]   
        [DisplayFormat(DataFormatString = " {0} €",ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }
        
    }
}