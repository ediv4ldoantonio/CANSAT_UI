﻿using CANSAT_UI.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace CANSAT_UI.Views.Pages
{
    /// <summary>
    /// Interação lógica para Alerts.xam
    /// </summary>
    public partial class Alerts : Page, INavigableView<AlertsViewModel>
    {
        public Alerts(AlertsViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();
        }

        public AlertsViewModel ViewModel { get; }

        private async void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await ViewModel.LoadDataCommand.ExecuteAsync(this);
        }
    }
}
