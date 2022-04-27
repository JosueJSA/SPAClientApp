using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPAClientApp
{
    public class GoogleDriveAPI
    {
        private const int ID_NUMBER = 31;

        private static string[] Scopes = {
            DriveService.Scope.Drive,
            DriveService.Scope.DriveAppdata,
            DriveService.Scope.DriveFile,
            DriveService.Scope.DriveMetadataReadonly,
            DriveService.Scope.DriveReadonly,
            DriveService.Scope.DriveScripts
        };

        private static string ApplicationName = "SAP Project";

        [Obsolete]
        private static async Task<DriveService> ConfigurarDriveAPI()
        {
            try
            {
                DriveService service;
                UserCredential Credential;
                string settings = Path.GetFullPath(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"), @"Resources\client_secret.json"));
                using (var stream = new FileStream(settings, FileMode.Open, FileAccess.Read))
                {
                    string credPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".credentials/drive-dotnet-quickstart.json");
                    Credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None, new FileDataStore(credPath, true));
                }

                service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = Credential,
                    ApplicationName = ApplicationName,
                });
                return service;
            }
            catch (Exception)
            {
                throw new Exception("Se detectó un problema con la autorización a la carpeta de Google Drive, favor de " +
                    "contactar a soporte técnico en caso de presentar el mismo inconveniente.");
            }
        }

        [Obsolete]
        public static async Task<string> CargarImagen(string path, string oldPath)
        {
            string returnValue = string.Empty;
            try
            {
                DriveService service = await ConfigurarDriveAPI();
                if (!string.IsNullOrEmpty(path))
                {
                    CheckSize(path);
                    if (!string.IsNullOrEmpty(oldPath))
                        await EliminarFoto(oldPath.Substring(ID_NUMBER), await ConfigurarDriveAPI());
                    var file = new Google.Apis.Drive.v3.Data.File
                    {
                        Parents = new string[] { "1AE2JMSauYqhETj7QDnmsSbSzbuMkFCcE" }
                    };
                    FilesResource.CreateMediaUpload request;
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        file.Name = stream.Name;
                        request = service.Files.Create(file, stream, file.MimeType);
                        request.Fields = "id";
                        await request.UploadAsync();
                    }
                    var response = request.ResponseBody;
                    returnValue = $"https://drive.google.com/uc?id={response.Id}";
                    service.Permissions.Create(new Permission() { Type = "anyone", Role = "writer" }, response.Id).Execute(); //Creating Permission after folder creation.
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Obsolete]
        public static async Task<bool> EliminarFoto(string id, DriveService service = null)
        {
            try
            {
                if(service == null)
                    service = await ConfigurarDriveAPI();
                if (!string.IsNullOrEmpty(id))
                {
                    FilesResource.DeleteRequest auxRequest;
                    auxRequest = service.Files.Delete(id);
                    await auxRequest.ExecuteAsync();
                }
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Lo sentimos, se presentó un problema intentando eliminar el archivo anteriormente cargado" +
                    " si el problema persiste, favor de contactar a soporte técnico");
            }
        }

        private static void CheckSize(string path)
        {
            var info = new FileInfo(path);
            if (info.Length > 10000000)
                throw new Exception("El tamaño del archivo supera los 10 Mb");
        } 
    }
}
