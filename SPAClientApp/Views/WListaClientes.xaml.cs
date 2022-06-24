using SPAClientApp.ClientesService;
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
    /// Lógica de interacción para WListaClientes.xaml
    /// </summary>
    public partial class WListaClientes : Window
    {
        private readonly ClientesServiceClient client = new ClientesServiceClient();
        //private readonly ProveedorService client = new ProveedorService();
        private WHome HomeWindow { get; set; }
        private Notifier notifier;
        private string Status { get; set; } = string.Empty;
        private string Valor { get; set; } = string.Empty;
        private static bool IsClosed { get; set; } = true;
        private static WListaClientes CurrentWindow { get; set; } = null;


        public WListaClientes(WHome home)
        {
            InitializeComponent();
            HomeWindow = home;
            ConfigurarToastNotifier(GetWindow(this), 3);
            IsClosed = false;

        }

        private void DarDeBaja(object sender, RoutedEventArgs e)
        {
            if(MostrarCuadroConfirmacion("¿Estas seguro de que quieres dar de baja al usaurio seleccionado?"))
            {
                try
                {
                    var cliente = ((FrameworkElement)sender).DataContext as ECliente;
                    client.ChangeStatusCliente(cliente.Id, "Dado de baja");
                    MostrarToastMessage("Exito", "Cliente dado de baja");
                }
                catch (Exception)
                {
                    MostrarToastMessage("Error", "Lo sentimos, algo ha salido mal");
                }
                RefrescarTabla();
            }
        }

        private void Activar(object sender, RoutedEventArgs e)
        {
            if(MostrarCuadroConfirmacion("¿Eatas seguro de que quieres dar de alta al usuario seleccionado?"))
            {
                try
                {
                    var cliente = ((FrameworkElement)sender).DataContext as ECliente;
                    client.ChangeStatusCliente(cliente.Id, "Activo");
                    MostrarToastMessage("Exito", "Cliente dado de alta");
                }
                catch (Exception)
                {
                    MostrarToastMessage("Error", "Lo sentimos, algo ha salido mal");
                }
                RefrescarTabla();
            }
        }

        private void ConsultarCliente(object sender, RoutedEventArgs e)
        {
            var cliente = ((FrameworkElement)sender).DataContext as ECliente;
            new WCliente(this, "Consulta", cliente).Show();
        }

        private void Modificar(object sender, RoutedEventArgs e)
        {
            var cliente = ((FrameworkElement)sender).DataContext as ECliente;
            new WCliente(this, "Actualización", cliente).Show();
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool MostrarCuadroConfirmacion(string message)
        {
            MessageBoxResult boxResult = MessageBox.Show(message, "Advertencia", MessageBoxButton.YesNoCancel);
            return MessageBoxResult.Yes == boxResult;
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

        private void ConfigurarToastNotifier(Window ventana, int segundos)
        {
            notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(parentWindow: ventana, corner: Corner.BottomRight, offsetX: 10, offsetY: 10);
                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(notificationLifetime: TimeSpan.FromSeconds(segundos), maximumNotificationCount: MaximumNotificationCount.FromCount(segundos));
                cfg.Dispatcher = Application.Current.Dispatcher;
            });
        }

        private void RefrescarTabla()
        {
            var clientes = client.GetClientes(Status, Valor);
            tablaDatos.ItemsSource = clientes.ToList();
        }

        private void BuscarClientes(object sender, RoutedEventArgs e)
        {
            Valor = string.IsNullOrEmpty(ValorBusqueda.Text) ? null : ValorBusqueda.Text;
            Status = (bool)soloActivos.IsChecked ? "Activo" : "Dado de baja";
            var clientes = client.GetClientes(Status, Valor);
            tablaDatos.ItemsSource = clientes.ToList();
        }
    }
}
