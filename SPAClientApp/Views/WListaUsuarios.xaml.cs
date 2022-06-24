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

namespace SPAClientApp
{
    /// <summary>
    /// Interaction logic for WListaInsumos.xaml
    /// </summary>
    public partial class WListaUsuarios : Window
    {
        private readonly UsuariosServiceClient  client = new UsuariosServiceClient();
        AnswerMessage answer = new AnswerMessage();

        private Notifier notifier; 
        private WHome HomeWindow { get; set; }
        private string Status { get; set; } 
        private string Valor { get; set; }

        public WListaUsuarios(WHome home)
        {
            InitializeComponent();
            HomeWindow = home;
            ConfigurarToastNotifier(this, 3);
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DarDeBaja(object sender, RoutedEventArgs e)
        {
            var usuario = ((FrameworkElement)sender).DataContext as EUsuario;
            if (MostrarCuadroConfirmacion("Deseas dar de baja el insumo seleccionado"))
            {
                answer = client.ChangeStatusUsuario(usuario.Clave, "Dado de baja");
                if (answer.Key > 0)
                    MostrarToastMessage("Exito", "El usuario ha sido dado de baja");
                else
                    MostrarToastMessage("Error", "Los sentimos, algo ha salido mal");
                RefrescarTabla();
            }
        }

        private void Activar(object sender, RoutedEventArgs e)
        {
            var usuario = ((FrameworkElement)sender).DataContext as EUsuario;
            if (MostrarCuadroConfirmacion("Deseas dar de alta el insumo seleccionado"))
            {
                answer = client.ChangeStatusUsuario(usuario.Clave, "Activo");
                if (answer.Key > 0)
                    MostrarToastMessage("Exito", "El usuario ha sido dado de alta");
                else
                    MostrarToastMessage("Error", "Los sentimos, algo ha salido mal");
                RefrescarTabla();
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

        private bool MostrarCuadroConfirmacion(string message)
        {
            MessageBoxResult boxResult = MessageBox.Show(message, "Advertencia", MessageBoxButton.YesNoCancel);
            return MessageBoxResult.Yes == boxResult;
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

        private void Window_Closing(object sender, EventArgs e)
        {
            Close();
        }

        private void RefrescarTabla()
        {
            var usuarios = client.GetUsuarios(Status, Valor);
            tablaDatos.ItemsSource = usuarios.ToList();
        }

        private void BuscarUsuarios(object sender, RoutedEventArgs e)
        {
            try
            {
                Status = (bool)CheckBoxActivos.IsChecked ? "Activo" : "Dado de baja";
                Valor = string.IsNullOrEmpty(ValorBusqueda.Text) ? null: ValorBusqueda.Text;
                var usuarios = client.GetUsuarios(Status, Valor);
                tablaDatos.ItemsSource = usuarios.ToList();
                if(Status == "Activo")
                {
                    ColumnActive.Visibility = Visibility.Collapsed;
                    ColumnEliminate.Visibility = Visibility.Visible;
                }
                else
                {
                    ColumnEliminate.Visibility = Visibility.Collapsed;  
                    ColumnActive.Visibility = Visibility.Visible;
                }
            }
            catch (ArgumentException)
            {
                MostrarToastMessage("Warning", "Debes ingresar el nombre del usuario que deseas buscar");
            }
        }
    }
}

