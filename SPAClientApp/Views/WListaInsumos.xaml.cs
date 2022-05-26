using SPAClientApp.InsumosService;
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
    public partial class WListaInsumos : Window
    {
        private readonly InsumosServiceClient  client = new InsumosServiceClient();
        private Notifier notifier;
        private string Status { get; set; } = string.Empty; 
        private DateTime Fecha { get; set; } = DateTime.Now;
        private string Valor { get; set; } = string.Empty;  
        private string CriterioSeleccionado { get; set; } = "Todos";   
        private WHome HomeWindow { get; set; }
        private DateTime Tiempo { get; set; }
        private static bool IsClosed { get; set; } = false;
        private static WListaInsumos CurrentWindow { get; set; } = null;  

        private WListaInsumos(WHome home)
        {
            InitializeComponent();
            HomeWindow = home;
            ConfigurarToastNotifier(GetWindow(this), 3);
            IsClosed = false;
        }

        public static WListaInsumos GetWListaInsumos(WHome window = null)
        {
            if (IsClosed || CurrentWindow == null)
                return (CurrentWindow = new WListaInsumos(window));
            else
                return CurrentWindow;  
        }

        private async void BuscarInsumos(object sender, RoutedEventArgs e)
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
                var result = await client.GetInsumosListAsync(CriterioSeleccionado, Valor, Fecha, Status);
                ActualizarTablaInsumos(result.ToList());   
            }
            catch (ArgumentException ae)
            {
                MostrarToastMessage("Warning", ae.Message);
            }
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

        private void SeleccionarCriterio(object sender, MouseButtonEventArgs e)
        {
            if (Criterio.Text == "Todos")
                ValorBusqueda.IsEnabled = false;
            else
                ValorBusqueda.IsEnabled = true;
        }

        private void ActualizarTablaInsumos(List<EInsumo> insumos)
        {
            if (insumos != null)
                tablaDatos.ItemsSource = insumos;
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

        private void Salir(object sender, RoutedEventArgs e)
        {
            IsClosed = true;
            Close();
        }

        private void DarDeBaja(object sender, RoutedEventArgs e)
        {
            if (TablaActualizada())
            {
                var insumo = ((FrameworkElement)sender).DataContext as EInsumo;
                if (!EsUsadoEnReceta(insumo.Codigo))
                {
                    string mensaje = $"¿Seguro(a) que deseas dar de baja el Insumo '{insumo.Nombre}' seleccionado?";
                    if (MostrarCuadroConfirmacion(mensaje))
                    {
                        AnswerMessage response = client.ChangeInsumoStatus(insumo.Codigo, "Dado de baja");
                        if (response.Key >= 0)
                        {
                            MostrarToastMessage("Exito", "El Insumo ha sido dado de baja");
                            var result = client.GetInsumosList(CriterioSeleccionado, Valor, Fecha, Status).ToList();
                            ActualizarTablaInsumos(result);
                        }
                        else
                        {
                            MostrarToastMessage("Error", "Lo sentimos, ha ocurrido un error en el servidor, favor de contactar a soporte técnico");
                        }
                    }
                }
                else
                {
                    MostrarToastMessage("Warning", "Lo sentimos, el Insumo es parte de al menos 1 receta en el sistema");
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
                var insumo = ((FrameworkElement)sender).DataContext as EInsumo;
                string mensaje = $"¿Seguro(a) que deseas dar de alta el Insumo '{insumo.Nombre}' seleccionado?";
                if (MostrarCuadroConfirmacion(mensaje))
                {
                    AnswerMessage response = client.ChangeInsumoStatus(insumo.Codigo, "Activo");
                    if (response.Key >= 0)
                    {
                        MostrarToastMessage("Exito", "El Insumo ha sido dado de alta");
                        RefrescarTablaInsumos();
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

        public void RefrescarTablaInsumos()
        {
            var result = client.GetInsumosList(CriterioSeleccionado, Valor, Fecha, Status).ToList();
            ActualizarTablaInsumos(result);
        }

        private void Consultar(object sender, RoutedEventArgs e)
        {
            if (TablaActualizada())
            {
                var insumo = ((FrameworkElement)sender).DataContext as EInsumo;
                var window = new WInsumo(this);
                window.ActivarModoLectura(insumo);
                window.Show();
            }
            else
            {
                MostrarToastMessage("Warning", "La tabla ha sido refresacada debido a que el tiempo de actividad superó los 5 min.");
            }
        }

        private void Modificar(object sender, RoutedEventArgs e)
        {
            if (TablaActualizada())
            {
                var insumo = ((FrameworkElement)sender).DataContext as EInsumo;
                var window = new WInsumo(this);
                window.ActivarModoEdicion("Actualizacion", insumo);
                window.Show();
            }
            else
            {
                MostrarToastMessage("Warning", "La tabla ha sido refresacada debido a que el tiempo de actividad superó los 5 min.");
            }
        }

        private bool MostrarCuadroConfirmacion(string message)
        {
            MessageBoxResult boxResult = MessageBox.Show(message, "Advertencia", MessageBoxButton.YesNoCancel);
            return MessageBoxResult.Yes == boxResult;
        }

        private void AgregarInsumo(object sender, RoutedEventArgs e)
        {
            var window = new WInsumo(this);
            window.ActivarModoEdicion("Registro", new EInsumo());
            window.Show();
        }

        private void LimpiarFiltro(object sender, RoutedEventArgs e)
        {
            ValorBusqueda.Text = String.Empty;
            FechaLimite.Text = String.Empty;    
            CheckBoxActivos.IsChecked = true;
            CheckBoxConFecha.IsChecked = false;
            Criterio.SelectedIndex = 0;
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

        private bool EsUsadoEnReceta(int codigoInsumo)
        {
            return client.IsUsedInReceta(codigoInsumo);
        }

        private bool TablaActualizada()
        {
            bool isCurrent = true;
            if((DateTime.Now - Tiempo).TotalMinutes > 5)
            {
                isCurrent = false;
                RefrescarTablaInsumos();
            }
            return isCurrent;
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            IsClosed = true;
            Close();
        }

        private void SeleccionarCriterio(object sender, EventArgs e)
        {
            var cbBox = sender as ComboBox;
            cbBox.IsDropDownOpen = false;
            busquedaLbl.Content = $"Ingresa el {cbBox.Text}";
            ValorBusqueda.IsEnabled = true;
            if (cbBox.Text == "Todos")
            {
                busquedaLbl.Content = "No requerido";
                ValorBusqueda.Text = string.Empty;
                ValorBusqueda.IsEnabled = false;
            }
        }
    }
}

