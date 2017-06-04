using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinesApp.ViewModels;

namespace WinesApp.Infrastructure
{
    public class InstanceLocator
    {
        //aqui hago una inyeccción del patron MVVM:
        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            Main = new MainViewModel();
        }
    }
}
