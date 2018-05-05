using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.Playstation.WPF.WindowControllers;

namespace CrowdDj.Playstation.WPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {

        public MainWindowViewModel() : base(null)
        {
        }

        public MainWindowViewModel(IWindowController windowController) : base(windowController)
        {
        }
    }
}
