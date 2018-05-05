using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.Playstation.WPF.ViewModels;
using MahApps.Metro.Controls;

namespace CrowdDj.Playstation.WPF.WindowControllers
{
    class MainWindowController : IWindowController
    {
        public void ShowWindow(BaseViewModel viewModel)
        {
            MetroWindow window = new MainWindow();
            window.DataContext = viewModel;
            window.Show();
        }

        public void CloseWindow()
        {
        }
    }
}
