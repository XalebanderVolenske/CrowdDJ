using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.Playstation.WPF.WindowControllers;

namespace CrowdDj.Playstation.WPF.ViewModels
{
    public class BaseViewModel : NotifyPropertyChanged
    {

        public IWindowController Controller { get; private set; }

        public BaseViewModel(IWindowController contr)
        {
            Controller = contr;
        }
    }
}
