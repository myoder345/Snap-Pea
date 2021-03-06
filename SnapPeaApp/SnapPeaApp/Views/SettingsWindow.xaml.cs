﻿using SnapPeaApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SnapPeaApp.Views
{
    /// <summary>
    /// Code behind for Window1.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            DataContext = new SettingsWindowViewModel();
            InitializeComponent();

            this.Closing += (DataContext as ViewModels.SettingsWindowViewModel).SettingsWindow_Closing;
        }

        private void Done_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
