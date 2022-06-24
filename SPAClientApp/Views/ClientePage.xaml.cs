using SPAClientApp.ClientesService;
using SPAClientApp.PedidosClientesService;
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
    /// Interaction logic for ClientePage.xaml
    /// </summary>
    public partial class ClientePage : Page
    {
        private WPedidoCliente PedidoWindow { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public ECliente Cliente { get; set; } = null;
        private readonly ClientesServiceClient client = new ClientesServiceClient();
        private readonly List<TextBox> CamposCliente;
        private readonly List<TextBox> CamposDireccion;
        private Notifier notifier;

        public ClientePage()
        {
            InitializeComponent();
            CamposCliente = new List<TextBox>()
            {
                NombreTxt,
                ApellidosTxt,
                TelefonoTxt,
                CorreoElectronicoTxt,
                CiudadTxt,
                CodigoPostalTxt
            };
            CamposDireccion = new List<TextBox>()
            {
                ColoniaTxt,
                CalleTxt,
                NumeroTxt
            };
        }

        public void CamposSoloLectura()
        {
            CamposCliente.ForEach(c => c.IsReadOnly = true);
        }

        public ClientePage(WPedidoCliente parent)
        {
            InitializeComponent();
            PedidoWindow = parent;
            ConfigurarToastNotifier(PedidoWindow, 3);
            CamposCliente = new List<TextBox>()
            {
                NombreTxt,
                ApellidosTxt,
                TelefonoTxt,
                CorreoElectronicoTxt,
                CiudadTxt,
                CodigoPostalTxt
            };
            CamposDireccion = new List<TextBox>()
            {
                ColoniaTxt,
                CalleTxt,
                NumeroTxt
            };
        }

        public StackPanel ObtenerTogglerAgregarDireccion()
        {
            return MostrarDomrularioDireccionBtn;
        }

        public ComboBox ObtenerCmbBoxDirecciones()
        {
            return DireccionesCmbx;
        }

        public async Task<ECliente> ActualizarCliente()
        {
            try
            {
                var cliente = PrepararCliente();
                var result = await client.UpdateClienteAsync(cliente);
                if (result.Key < 0)
                    throw new Exception(result.Message);
                return Cliente = await client.GetClienteAsync(cliente.Id);
            }
            catch (Exception ex)
            {
                MostrarToastMessage("Error", ex.Message);
                return null;
            }
        }

        public void ActualizarClienteBasico(ECliente cliente)
        {
            try
            {
                cliente = PrepararClienteNormal();
                var result = client.UpdateBasicClient(cliente);
                if (result.Key < 0)
                    throw new Exception(result.Message);
            }
            catch (Exception ex)
            {
                MostrarToastMessage("Error", ex.Message);
            }
        }

        public async Task<ECliente> RegistrarCliente()
        {
            try
            {
                var cliente = PrepararCliente();
                var result = await client.AddClienteAsync(Cliente);
                if (result.Key < 0)
                    throw new Exception(result.Message);
                return Cliente = await client.GetClienteAsync(result.Key);
            }
            catch (Exception ex)
            {
                MostrarToastMessage("Error", ex.Message);
                return null;
            }            
        }

        public void ValidarCliente(string opcion)
        {
            if(opcion != "Registro")
            {
                if (Cliente == null)
                    throw new Exception("Lo sentimos, el cliente no ha sido cargado en el sistema");
                if (string.IsNullOrEmpty(Direccion))
                    throw new Exception("Debes seleccionar una dirección del usuario o registrar una nueva");
            }
            
            //Other instructions
        }

        private ECliente PrepararClienteNormal()
        {
            if (Cliente == null)
                Cliente = new ECliente();
            Cliente.Email = CorreoElectronicoTxt.Text;
            Cliente.CodigoPostal = Convert.ToInt32(CodigoPostalTxt.Text);
            Cliente.Nombre = NombreTxt.Text;
            Cliente.Apellido = ApellidosTxt.Text;
            Cliente.Status = "Activo";
            Cliente.Ciudad = CiudadTxt.Text;
            Cliente.Telefono = TelefonoTxt.Text;
            Cliente.Nacimiento = Convert.ToDateTime(NacimientoDatePicker.Text);
            return Cliente;
        }

        private ECliente PrepararCliente()
        {
            if (Cliente == null)
                Cliente = new ECliente();
            Cliente.Email = CorreoElectronicoTxt.Text;
            Cliente.CodigoPostal = Convert.ToInt32(CodigoPostalTxt.Text);
            Cliente.Nombre = NombreTxt.Text;
            Cliente.Apellido = ApellidosTxt.Text;
            Cliente.Status = "Activo";
            Cliente.Ciudad = CiudadTxt.Text;
            Cliente.Telefono = TelefonoTxt.Text;
            Cliente.Nacimiento = Convert.ToDateTime(NacimientoDatePicker.Text);
            if(Cliente.Direcciones == null || Cliente.Direcciones.Length == 0)
            {
                Direccion = $"{ColoniaTxt.Text}{CalleTxt.Text}{NumeroTxt.Text}".ToLower().Replace(" ", string.Empty);
                Cliente.Direcciones = new List<EDireccion>() { new EDireccion() { 
                    Colonia = ColoniaTxt.Text,
                    Calle = CalleTxt.Text,
                    Numero = Convert.ToInt32(NumeroTxt.Text)
                }}.ToArray();
            }
            return Cliente;
        }

        private void MostrarToastMessage(string tipo, string mensaje)
        {
            if (tipo == "Advertencia")
                notifier.ShowWarning(mensaje);
            if (tipo == "Info")
                notifier.ShowInformation(mensaje);
            if (tipo == "Exito")
            {
                ConfigurarToastNotifier(PedidoWindow, 5);
                notifier.ShowSuccess(mensaje);
            }
            if (tipo == "Error")
            {
                ConfigurarToastNotifier(PedidoWindow, 5);
                notifier.ShowError(mensaje);
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

        public void MostrarClienteInfo(ECliente cliente)
        {
            Cliente = cliente;
            NombreTxt.Text = cliente.Nombre;
            ApellidosTxt.Text = cliente.Apellido;
            TelefonoTxt.Text = cliente.Telefono;
            CorreoElectronicoTxt.Text = cliente.Email;
            CiudadTxt.Text = cliente.Ciudad;
            CodigoPostalTxt.Text = cliente.CodigoPostal.ToString();
            NacimientoDatePicker.Text = cliente.Nacimiento.ToString();
            EdadLbl.Content = cliente.Edad.ToString();
            StatusLbl.Content = cliente.Status.ToString();
            DireccionesCmbx.Items.Clear();
            if (cliente.Direcciones != null)
            {
                cliente.Direcciones.ToList().ForEach(direccion => DireccionesCmbx.Items.Add($"{direccion.Colonia}, {direccion.Calle}, {direccion.Numero}"));
            }
        }

        public void MostrarClienteDePedido(EPedidoClienteDetallado pedido)
        {
            DireccionesCmbx.Visibility = Visibility.Collapsed;
            addBtn.Visibility = Visibility.Collapsed;
            CamposCliente.ForEach(c => c.IsReadOnly = true);
            CamposDireccion.ForEach(c => c.IsReadOnly = true);
            NombreTxt.Text = pedido.Nombre;
            ApellidosTxt.Text = pedido.Apellido;
            TelefonoTxt.Text = pedido.Telefono;
            CorreoElectronicoTxt.Text = pedido.Email;
            CiudadTxt.Text = pedido.Ciudad;
            CodigoPostalTxt.Text = pedido.CodigoPostal.ToString();
            NacimientoDatePicker.Text = pedido.Nacimiento.ToString();
            EdadLbl.Content = pedido.Edad.ToString();
            StatusLbl.Content = pedido.StatusPedido.ToString();
            CalleTxt.Text = pedido.Calle;
            NumeroTxt.Text = pedido.Numero.ToString();
            ColoniaTxt.Text = pedido.Colonia;

        }

        public int ObtenerIdDireccion(string direccion, List<EDireccion> direcciones)
        {
            int returnValue = -1;
            foreach(var d in direcciones)
            {
                if($"{d.Colonia}{d.Calle}{d.Numero}".Replace(" ", string.Empty).ToLower() == direccion.Replace(" ", string.Empty).ToLower())
                {
                    returnValue = d.Id;
                    break;
                }
            }
            return returnValue;
        }

        private void AgregarDireccion(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarDireccion();
                var list = Cliente.Direcciones.ToList();
                list.Add(new EDireccion()
                {
                    Id = -1,
                    IdCliente = Cliente.Id,
                    Calle = CalleNuevaTxt.Text,
                    Numero = Convert.ToInt32(NumeroNuevoTxt.Text),
                    Colonia = ColoniaNuevaTxt.Text
                });
                Cliente.Direcciones = list.ToArray();
                DireccionesCmbx.Items.Clear();
                Cliente.Direcciones.ToList().ForEach(direccion => DireccionesCmbx.Items.Add($"{direccion.Colonia}, {direccion.Calle}, {direccion.Numero}"));
            }catch (Exception ex)
            {
                MostrarToastMessage("Advertencia", ex.Message);
            }
        }

        private void ValidarDireccion()
        {
            if (Cliente == null)
                throw new Exception("No puedes agregar una nueva dirección sin haber seleccionado a un cliente");
            if (ObtenerIdDireccion($"{ColoniaNuevaTxt.Text}{CalleNuevaTxt.Text}{NumeroNuevoTxt.Text}".ToLower().Replace(" ", string.Empty), Cliente.Direcciones.ToList()) > 0)
                throw new Exception("Lo sentimos, la dirección ya esta registrada para el cliente seleccionado");

            //Other validations
        }

        private void SeleccionarDireccion(object sender, EventArgs e)
        {
            if (DireccionesCmbx.SelectedItem != null)
            {
                Direccion = DireccionesCmbx.SelectedItem.ToString().Replace(",", string.Empty);
                var direccion = Cliente.Direcciones.ToList().Where(d => $"{d.Colonia}{d.Calle}{d.Numero}".Replace(" ", string.Empty).ToLower() == Direccion.ToLower().Replace(" ", string.Empty)).First();
                if (direccion != null)
                {
                    ColoniaTxt.Text = direccion.Colonia;
                    CalleTxt.Text = direccion.Calle;
                    NumeroTxt.Text = direccion.Numero.ToString();
                }
            }
        }
    }
}
