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
        //private readonly ProveedorSrs client = new ProveedorSrs();
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
            UiInputElements = new List<TextBox>() { NombreTxt, RfcTxt, TelefonoTxt, CorreoElectronicoTxt, InsumoTxt};
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
            StatusLbl.Content = proveedor.Status;
            DireccionesCmbx.Items.Clear();
            proveedor.Direcciones.ToList().ForEach(direccion => DireccionesCmbx.Items.Add($"{direccion.Calle}, {direccion.Numero}"));
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
                throw new ArgumentException("El nombre del proveedor debe ser una cadena de texto");
            if (ValidarTelefonos7a10Digitos(TelefonoTxt.Text))
                throw new ArgumentException("El Registro Federal de Contribuyentes tiene sus reglas, favor de verificar el RFC");
            if (ValidarAuxiliar(CorreoElectronicoTxt.Text))
                throw new ArgumentException("El email debe estar en formato de email [ejemplo@ejemplo.com]");
            if (ValidarAuxiliar(InsumoTxt.Text))
                throw new ArgumentException("El insumo debe ser una cadena de texto");
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

        private void ValidarDireccion()
        {
            if (Proveedor == null)
                throw new Exception("No puedes agregar una nueva dirección sin haber seleccionado a un cliente");
            if (ObtenerIdDireccion($"{CalleNuevaTxt.Text}{NumeroNuevoTxt.Text}".ToLower().Replace(" ", string.Empty), Proveedor.Direcciones.ToList()) > 0)
                throw new Exception("Lo sentimos, la dirección ya esta registrada para el cliente seleccionado");
        }

        private bool ValidarAuxiliar(string value)
        {
            return (string.IsNullOrEmpty(value) || double.TryParse(value, out _) || string.IsNullOrEmpty(value.Trim()));
        }

        private void SeleccionarDireccion(object sender, SelectionChangedEventArgs e)
        {
            if (DireccionesCmbx.SelectedItem != null)
            {
                Direccion = DireccionesCmbx.SelectedItem.ToString().Replace(",", string.Empty);
                Direccion = Direccion.ToLower();
                var direccion = Proveedor.Direcciones.ToList().Where(d => $"{d.Calle}{d.Numero}".ToLower() == Direccion.Replace(" ", string.Empty)).First();
                if (direccion != null)
                {
                    CalleTxt.Text = direccion.Calle;
                    NumeroTxt.Text = direccion.Numero.ToString();
                }
            }
        }

        private void AgregarDireccion(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarDireccion();
                var list = Proveedor.Direcciones.ToList();
                list.Add(new EDireccion()
                {
                    Id = -1,
                    IdProveedor = Proveedor.Id,
                    Calle = CalleNuevaTxt.Text,
                    Numero = Convert.ToInt32(NumeroNuevoTxt.Text)
                });
                Proveedor.Direcciones = list.ToArray();
                DireccionesCmbx.Items.Clear();
                Proveedor.Direcciones.ToList().ForEach(direccion => DireccionesCmbx.Items.Add($"{direccion.Calle}, {direccion.Numero}"));
            }
            catch (Exception ex)
            {
                MostrarToastMessage("Advertencia", ex.Message);
            }
        }

        public int ObtenerIdDireccion(string direccion, List<EDireccion> direcciones)
        {
            int returnValue = -1;
            foreach (var d in direcciones)
            {
                if ($"{d.Calle}{d.Numero}".ToLower() == direccion.Replace(" ", string.Empty))
                {
                    returnValue = d.Id;
                    break;
                }
            }
            return returnValue;
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
                AnswerMessage response = client.UpdateProveedor(Proveedor.Id, Proveedor);
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
                var result = await client.AddProveedorAsync(Proveedor);
                if (result.Key < 0)
                    throw new Exception(result.Message);
                return Proveedor = await client.GetProveedorAsync(result.Key);
            }
            catch (Exception ex)
            {
                MostrarToastMessage("Error", ex.Message);
                return null;
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
            if (Proveedor.Direcciones == null || Proveedor.Direcciones.Length == 0)
            {
                Direccion = $"{CalleTxt.Text}{NumeroTxt.Text}".ToLower().Replace(" ", string.Empty);
                Proveedor.Direcciones = new List<EDireccion>() { new EDireccion() {
                    Calle = CalleTxt.Text,
                    Numero = Convert.ToInt32(NumeroTxt.Text)
                }}.ToArray();
            }
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
