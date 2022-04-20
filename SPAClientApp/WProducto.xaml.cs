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
        private string[] Scopes = { 
            DriveService.Scope.Drive,
            DriveService.Scope.DriveAppdata,
            DriveService.Scope.DriveFile,
            DriveService.Scope.DriveMetadataReadonly,
            DriveService.Scope.DriveReadonly,
            DriveService.Scope.DriveScripts
        };

        private string ApplicationName = "SAP Project";

        private readonly ProductosServiceClient client = new ProductosServiceClient();
        private readonly List<TextBox> UiInputElements;
        private readonly List<Button> UiButtons;
        private List<EIngrediente> Ingredientes { get; set; } = new List<EIngrediente>();
        private Notifier notifier;
        private string Operacion { get; set; } = "Consulta";
        private EProducto Producto { get; set; } = new EProducto();
        private EReceta Receta { get; set; } = new EReceta();
        private WListaProductos ParentWindow { get; set; }
        private UserCredential Credential { get; set; }
        private string FileUrl { get; set; } = string.Empty;   

        public WProducto(WListaProductos parent)
        {
            InitializeComponent();
            ParentWindow = parent;
            UiInputElements = new List<TextBox>() { NombreTxt, PrecioCompraTxt, CantidadTxt, DescripcionTxt, RestriccionesTxt, PrecioVentaTxt, ProcedimientoTxt };
            UiButtons = new List<Button>() { CancelarBtn, ActualizarBtn, RegistrarBtn, CerrarBtn, EditarBtn, UploadBtn };
            ConfigurarToastNotifier(this, 3);
        }

        public void ActivarModoLectura(EProducto producto)
        {
            var receta = client.GetReceta(producto.CodigoReceta);
            if (receta != null)
            {
                LlenarCampos(producto, receta);
                Operacion = "Consulta";
                UiInputElements.ForEach(e => e.IsReadOnly = true);
                UiButtons.ForEach(b => b.Visibility = Visibility.Collapsed);
                CerrarBtn.Visibility = Visibility.Visible;
                EstadoTxt.Visibility = Visibility.Visible;
                EstadoTxt.Visibility = Visibility.Visible;
            }
            else
            {
                MostrarToastMessage("Error", "Lo sentimos, el servidor no ha respondido correctamente, si el problema persiste" +
                    " favor de contactar a soporte técnico");
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
                var receta = client.GetReceta(producto.CodigoReceta);
                if (receta != null)
                {
                    Operacion = "Actualizacion";
                    new List<Button> { CancelarBtn, ActualizarBtn }.ForEach(b => b.Visibility = Visibility.Visible);
                    LlenarCampos(producto, receta);
                    Ingredientes = receta.Ingredientes.ToList();
                }
                else
                {
                    MostrarToastMessage("Error", "Lo sentimos, el servidor no ha respondido correctamente, si el problema persiste" +
                        " favor de contactar a soporte técnico");
                }
            }
        }

        private void LlenarCampos(EProducto producto, EReceta receta)
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
            ProcedimientoTxt.Text = receta.Descripcion;
            tablaDatos.ItemsSource = receta.Ingredientes;
            try
            {
                Image.ImageSource = new BitmapImage(new Uri(producto.Foto));
            }
            catch (Exception e) { }
        }

        public void ActualizarTablaIngredientes(List<EIngrediente> ingredientes)
        {
            Ingredientes = ingredientes;
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
            window.CargarIngredientes(Ingredientes);
        }

        private void CancelarOperacion(object sender, RoutedEventArgs e)
        {
            string mensaje = "¿Deseas salir sin guardar los cambios?";
            if (MostrarCuadroConfirmacion(mensaje))
            {
                if (!string.IsNullOrEmpty(FileUrl))
                {
                    EliminarFoto(FileUrl.Substring(31), ConfigurarDriveAPI());
                }
                Close();
            }
        }

        private bool MostrarCuadroConfirmacion(string message)
        {
            MessageBoxResult boxResult = MessageBox.Show(message, "Advertencia", MessageBoxButton.YesNoCancel);
            return MessageBoxResult.Yes == boxResult;
        }

        private void Registrar(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarProducto();
                PrepararNuevoProducto();
                PrepararReceta();
                AnswerMessage response = client.AddProducto(Producto, Receta);
                if (response.Key > 0)
                {
                    MostrarToastMessage("Exito", "El producto se ha registrado exitosamente");
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

        private void ValidarProducto()
        {
            if (ValidarAuxiliar(NombreTxt.Text))
                throw new ArgumentException("El nombre debe ser una cadena de texto");
            float auxf = 0;
            if (string.IsNullOrEmpty(PrecioVentaTxt.Text) || !float.TryParse(PrecioVentaTxt.Text, out auxf) || auxf < 0)
                throw new ArgumentException("El precio de venta del producto debe ser un número > 0");
            if (string.IsNullOrEmpty(PrecioCompraTxt.Text) || !float.TryParse(PrecioCompraTxt.Text, out auxf) || auxf < 0)
                throw new ArgumentException("El precio de compra del producto debe ser un número > 0");
            if (ValidarAuxiliar(DescripcionTxt.Text))
                throw new ArgumentException("La descripción debe ser una cadena de texto");
            if (ValidarAuxiliar(RestriccionesTxt.Text))
                throw new ArgumentException("Las Restricciones deben ser estar escritas en formato de texto");
            if (!TieneNombreUnico(Producto.Nombre, NombreTxt.Text))
                throw new ArgumentException("El nombre del Insumo ya has sido registrado en el sistema");
            if (Operacion == "Registro" && string.IsNullOrEmpty(FileUrl))
                throw new ArgumentException("Debes subir una foto del producto");
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

        private void PrepararNuevoProducto()
        {
            Producto.PrecioCompra = float.Parse(PrecioCompraTxt.Text);
            Producto.PrecioVenta = float.Parse(PrecioVentaTxt.Text);
            Producto.Nombre = NombreTxt.Text;
            if (!string.IsNullOrEmpty(FileUrl))
            {
                var aux = Producto.Foto;
                Producto.Foto = FileUrl;
                if(!string.IsNullOrEmpty(aux))
                    EliminarFoto(aux.Substring(31), ConfigurarDriveAPI());
            }
            Producto.Descripcion = DescripcionTxt.Text;
            Producto.Restricciones = RestriccionesTxt.Text;
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

        private void Actualizar(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarProducto();
                PrepararNuevoProducto();
                PrepararReceta();
                AnswerMessage response = client.UpdateProducto(Producto, Receta);
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

        [Obsolete]
        private DriveService ConfigurarDriveAPI()
        {
            DriveService service = null;
            try
            {
                string settings = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"), @"Resources\client_secret.json"));
                using (var stream = new FileStream(settings, FileMode.Open, FileAccess.Read))
                {
                    string credPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".credentials/drive-dotnet-quickstart.json");
                    Credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = Credential,
                    ApplicationName = ApplicationName,
                });
            }
            catch (Exception ex)
            {
                service = null;
                MostrarToastMessage("Error", ex.Message);
            }
            return service;
        }

        private void EliminarFoto(string id, DriveService service)
        {
            if (service != null)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    FilesResource.DeleteRequest auxRequest;
                    auxRequest = service.Files.Delete(id);
                    auxRequest.Execute();
                }
            }
            else
            {
                MostrarToastMessage("Error", "El servicio no fue iniciado correctamente");
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

        [Obsolete]
        private void CargarImagen(object sender, RoutedEventArgs e)
        {
            var service = ConfigurarDriveAPI();
            if (service != null)
            {
                try
                {
                    string path;
                    if (!string.IsNullOrEmpty(path = ObtenerImagenPath()))
                    {
                        if (!string.IsNullOrEmpty(FileUrl))
                            EliminarFoto(FileUrl.Substring(31), ConfigurarDriveAPI());
                        var file = new Google.Apis.Drive.v3.Data.File();
                        file.Parents = new string[] { "1AE2JMSauYqhETj7QDnmsSbSzbuMkFCcE" };
                        FilesResource.CreateMediaUpload request;
                        using (var stream = new FileStream(path, FileMode.Open))
                        {
                            file.Name = stream.Name;
                            request = service.Files.Create(file, stream, file.MimeType);
                            request.Fields = "id";
                            request.Upload();
                        }
                        var response = request.ResponseBody;
                        FileUrl = $"https://drive.google.com/uc?id={response.Id}";
                        service.Permissions.Create(new Permission() { Type = "anyone", Role = "writer" }, response.Id).Execute(); //Creating Permission after folder creation.
                        Image.ImageSource = new BitmapImage(new Uri(FileUrl));
                        MostrarToastMessage("Info", "La imagen se ha cargado correctamente");
                    }
                }
                catch (Exception ex)
                {
                    MostrarToastMessage("Error", ex.Message);
                }
            }
        }
    }
}
