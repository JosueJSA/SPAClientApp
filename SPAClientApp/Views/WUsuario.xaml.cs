using SPAClientApp.UsuariosService;
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
    /// Interaction logic for WUsuario.xaml
    /// </summary>
    public partial class WUsuario : Window
    {
        public Notifier notifier { get; set; }

        private List<TextBox> fields = new List<TextBox>();
        private List<Button> buttons = new List<Button>();
        private UsuariosServiceClient client = new UsuariosServiceClient();
        private AnswerMessage answer = new AnswerMessage();
        public EUsuario USUARIO = new EUsuario();
        public WHome HOME;

        public WUsuario(string mode, EUsuario usuario = null, WHome home = null)
        {
            InitializeComponent();
            ConfigurarToastNotifier(this, 3);
            fields = new List<TextBox>()
            {
                email, nombre, apellidos, codigoPostal, telefono,
                ciudad, salario
            };
            buttons = new List<Button>()
            {
                cancelarBtn, signUpBtn, updateBtn
            };
            if(mode == "Update")
            {
                USUARIO = usuario;
                ActivarModoEscritura(mode);
                LlenarCampos();
                statusLbl.Visibility = Visibility.Collapsed;
                fechaBajaLbl.Visibility = Visibility.Collapsed; 
                fechaRegistro.Visibility = Visibility.Collapsed;
                cancelarBtn.Visibility = Visibility.Visible;
                updateBtn.Visibility = Visibility.Visible;
            }
            else if(mode == "SignUp")
            {
                ActivarModoEscritura(mode);
                statusLbl.Visibility = Visibility.Collapsed;
                fechaBajaLbl.Visibility = Visibility.Collapsed;
                fechaRegistro.Visibility = Visibility.Collapsed;
            }
            else
            {
                USUARIO = usuario;
                LlenarCampos();
                ActivarModoLectura();
                password.IsEnabled = false;
            }
            if (home != null)
                HOME = home;
        }

        public void ActivarModoLectura()
        {
            fields.ForEach(f => f.IsReadOnly = true);
            buttons.ForEach(b => b.Visibility = Visibility.Collapsed);
        }

        public void ActivarModoEscritura(String mode)
        {
            fields.ForEach(f => f.IsReadOnly = false);
            if(mode == "Update")
                signUpBtn.Visibility = Visibility.Collapsed;
            else
                updateBtn.Visibility = Visibility.Collapsed;
        }

        public void ConfigurarToastNotifier(Window ventana, int segundos)
        {
            notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(parentWindow: ventana, corner: Corner.BottomRight, offsetX: 10, offsetY: 10);
                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(notificationLifetime: TimeSpan.FromSeconds(segundos), maximumNotificationCount: MaximumNotificationCount.FromCount(segundos));
                cfg.Dispatcher = Application.Current.Dispatcher;
            });
        }

        public void MostrarToastMessage(string tipo, string mensaje)
        {
            if (tipo == "Advertencia")
                notifier.ShowWarning(mensaje);
            if (tipo == "Exito")
                notifier.ShowSuccess(mensaje);
            if (tipo == "Info")
                notifier.ShowInformation(mensaje);
            if (tipo == "Error")
            {
                ConfigurarToastNotifier(this, 5);
                notifier.ShowError(mensaje);
                Close();
            }
        }

        private void ValidarUsuario()
        {
            if (ValidarAuxiliar(email.Text))
                throw new ArgumentException("Debes ingresar un correo");
            if (ValidarAuxiliar(nombre.Text))
                throw new ArgumentException("Debes ingresar un nombre");
            if (ValidarAuxiliar(apellidos.Text))
                throw new ArgumentException("Debes ingresar un apellido");
            if (ValidarAuxiliar(password.Password.ToString()))
                throw new ArgumentException("Debes ingresar una contraseña");
            if (string.IsNullOrEmpty(codigoPostal.Text) && string.IsNullOrEmpty(codigoPostal.Text.Trim()))
                throw new ArgumentException("Debes ingresar un código postal");
            float aux = 0;
            if (string.IsNullOrEmpty(salario.Text) || !float.TryParse(salario.Text, out aux) || aux < 0)
                throw new ArgumentException("Salario no puede ser negativo");
            if (ValidarAuxiliar(telefono.Text))
                throw new ArgumentException("Debes ingresar un número telefónico");
            if (ValidarAuxiliar(ciudad.Text))
                throw new ArgumentException("Debes ingresar una ciudad");
        }

        private bool ValidarAuxiliar(string value)
        {
            return (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()));
        }

        private void LlenarCampos()
        {

            email.Text = USUARIO.Email;
            password.Password = USUARIO.Password;
            nombre.Text = USUARIO.Nombre;
            apellidos.Text = USUARIO.Apellidos;
            tipoUsuario.Text = USUARIO.TipoUsuario;
            codigoPostal.Text = USUARIO.CodigoPostal;
            salario.Text = USUARIO.Salario.ToString();
            telefono.Text = USUARIO.Telefono;
            ciudad.Text = USUARIO.Ciudad;
        }

        private EUsuario PrepareUsuario()
        {
            return new EUsuario()
            {
                Clave = -1,
                Email = email.Text,
                Password = password.Password.ToString(),
                Nombre = nombre.Text,
                Apellidos = apellidos.Text,
                TipoUsuario = tipoUsuario.Text,
                CodigoPostal = codigoPostal.Text,
                Status = "Activo",
                Salario = Convert.ToDouble(salario.Text),
                Telefono = telefono.Text,
                Ciudad = ciudad.Text,
                Registro = DateTime.Now,
                Baja = DateTime.Now
            };
        }

        private EUsuario PrepareUsuarioUpdate()
        {
            USUARIO.Email = email.Text;
            USUARIO.Password = password.Password.ToString();
            USUARIO.Nombre = nombre.Text;
            USUARIO.Apellidos = apellidos.Text;
            USUARIO.TipoUsuario = tipoUsuario.Text;
            USUARIO.CodigoPostal = codigoPostal.Text;
            USUARIO.Salario = Convert.ToDouble(salario.Text);
            USUARIO.Telefono = telefono.Text;
            USUARIO.Ciudad = ciudad.Text;
            return USUARIO;
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarUsuario();
                var user = PrepareUsuario();
                answer = client.AddUsuario(user);
                if(answer.Key > 0)
                {
                    new WHome(client.GetUsuarioByEmail(user.Email, user.Password)).Show();
                    Close();
                }
                else
                {
                    MostrarToastMessage("Error","La operación no se ha llevado a cabo como se esperaba");
                    Close();
                }
            }catch(Exception ex)
            {
                MostrarToastMessage("Advertencia", ex.Message);
            }
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarUsuario();
                var user = PrepareUsuarioUpdate();
                answer = client.UpdateUsuario(user);
                if (answer.Key > 0)
                {
                    HOME.UpdateUser(client.GetUsuarioByEmail(user.Email, user.Password));
                    Close();
                }
                else
                {
                    MostrarToastMessage("Error", "La operación no se ha llevado a cabo como se esperaba");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MostrarToastMessage("Advertencia", ex.Message);
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();    
        }
    }
}
