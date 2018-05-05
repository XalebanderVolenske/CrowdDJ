using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.Playstation.WPF.ViewModels;

namespace CrowdDj.Playstation.WPF.WindowControllers
{
    public interface IWindowController
    {
        void ShowWindow(BaseViewModel viewModel);
        void CloseWindow();
    }
}
