using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using SIAT.PhoneApp.ViewModels;

namespace SIAT.PhoneApp
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        private SettingsViewModel _settingsViewModel;

        public SettingsPage()
        {
            InitializeComponent();

            _settingsViewModel = new SettingsViewModel();
            this.DataContext = _settingsViewModel;
        }
    }
}