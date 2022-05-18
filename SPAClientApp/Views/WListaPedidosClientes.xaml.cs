using SPAClientApp.PedidosClientesService;
using SPAClientApp.Views;
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
    /// Interaction logic for WListaPedidosClientes.xaml
    /// </summary>
    public partial class WListaPedidosClientes : Window
    {
        private const int INTERVAL = 10000; //1/6 minuto
        private readonly PedidosClientesServiceClient client = new PedidosClientesServiceClient();
        private Notifier notifier;
        private WHome HomeWindow { get; set; }
        private readonly Dictionary<string, DataGrid> tablas;
        readonly System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            

        public WListaPedidosClientes(WHome home)
        {
            InitializeComponent();
            HomeWindow = home;
            ConfigurarToastNotifier(this, 3);
            tablas = new Dictionary<string, DataGrid>()
            {
                {"Ordenado", TablaOrden },
                {"En preparación", TablaEnPreparacion },
                {"Preparado", TablaPreparado },
                {"Entregado", TablaEntregado },
                {"Cancelado", TablaCancelado }
            };
            ActualizarTabla();
            timer.Tick += new EventHandler(RefrescarTabla);
            timer.Interval = INTERVAL;
            timer.Start();
        }

        private void RefrescarTabla(object sender, EventArgs e)
        {
            ActualizarTabla();
        }

        private void ActualizarTabla()
        {
            CleanTables();
            var result = client.GetCommonPedidosList();
            result.ToList().ForEach(element => { tablas[element.Status].Items.Add(element); });
            BadgedOrdenes.Badge = TablaOrden.Items.Count;
            BadgedEnPreparacion.Badge = TablaEnPreparacion.Items.Count;
            BadgedPreparado.Badge = TablaPreparado.Items.Count;
            BadgedEntregado.Badge = TablaEntregado.Items.Count;
            BadgedCancelado.Badge = TablaCancelado.Items.Count;
        }

        private void CleanTables()
        {
            DataGrid tabla = null;
            foreach (var item in tablas.Keys)
            {
                if(item != "Entregao" && item != "Cancelado")
                    tablas.TryGetValue(item, out tabla);
                if (tabla != null)
                    tabla.Items.Clear();
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
                ConfigurarToastNotifier(HomeWindow, 5);
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

        private void Salir(object sender, RoutedEventArgs e)
        {
            timer.Stop();   
            Close();
        }

        private void RegistrarPedido(object sender, RoutedEventArgs e)
        {
            WPedidoCliente.GetWListaPedidosClientesWindow(this).Show();
        }

        private async void PrepararPedido(object sender, RoutedEventArgs e)
        {
            var pedido = ((FrameworkElement)sender).DataContext as EPedidoCliente;
            if(MostrarCuadroConfirmacion("Las cantidades de los productos serán actualizadas automáticamente sin cancelación, ¿Seguro que deseas continuar?"))
            {
                var result = await client.ChangeStatusPedidoClienteAsync(pedido.Codigo, "Ordenado");
                if (result.Key > 0)
                    MostrarToastMessage("Exito", "Las cantidades de productos han sido actualizadas");
                else
                    MostrarToastMessage("Error", result.Message);
                ActualizarTabla();
            }
        }

        private bool MostrarCuadroConfirmacion(string message)
        {
            MessageBoxResult boxResult = MessageBox.Show(message, "Advertencia", MessageBoxButton.YesNoCancel);
            return MessageBoxResult.Yes == boxResult;
        }

        private async void TerminarPreparacion(object sender, RoutedEventArgs e)
        {
            var pedido = ((FrameworkElement)sender).DataContext as EPedidoCliente;
            if (MostrarCuadroConfirmacion("El pedido pasará a preparado sin posibilidad de volver a preparación, ¿Seguro que deseas continuar?"))
            {
                var result = await client.ChangeStatusPedidoClienteAsync(pedido.Codigo, "En preparación");
                if (result.Key > 0)
                    MostrarToastMessage("Exito", "Platillo preparado");
                else
                    MostrarToastMessage("Error", result.Message);
                ActualizarTabla();
            }
        }

        private async void EntregarPedido(object sender, RoutedEventArgs e)
        {
            var pedido = ((FrameworkElement)sender).DataContext as EPedidoCliente;
            if (MostrarCuadroConfirmacion("El pedido pasará a entregado sin posibilidad de volver a preparación, ¿Seguro que deseas continuar?"))
            {
                var result = await client.ChangeStatusPedidoClienteAsync(pedido.Codigo, "Preparado");
                if (result.Key > 0)
                    MostrarToastMessage("Exito", "Platillo entregado");
                else
                    MostrarToastMessage("Error", result.Message);
                ActualizarTabla();
            }
        }

        private async void CancelarPedido(object sender, RoutedEventArgs e)
        {
            var pedido = ((FrameworkElement)sender).DataContext as EPedidoCliente;
            if (MostrarCuadroConfirmacion("El pedido pasará a entregado sin posibilidad de volver a preparación, ¿Seguro que deseas continuar?"))
            {
                var result = await client.ChangeStatusPedidoClienteAsync(pedido.Codigo, "Cancelado");
                if (result.Key > 0)
                    MostrarToastMessage("Exito", "Info cancelado");
                else
                    MostrarToastMessage("Error", result.Message);
                ActualizarTabla();
            }
        }

        private void ConsultarPedido(object sender, RoutedEventArgs e)
        {
            var pedido = ((FrameworkElement)sender).DataContext as EPedidoCliente;
            var window = new WPedidoClienteConsulta(this, pedido);
            window.Show();
        }
    }
}
