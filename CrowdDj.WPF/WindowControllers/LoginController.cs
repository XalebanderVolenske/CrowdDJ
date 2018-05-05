using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.WPF.ViewModels;
using MahApps.Metro.Controls;

namespace CrowdDj.WPF.WindowControllers
{
    public class LoginController : IWindowController
    {
        private MetroWindow _window;
        #region IController Members

        public void ShowWindow(BaseViewModel viewModel)
        {
            _window = new GuestLoginPage {DataContext = viewModel};
            _window.ShowDialog();
        }

        public void CloseWindow()
        {
            _window.Close();
        }
        #endregion
    }
}
