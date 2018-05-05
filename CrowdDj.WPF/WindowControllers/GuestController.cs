using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.BL.PoCos;
using CrowdDj.DAL;
using CrowdDj.WPF.ViewModels;
using MahApps.Metro.Controls;

namespace CrowdDj.WPF.WindowControllers
{
    class GuestController : IWindowController
    {
        private MetroWindow _window;

        public void ShowWindow(BaseViewModel viewModel)
        {
            _window = new GuestWindow() { DataContext = viewModel };
            _window.ShowDialog();
        }

        public void CloseWindow()
        {
            _window.Close();
        }
    }
}
