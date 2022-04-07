using MaterialDesignThemes.Wpf.Transitions;
using SPAClientApp.InsumosService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace SPAClientApp
{
    /// <summary>
    /// Interaction logic for UserControl2.xaml
    /// </summary>
    public partial class WListaInsumos : UserControl
    {
        private readonly InsumosServiceClient client = new InsumosServiceClient();
        private InsumosContenedor Contenedor { get; set; }
        private readonly Notifier notifier;

        public WListaInsumos()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                Contenedor = Window.GetWindow(this) as InsumosContenedor;
            };
            notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(parentWindow: Window.GetWindow(this), corner: Corner.BottomRight, offsetX: 10, offsetY: 10);
                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(notificationLifetime: TimeSpan.FromSeconds(3), maximumNotificationCount: MaximumNotificationCount.FromCount(3));
                cfg.Dispatcher = Application.Current.Dispatcher;
            });
        }

        private void BuscarInsumos(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarFiltro();
                string status = ((bool)CheckBoxActivos.IsChecked) ? "Activo" : "Dado de baja";
                DateTime fecha = ((bool)CheckBoxConFecha.IsChecked) ? Convert.ToDateTime(FechaLimite.Text) : Convert.ToDateTime("1/1/1900 00:00:00");
                string valor = (Criterio.Text == "Nombre") ? $"%{ValorBusqueda.Text}%" : ValorBusqueda.Text;
                if (Criterio.Text == "Todos")
                    valor = null;
                string criterio = Criterio.Text;
                var result = client.GetInsumosList(criterio, valor, fecha, status).ToList();
                ActualizarTablaInsumos(result); 
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

        private void LimpiarFiltro(object sender, RoutedEventArgs e)
        {
            CheckBoxConFecha.IsChecked = false;
            FechaLimite.Text = String.Empty;
            ValorBusqueda.Text = String.Empty;  
            Criterio.SelectedIndex = 1;
            CheckBoxActivos.IsChecked = true;
        }

        private void ConsultarInsumo(object sender, MouseButtonEventArgs e)
        {
            var item = (EInsumo)tablaDatos.SelectedItem;
            if (item != null)
            {
                Contenedor.Insumo = client.GetInsumosList("Código", item.Codigo.ToString(), item.Registro, item.Status).First();
                if (Contenedor.Insumo != null)
                {
                    Transitioner.MoveNextCommand.Execute(1, this);
                    Contenedor.InsumoPage.ActivarModoLectura();
                }
                else
                {
                    MostrarToastMessage("Error", "Hubo un error en el servidor, si los " +
                    "problemas persisten, favor de contactar a soporte técnico");
                }
            }
        }

        private void AgregarInsumo(object sender, RoutedEventArgs e)
        {
            Contenedor.Insumo = null;
            Contenedor.InsumoPage.ActivarModoEdicion("Registro");
            Transitioner.MoveNextCommand.Execute(1, this);
        }

        private void SeleccionarCriterio(object sender, MouseButtonEventArgs e)
        {
            if (Criterio.Text == "Todos")
                ValorBusqueda.IsEnabled = false;
            else
                ValorBusqueda.IsEnabled = true;
        }

        private void MostrarToastMessage(string tipo, string mensaje)
        {
            if (tipo == "Warning")
                notifier.ShowWarning(mensaje);
            else
                notifier.ShowError(mensaje);
        }

        private void ActualizarTablaInsumos(List<EInsumo> insumos)
        {
            if (insumos != null)
                tablaDatos.ItemsSource = insumos;
            else
                MostrarToastMessage("Error", "Hubo un error en el servidor, si los " +
                    "problemas persisten, favor de contactar a soporte técnico");
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            Contenedor.Close();
        }
    }
}
