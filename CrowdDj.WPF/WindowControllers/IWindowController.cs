using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.WPF.ViewModels;

namespace CrowdDj.WPF.WindowControllers
{
    public interface IWindowController
    {
        void ShowWindow(BaseViewModel viewModel);
        void CloseWindow();
    }
}
