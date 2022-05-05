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
    /// Interaction logic for WCliente.xaml
    /// </summary>
    public partial class WCliente : Window
    {
        private static WCliente window = null;
        public Notifier notifier { get; set; }
        public WHome Home { get; set; }

        public static WCliente GetWClient(WHome home)
        {
            if(window == null)
            {
                return (window = new WCliente() {Home = home});
            }
            else
            {
                return window;
            }
        }

        public static WCliente GetWClient()
        {
            if (window == null)
            {
                return (window = new WCliente());
            }
            else
            {
                return window;
            }
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
                ConfigurarToastNotifier(Home, 5);
                notifier.ShowError(mensaje);
                Close();
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

        private WCliente()
        {
            InitializeComponent();
            ConfigurarToastNotifier(this, 3);
        }
    }
}
