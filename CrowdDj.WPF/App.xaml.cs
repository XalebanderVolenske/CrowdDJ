using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CrowdDj.WPF.ViewModels;
using CrowdDj.WPF.WindowControllers;

namespace CrowdDj.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            MainWindowController controller =
                new MainWindowController();
            controller.ShowWindow(
                new MainWindowViewModel(controller));
        }
    }
}
