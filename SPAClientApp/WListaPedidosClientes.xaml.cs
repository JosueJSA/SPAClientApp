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
using System.Windows.Shapes;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace SPAClientApp
{
    /// <summary>
    /// Interaction logic for WListaPedidosClientes.xaml
    /// </summary>
    public partial class WListaPedidosClientes : Window
    {
        private const int INTERVAL = 10000; //1/6 minuto
        private readonly PedidosClientesServiceClient client = new PedidosClientesServiceClient();
        private Notifier notifier;
        private WHome HomeWindow { get; set; }
        private readonly Dictionary<string, DataGrid> tablas;
        readonly System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            

        public WListaPedidosClientes(WHome home)
        {
            InitializeComponent();
            HomeWindow = home;
            ConfigurarToastNotifier(this, 3);
            tablas = new Dictionary<string, DataGrid>()
            {
                {"Ordenado", TablaOrden },
                {"En preparación", TablaEnPreparacion },
                {"Preparado", TablaPreparado },
                {"Entregado", TablaEntregado },
                {"Cancelado", TablaCancelado }
            };
            timer.Tick += new EventHandler(RefrescarTabla);
            timer.Interval = INTERVAL;
            timer.Start();
        }

        private void RefrescarTabla(object sender, EventArgs e)
        {
            CleanTables();
            var result = client.GetCommonPedidosList();
            result.ToList().ForEach(element => { tablas[element.Status].Items.Add(element); });
            BadgedOrdenes.Badge = TablaOrden.Items.Count;
            BadgedEnPreparacion.Badge = TablaEnPreparacion.Items.Count;
            BadgedPreparado.Badge = TablaPreparado.Items.Count;
            BadgedEntregado.Badge = TablaEntregado.Items.Count;
            BadgedCancelado.Badge = TablaCancelado.Items.Count; 
        }

        private void CleanTables()
        {
            DataGrid tabla = null;
            foreach (var item in tablas.Keys)
            {
                if(item != "Entregao" && item != "Cancelado")
                    tablas.TryGetValue(item, out tabla);
                if (tabla != null)
                    tabla.Items.Clear();
            }
        }

        private void MostrarToastMessage(string tipo, string mensaje)
        {
            if (tipo == "Advertencia")
                notifier.ShowWarning(mensaje);
            if (tipo == "Exito")
                notifier.ShowSuccess(mensaje);
            if (tipo == "Info")
                notifier.ShowInformation(mensaje);
            if (tipo == "Error")
            {
                ConfigurarToastNotifier(HomeWindow, 5);
                notifier.ShowError(mensaje);
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

        private void Salir(object sender, RoutedEventArgs e)
        {
            timer.Stop();   
            Close();
        }

        private void RegistrarPedido(object sender, RoutedEventArgs e)
        {
            WPedidoCliente.GetWListaPedidosClientesWindow(this).Show();
        }
    }
}
