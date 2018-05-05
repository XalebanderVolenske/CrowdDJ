using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.WPF.WindowControllers;

namespace CrowdDj.WPF.ViewModels
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
