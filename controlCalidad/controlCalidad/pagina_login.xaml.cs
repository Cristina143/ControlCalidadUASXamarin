﻿using System;
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

        int carreraElegida = ((App)Application.Current).CarreraElegida;
        int facultadElegida = ((App)Application.Current).FacultadElegida;
        int zonaElegida = ((App)Application.Current).ZonaElegida;

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
                    //await DisplayAlert("Mensaje", "Tiene internet", "OK");

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

                    //string RequestApi = "http://10.1.1.140:45455/api/Usuarios/autenticacion";
                    //string RequestApi = "https://marianitaaa123-001-site1.etempurl.com/api/Usuarios/autenticacion";
                    string RequestApi = "https://adminuas-001-site3.gtempurl.com/api/Usuarios/autenticacion";
                    //string RequestApi = "https://adminuas1-001-site1.itempurl.com/api/Usuarios/autenticacion";

                    // Crear HttpClient y enviar la solicitud POST
                    HttpClient client = new HttpClient();
                    //var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(log);
                    var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(RequestApi, contentJson);
                    //var response = await client.PostAsync(RequestApi, contentJson);

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

                            carreraElegida = 0;
                            facultadElegida = 0;
                            zonaElegida = 0;

                            // Redirigir según el rol del usuario
                            if (rol == "administrador" || rol == "general") //superAdmin
                            {
                                await Navigation.PushAsync(new Tabbed_menu());
                            }
                            /*else if (rol == "facultad") //facultad
                            {
                                //await Navigation.PushAsync(new Tabbed_menu2());
                                await DisplayAlert("Mensaje", "Admin Facultad", "OK");
                            }
                            else if (rol == "carrera") //carrera
                            {
                                await DisplayAlert("Mensaje", "Admin Carrera", "OK");

                            }
                            else if (rol == "zona") //zona
                            {
                                await DisplayAlert("Mensaje", "Admin zona", "OK");
                            }*/
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
                    //await DisplayAlert("Mensaje", "No tiene internet", "OK");
                    string jsonGuardado = File.ReadAllText(rutaArchivo);
                    var resp = JsonConvert.DeserializeObject<loginResponse>(jsonGuardado);
                    //await DisplayAlert("Mensaje", resp.Usuario[0].id_usuario.ToString(), "OK");
                    if(resp.Token.ToString() != "")
                    {
                        // Verificar credenciales almacenadas localmente
                        if (resp.Usuario[0].correo.ToString() == txtcorreo.Text && resp.Usuario[0].clave.ToString() == contraseña_usuario.Text)
                        {
                            // Redirigir según el rol del usuario
                            if (resp.Usuario[0].puesto.ToString() == "general" || resp.Usuario[0].puesto.ToString() == "administrador") //superAdmin
                            {
                                await Navigation.PushAsync(new Tabbed_menu());
                            }
                            /*else if (resp.Usuario[0].puesto.ToString() == "facultad") //facultad
                            {
                                //await Navigation.PushAsync(new Tabbed_menu2());
                                await DisplayAlert("Mensaje", "Admin Facultad", "OK");
                            }
                            else if (resp.Usuario[0].puesto.ToString() == "carrera") //carrera
                            {
                                await DisplayAlert("Mensaje", "Admin Carrera", "OK");

                            }
                            else if (resp.Usuario[0].puesto.ToString() == "zona") //zona
                            {
                                await DisplayAlert("Mensaje", "Admin zona", "OK");
                            }*/
                            //await DisplayAlert("Mensaje","si valida", "OK");
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