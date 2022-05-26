using SPAClientApp.ClientesService;
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
        private List<string> Clientes { get; set; }
        public ECliente Cliente { get; set; }
        private EPedidoCliente Pedido { get; set; }
        private ClientePage ClienteExistPage { get; set; }  
        private ClientePage NewClientPage { get; set; }
        private Notifier notifier;
        private static WPedidoCliente PedidoWindow = null;
        private readonly PedidosClientesServiceClient client = new PedidosClientesServiceClient();
        private readonly ClientesServiceClient clientCliente = new ClientesServiceClient();
        private WListaPedidosClientes Parent { get; set; }

        private WPedidoCliente()
        {
            InitializeComponent();
            ConfigurarToastNotifier(this, 3);
            CargarProductos();
            Frame.Content = (ClienteExistPage = new ClientePage(this));
            SecondFrame.Content = (NewClientPage = new ClientePage(this));
            NewClientPage.ObtenerCmbBoxDirecciones().Visibility = Visibility.Collapsed;
            NewClientPage.ObtenerTogglerAgregarDireccion().Visibility = Visibility.Collapsed;
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
            ActualizarTotal();
        }

        private void ActualizarTotal()
        {
            if (TablaProductosSeleccionados.Items.Count > 0)
            {
                double total = 0;
                foreach (EProductoComprado producto in TablaProductosSeleccionados.Items)
                {
                    total += producto.Precio;
                }
                Total.Content = total.ToString();
            }
            else
            {
                Total.Content = "0";
            }
        }

        private void RemoverProducto(object sender, RoutedEventArgs e)
        {
            var producto = ((FrameworkElement)sender).DataContext as EProductoComprado;
            TablaProductosSeleccionados.Items.Remove(producto);
            ActualizarTotal();
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
            if (tipo == "Info")
                notifier.ShowInformation(mensaje);
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

        private void CambiarCantidades(object sender, KeyEventArgs e)
        {
            foreach (EProductoComprado producto in TablaProductosSeleccionados.Items)
            {
                producto.Precio = producto.Cantidad * producto.PrecioVenta;
                if (producto.Cantidad > producto.stock)
                    producto.Cantidad = producto.stock;
            }
            ActualizarTotal();
        }

        private void ActivarCliente(object sender, RoutedEventArgs e)
        {
            ClientSection.IsEnabled = true;
        }

        private void DesactivarCliente(object sender, RoutedEventArgs e)
        {
            ClientSection.IsEnabled = false;
        }

        private async void VerClientes(object sender, EventArgs e)
        {
            var result = await clientCliente.GetClientesListAsync();
            Clientes = result.ToList();
            ClientesCbmbx.ItemsSource = Clientes;
            ClientesCbmbx.IsDropDownOpen = true;
        }

        private async void SeleccionarCliente(object sender, SelectionChangedEventArgs e)
        {
            if (ClientesCbmbx.SelectedItem != null)
            {
                int clave = Convert.ToInt32(ClientesCbmbx.SelectedItem.ToString().Substring(0, ClientesCbmbx.SelectedItem.ToString().IndexOf(':')));
                Cliente = await clientCliente.GetClienteAsync(clave);
                MostrarCleinteInfo(Cliente);
            }
        }

        private void MostrarCleinteInfo(ECliente cliente)
        {
            if (cliente != null)
            {
                Cliente = cliente;
                ClienteExistPage.MostrarClienteInfo(cliente);
            }
        }

        private async void RealizarPedido(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarPedido();
                PrepararPedidoCliente();
                PedidosClientesService.AnswerMessage response;
                if (Convert.ToBoolean(CheckBoxPedido.IsChecked))
                {
                    var cliente = await GuardarCliente();
                    int idDireccion = ClienteExistenteTab.IsSelected ? ClienteExistPage.ObtenerIdDireccion(ClienteExistPage.Direccion, 
                        cliente.Direcciones.ToList()) : NewClientPage.ObtenerIdDireccion(NewClientPage.Direccion, cliente.Direcciones.ToList());
                    response = await client.AddPedidoClienteAsync(Pedido, ObtenerProductosSeleccionados().ToArray(), cliente.Id, idDireccion);
                }
                else
                {
                    response = await client.AddPedidoClienteAsync(Pedido, ObtenerProductosSeleccionados().ToArray(), -1, -1);
                }
                if (response.Key > 0)
                    MostrarToastMessage("Exito", "El pedido se ha realizado exitosamente");
                else
                    MostrarToastMessage("Error", response.Message);
            }
            catch (Exception ex)
            {
                MostrarToastMessage("Advertencia", ex.Message);
            }
        }

        private async Task<ECliente> GuardarCliente()
        {
            ECliente cliente;
            if (ClienteExistenteTab.IsSelected)
                cliente = await ClienteExistPage.ActualizarCliente();
            else
                cliente = await NewClientPage.RegistrarCliente();
            return cliente;
        }


        private void PrepararPedidoCliente()
        {
            if (Pedido == null)
                Pedido = new EPedidoCliente();
            Pedido.CostoTotal = Convert.ToDouble(Total.Content);
            Pedido.Status = "Ordenado";
            Pedido.Solicitud = DateTime.Now;
            Pedido.Entrega = DateTime.Now;
            Pedido.Cantidad = 1;
            Pedido.TipoPedido = Convert.ToBoolean(CheckBoxPedido.IsChecked) ? "A domicilio" : "En establecimiento";
            Pedido.NumeroProductos = TablaProductosSeleccionados.Items.Count;
        }

        private async void ValidarPedido()
        {
            if (TablaProductosSeleccionados.Items.Count == 0)
                throw new FormatException("Debes seleccionar al menos 1 producto para comprar");
            var result = await client.CheckProductosSeleccionadosAsync(ObtenerProductosSeleccionados().ToArray());
            if (!string.IsNullOrEmpty(result))
                throw new FormatException(result);
            if (Convert.ToBoolean(CheckBoxPedido.IsChecked))
            {
                if (ClienteExistenteTab.IsSelected)
                    ClienteExistPage.ValidarCliente("Actualización");
                else
                    NewClientPage.ValidarCliente("Registro");
            }
        }

        private List<EProductoComprado> ObtenerProductosSeleccionados()
        {
            List<EProductoComprado> productos = new List<EProductoComprado>();
            foreach (EProductoComprado p in TablaProductosSeleccionados.Items)
            {
                productos.Add(p);
            }
            return productos;
        }

        private void BuscarCliente(object sender, KeyEventArgs e)
        {
            if (ClientesCbmbx.Items.Count > 0)
            {
                if (string.IsNullOrEmpty(ClientesCbmbx.Text))
                {
                    ClientesCbmbx.ItemsSource = Clientes;
                }
                else
                {
                    ClientesCbmbx.IsDropDownOpen = true;
                    ClientesCbmbx.ItemsSource = Clientes.Where(c => c.Contains(ClientesCbmbx.Text));
                }
            }
        }

        private void CancelarOperacion(object sender, RoutedEventArgs e)
        {
            if (MostrarCuadroConfirmacion("¿Estás seguro(a) que deseas cancelar el registro del pedido?"))
                Close();
        }

        private bool MostrarCuadroConfirmacion(string message)
        {
            MessageBoxResult boxResult = MessageBox.Show(message, "Advertencia", MessageBoxButton.YesNoCancel);
            return MessageBoxResult.Yes == boxResult;
        }
    }
}
