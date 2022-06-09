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

        //private readonly ProveedorService client = new ProveedorService();
        private WHome HomeWindow { get; set; }
        private Notifier notifier;
        private string Status { get; set; } = string.Empty;
        private string Valor { get; set; } = string.Empty;
        private DateTime Tiempo { get; set; }
        private static bool IsClosed { get; set; } = false;
        private string CriterioSeleccionado { get; set; } = "Todos";
        private static WListaClientes CurrentWindow { get; set; } = null;


        public WListaClientes(WHome home)
        {
            InitializeComponent();
            HomeWindow = home;
            ConfigurarToastNotifier(GetWindow(this), 3);
            IsClosed = false;

        }

        public static WListaClientes GetWListaClientes(WHome window = null)
        {
            if (IsClosed || CurrentWindow == null)
                return (CurrentWindow = new WListaClientes(window));
            else
                return CurrentWindow;
        }

        private void BuscarClientes(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarFiltro();
                CriterioSeleccionado = Criterio.Text;
                Status = ((bool)CheckBoxActivos.IsChecked) ? "Activo" : "Dado de baja";
                Valor = (Criterio.Text == "Nombre") ? $"%{ValorBusqueda.Text}%" : ValorBusqueda.Text;
                if (Criterio.Text == "Todos")
                    Valor = null;
                string criterio = Criterio.Text;
                var result = await client.GetClientesAsync(CriterioSeleccionado, Valor, Status);
                ActualizarTablaClientes(result.ToList());
            }
            catch (ArgumentException ae)
            {
                MostrarToastMessage("Warning", ae.Message);
            }
        }

        public void ValidarFiltro()
        {
            if (Criterio.Text == "Nombre")
                if (string.IsNullOrEmpty(ValorBusqueda.Text) || string.IsNullOrEmpty(ValorBusqueda.Text.Trim()))
                    throw new ArgumentException("Debes escribir un nombre en el valor de búsqueda");
            if (Criterio.Text == "Telefono")
                if (string.IsNullOrEmpty(ValorBusqueda.Text) || string.IsNullOrEmpty(ValorBusqueda.Text.Trim()) || !int.TryParse(ValorBusqueda.Text, out _))
                    throw new ArgumentException("El valor de búsquda debe ser un número valido");
        }

        private void ActualizarTablaClientes(List<ECliente> clientes)
        {
            if (clientes != null)
                tablaDatos.ItemsSource = clientes;
            else
                MostrarToastMessage("Error", "Hubo un error en el servidor, si los " +
                    "problemas persisten, favor de contactar a soporte técnico");
            if ((bool)CheckBoxActivos.IsChecked)
            {
                ColumnActive.Visibility = Visibility.Collapsed;
                ColumnEliminate.Visibility = Visibility.Visible;
            }
            else
            {
                ColumnActive.Visibility = Visibility.Visible;
                ColumnEliminate.Visibility = Visibility.Collapsed;
            }
            Tiempo = DateTime.Now;
        }

        private void SeleccionarCriterio(object sender, MouseButtonEventArgs e)
        {
            if (Criterio.Text == "Todos")
                ValorBusqueda.IsEnabled = false;
            else
                ValorBusqueda.IsEnabled = true;
        }

        private void LimpiarFiltro(object sender, RoutedEventArgs e)
        {
            ValorBusqueda.Text = String.Empty;
            CheckBoxActivos.IsChecked = true;
            Criterio.SelectedIndex = 0;
        }

        private void DarDeBaja(object sender, RoutedEventArgs e)
        {
            if (TablaActualizada())
            {
                var cliente = ((FrameworkElement)sender).DataContext as ECliente;
                string mensaje = $"¿Seguro(a) que deseas dar de baja al Proveedor '{cliente.Nombre}' seleccionado?";
                if (MostrarCuadroConfirmacion(mensaje))
                {
                    AnswerMessage response = client.ChangeClienteStatus(cliente.id, "Dado de baja");
                    if (response.Key >= 0)
                    {
                        MostrarToastMessage("Exito", "El Proveedor ha sido dado de baja");
                        var result = client.GetClientes(CriterioSeleccionado, Valor, Status).ToList();
                        ActualizarTablaClientes(result);
                    }
                    else
                    {
                        MostrarToastMessage("Error", "Lo sentimos, ha ocurrido un error en el servidor, favor de contactar a soporte técnico");
                    }
                }
            }
            else
            {
                MostrarToastMessage("Warning", "La tabla ha sido refresacada debido a que el tiempo de actividad superó los 5 min.");
            }
        }

        private void Activar(object sender, RoutedEventArgs e)
        {
            if (TablaActualizada())
            {
                var cliente = ((FrameworkElement)sender).DataContext as EProveedor;
                string mensaje = $"¿Seguro(a) que deseas dar de alta el Proveedor '{cliente.Nombre}' seleccionado?";
                if (MostrarCuadroConfirmacion(mensaje))
                {
                    AnswerMessage response = proveedor.ChangeInsumoStatus(cliente.Id, "Activo");
                    if (response.Key >= 0)
                    {
                        MostrarToastMessage("Exito", "El Proveedor ha sido dado de alta");
                        RefrescarTablaCliente();
                    }
                    else
                    {
                        MostrarToastMessage("Error", "Lo sentimos, ha ocurrido un error en el servidor, favor de contactar a soporte técnico");
                    }
                }
            }
            else
            {
                MostrarToastMessage("Warning", "La tabla ha sido refresacada debido a que el tiempo de actividad superó los 5 min.");
            }
        }

        private void ConsultarCliente(object sender, RoutedEventArgs e)
        {
            if (TablaActualizada())
            {
                var cliente = ((FrameworkElement)sender).DataContext as ECliente;
                var window = new WCliente(this);
                window.ActivarModoLectura(cliente);
                window.Show();
            }
            else
            {
                MostrarToastMessage("Warning", "La tabla ha sido refresacada debido a que el tiempo de actividad superó los 5 min.");
            }
        }

        private void ConsultarDirecciones(object sender, RoutedEventArgs e)
        {
            //
        }

        private void Modificar(object sender, RoutedEventArgs e)
        {
            if (TablaActualizada())
            {
                var cliente = ((FrameworkElement)sender).DataContext as ECliente;
                var window = new WCliente(this);
                window.ActivarModoEdicion("Actualizacion", cliente);
                window.Show();
            }
            else
            {
                MostrarToastMessage("Warning", "La tabla ha sido refresacada debido a que el tiempo de actividad superó los 5 min.");
            }
        }


        private void AgregarCliente(object sender, RoutedEventArgs e)
        {
            //var window = new WCliente(HomeWindow);
            WCliente.GetWClient(HomeWindow, "Registro").Show();
            //window.ActivarModoEdicion("Registro", new ECliente());
            //window.Show();
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void RefrescarTablaCliente()
        {
            var result = client.GetProveedorList(CriterioSeleccionado, Valor, Status).ToList();
            ActualizarTablaClientes(result);
        }


        private bool TablaActualizada()
        {
            bool isCurrent = true;
            if ((DateTime.Now - Tiempo).TotalMinutes > 5)
            {
                isCurrent = false;
                RefrescarTablaCliente();
            }
            return isCurrent;
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
    }
}
