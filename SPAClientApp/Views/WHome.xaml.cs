using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
            WListaInsumos.GetWListaInsumos(this).Show();
        }

        private void VerProductos(object sender, RoutedEventArgs e)
        {
            WListaProductos.GetWListaProductos(this).Show();
        }

        private void VerPedidosClientes(object sender, RoutedEventArgs e)
        {
            WListaPedidosClientes.GetPedidosClientesWindow(this).Show();
        }

        private void VerCliente(object sender, RoutedEventArgs e)
        {
            WCliente.GetWClient(this).Show();
        }
    }
}
