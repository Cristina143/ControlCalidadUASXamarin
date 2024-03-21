using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json; //paquete de json
using controlCalidad.Modelos; //importamos lo que este en la carpeta modelos para hacer el objeto
using System.Net.Http; //paquete para el HttpClient
using System.Net; //importacion para HttpStatusCode
using Xamarin.Essentials;
using System.IO;

namespace controlCalidad
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pagina_login : ContentPage
    {
        public pagina_login()
        {
            InitializeComponent();
        }

        private async void butt_login_Clicked(object sender, EventArgs e)
        {
            // Verificar que los campos no estén vacíos
            if (txtcorreo.Text != null || contraseña_usuario.Text != null)
            {
                // Obtener la ruta del archivo
                string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "usuario.json");

                if (internet.TieneConexionInternet())
                {
                    // El dispositivo tiene acceso a Internet
               
                    Class_login log = new Class_login
                    {
                        // Crear objeto para la autenticación
                        id_usuario = 0,
                        id_carrera = 0,
                        id_facultad = 0,
                        id_zona = 0,
                        correo = txtcorreo.Text,
                        clave = contraseña_usuario.Text,
                        nombre = "string",
                        puesto = "string",
                        fecha_alta = "2024-01-25T22:18:05.211Z"
                    };

                    // URL de la API de autenticación
                    string RequestApi = "https://adminuas-001-site3.gtempurl.com/api/Usuarios/autenticacion";

                    // Crear HttpClient y enviar la solicitud POST
                    HttpClient client = new HttpClient();
                    //var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(log);
                    var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(RequestApi, contentJson);

                    // Verificar la respuesta del servidor
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // Leer el contenido de la respuesta
                        string content = await response.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<loginResponse>(content);

                        // Guardar datos en el archivo local
                        File.WriteAllText(rutaArchivo, content);

                        // Verificar si se obtuvo un token 
                        if (resultado.Token != "")
                        {
                            // Extraer datos del usuario
                            int idCarrera = resultado.Usuario[0].id_carrera;
                            int idFacultad = resultado.Usuario[0].id_facultad;
                            int idZona = resultado.Usuario[0].id_zona;
                            int idUsuario = resultado.Usuario[0].id_usuario;
                            string rol = resultado.Usuario[0].puesto;
                            string token = resultado.Token;

                            // Asignar datos al objeto de aplicación
                            ((App)Application.Current).CarreraPersona = idCarrera;
                            ((App)Application.Current).FacultadPersona = idFacultad;
                            ((App)Application.Current).ZonaPersona = idZona;
                            ((App)Application.Current).tokenPersona = token;
                            ((App)Application.Current).NombrePersona = resultado.Usuario[0].nombre;

                            // Redirigir según el rol del usuario
                            if (rol == "administrador" || rol == "general") //superAdmin
                            {
                                await Navigation.PushAsync(new MainPage());
                            }
                        }
                        else
                        {
                            await DisplayAlert("Mensaje", "Datos invalidos", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Mensaje", "Datos invalidos", "OK");
                    }
                }
                else
                {
                    // El dispositivo no tiene acceso a Internet
                    string jsonGuardado = File.ReadAllText(rutaArchivo);
                    var resp = JsonConvert.DeserializeObject<loginResponse>(jsonGuardado);
                    if(resp.Token.ToString() != "")
                    {
                        // Verificar credenciales almacenadas localmente
                        if (resp.Usuario[0].correo.ToString() == txtcorreo.Text && resp.Usuario[0].clave.ToString() == contraseña_usuario.Text)
                        {
                            // Redirigir según el rol del usuario
                            if (resp.Usuario[0].puesto.ToString() == "general" || resp.Usuario[0].puesto.ToString() == "administrador") //superAdmin
                            {
                                int idCarrera = resp.Usuario[0].id_carrera;
                                int idFacultad = resp.Usuario[0].id_facultad;
                                int idZona = resp.Usuario[0].id_zona;
                                int idUsuario = resp.Usuario[0].id_usuario;
                                string rol = resp.Usuario[0].puesto;
                                string token = resp.Token;

                                // Asignar datos al objeto de aplicación
                                ((App)Application.Current).CarreraPersona = idCarrera;
                                ((App)Application.Current).FacultadPersona = idFacultad;
                                ((App)Application.Current).ZonaPersona = idZona;
                                ((App)Application.Current).tokenPersona = token;
                                ((App)Application.Current).NombrePersona = resp.Usuario[0].nombre;

                                await Navigation.PushAsync(new MainPage());
                            }
                        }
                        else
                        {
                            await DisplayAlert("Mensaje", "Datos invalidos", "OK");
                        }
                    }
                }
            }
            else
            {
                await DisplayAlert("Mensaje", "Campos vacios", "OK");
            }
        }
    }
}