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

namespace SPAClientApp
{
    /// <summary>
    /// Interaction logic for WListaProductos.xaml
    /// </summary>
    public partial class WListaProductos : Window
    {
        private readonly ProductosServiceClient client = new ProductosServiceClient();
        private Notifier notifier;
        private string Status { get; set; } = string.Empty;
        private DateTime Fecha { get; set; } = DateTime.Now;
        private string Valor { get; set; } = string.Empty;
        private string CriterioSeleccionado { get; set; } = "Todos";
        private WHome HomeWindow { get; set; }
        private DateTime Tiempo { get; set; }


        public WListaProductos(WHome homeWindow)
        {
            InitializeComponent();
            HomeWindow = homeWindow;
            ConfigurarToastNotifier(this, 3);
        }

        private void BuscarProductos(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarFiltro();
                CriterioSeleccionado = Criterio.Text;
                Status = ((bool)CheckBoxActivos.IsChecked) ? "Activo" : "Dado de baja";
                Fecha = ((bool)CheckBoxConFecha.IsChecked) ? Convert.ToDateTime(FechaLimite.Text) : Convert.ToDateTime("1/1/1900 00:00:00");
                Valor = (Criterio.Text == "Nombre") ? $"%{ValorBusqueda.Text}%" : ValorBusqueda.Text;
                if (Criterio.Text == "Todos")
                    Valor = null;
                string criterio = Criterio.Text;
                var result = client.GetProductosList(CriterioSeleccionado, Valor, Fecha, Status).ToList();
                ActualizarTablaProductos(result);
            }
            catch (ArgumentException ae)
            {
                MostrarToastMessage("Warning", ae.Message);
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

        private void ActualizarTablaProductos(List<EProducto> productos)
        {
            if (productos != null)
                tablaDatos.ItemsSource = productos;
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

        public void RefrescarTabla()
        {
            var result = client.GetProductosList(CriterioSeleccionado, Valor, Fecha, Status).ToList();
            ActualizarTablaProductos(result);
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

        private void LimpiarFiltro(object sender, RoutedEventArgs e)
        {
            ValorBusqueda.Text = String.Empty;
            FechaLimite.Text = String.Empty;
            CheckBoxActivos.IsChecked = true;
            CheckBoxConFecha.IsChecked = false;
            Criterio.SelectedIndex = 0;
        }

        public void ValidarFiltro()
        {
            if ((bool)CheckBoxConFecha.IsChecked)
                if (string.IsNullOrEmpty(FechaLimite.Text))
                    throw new ArgumentException("Debes indicar una fecha de límite de búaqueda");
            if (Criterio.Text == "Código")
                if (string.IsNullOrEmpty(ValorBusqueda.Text) || string.IsNullOrEmpty(ValorBusqueda.Text.Trim()) || !int.TryParse(ValorBusqueda.Text, out _))
                    throw new ArgumentException("El valor de búsquda debe ser un número entero");
            if (Criterio.Text == "Nombre")
                if (string.IsNullOrEmpty(ValorBusqueda.Text) || string.IsNullOrEmpty(ValorBusqueda.Text.Trim()))
                    throw new ArgumentException("Debes escribir un nombre en el valor de búsqueda");
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Consultar(object sender, RoutedEventArgs e)
        {
            if (TablaActualizada())
            {
                var producto = ((FrameworkElement)sender).DataContext as EProducto;
                var window = new WProducto(this);
                window.ActivarModoLectura(producto);
                window.Show();
            }
            else
            {
                MostrarToastMessage("Warning", "La tabla ha sido refresacada debido a que el tiempo de actividad superó los 5 min.");
            }
        }

        private bool TablaActualizada()
        {
            bool isCurrent = true;
            if ((DateTime.Now - Tiempo).TotalMinutes > 5)
            {
                isCurrent = false;
                RefrescarTabla();
            }
            return isCurrent;
        }

        private bool MostrarCuadroConfirmacion(string message)
        {
            MessageBoxResult boxResult = MessageBox.Show(message, "Advertencia", MessageBoxButton.YesNoCancel);
            return MessageBoxResult.Yes == boxResult;
        }

        private void Registrar(object sender, RoutedEventArgs e)
        {
            WProducto productoWindow = new WProducto(this);
            productoWindow.ActivarModoEdicion("Registro", new EProducto());
            productoWindow.Show();
        }

        private void Modificar(object sender, RoutedEventArgs e)
        {
            if (TablaActualizada())
            {
                var producto = ((FrameworkElement)sender).DataContext as EProducto;
                var window = new WProducto(this);
                window.ActivarModoEdicion("Actualización", producto);
                window.Show();
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
                var producto = ((FrameworkElement)sender).DataContext as EProducto;
                string mensaje = $"¿Seguro(a) que deseas dar de alta el Producto {producto.Nombre} seleccionado?";
                if (MostrarCuadroConfirmacion(mensaje))
                {
                    AnswerMessage response = client.ChangeProductoStatus(producto.Codigo, "Activo");
                    if (response.Key >= 0)
                    {
                        MostrarToastMessage("Exito", "El Producto ha sido dado de alta");
                        RefrescarTabla();
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

        private void DarDeBaja(object sender, RoutedEventArgs e)
        {
            if (TablaActualizada())
            {
                var producto = ((FrameworkElement)sender).DataContext as EProducto;
                string mensaje = $"¿Seguro(a) que deseas dar de baja el Producto '{producto.Nombre}' seleccionado?";
                if (MostrarCuadroConfirmacion(mensaje))
                {
                    AnswerMessage response = client.ChangeProductoStatus(producto.Codigo, "Dado de baja");
                    if (response.Key >= 0)
                    {
                        MostrarToastMessage("Exito", "El Producto ha sido dado de baja");
                        var result = client.GetProductosList(CriterioSeleccionado, Valor, Fecha, Status).ToList();
                        ActualizarTablaProductos(result);
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

        private void ElegirCriterio(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
