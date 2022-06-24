using SPAClientApp.ProveedoresService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para WProveedor.xaml
    /// </summary>
    public partial class WProveedor : Window
    {
        private readonly ProveedoresServiceClient client = new ProveedoresServiceClient();
        private WListaProveedores ParentWindow { get; set; }
        private readonly List<TextBox> UiInputElements;
        private readonly List<Button> UiButtons;
        private Notifier notifier;
        private EProveedor Proveedor { get; set; }
        private string Operacion { get; set; } = "Consulta";
        public string Direccion { get; set; } = string.Empty;

        public WProveedor(WListaProveedores parent)
        {
            InitializeComponent();
            ParentWindow = parent;
            UiInputElements = new List<TextBox>() { NombreTxt, RfcTxt, TelefonoTxt, CorreoElectronicoTxt, InsumoTxt };
            UiButtons = new List<Button>() { SalirBtn, ActualizarBtn, RegistrarBtn, CancelarBtn };
            ConfigurarToastNotifier(this, 3);
        }

        public void ActivarModoLectura(EProveedor proveedor)
        {
            LlenarCampos(proveedor);
            Operacion = "Consulta";
            UiInputElements.ForEach(e => e.IsReadOnly = true);
            UiButtons.ForEach(b => b.Visibility = Visibility.Collapsed);
            SalirBtn.Visibility = Visibility.Visible;
            StatusTxt.Visibility = Visibility.Visible;
            StatusLbl.Visibility = Visibility.Visible;
        }

        public void ActivarModoEdicion(string modo, EProveedor proveedor)
        {
            UiInputElements.ForEach(e => e.IsReadOnly = false);
            UiButtons.ForEach(b => b.Visibility = Visibility.Collapsed);
            StatusTxt.Visibility = Visibility.Collapsed;
            StatusLbl.Visibility = Visibility.Collapsed;
            if ((Operacion = modo) == "Registro")
            {
                Proveedor = proveedor;
                (new List<Button> { CancelarBtn, RegistrarBtn }).ForEach(b => b.Visibility = Visibility.Visible);
            }
            else
            {
                Operacion = "Actualizacion";
                Proveedor = proveedor;
                (new List<Button> { CancelarBtn, ActualizarBtn }).ForEach(b => b.Visibility = Visibility.Visible);
                LlenarCampos(proveedor);
            }
        }

        private void LlenarCampos(EProveedor proveedor)
        {
            NombreTxt.Text = proveedor.Nombre;
            RfcTxt.Text = proveedor.Rfc;
            TelefonoTxt.Text = proveedor.Telefono;
            CorreoElectronicoTxt.Text = proveedor.Email;
            InsumoTxt.Text = proveedor.CategoriaInsumo;
            DireccionTxt.Text = proveedor.DireccionEmpresa;
            StatusLbl.Content = proveedor.Status;
        }

        private bool MostrarCuadroConfirmacion(string message)
        {
            MessageBoxResult boxResult = MessageBox.Show(message, "Advertencia", MessageBoxButton.YesNoCancel);
            return MessageBoxResult.Yes == boxResult;
        }

        private void ValidarProveedor()
        {
            if (ValidarAuxiliar(NombreTxt.Text))
                throw new ArgumentException("El nombre debe ser una cadena de texto");
            if (ValidarRFC(RfcTxt.Text))
                throw new ArgumentException("El Registro Federal de Contribuyentes tiene sus reglas, favor de verificar el RFC");
            if (ValidarTelefonos7a10Digitos(TelefonoTxt.Text))
                throw new ArgumentException("El numero de telefono debe contener entre 7 y 10 digitos");
            if (ValidarAuxiliar(CorreoElectronicoTxt.Text))
                throw new ArgumentException("El email debe estar en formato de email [ejemplo@ejemplo.com]");
            if (ValidarAuxiliar(InsumoTxt.Text))
                throw new ArgumentException("El insumo debe ser una cadena de texto");
            if (ValidarAuxiliar(DireccionTxt.Text))
                throw new ArgumentException("Verifique que este escrita correctamente la dirección");
        }

        public bool ValidarRFC(string strRFC)
        {
            Regex regex = new Regex("^[A-Z&Ñ]{3,4}[0-9]{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[A-Z0-9]{2}[0-9A]$");
            Match match = regex.Match(strRFC);
            if (!match.Success)
                return true;
            else
                return false;
        }

        public bool ValidarTelefonos7a10Digitos(string strNumber)
        {
            Regex regex = new Regex("[0-9]{7,10}");
            Match match = regex.Match(strNumber);
            if (!match.Success)
                return true;
            else
                return false;
        }

        private bool ValidarAuxiliar(string value)
        {
            return (string.IsNullOrEmpty(value) || double.TryParse(value, out _) || string.IsNullOrEmpty(value.Trim()));
        }

        private void CancelarProceso(object sender, RoutedEventArgs e)
        {
            string mensaje = "¿Deseas salir sin guardar los cambios?";
            if (MostrarCuadroConfirmacion(mensaje))
                Close();
        }

        private void Actualizar(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarProveedor();
                Proveedor = PrepararProveedor();
                AnswerMessage response = client.UpdateProveedor(Proveedor.Clave, Proveedor);
                if (response.Key > 0)
                {
                    MostrarToastMessage("Exito", "El proveedor se ha actualizado exitosamente");
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
                ValidarProveedor();
                Proveedor = PrepararProveedor();
                AnswerMessage response = client.AddProveedor(Proveedor);
                if (response.Key > 0)
                {
                    Proveedor = client.GetProveedor(response.Key);
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

        private EProveedor PrepararProveedor()
        {
            if (Proveedor == null)
                Proveedor = new EProveedor();
            Proveedor.Nombre = NombreTxt.Text;
            Proveedor.Rfc = RfcTxt.Text;
            Proveedor.Telefono = TelefonoTxt.Text;
            Proveedor.Email = CorreoElectronicoTxt.Text;
            Proveedor.CategoriaInsumo = InsumoTxt.Text;
            Proveedor.DireccionEmpresa = DireccionTxt.Text;
            return Proveedor;
        }

        private void MostrarToastMessage(string tipo, string mensaje)
        {
            if (tipo == "Advertencia")
                notifier.ShowWarning(mensaje);
            if (tipo == "Exito")
            {
                ConfigurarToastNotifier(ParentWindow, 5);
                notifier.ShowSuccess(mensaje);
                ParentWindow.RefrescarTablaProveedor();
                Close();
            }
            if (tipo == "Error")
            {
                ConfigurarToastNotifier(ParentWindow, 5);
                notifier.ShowError(mensaje);
                ParentWindow.RefrescarTablaProveedor();
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
