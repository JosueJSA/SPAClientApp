using SPAClientApp.ProductosService;
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
    public partial class WIngredientes : Window
    {
        private readonly ProductosServiceClient clien = new ProductosServiceClient();
        private static bool IsClosed { get; set; } = false;
        private static WIngredientes IngredientesWindow { get; set; } = null;  
        private static WProducto Parent { get; set; }
        private Notifier notifier { get; set; }
        private List<EInsumo> Insumos { get; set; } = null;

        public static WIngredientes GetWindow(WProducto parent)
        {
            Parent = parent;
            if(IngredientesWindow == null || IsClosed)
                IngredientesWindow = new WIngredientes();
            return IngredientesWindow;  
        }
        private WIngredientes()
        {
            InitializeComponent();
            ConfigurarToastNotifier(this, 3);
            IsClosed = false;
            var ingredientes = clien.GetIngredientes();
            if (ingredientes != null)
                LlenarTablaInsumos(Insumos = ingredientes.ToList());
            else
                MostrarToastMessage("Error", "Lo sentimos, el servidor no está respondiendo correctamente" +
                    " si el problema persiste, contacte a soporte técnico");
        }

        private void LlenarTablaInsumos(List<EInsumo> insumos)
        {
            tablaDatos.ItemsSource = insumos;
        }

        public void CargarIngredientes(List<EIngrediente> ingredientes)
        {
            foreach (EIngrediente ingrediente in ingredientes)
            {
                bool Existe = false;
                foreach (EInsumo insumo in tablaDatos.Items)
                {
                    if(insumo.Codigo == ingrediente.CodigoInsumo)
                        Existe = true;
                }
                if (!Existe)
                    MostrarToastMessage("Advertencia", $"El insumo '{ingrediente.NombreInsumo}' no se exntontró dado de alta en el sistema");
                else if(Existe && !EstaAgregado(ingrediente.CodigoInsumo))
                    tablaIngredientes.Items.Add(ingrediente);
            }
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
            var ingrediente = ConvertirAIngrediente(((FrameworkElement)sender).DataContext as EInsumo);
            if (EstaAgregado(ingrediente.CodigoInsumo))
                MostrarToastMessage("Advertencia", "El insumo ya ha sido agregado a la lista de ingredientes");
            else
                tablaIngredientes.Items.Add(ingrediente);
        }

        private EIngrediente ConvertirAIngrediente(EInsumo insumo)
        {
            return new EIngrediente()
            {
                CodigoInsumo = insumo.Codigo,
                NombreInsumo = insumo.Nombre,
                CantidadIngrediente = 1,
                CantidadInsumo = insumo.Cantidad,   
                Unidad = insumo.UnidadMedida,
                PrecioInsumo = insumo.PrecioCompra
            };
        }

        private void RemoverIngrediente(object sender, RoutedEventArgs e)
        {
            var ingrediente = ((FrameworkElement)sender).DataContext as EIngrediente;
            tablaIngredientes.Items.Remove(ingrediente);
        }

        private bool EstaAgregado(int id)
        {
            bool Agregado = false;
            foreach (EIngrediente ingrediente in tablaIngredientes.Items)
            {
                if(ingrediente.CodigoInsumo == id)
                    Agregado = true;    
            }
            return Agregado;
        }

        private void Guardar(object sender, RoutedEventArgs e)
        {
            List<EIngrediente> ingredientes = new List<EIngrediente>();
            foreach (EIngrediente ingrediente in tablaIngredientes.Items)
            {
                ingredientes.Add(ingrediente);
            }
            Parent.ActualizarTablaIngredientes(ingredientes);
            MostrarToastMessage("Exito", "los ingredientes han sido agregados exitosamente");
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
            if (!string.IsNullOrEmpty(ValorBusqueda.Text))
            {
                if (Criterio.Text == "Nombre")
                {
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
            else
            {
                MostrarToastMessage("Advertencia", "Debes escribir un valor en el cuadro de búsqueda");
            }
        }
    }
}
