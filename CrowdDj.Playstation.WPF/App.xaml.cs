using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CrowdDj.Playstation.WPF.ViewModels;
using CrowdDj.Playstation.WPF.WindowControllers;

namespace CrowdDj.Playstation.WPF
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
