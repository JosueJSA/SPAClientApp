using SPAClientApp.ClientesService;
using SPAClientApp.PedidosClientesService;
using SPAClientApp.ProductosService;
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
    /// Interaction logic for WPedidoClienteConsulta.xaml
    /// </summary>
    public partial class WPedidoClienteConsulta : Window
    {
        private WListaPedidosClientes Parent { get; set; }
        private ClientePage ClienteExistPage { get; set; }
        private readonly ClientesServiceClient clientService = new ClientesServiceClient();
        private readonly PedidosClientesServiceClient pedidoService = new PedidosClientesServiceClient();
        private readonly ProductosServiceClient productoService = new ProductosServiceClient();
        private Notifier notifier;

        public WPedidoClienteConsulta(WListaPedidosClientes parent, EPedidoCliente pedido)
        {
            InitializeComponent();
            Parent = parent;
            Frame.Content = (ClienteExistPage = new ClientePage());
            Total.Content = pedido.CostoTotal.ToString();
            CargarProductos(pedido.Codigo);
            if (pedido.TipoPedido == "A domicilio")
                CargarCliente(pedido.Codigo);
        }

        private async void CargarProductos(int idPedido)
        {
            var productos =  await pedidoService.GetProductosCompradosAsync(idPedido);
            TablaProductosSeleccionados.ItemsSource = productos.ToList();
        }

        private async void CargarCliente(int idPedido)
        {
            var cliente = await clientService.GetClienteByPedidoAsync(idPedido);
            ClienteExistPage.MostrarClienteInfo(cliente);
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            Close();    
        }

        private async void ConsultarProducto(object sender, RoutedEventArgs e)
        {
            var productoSeleccionado = ((FrameworkElement)sender).DataContext as EProductoComprado;
            var producto = await productoService.GetProductByIdAsync(productoSeleccionado.Codigo);
            var window = new WProducto(this);
            window.ActivarModoLectura(producto);
            window.Show();
        }

        private void MostrarToastMessage(string tipo, string mensaje)
        {
            if (tipo == "Advertencia")
                notifier.ShowWarning(mensaje);
            if (tipo == "Exito")
            {
                ConfigurarToastNotifier(Parent, 5);
                notifier.ShowSuccess(mensaje);
                Close();
            }
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
