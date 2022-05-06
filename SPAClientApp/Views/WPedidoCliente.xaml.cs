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

namespace SPAClientApp
{
    /// <summary>
    /// Interaction logic for WPedidoCliente.xaml
    /// </summary>
    public partial class WPedidoCliente : Window
    {
        private Notifier notifier;
        private static WPedidoCliente PedidoWindow = null;
        private readonly PedidosClientesServiceClient client = new PedidosClientesServiceClient(); 
        private WListaPedidosClientes Parent { get; set; }

        private WPedidoCliente()
        {
            InitializeComponent();
            ConfigurarToastNotifier(this, 3);
            CargarProductos();
        }

        public static WPedidoCliente GetWListaPedidosClientesWindow(WListaPedidosClientes parent)
        {
            if (PedidoWindow == null)
                return (PedidoWindow = new WPedidoCliente() { Parent = parent });
            return PedidoWindow;
        }

        private void AgregarProducto(object sender, RoutedEventArgs e)
        {
            var producto = ConvertirAProductoSeleccionado(((FrameworkElement)sender).DataContext as EProducto);
            if (EstaAgregado(producto.CodigoProductoVenta))
                MostrarToastMessage("Advertencia", $"El producto '{producto.Nombre}' ya ha sido agregado a la lista de productos comprados");
            else
                TablaProductosSeleccionados.Items.Add(producto);
        }

        private void RemoverProducto(object sender, RoutedEventArgs e)
        {
            var producto = ((FrameworkElement)sender).DataContext as EProducto;
            TablaProductosSeleccionados.Items.Remove(producto);
        }

        private void CargarProductos()
        {
            var result = client.GetProductosList().ToList();
            if (result != null)
                TablaProductos.ItemsSource = result;
            else
                MostrarToastMessage("Error", "Lo sentimos, el servidor no respondió correctamente, " +
                    "si el problema persiste, favor de contactar a soporte técnico");
        }

        private EProductoComprado ConvertirAProductoSeleccionado(EProducto producto)
        {
            return new EProductoComprado()
            {
                CodigoProductoVenta = producto.Codigo,
                Cantidad = 1,
                Precio = producto.PrecioVenta,
                CodigoReceta = producto.CodigoReceta,
                PrecioCompra = producto.PrecioCompra,
                PrecioVenta = producto.PrecioVenta,
                stock = producto.Cantidad,
                Nombre = producto.Nombre,
                Foto = producto.Foto,
                Descripcion = producto.Descripcion,
                Restricciones = producto.Restricciones
    };
        }

        private bool EstaAgregado(int id)
        {
            bool Agregado = false;
            foreach (EProductoComprado producto in TablaProductosSeleccionados.Items)
            {
                if (producto.CodigoProductoVenta == id)
                    Agregado = true;
            }
            return Agregado;
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

        private void CambiarCantidades(object sender, DataTransferEventArgs e)
        {
            string sentece = "";
            foreach (EProductoComprado producto in TablaProductosSeleccionados.Items)
            {
                sentece += $"{producto.Nombre}, \n";
            }
            MessageBox.Show(sentece + "aaaa");
        }

        private void CambiarCantidades(object sender, KeyEventArgs e)
        {
            foreach (EProductoComprado producto in TablaProductosSeleccionados.Items)
            {
                producto.Precio = producto.Cantidad * producto.PrecioVenta;
            }
        }

        private void ActivarCliente(object sender, RoutedEventArgs e)
        {

        }

        private void DesactivarCliente(object sender, RoutedEventArgs e)
        {

        }
    }
}
