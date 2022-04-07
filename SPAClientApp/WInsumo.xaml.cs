using MaterialDesignThemes.Wpf.Transitions;
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
using System.Windows.Navigation;
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
    public partial class WInsumo : UserControl
    {
        private readonly InsumosServiceClient client = new InsumosServiceClient();
        private readonly List<TextBox> UiInputElements;
        private readonly List<Button> UiButtons;
        private readonly Notifier notifier;
        private InsumosContenedor Contenedor { get; set; }

        public WInsumo()
        {
            InitializeComponent();
            UiInputElements = new List<TextBox>() {NombreTxt, ProveedorTxt, CantidadTxt, DescripcionTxt, RestriccionesTxt, CostoTxt};
            UiButtons = new List<Button>() {EliminarBtn, ActivarBtn, ModificarBtn, CancelarBtn, ActualizarBtn, RegistrarBtn, CerrarBtn};
            this.Loaded += (s, e) =>
            {
                Contenedor = Window.GetWindow(this) as InsumosContenedor;
            };
            notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(parentWindow: Window.GetWindow(this), corner: Corner.BottomLeft, offsetX: 10, offsetY: 10);
                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(notificationLifetime: TimeSpan.FromSeconds(3), maximumNotificationCount: MaximumNotificationCount.FromCount(3));
                cfg.Dispatcher = Application.Current.Dispatcher;
            });
        }

        public void ActivarModoLectura()
        {
            LlenarCampos();
            UiInputElements.ForEach(e => e.IsReadOnly = true);
            UiButtons.ForEach(b => b.Visibility = Visibility.Collapsed);
            (new List<Button>() { EliminarBtn, ActivarBtn, CerrarBtn, ModificarBtn }).ForEach(b => b.Visibility = Visibility.Visible);
            EstadoTxt.Visibility = Visibility.Visible;  
            estadoLbl.Visibility = Visibility.Visible;
        }

        public void ActivarModoEdicion(string modo)
        {
            UiInputElements.ForEach(e => { e.IsReadOnly = false; e.Text = String.Empty; });
            UiButtons.ForEach(b => b.Visibility = Visibility.Collapsed);
            EstadoTxt.Visibility = Visibility.Collapsed;
            estadoLbl.Visibility = Visibility.Collapsed;
            if (modo == "Registro")
            {
                FechaDatePicker.Text = DateTime.Now.ToString();
                (new List<Button> { CancelarBtn, RegistrarBtn }).ForEach(b => b.Visibility = Visibility.Visible);
            }
            else
            {
                (new List<Button> { CancelarBtn, ActualizarBtn }).ForEach(b => b.Visibility = Visibility.Visible);
                LlenarCampos();
            }
        }

        public void LlenarCampos()
        {
            NombreTxt.Text = Contenedor.Insumo.Nombre;
            FechaDatePicker.Text = Contenedor.Insumo.Registro.ToString();
            Codigolbl.Content = Contenedor.Insumo.Codigo.ToString();
            EstadoTxt.Text = Contenedor.Insumo.Status;
            ProveedorTxt.Text = Contenedor.Insumo.ProveedorDeInsumo;
            CantidadTxt.Text = Contenedor.Insumo.Cantidad.ToString();
            CostoTxt.Text = Contenedor.Insumo.PrecioCompra.ToString();
            UnidadComboBox.SelectedItem = Contenedor.Insumo.UnidadMedida;
            DescripcionTxt.Text = Contenedor.Insumo.Descripcion;
            RestriccionesTxt.Text = Contenedor.Insumo.Restricciones;
            if (Contenedor.Insumo.Status == "Activo")
            {
                EliminarBtn.IsEnabled = true;
                ActivarBtn.IsEnabled = false;
            }
            else
            {
                EliminarBtn.IsEnabled = false;
                ActivarBtn.IsEnabled = true;
            }
        }

        private void Registrar(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarInsumo();
                Contenedor.Insumo = new EInsumo();
                PrepararNuevoInsumo();
                AnswerMessage response = client.AddInsumo(Contenedor.Insumo);
                if (response.Key > 0)
                {
                    Contenedor.Insumo = client.GetInsumosList("Código", response.Key.ToString(), Convert.ToDateTime("1/1/1900 00:00:00"), "Activo").First();
                    MostrarToastMessage("Exito", "El insumo se ha registrado exitosamente");
                    ActivarModoLectura();
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

        private void PrepararNuevoInsumo()
        {
            Contenedor.Insumo.Nombre = NombreTxt.Text;
            Contenedor.Insumo.ProveedorDeInsumo = ProveedorTxt.Text;
            Contenedor.Insumo.Descripcion = DescripcionTxt.Text;
            Contenedor.Insumo.Restricciones = RestriccionesTxt.Text;
            Contenedor.Insumo.Cantidad = Convert.ToInt32(CantidadTxt.Text);
            Contenedor.Insumo.PrecioCompra = float.Parse(CostoTxt.Text);
            Contenedor.Insumo.UnidadMedida = UnidadComboBox.Text;
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
            if (!TieneNombreUnico(NombreTxt.Text))
                throw new ArgumentException("El nombre del Insumo ya has sido registrado en el sistema");
        }

        public bool TieneNombreUnico(string nombre)
        {
            if (Contenedor.Insumo == null)
                return !client.IsDuplicated(" ", nombre);
            else
                return !client.IsDuplicated(Contenedor.Insumo.Nombre, nombre);
        }

        private bool ValidarAuxiliar(string value)
        {
            return (string.IsNullOrEmpty(value) || double.TryParse(value, out _) || string.IsNullOrEmpty(value.Trim()));
        }

        private void CancelarOperacion(object sender, RoutedEventArgs e)
        {
            string mensaje = "¿Deseas salir sin guardar los cambios?";
            if (MostrarCuadroConfirmacion(mensaje))
            {
                if (Contenedor.Insumo == null)
                    Transitioner.MovePreviousCommand.Execute(0, this);
                else
                    ActivarModoLectura();
            }
        }

        private void Actualizar(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarInsumo();
                PrepararNuevoInsumo();
                AnswerMessage response = client.UpdateInsumo(Contenedor.Insumo.Codigo, Contenedor.Insumo);
                if (response.Key > 0)
                {
                    MostrarToastMessage("Exito", "El insumo se ha actualizado exitosamente");
                    ActivarModoLectura();
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

        private void Modificar(object sender, RoutedEventArgs e)
        {
            ActivarModoEdicion("Edición");
        }
        private void MostrarToastMessage(string tipo, string mensaje)
        {
            if (tipo == "Exito")
                notifier.ShowSuccess(mensaje);
            if(tipo == "Advertencia")
                notifier.ShowWarning(mensaje);  
            if(tipo == "Error")
                notifier.ShowError(mensaje);    
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            Transitioner.MovePreviousCommand.Execute(0, this);
        }

        private void DarDeBaja(object sender, RoutedEventArgs e)
        {
            string mensaje = "¿Seguro(a) que deseas dar de baja el Insumo seleccionado?";
            if (MostrarCuadroConfirmacion(mensaje))
            {
                AnswerMessage response = client.ChangeInsumoStatus(Contenedor.Insumo.Codigo, "Dado de baja");
                if (response.Key >= 0)
                {
                    ActualizarStatus("Dado de baja");
                    MostrarToastMessage("Exito", "El Insumo ha sido dado de baja");
                }
                else
                {
                    MostrarToastMessage("Error", "Lo sentimos, ha ocurrido un error en el servidor, favor de contactar a soporte técnico");
                }
            }
        }

        private bool MostrarCuadroConfirmacion(string message)
        {
            MessageBoxResult boxResult = MessageBox.Show(message, "Advertencia", MessageBoxButton.YesNoCancel);
            return MessageBoxResult.Yes == boxResult;
        }

        private void ActualizarStatus(string status)
        {
            Contenedor.Insumo.Status = status;
            EstadoTxt.Text = status;
            if(status == "Activo")
            {
                EliminarBtn.IsEnabled = true;
                ActivarBtn.IsEnabled = false;
            }
            else
            {
                EliminarBtn.IsEnabled = false;
                ActivarBtn.IsEnabled = true;
            }

        }

        private void DarDeAlta(object sender, RoutedEventArgs e)
        {
            string mensaje = "¿Seguro(a) que deseas dar de alta el Insumo seleccionado?";
            if (MostrarCuadroConfirmacion(mensaje))
            {
                AnswerMessage response = client.ChangeInsumoStatus(Contenedor.Insumo.Codigo, "Activo");
                if (response.Key >= 0)
                {
                    ActualizarStatus("Activo");
                    MostrarToastMessage("Exito", "El Insumo ha sido dado de alta");
                }
                else
                {
                    MostrarToastMessage("Error", "Lo sentimos, ha ocurrido un error en el servidor, favor de contactar a soporte técnico");
                }
            }
        }
    }
}
