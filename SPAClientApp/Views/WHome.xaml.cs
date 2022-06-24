using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Microsoft.Win32;
using SPAClientApp.UsuariosService;
using SPAClientApp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class WHome : Window
    {
        private EUsuario USUARIO;
        public Notifier notifier { get; set; }

        public WHome(EUsuario usaurio)
        {
            InitializeComponent();
            USUARIO = usaurio;
            userLbl.Content = $"[{USUARIO.TipoUsuario}]: {USUARIO.Nombre}";
            ConfigurarToastNotifier(this, 3);
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();  
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Maximizar(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void Minimizar(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void VerInsumos(object sender, RoutedEventArgs e)
        {
            if (USUARIO.TipoUsuario == "Administrador" || USUARIO.TipoUsuario == "Chef")
            {
                WListaInsumos.GetWListaInsumos(this).Show();
            }
            else
            {
                MostrarToastMessage("Advertencia", "Lo sentimos, solo los administradores o chefs pueden entrar aquí");
            }
        }

        private void VerProductos(object sender, RoutedEventArgs e)
        {
            if (USUARIO.TipoUsuario == "Administrador" || USUARIO.TipoUsuario == "Chef")
            {
                WListaProductos.GetWListaProductos(this).Show();
            }
            else
            {
                MostrarToastMessage("Advertencia", "Lo sentimos, solo los administradores o chefs pueden entrar aquí");
            }
        }

        private void VerPedidosClientes(object sender, RoutedEventArgs e)
        {
            WListaPedidosClientes.GetPedidosClientesWindow(this).Show();
        }

        private void VerCliente(object sender, RoutedEventArgs e)
        {
            if (USUARIO.TipoUsuario == "Administrador")
            {
               new WListaClientes(this).Show(); 
            }
            else
            {
                MostrarToastMessage("Advertencia", "Lo sentimos, solo los administradores pueden entrar aquí");
            }
        }

        private void VerUsuarios(object sender, RoutedEventArgs e)
        {
            if (USUARIO.TipoUsuario == "Administrador")
            {
                new WListaUsuarios(this).Show();
            }
            else
            {
                MostrarToastMessage("Advertencia", "Lo sentimos, solo los administradores pueden entrar aquí");
            }
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

        private void ConsultarUsuario(object sender, RoutedEventArgs e)
        {
            new WUsuario("Request", USUARIO).Show();
        }

        private void UpdateUser(object sender, RoutedEventArgs e)
        {
            new WUsuario("Update", USUARIO).Show();
        }

        private void CerrarSesion(object sender, RoutedEventArgs e)
        {
            new WLogin().Show();
            Close();
        }

        private void CerrarPrograma(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void VerProveedores(object sender, RoutedEventArgs e)
        {
            new WListaProveedores(this).Show();
        }

        private void VerPedidosProveedores(object sender, RoutedEventArgs e)
        {
            new WListaPedidosProveedores(this).Show();
        }

        public void UpdateUser(EUsuario usuario)
        {
            USUARIO = usuario;
            MostrarToastMessage("Exito", "Para que tus cambios es recomendable vean reflejados debes iniciar sesión nuevamente");
        }
    }
}
