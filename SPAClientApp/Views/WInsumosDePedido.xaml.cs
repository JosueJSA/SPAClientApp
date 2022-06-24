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
    public partial class WInsumosDePedido : Window
    {
        private readonly PedidosProveedoresServiceClient clien = new PedidosProveedoresServiceClient();
        private readonly InsumosServiceClient insumosService = new InsumosServiceClient();
        private readonly ProveedoresServiceClient proveedoresService = new ProveedoresServiceClient();
        private static bool IsClosed { get; set; } = false;
        private static WInsumosDePedido IngredientesWindow { get; set; } = null;  
        private static WListaPedidosProveedores Parent { get; set; }
        private Notifier notifier { get; set; }
        private List<EInsumo> Insumos { get; set; } = null;
        private List<EProveedor> Proveedores { get; set;}

        public static WInsumosDePedido GetWindow(WListaPedidosProveedores parent)
        {
            Parent = parent;
            if(IngredientesWindow == null || IsClosed)
                IngredientesWindow = new WInsumosDePedido();
            return IngredientesWindow;  
        }

        private void LoadProveedores()
        {
            Proveedores = proveedoresService.GetProveedoresList("Todos", null, "Activo").ToList();
            foreach(var proveedor in Proveedores)
            {
                comboboxProveedores.Items.Add(proveedor.Nombre);
            }
        }

        private int FindIdProveedorByName(string name)
        {
            int clave = -1;
            clave = Proveedores.FirstOrDefault(p => p.Nombre == name).Clave;
            return clave;
        }

        private WInsumosDePedido()
        {
            InitializeComponent();
            ConfigurarToastNotifier(this, 3);
            IsClosed = false;
            var insumos = insumosService.GetInsumosList("Todos",null, Convert.ToDateTime("1800-01-01"), "Activo");
            LoadProveedores();
            if (insumos != null)
                LlenarTablaInsumos(Insumos = insumos.ToList());
            else
                MostrarToastMessage("Error", "Lo sentimos, el servidor no está respondiendo correctamente" +
                    " si el problema persiste, contacte a soporte técnico");
        }

        private void LlenarTablaInsumos(List<EInsumo> insumos)
        {
            tablaDatos.ItemsSource = insumos;
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

        private void AgregarIngrediente(object sender, RoutedEventArgs e)
        {
            var insumoPedido = ConvertirAInsumoPedido(((FrameworkElement)sender).DataContext as EInsumo);
            if (EstaAgregado(insumoPedido.CodigoInsumo))
                MostrarToastMessage("Advertencia", "El insumo ya ha sido agregado a la lista de ingredientes");
            else
                tablaInsumos.Items.Add(insumoPedido);
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

        private void RemoverIngrediente(object sender, RoutedEventArgs e)
        {
            var insumo = ((FrameworkElement)sender).DataContext as EInsumoPedido;
            tablaInsumos.Items.Remove(insumo);
        }

        private bool EstaAgregado(int id)
        {
            bool Agregado = false;
            foreach (EInsumoPedido insumo in tablaInsumos.Items)
            {
                if(insumo.CodigoInsumo == id)
                    Agregado = true;    
            }
            return Agregado;
        }

        private void Guardar(object sender, RoutedEventArgs e)
        {
            List<EInsumoPedido> insumos = new List<EInsumoPedido>();
            foreach (EInsumoPedido insumo in tablaInsumos.Items)
            {
                insumos.Add(insumo);
            }
            PedidosProveedoresService.AnswerMessage answer = clien.AddPedidoProveedor(new EPedidoProveedor()
            {
                Codigo = -1,
                ClaveProveedor = FindIdProveedorByName(comboboxProveedores.Text),
                Cantidad = 1,
                TipoPedido = "Entrega",
                Status = "Activo",
                Insumos = insumos.ToArray()
            });
            if (answer.Key > 0)
                MostrarToastMessage("Exito", "Se ha realizado el pedido");
            else
                MostrarToastMessage("Error", "Lo sentimos algo ha salido mal");
        }

        private void Cancelar(object sender, RoutedEventArgs e)
        {
            IsClosed = true;
            Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            IsClosed = true;
            base.OnClosing(e);
        }

        private void Buscar(object sender, RoutedEventArgs e)
        {
            if (Criterio.Text == "Nombre")
            {
                if (string.IsNullOrEmpty(ValorBusqueda.Text))
                    MostrarToastMessage("Advertencia", "Debes escribir un valor en el cuadro de búsqueda");
                else
                    LlenarTablaInsumos((Insumos.Where(i => i.Nombre.ToLower().Contains(ValorBusqueda.Text.ToLower()))).ToList());
            }
            else if (Criterio.Text == "Código")
            {
                int value = 0;
                if (int.TryParse(ValorBusqueda.Text, out value))
                    LlenarTablaInsumos((Insumos.Where(i => i.Codigo == value)).ToList());
                else
                    MostrarToastMessage("Advertencia", "El valor del código debe ser númerico");
            }
            else
            {
                LlenarTablaInsumos(Insumos);
            }
        }

        private void SeleccionarCriterio(object sender, EventArgs e)
        {
            var criterioCbBox= sender as ComboBox;
            if (criterioCbBox.Text == "Todos")
            {
                ValorBusqueda.IsEnabled = false;
                HintAssist.SetHint(ValorBusqueda, "Valor no requerido");
            }
            else
            {
                ValorBusqueda.IsEnabled = true;
                HintAssist.SetHint(ValorBusqueda, $"Escribe el {criterioCbBox.Text} del insumo");
            }
        }
    }
}
