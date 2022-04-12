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
    /// Interaction logic for WInsumo.xaml
    /// </summary>
    public partial class WInsumo : Window
    {
        private readonly InsumosServiceClient client = new InsumosServiceClient();
        private readonly List<TextBox> UiInputElements;
        private readonly List<Button> UiButtons;
        private Notifier notifier;
        private string Operacion { get; set; } = "Consulta";
        private EInsumo Insumo { get; set; }
        private static WInsumo WindowInsumo { get; set; } = null;
        private WListaInsumos ParentWindow { get; set; }   

        public WInsumo(WListaInsumos parent)
        {
            InitializeComponent();
            ParentWindow = parent;
            UiInputElements = new List<TextBox>() { NombreTxt, ProveedorTxt, CantidadTxt, DescripcionTxt, RestriccionesTxt, CostoTxt };
            UiButtons = new List<Button>() { CancelarBtn, ActualizarBtn, RegistrarBtn, CerrarBtn };
            notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(parentWindow: Window.GetWindow(this), corner: Corner.BottomLeft, offsetX: 10, offsetY: 10);
                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(notificationLifetime: TimeSpan.FromSeconds(3), maximumNotificationCount: MaximumNotificationCount.FromCount(3));
                cfg.Dispatcher = Application.Current.Dispatcher;
            });
        }

        public void ActivarModoLectura(EInsumo insumo)
        {
            LlenarCampos(insumo);
            Operacion = "Consulta";
            UiInputElements.ForEach(e => e.IsReadOnly = true);
            UiButtons.ForEach(b => b.Visibility = Visibility.Collapsed);
            CerrarBtn.Visibility = Visibility.Visible;
            EstadoTxt.Visibility = Visibility.Visible;
            estadoLbl.Visibility = Visibility.Visible;
            UnidadComboBox.IsEnabled = false;
        }

        public void ActivarModoEdicion(string modo, EInsumo insumo)
        {
            UiInputElements.ForEach(e => e.IsReadOnly = false);
            UiButtons.ForEach(b => b.Visibility = Visibility.Collapsed);
            EstadoTxt.Visibility = Visibility.Collapsed;
            estadoLbl.Visibility = Visibility.Collapsed;
            if ((Operacion = modo) == "Registro")
            {
                FechaDatePicker.Text = DateTime.Now.ToString();
                Insumo = insumo;
                (new List<Button> { CancelarBtn, RegistrarBtn }).ForEach(b => b.Visibility = Visibility.Visible);
            }
            else
            {
                Operacion = "Actualizacion";
                Insumo = insumo;
                (new List<Button> { CancelarBtn, ActualizarBtn }).ForEach(b => b.Visibility = Visibility.Visible);
                LlenarCampos(insumo);
            }
        }

        private void CancelarOperacion(object sender, RoutedEventArgs e)
        {
            string mensaje = "¿Deseas salir sin guardar los cambios?";
            if (MostrarCuadroConfirmacion(mensaje))
                Close();
        }

        private void Actualizar(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarInsumo();
                PrepararNuevoInsumo();
                AnswerMessage response = client.UpdateInsumo(Insumo.Codigo, Insumo);
                if (response.Key > 0)
                {
                    MostrarToastMessage("Exito", "El insumo se ha actualizado exitosamente");
                }
                else
                {
                    MostrarToastMessage("Error", response.Message);
                }
            }
            catch (ArgumentException ex)
            {
                MostrarToastMessage("Advertencia", ex.Message);
            }

        }

        private void Registrar(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarInsumo();
                PrepararNuevoInsumo();
                AnswerMessage response = client.AddInsumo(Insumo);
                if (response.Key > 0)
                {
                    Insumo = client.GetInsumosList("Código", response.Key.ToString(), Convert.ToDateTime("1/1/1900 00:00:00"), "Activo").First();
                    MostrarToastMessage("Exito", "El insumo se ha registrado exitosamente");
                }
                else
                {
                    MostrarToastMessage("Error", response.Message);
                }
            }
            catch (ArgumentException ex)
            {
                MostrarToastMessage("Advertencia", ex.Message);
            }
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MostrarToastMessage(string tipo, string mensaje)
        {
            if (tipo == "Advertencia")
                notifier.ShowWarning(mensaje);
            if (tipo == "Exito")
            {
                notifier = new Notifier(cfg =>
                {
                    cfg.PositionProvider = new WindowPositionProvider(parentWindow: ParentWindow, corner: Corner.BottomLeft, offsetX: 10, offsetY: 10);
                    cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(notificationLifetime: TimeSpan.FromSeconds(5), maximumNotificationCount: MaximumNotificationCount.FromCount(3));
                    cfg.Dispatcher = Application.Current.Dispatcher;
                });
                notifier.ShowSuccess(mensaje);
                ParentWindow.RefrescarTablaInsumos();
                Close();
            }
            if (tipo == "Error")
            {
                notifier = new Notifier(cfg =>
                {
                    cfg.PositionProvider = new WindowPositionProvider(parentWindow: ParentWindow, corner: Corner.BottomLeft, offsetX: 10, offsetY: 10);
                    cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(notificationLifetime: TimeSpan.FromSeconds(5), maximumNotificationCount: MaximumNotificationCount.FromCount(3));
                    cfg.Dispatcher = Application.Current.Dispatcher;
                });
                notifier.ShowError(mensaje);
                ParentWindow.RefrescarTablaInsumos();
                Close();    
            }
        }

        private void LlenarCampos(EInsumo insumo)
        {
            NombreTxt.Text = insumo.Nombre;
            FechaDatePicker.Text = insumo.Registro.ToString();
            Codigolbl.Content = insumo.Codigo.ToString();
            EstadoTxt.Text = insumo.Status;
            ProveedorTxt.Text = insumo.ProveedorDeInsumo;
            CantidadTxt.Text = insumo.Cantidad.ToString();
            CostoTxt.Text = insumo.PrecioCompra.ToString();
            UnidadComboBox.SelectedItem = insumo.UnidadMedida;
            DescripcionTxt.Text = insumo.Descripcion;
            RestriccionesTxt.Text = insumo.Restricciones;
        }

        private bool MostrarCuadroConfirmacion(string message)
        {
            MessageBoxResult boxResult = MessageBox.Show(message, "Advertencia", MessageBoxButton.YesNoCancel);
            return MessageBoxResult.Yes == boxResult;
        }

        private void ValidarInsumo()
        {
            if (ValidarAuxiliar(NombreTxt.Text))
                throw new ArgumentException("El nombre debe ser una cadena de texto");
            if (ValidarAuxiliar(ProveedorTxt.Text))
                throw new ArgumentException("El nombre del proveedor debe ser una cadena de texto");
            if (ValidarAuxiliar(DescripcionTxt.Text))
                throw new ArgumentException("La descripción debe ser texto");
            if (ValidarAuxiliar(RestriccionesTxt.Text))
                throw new ArgumentException("Las restricciones deben estar en formato de texto");
            int aux = 0;
            float auxd = 0;
            if (string.IsNullOrEmpty(CantidadTxt.Text) || !int.TryParse(CantidadTxt.Text, out aux) || aux < 0)
                throw new ArgumentException("La cantidad debe ser un número entero positivo");
            if (string.IsNullOrEmpty(CostoTxt.Text) || !float.TryParse(CostoTxt.Text, out auxd) || auxd < 0)
                throw new ArgumentException("El costo del producto debe ser un número positivo");
            if (!TieneNombreUnico(Insumo.Nombre, NombreTxt.Text))
                throw new ArgumentException("El nombre del Insumo ya has sido registrado en el sistema");
        }

        private bool ValidarAuxiliar(string value)
        {
            return (string.IsNullOrEmpty(value) || double.TryParse(value, out _) || string.IsNullOrEmpty(value.Trim()));
        }

        private void PrepararNuevoInsumo()
        {
            Insumo.Nombre = NombreTxt.Text;
            Insumo.ProveedorDeInsumo = ProveedorTxt.Text;
            Insumo.Descripcion = DescripcionTxt.Text;
            Insumo.Restricciones = RestriccionesTxt.Text;
            Insumo.Cantidad = Convert.ToInt32(CantidadTxt.Text);
            Insumo.PrecioCompra = float.Parse(CostoTxt.Text);
            Insumo.UnidadMedida = UnidadComboBox.Text;
        }

        private bool TieneNombreUnico(string nombreActual, string nombreABuscar)
        {
            if (Operacion == "Registro")
                return !client.IsDuplicated(" ", nombreABuscar);
            else
                return !client.IsDuplicated(nombreActual, nombreABuscar);
        }
    }
}
