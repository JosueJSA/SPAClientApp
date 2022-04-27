using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using SPAClientApp.ProductosService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using System.Windows.Shapes;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace SPAClientApp
{
    /// <summary>
    /// Interaction logic for WProducto.xaml
    /// </summary>
    public partial class WProducto : Window
    {

        private readonly ProductosServiceClient client = new ProductosServiceClient();
        private readonly List<TextBox> UiInputElements;
        private readonly List<Button> UiButtons;

        private Notifier notifier;
        private string Operacion { get; set; } = "Consulta";
        private EProducto Producto { get; set; } = new EProducto();
        private EReceta Receta { get; set; } = new EReceta();
        private WListaProductos ParentWindow { get; set; }
        private UserCredential Credential { get; set; }
        private string FileUrl { get; set; } = string.Empty;   
        private bool ConReceta { get; set; } = false;

        public WProducto(WListaProductos parent)
        {
            InitializeComponent();
            ParentWindow = parent;
            UiInputElements = new List<TextBox>() { NombreTxt, PrecioCompraTxt, CantidadTxt, DescripcionTxt, RestriccionesTxt, PrecioVentaTxt, ProcedimientoTxt, EstadoTxt };
            UiButtons = new List<Button>() { CancelarBtn, ActualizarBtn, RegistrarBtn, CerrarBtn, EditarBtn, UploadBtn };
            ConfigurarToastNotifier(this, 3);
        }

        public void ActivarModoLectura(EProducto producto)
        {
            LlenarCampos(producto);
            Operacion = "Consulta";
            UiInputElements.ForEach(e => e.IsReadOnly = true);
            UiButtons.ForEach(b => b.Visibility = Visibility.Collapsed);
            CheckBoxReceta.IsEnabled = false;
            CerrarBtn.Visibility = Visibility.Visible;
            EstadoTxt.Visibility = Visibility.Visible;
            EstadoTxt.Visibility = Visibility.Visible;

            if (producto.CodigoReceta > 0)
            {
                Receta = client.GetReceta(Convert.ToInt32(producto.CodigoReceta));
                if (Receta != null)
                {
                    CheckBoxReceta.IsEnabled = false;
                    CheckBoxReceta.IsChecked = true;
                    LlenarCamposReceta(Receta);
                }
                else
                {
                    MostrarToastMessage("Error", "Lo sentimos, el servidor no ha respondido correctamente, si el problema persiste" +
                        " favor de contactar a soporte técnico");
                }
            } 
        }

        public void ActivarModoEdicion(string modo, EProducto producto)
        {
            UiInputElements.ForEach(e => e.IsReadOnly = false);
            UiButtons.ForEach(b => b.Visibility = Visibility.Collapsed);
            EditarBtn.Visibility = Visibility.Visible;  
            EstadoTxt.Visibility = Visibility.Collapsed;
            Estadolbl.Visibility = Visibility.Collapsed;
            UploadBtn.Visibility = Visibility.Visible;  
            Producto = producto;
            if ((Operacion = modo) == "Registro")
            {
                FechaDatePicker.Text = DateTime.Now.ToString();
                (new List<Button> { CancelarBtn, RegistrarBtn }).ForEach(b => b.Visibility = Visibility.Visible);
            }
            else
            {
                new List<Button> { CancelarBtn, ActualizarBtn }.ForEach(b => b.Visibility = Visibility.Visible);
                Operacion = "Actualizacion";
                CheckBoxReceta.IsChecked = false;
                LlenarCampos(producto);
                if (producto.CodigoReceta >= 0)
                {
                    Receta = client.GetReceta(Convert.ToInt32(producto.CodigoReceta));
                    if (Receta != null)
                    {
                        CheckBoxReceta.IsChecked = true;
                        LlenarCamposReceta(Receta);
                    }
                    else
                    {
                        MostrarToastMessage("Error", "Lo sentimos, el servidor no ha respondido correctamente, si el problema persiste" +
                            " favor de contactar a soporte técnico");
                    }
                }
            }
        }

        private void LlenarCampos(EProducto producto)
        {
            NombreTxt.Text = producto.Nombre;
            FechaDatePicker.Text = producto.Registro.ToString();
            Codigolbl.Content = producto.Codigo.ToString();
            EstadoTxt.Text = producto.Status;
            CantidadTxt.Text = producto.Cantidad.ToString();
            PrecioCompraTxt.Text = producto.PrecioCompra.ToString();
            PrecioVentaTxt.Text = producto.PrecioVenta.ToString();
            DescripcionTxt.Text = producto.Descripcion;
            RestriccionesTxt.Text = producto.Restricciones;
            Image.ImageSource = new BitmapImage(new Uri(producto.Foto));
        }

        private void LlenarCamposReceta(EReceta receta)
        {
            ProcedimientoTxt.Text = receta.Descripcion;
            ActualizarTablaIngredientes(receta.Ingredientes.ToList());
        }

        public void ActualizarTablaIngredientes(List<EIngrediente> ingredientes)
        {
            tablaDatos.ItemsSource = ingredientes;
        }

        private void MostrarToastMessage(string tipo, string mensaje)
        {
            if (tipo == "Advertencia")
                notifier.ShowWarning(mensaje);
            if (tipo == "Info")
                notifier.ShowInformation(mensaje);
            if (tipo == "Exito")
            {
                ConfigurarToastNotifier(ParentWindow, 5);
                notifier.ShowSuccess(mensaje);
                ParentWindow.RefrescarTabla();
                Close();
            }
            if (tipo == "Error")
            {
                ConfigurarToastNotifier(ParentWindow, 5);
                notifier.ShowError(mensaje);
                ParentWindow.RefrescarTabla();
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

        private void EditarIngredientes(object sender, RoutedEventArgs e)
        {
            var window = WIngredientes.GetWindow(this);
            window.Show();
            if (Receta.Ingredientes != null)
                window.CargarIngredientes(Receta.Ingredientes.ToList());
        }

        [Obsolete]
        private async void CancelarOperacion(object sender, RoutedEventArgs e)
        {
            string mensaje = "¿Deseas salir sin guardar los cambios?";
            if (MostrarCuadroConfirmacion(mensaje))
            {
                if (!string.IsNullOrEmpty(FileUrl))
                {
                    try
                    {
                        await GoogleDriveAPI.EliminarFoto(FileUrl.Substring(31), null);
                    }
                    catch (Exception ex)
                    {
                        MostrarToastMessage("Error", ex.Message);
                    }
                }
                Close();
            }
        }

        private bool MostrarCuadroConfirmacion(string message)
        {
            MessageBoxResult boxResult = MessageBox.Show(message, "Advertencia", MessageBoxButton.YesNoCancel);
            return MessageBoxResult.Yes == boxResult;
        }

        [Obsolete]
        private void Registrar(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarProducto();
                PrepararNuevoProducto();
                AnswerMessage response;
                if (ConReceta)
                {
                    PrepararReceta();
                    response = client.AddProducto(Producto, Receta);
                }
                else
                {
                    Producto.CodigoReceta = -1;
                    response = client.AddProducto(Producto, null);
                }
                if (response.Key > 0)
                    MostrarToastMessage("Exito", "El producto se ha registrado exitosamente");
                else
                    MostrarToastMessage("Error", response.Message);
            }
            catch (ArgumentException ex)
            {
                MostrarToastMessage("Advertencia", ex.Message);
            }
        }

        private void ValidarProducto()
        {
            if (ValidarAuxiliar(NombreTxt.Text))
                throw new ArgumentException("El nombre debe ser una cadena de texto");
            float auxf = 0;
            if (string.IsNullOrEmpty(PrecioVentaTxt.Text) || !float.TryParse(PrecioVentaTxt.Text, out auxf) || auxf < 0)
                throw new ArgumentException("El precio de venta del producto debe ser un número > 0");
            if (string.IsNullOrEmpty(PrecioCompraTxt.Text) || !float.TryParse(PrecioCompraTxt.Text, out auxf) || auxf < 0)
                throw new ArgumentException("El precio de compra del producto debe ser un número > 0");
            if (!ConReceta)
            {
                int auxi = 0;
                if (string.IsNullOrEmpty(CantidadTxt.Text) || !int.TryParse(PrecioCompraTxt.Text, out auxi) || auxi < 0)
                    throw new ArgumentException("El precio de compra del producto debe ser un número > 0");
            }
            if (ValidarAuxiliar(DescripcionTxt.Text))
                throw new ArgumentException("La descripción debe ser una cadena de texto");
            if (ValidarAuxiliar(RestriccionesTxt.Text))
                throw new ArgumentException("Las Restricciones deben ser estar escritas en formato de texto");
            if (!TieneNombreUnico(Producto.Nombre, NombreTxt.Text))
                throw new ArgumentException("El nombre del Insumo ya has sido registrado en el sistema");
            if (Operacion == "Registro" && string.IsNullOrEmpty(FileUrl))
                throw new ArgumentException("Debes subir una foto del producto");
            if (CheckBoxReceta.IsChecked ?? false)
                ValidarReceta();
        }

        private void ValidarReceta()
        {
            if (ValidarAuxiliar(ProcedimientoTxt.Text))
                throw new ArgumentException("La descripción de la receta, debe describir el procedimiento en formato texto");
            if (tablaDatos.Items.Count <= 0)
                throw new ArgumentException("La receta al menos debe contener 1 ingrediente y su respectiva cantidad");
            string response;
            if (!string.IsNullOrEmpty(response = RevisarIngredientes()))
                throw new ArgumentException(response);
        }

        private bool ValidarAuxiliar(string value)
        {
            return (string.IsNullOrEmpty(value) || double.TryParse(value, out _) || string.IsNullOrEmpty(value.Trim()));
        }

        private bool TieneNombreUnico(string nombreActual, string nombreABuscar)
        {
            if (Operacion == "Registro")
                return !client.IsDuplicated(" ", nombreABuscar);
            else
                return !client.IsDuplicated(nombreActual, nombreABuscar);
        }

        private string RevisarIngredientes()
        {
            List<EIngrediente> ingredientes = new List<EIngrediente>();
            foreach (EIngrediente ingrediente in tablaDatos.Items)
            {
                ingredientes.Add(ingrediente);
            }
            return client.CheckIngredientesStatus(ingredientes.ToArray());
        }

        [Obsolete]
        private async void PrepararNuevoProducto()
        {
            try
            {
                Producto.PrecioCompra = float.Parse(PrecioCompraTxt.Text);
                Producto.PrecioVenta = float.Parse(PrecioVentaTxt.Text);
                Producto.Nombre = NombreTxt.Text;
                if (!string.IsNullOrEmpty(FileUrl))
                {
                    var aux = Producto.Foto;
                    Producto.Foto = FileUrl;
                    if (!string.IsNullOrEmpty(aux))
                        await GoogleDriveAPI.EliminarFoto(aux.Substring(31), null);
                }
                if (!ConReceta)
                    Producto.Cantidad = int.Parse(CantidadTxt.Text);
                Producto.Descripcion = DescripcionTxt.Text;
                Producto.Restricciones = RestriccionesTxt.Text;
            } catch (Exception e)
            {
                MostrarToastMessage("Error", e.Message);
            }
        }

        private void PrepararReceta()
        {
            Receta.Descripcion = ProcedimientoTxt.Text;
            List<EIngrediente> ingredientes = new List<EIngrediente>();
            foreach (EIngrediente ingrediente in tablaDatos.Items)
            {
                ingredientes.Add(ingrediente);
            }
            Receta.Ingredientes = ingredientes.ToArray();
        }

        [Obsolete]
        private void Actualizar(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarProducto();
                PrepararNuevoProducto();
                AnswerMessage response;
                if (ConReceta)
                {
                    PrepararReceta();
                    response = client.UpdateProducto(Producto, Receta);
                }
                else
                {
                    Producto.CodigoReceta = -1;
                    response = client.UpdateProducto(Producto, null);
                }
                if (response.Key > 0)
                {
                    MostrarToastMessage("Exito", "El producto se ha actualizado exitosamente");
                }
                else
                {
                    MostrarToastMessage("Error", response.Message);
                }
            }
            catch (ArgumentException ex)
            {
                MostrarToastMessage("Advertencia", ex.Message);
            }
        }

        private string ObtenerImagenPath()
        {
            string path = string.Empty;
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog()
            {
                Title = "Selecciona la imagen a subir",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.img;*.image;*.bitmap;",
                FilterIndex = 0
            };
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                path = dialog.FileName;
            }
            return path;
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ActivarReceta(object sender, RoutedEventArgs e)
        {
            this.Height = 820;
            ConReceta = true;
            RecetaSection.Visibility = Visibility.Visible;
            ProcedimientoSection.Visibility = Visibility.Visible;
            IngredientesSection.Visibility = Visibility.Visible;
            CantidadTxt.IsEnabled = false;
        }

        private void DesactivarReceta(object sender, RoutedEventArgs e)
        {
            this.Height = 420;
            ConReceta = false;
            RecetaSection.Visibility = Visibility.Collapsed;
            ProcedimientoSection.Visibility = Visibility.Collapsed;
            IngredientesSection.Visibility = Visibility.Collapsed;
            CantidadTxt.IsEnabled = true;
        }

        [Obsolete]
        private async void CargarImagen(object sender, RoutedEventArgs e)
        {
            string path;
            if (!string.IsNullOrEmpty(path = ObtenerImagenPath()))
            {
                try
                {
                    Progress.Visibility = Visibility.Visible;
                    FileUrl = await GoogleDriveAPI.CargarImagen(path, FileUrl);
                    Image.ImageSource = new BitmapImage(new Uri(FileUrl));
                    Progress.Visibility = Visibility.Collapsed;
                    MostrarToastMessage("Info", "Imágen cargada exitosamente");
                }catch (Exception ex)
                {
                    MostrarToastMessage("Advertencia", ex.Message);
                }
            }
        }
    }
}
