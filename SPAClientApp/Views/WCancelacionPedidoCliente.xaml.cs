using SPAClientApp.PedidosClientesService;
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

namespace SPAClientApp.Views
{
    /// <summary>
    /// Interaction logic for WCancelacionPedidoCliente.xaml
    /// </summary>
    public partial class WCancelacionPedidoCliente : Window
    {
        private readonly PedidosClientesServiceClient client = new PedidosClientesServiceClient();    

        private Notifier notifier;
        private WListaPedidosClientes Parent { get; set; }
        private static WCancelacionPedidoCliente CurrentWindow { get; set; } = null;
        private static bool IsClosed { get; set; } = true;
        private EPedidoCliente Pedido { get; set; }
        private List<EProductoComprado> ProductosParaRecuperar { get; set; } = new List<EProductoComprado>();

        private WCancelacionPedidoCliente(WListaPedidosClientes parent, EPedidoCliente pedido)
        {
            InitializeComponent();
            ConfigurarToastNotifier(this, 3);
            Parent = parent;
            Pedido = pedido;
            ShowData();
        }

        public static WCancelacionPedidoCliente GetCancelationWindow(WListaPedidosClientes parent, EPedidoCliente pedido)
        {
            if (CurrentWindow == null || IsClosed)
            {
                IsClosed = false;
                return (CurrentWindow = new WCancelacionPedidoCliente(parent, pedido));
            }
            else
            {
                IsClosed = false;
                return CurrentWindow;
            }
        }

        private async Task ShowData()
        {
            var productos = await client.GetProductosCompradosAsync(Pedido.Codigo);
            TablaProductos.ItemsSource = productos;
        }

        private void AgregarProductoParaRestaurar(object sender, RoutedEventArgs e)
        {
            var producto = ((FrameworkElement)sender).DataContext as EProductoComprado;
            ProductosParaRecuperar.Add(producto);
        }

        private void RemoverProducto(object sender, RoutedEventArgs e)
        {
            var producto = ((FrameworkElement)sender).DataContext as EProductoComprado;
            ProductosParaRecuperar.Remove(producto);
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            IsClosed = true;
            Close();
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            IsClosed = true;
            Close();
        }

        private void CancelarPedido(object sender, RoutedEventArgs e)
        {
            var motivoCancelacion = CmbxMotivoCancelacion.Text == "System.Windows.Controls.StackPanel" ? motivo.Text : CmbxMotivoCancelacion.Text;
            if (string.IsNullOrEmpty(motivoCancelacion.Trim()))
            {
                MostrarToastMessage("Advertencia", "Debes seleccionar o escribir un motivo de cancelación");
            }
            else
            {
                var answer = client.CancelPedidoCliente(Pedido.Codigo, motivoCancelacion, ProductosParaRecuperar.Count() > 0 ? ProductosParaRecuperar.ToArray() : null);
                if (answer.Key > 0)
                {
                    Parent.ActualizarTabla();
                    IsClosed = true;
                    ConfigurarToastNotifier(Parent, 3);
                    MostrarToastMessage("Info", "El pedido ha sido cancelado");
                    Close();
                }
                else
                {
                    IsClosed = true;
                    MostrarToastMessage("Error", answer.Message);
                }
            }
        }

        private void MostrarToastMessage(string tipo, string mensaje)
        {
            if (tipo == "Advertencia")
                notifier.ShowWarning(mensaje);
            if (tipo == "Exito")
                notifier.ShowSuccess(mensaje);
            if (tipo == "Info")
                notifier.ShowInformation(mensaje);
            if (tipo == "Error")
            {
                ConfigurarToastNotifier(Parent, 5);
                notifier.ShowError(mensaje);
                Close();
            }
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
    }
}
