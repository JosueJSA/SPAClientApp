using MaterialDesignThemes.Wpf;
using SPAClientApp.InsumosService;
using SPAClientApp.PedidosProveedoresService;
using SPAClientApp.ProveedoresService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for WIngredientes.xaml
    /// </summary>
    public partial class WPedidoProveedor : Window
    {
        private readonly PedidosProveedoresServiceClient clien = new PedidosProveedoresServiceClient();
        private readonly InsumosServiceClient insumosService = new InsumosServiceClient();
        private readonly ProveedoresServiceClient proveedoresService = new ProveedoresServiceClient();
        private static bool IsClosed { get; set; } = false;
        private static WPedidoProveedor IngredientesWindow { get; set; } = null;  
        private static WListaPedidosProveedores Parent { get; set; }
        private Notifier notifier { get; set; }
        private List<EInsumo> Insumos { get; set; } = null;
        private List<EProveedor> Proveedores { get; set;}
        private static EPedidoProveedor PEDIDO { get; set; }    

        public static WPedidoProveedor GetWindow(WListaPedidosProveedores parent, EPedidoProveedor pedido)
        {
            Parent = parent;
            PEDIDO = pedido;
            if(IngredientesWindow == null || IsClosed)
                IngredientesWindow = new WPedidoProveedor();
            return IngredientesWindow;  
        }

        private string FindNameProveedor(int id)
        {
            string nombre = Proveedores.FirstOrDefault(p => p.Clave == id).Nombre;
            return nombre;
        }

        private WPedidoProveedor()
        {
            InitializeComponent();
            ConfigurarToastNotifier(this, 3);
            IsClosed = false;
            Proveedores = proveedoresService.GetProveedoresList("Todos", null, "Activo").ToList();
            proveedor.Content = FindNameProveedor(PEDIDO.ClaveProveedor);
            LlenarTablaInsumos(PEDIDO.Insumos.ToList());
        }

        private void LlenarTablaInsumos(List<EInsumoPedido> insumos)
        {
            insumos.ForEach(i =>
            {
                i.Precio *= i.Cantidad;
            });
            tablaInsumos.ItemsSource = insumos;
        }

        private void MostrarToastMessage(string tipo, string mensaje)
        {
            if (tipo == "Advertencia")
                notifier.ShowWarning(mensaje);
            if (tipo == "Exito")
            {
                ConfigurarToastNotifier(Parent, 5);
                notifier.ShowSuccess(mensaje);
                IsClosed = true;    
                Close();
            }
            if (tipo == "Error")
            {
                ConfigurarToastNotifier(Parent, 5);
                notifier.ShowError(mensaje);
                IsClosed = true;
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

        private EInsumoPedido ConvertirAInsumoPedido(EInsumo insumo)
        {
            return new EInsumoPedido()
            {
                CodigoPedidoProveedor = -1,
                CodigoInsumo = insumo.Codigo,
                Cantidad = 1,
                Precio = insumo.PrecioCompra,
                Nombre = insumo.Nombre,
                Codigo = insumo.Codigo,
                Status = insumo.Status
            };
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            IsClosed = true;
            base.OnClosing(e);
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
