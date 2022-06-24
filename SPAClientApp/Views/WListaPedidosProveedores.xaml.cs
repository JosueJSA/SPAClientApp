using SPAClientApp.PedidosProveedoresService;
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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace SPAClientApp
{
    /// <summary>
    /// Interaction logic for WListaInsumos.xaml
    /// </summary>
    public partial class WListaPedidosProveedores : Window
    {
        private readonly PedidosProveedoresServiceClient  client = new PedidosProveedoresServiceClient();
        AnswerMessage answer = new AnswerMessage();

        private Notifier notifier; 
        private WHome HomeWindow { get; set; }
        private string Status { get; set; } 
        private string Valor { get; set; }

        public WListaPedidosProveedores(WHome home)
        {
            InitializeComponent();
            HomeWindow = home;
            ConfigurarToastNotifier(this, 3);
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DarDeBaja(object sender, RoutedEventArgs e)
        {
            var pedido = ((FrameworkElement)sender).DataContext as EPedidoProveedor;
            if (MostrarCuadroConfirmacion("¿Deseas cambiar el pedido a completado?"))
            {
                answer = client.ChangeStatusPeidoProveedor(pedido.Codigo, "Cancelado");
                if (answer.Key > 0)
                    MostrarToastMessage("Exito", "El pedido ha sido cancelado");
                else
                    MostrarToastMessage("Error", "Los sentimos, algo ha salido mal");
                RefrescarTabla();
            }
        }

        private void Activar(object sender, RoutedEventArgs e)
        {
            var pedido = ((FrameworkElement)sender).DataContext as EPedidoProveedor;
            if (MostrarCuadroConfirmacion("¿Deseas cambiar el pedido a completado?"))
            {
                answer = client.ChangeStatusPeidoProveedor(pedido.Codigo, "Completado");
                if (answer.Key > 0)
                    MostrarToastMessage("Exito", "El pedido ha sido almacenado y las cantidades de los insumos actualizadas");
                else
                    MostrarToastMessage("Error", "Los sentimos, algo ha salido mal");
                RefrescarTabla();
            }
        }

        private void MostrarToastMessage(string tipo, string mensaje)
        {
            if (tipo == "Warning")
                notifier.ShowWarning(mensaje);
            if (tipo == "Exito")
                notifier.ShowSuccess(mensaje);
            if (tipo == "Error")
            {
                ConfigurarToastNotifier(HomeWindow, 5);
                notifier.ShowError(mensaje);
                Close();
            }
        }

        private bool MostrarCuadroConfirmacion(string message)
        {
            MessageBoxResult boxResult = MessageBox.Show(message, "Advertencia", MessageBoxButton.YesNoCancel);
            return MessageBoxResult.Yes == boxResult;
        }

        private void ConfigurarToastNotifier(Window ventana, int segundos)
        {
            notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(parentWindow: ventana, corner: Corner.BottomRight, offsetX: 10, offsetY: 10);
                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(notificationLifetime: TimeSpan.FromSeconds(segundos), maximumNotificationCount: MaximumNotificationCount.FromCount(segundos));
                cfg.Dispatcher = Application.Current.Dispatcher;
            });
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            Close();
        }

        private void RefrescarTabla()
        {
            try
            {
                int valorS = int.Parse(Valor);
                var pedidos = client.GetPedidosProveedores(valorS);
                tablaDatos.ItemsSource = pedidos.Where(p => p.Status == Status);
            }
            catch (Exception)
            {
                MostrarToastMessage("Advertencia", "Debes escribir un número en el código del pedido");
            }
        }


        private void AgregarPedido(object sender, RoutedEventArgs e)
        {
            var pedidos = WInsumosDePedido.GetWindow(this);
            pedidos.Show();
        }

        private void BuscarPedido(object sender, RoutedEventArgs e)
        {
            try
            {
                Status = Criterio.Text;
                List<EPedidoProveedor> pedidos = null;
                if (string.IsNullOrEmpty(ValorBusqueda.Text))
                    pedidos = client.GetPedidosProveedores(null).ToList();
                else
                    pedidos = client.GetPedidosProveedores(int.Parse(ValorBusqueda.Text)).ToList();
                tablaDatos.ItemsSource = pedidos.Where(p => p.Status == Status);
                if(Status == "Activo")
                {
                    ColumnActive.Visibility = Visibility.Visible;
                    ColumnEliminate.Visibility = Visibility.Visible;
                }else if(Status == "Cancelado")
                {
                    ColumnActive.Visibility = Visibility.Collapsed;
                    ColumnEliminate.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ColumnActive.Visibility = Visibility.Collapsed;
                    ColumnEliminate.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                MostrarToastMessage("Advertencia", "Debes escribir un número en el código del pedido");
            }
        }

        private void Consultar(object sender, RoutedEventArgs e)
        {
            var pedido = ((FrameworkElement)sender).DataContext as EPedidoProveedor;
            WPedidoProveedor.GetWindow(this, pedido).Show();
        }
    }
}

