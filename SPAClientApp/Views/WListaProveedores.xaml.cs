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
    /// Lógica de interacción para WListaProveedores.xaml
    /// </summary>
    public partial class WListaProveedores : Window
    {
        //private readonly ProveedorService client = new ProveedorService();
        private WHome HomeWindow { get; set; }
        private Notifier notifier;
        private string Status { get; set; } = string.Empty;
        private string Valor { get; set; } = string.Empty;
        private DateTime Tiempo { get; set; }
        private static bool IsClosed { get; set; } = false;
        private string CriterioSeleccionado { get; set; } = "Todos";
        private static WListaProveedores CurrentWindow { get; set; } = null;

        public WListaProveedores(WHome home)
        {
            InitializeComponent();
            HomeWindow = home;
            ConfigurarToastNotifier(GetWindow(this), 3);
            IsClosed = false;
        }

        public static WListaProveedores GetWListaProveedores(WHome window = null)
        {
            if (IsClosed || CurrentWindow == null)
                return (CurrentWindow = new WListaProveedores(window));
            else
                return CurrentWindow;
        }

        private void BuscarProveedores(object sender, RoutedEventArgs e)
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
                var result = await client.GetProveedoresAsync(CriterioSeleccionado, Valor, Status);
                ActualizarTablaProveedores(result.ToList());
            }
            catch (ArgumentException ae)
            {
                MostrarToastMessage("Warning", ae.Message);
            }
        }

        public void ValidarFiltro()             //CAMBIOS
        {
            if (Criterio.Text == "Nombre")
                if (string.IsNullOrEmpty(ValorBusqueda.Text) || string.IsNullOrEmpty(ValorBusqueda.Text.Trim()))
                    throw new ArgumentException("Debes escribir un nombre en el valor de búsqueda");
            if (Criterio.Text == "Categoria Insumo")
                if (string.IsNullOrEmpty(ValorBusqueda.Text) || string.IsNullOrEmpty(ValorBusqueda.Text.Trim()))
                    throw new ArgumentException("Debes escribir un insumo en el valor de búsqueda");
            if (Criterio.Text == "RFC")
                if (string.IsNullOrEmpty(ValorBusqueda.Text) || string.IsNullOrEmpty(ValorBusqueda.Text.Trim()))
                    throw new ArgumentException("Debes escribir un RFC en el valor de búsqueda");
        }

        private void ActualizarTablaProveedores(List<EProveedor> proveedores)
        {
            if (proveedores != null)
                tablaDatos.ItemsSource = proveedores;
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
                var proveedor = ((FrameworkElement)sender).DataContext as EProveedor;
                string mensaje = $"¿Seguro(a) que deseas dar de baja al Proveedor '{proveedor.Nombre}' seleccionado?";
                if (MostrarCuadroConfirmacion(mensaje))
                {
                   AnswerMessage response = client.ChangeProveedorStatus(proveedor.Clave, "Dado de baja");
                   if (response.Key >= 0)
                   {
                      MostrarToastMessage("Exito", "El Proveedor ha sido dado de baja");
                      var result = client.GetProveedores(CriterioSeleccionado, Valor, Status).ToList();
                      ActualizarTablaProveedores(result);
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
                var proveedor = ((FrameworkElement)sender).DataContext as EProveedor;
                string mensaje = $"¿Seguro(a) que deseas dar de alta el Proveedor '{proveedor.Nombre}' seleccionado?";
                if (MostrarCuadroConfirmacion(mensaje))
                {
                    AnswerMessage response = client.ChangeInsumoStatus(proveedor.Clave, "Activo");
                    if (response.Key >= 0)
                    {
                        MostrarToastMessage("Exito", "El Proveedor ha sido dado de alta");
                        RefrescarTablaProveedor();
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

        private void ConsultarProveedor(object sender, RoutedEventArgs e)
        {
            if (TablaActualizada())
            {
                var proveedor = ((FrameworkElement)sender).DataContext as EProveedor;
                var window = new WProveedor(this);
                window.ActivarModoLectura(proveedor);
                window.Show();
            }
            else
            {
                MostrarToastMessage("Warning", "La tabla ha sido refresacada debido a que el tiempo de actividad superó los 5 min.");
            }
        }

        private void ConsultarDirecciones(object sender, RoutedEventArgs e)
        {
            ///
        }

        private void Modificar(object sender, RoutedEventArgs e)
        {
            if (TablaActualizada())
            {
                var proveedor = ((FrameworkElement)sender).DataContext as EProveedor;
                var window = new WProveedor(this);
                window.ActivarModoEdicion("Actualizacion", proveedor);
                window.Show();
            }
            else
            {
                MostrarToastMessage("Warning", "La tabla ha sido refresacada debido a que el tiempo de actividad superó los 5 min.");
            }
        }

        private void AgregarProveedor(object sender, RoutedEventArgs e)
        {
            var window = new WProveedor(this);
            window.ActivarModoEdicion("Registro", new EProveedor());
            window.Show();
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void RefrescarTablaProveedor()
        {
            var result = client.GetProveedorList(CriterioSeleccionado, Valor, Status).ToList();
            ActualizarTablaProveedores(result);
        }


        private bool TablaActualizada()
        {
            bool isCurrent = true;
            if ((DateTime.Now - Tiempo).TotalMinutes > 5)
            {
                isCurrent = false;
                RefrescarTablaProveedor();
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
