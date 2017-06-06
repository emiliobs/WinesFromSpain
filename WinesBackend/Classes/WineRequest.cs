using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WinesBackend.Models;

namespace WinesBackend.Classes
{
    [NotMapped]
    public class WineRequest:Wine
    {
        public byte [] ImageArray { get; set; }
    }
}