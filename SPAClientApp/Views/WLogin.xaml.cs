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
    /// Interaction logic for WLogin.xaml
    /// </summary>
    public partial class WLogin : Window
    {
        public Notifier notifier { get; set; }
        UsuariosServiceClient client = new UsuariosServiceClient();

        public WLogin()
        {
            InitializeComponent();
            ConfigurarToastNotifier(this, 3);
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = client.GetUsuarioByEmail(email.Text, password.Password.ToString());
                if (user != null)
                {
                    new WHome(user).Show();
                    Close();
                }
                else
                {
                    MostrarToastMessage("Advertencia", "Verifica tu usuario o contraseña");
                }
            }catch (Exception)
            {
                MostrarToastMessage("Advertencia", "Verifica tu usuario o contraseña");
            }
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {
            new WUsuario("SignUp").Show();
            Close();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();    
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
    }
}
