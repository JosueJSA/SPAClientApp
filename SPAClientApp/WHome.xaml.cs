﻿using System;
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

namespace SPAClientApp
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class WHome : Window
    {
        public WHome()
        {
            InitializeComponent();
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();  
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Maximizar(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void Minimizar(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void VerInsumos(object sender, RoutedEventArgs e)
        {
            new InsumosContenedor().Show();
        }
    }
}
