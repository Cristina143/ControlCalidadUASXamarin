using controlCalidad.Modelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Newtonsoft.Json; //paquete de json
using controlCalidad.Modelos; //importamos lo que este en la carpeta modelos para hacer el objeto
using System.Net.Http; //paquete para el HttpClient
using System.Net; //importacion para HttpStatusCode
using System.IO;
using Xamarin.Essentials;

namespace controlCalidad
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pagina_fichas : ContentPage
    {
        // Variable para almacenar la facultad seleccionada
        private int facultad_seleccionada;
        // Constructor de la página
        public pagina_fichas()
        {
            InitializeComponent();
        }

        // Método que se llama cuando la página está a punto de aparecer
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Obtener información almacenada en la aplicación
            int carrera = ((App)Application.Current).CarreraPersona;
            string token = ((App)Application.Current).tokenPersona;
            int facultad = ((App)Application.Current).FacultadPersona;
            int zona = ((App)Application.Current).ZonaPersona;

            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "zonaFicha.json");

            try
            {
                // Verificar si hay conexión a Internet
                if (internet.TieneConexionInternet()) //si tiene internet entonces
                {
                    // Realizar una solicitud HTTP para obtener información de la zona
                    var request = new HttpRequestMessage();
                    request.RequestUri = new Uri("https://adminuas-001-site3.gtempurl.com/api/Zona/Consultar_Zona");
                    request.Method = HttpMethod.Get;
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);

                    // Verificar si la respuesta es exitosa (código 200)
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // Leer el contenido de la respuesta
                        string content = await response.Content.ReadAsStringAsync();
                        // Deserializar el contenido JSON a una lista de objetos ClassZona
                        var resultado = JsonConvert.DeserializeObject<List<ClassZona>>(content);
                        // Establecer la fuente de datos para el control Select_zona
                        Select_zona.ItemsSource = resultado;
                        // Guardar datos en el archivo local
                        File.WriteAllText(rutaArchivo, content);
                    }
                }
                else //si no tiene internet
                {
                    // Leer datos desde el archivo local
                    string jsonGuardado = File.ReadAllText(rutaArchivo);
                    var resp = JsonConvert.DeserializeObject<List<ClassZona>>(jsonGuardado);
                    Select_zona.ItemsSource = resp;
                }          
            }
            catch (Exception ex) //si se encuentra un error o una excepción entonces notificar al usurio de este
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");

            }

        }

        private async void Select_zona_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado un elemento en el control Select_zona
            if (Select_zona.SelectedItem != null)
            {
                // Limpiar las fuentes de datos y restablecer la selección en otros controles
                Seject_carrera.ItemsSource = null;
                Seject_carrera.SelectedItem = null;
                Select_facultad.ItemsSource = null;
                // Obtener el objeto ClassZona seleccionado
                ClassZona zonaSeleccionada = (ClassZona)Select_zona.SelectedItem;
                int idZonaSeleccionada = zonaSeleccionada.id_zona;
                // Obtener la ruta del archivo en el sistema de archivos local
                string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "facultadFicha.json");
                try
                {
                    // Verificar si hay conexión a Internet
                    if (internet.TieneConexionInternet()) //si tiene internet entonces
                    {
                        // Realizar una solicitud HTTP para obtener información de las facultades
                        var request = new HttpRequestMessage();
                        request.RequestUri = new Uri("https://adminuas-001-site3.gtempurl.com/api/Facultades/Consultar_Facultad");
                        request.Method = HttpMethod.Get;

                        HttpClient client = new HttpClient();
                        HttpResponseMessage response = await client.SendAsync(request);

                        // Verificar si la respuesta es exitosa (código 200)
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Leer el contenido de la respuesta
                            string content = await response.Content.ReadAsStringAsync();
                            // Deserializar el contenido JSON a una lista de objetos ClassFacultad
                            var facultades = JsonConvert.DeserializeObject<List<ClassFacultad>>(content);
                            // Filtrar las facultades por la zona seleccionada
                            var facultadesFiltradas = facultades.Where(f => f.id_zona == idZonaSeleccionada).ToList();
                            // Establecer la fuente de datos para el control Select_facultad
                            Select_facultad.ItemsSource = facultadesFiltradas;
                            // Guardar datos en el archivo local
                            File.WriteAllText(rutaArchivo, content);
                        }
                    }
                    else //si no tiene internet entonces 
                    {
                        // Leer datos desde el archivo local
                        string jsonGuardado = File.ReadAllText(rutaArchivo);
                        var resp = JsonConvert.DeserializeObject<List<ClassFacultad>>(jsonGuardado);
                        // Filtrar las facultades por la zona seleccionada
                        var facultadesFiltradas = resp.Where(f => f.id_zona == idZonaSeleccionada).ToList();
                        // Establecer la fuente de datos para el control Select_facultad
                        Select_facultad.ItemsSource = facultadesFiltradas;
                    }     
                }
                catch (Exception ex) //si se encuentra un error o una excepción entonces notificar al usurio de este
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    await DisplayAlert("Error de conexión", ex.Message, "OK");
                }
            }
        }

        // Método que se ejecuta cuando se selecciona un elemento en el control Select_facultad
        private async void Select_facultad_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado un elemento en el control Select_facultad
            if (Select_facultad.SelectedItem != null)
            {
                // Obtener el objeto ClassFacultad seleccionado
                ClassFacultad facultadSeleccionada = (ClassFacultad)Select_facultad.SelectedItem;
                int idFacultadSeleccionada = facultadSeleccionada.id_facultad;
                // Almacenar el ID de la facultad seleccionada en la variable facultad_seleccionada
                facultad_seleccionada = idFacultadSeleccionada;
                // Obtener la ruta del archivo en el sistema de archivos local
                string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "carreraFicha.json");

                try
                {
                    // Verificar si hay conexión a Internet
                    if (internet.TieneConexionInternet()) //si tiene internet entonces
                    {
                        // Realizar una solicitud HTTP para obtener información de las carreras
                        var request = new HttpRequestMessage();
                        request.RequestUri = new Uri("https://adminuas-001-site3.gtempurl.com/api/Carreras/Consultar_Carrera");
                        request.Method = HttpMethod.Get;

                        HttpClient client = new HttpClient();
                        HttpResponseMessage response = await client.SendAsync(request);

                        // Verificar si la respuesta es exitosa (código 200)
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Leer el contenido de la respuesta
                            string content = await response.Content.ReadAsStringAsync();
                            // Deserializar el contenido JSON a una lista de objetos ClassCarrera
                            var carreras = JsonConvert.DeserializeObject<List<ClassCarrera>>(content);

                            // Filtrar las carreras por la facultad seleccionada
                            var carrerasFiltradas = carreras.Where(c => c.id_facultad == idFacultadSeleccionada).ToList();
                            // Establecer la fuente de datos para el control Seject_carrera
                            Seject_carrera.ItemsSource = carrerasFiltradas;
                            // Guardar datos en el archivo local
                            File.WriteAllText(rutaArchivo, content);
                        }
                    }
                    else //si no hay internet entonces
                    {
                        // Leer datos desde el archivo local
                        string jsonGuardado = File.ReadAllText(rutaArchivo);
                        var resp = JsonConvert.DeserializeObject<List<ClassCarrera>>(jsonGuardado);
                        // Filtrar las carreras por la facultad seleccionada
                        var carrerasFiltradas = resp.Where(c => c.id_facultad == idFacultadSeleccionada).ToList();
                        // Establecer la fuente de datos para el control Seject_carrera
                        Seject_carrera.ItemsSource = carrerasFiltradas;
                    }       
                }
                catch (Exception ex) //si se encuentra un error o una excepción entonces notificar al usurio de este
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    await DisplayAlert("Error de conexión", ex.Message, "OK");
                }
            }
        }

        // Método que se ejecuta cuando se selecciona un elemento en el control Seject_carrera
        private async void Seject_carrera_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtener la carrera seleccionada
            ClassCarrera carreraSeleccionada = (ClassCarrera)Seject_carrera.SelectedItem;
            int idCarreraSeleccionada = carreraSeleccionada.id_carrera;

            //informe 1

            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe1.json");

            try
            {
                // Verificar si hay conexión a Internet
                if (internet.TieneConexionInternet()) //si tiene internet entonces
                {
                    // Construir la URL para obtener el informe1 de la carrera seleccionada
                    string url1 = "https://adminuas-001-site3.gtempurl.com/api/Informe/Consultar_Informe1?id_carrera=" + idCarreraSeleccionada;

                    var request1 = new HttpRequestMessage();
                    request1.RequestUri = new Uri(url1);
                    request1.Method = HttpMethod.Get;

                    HttpClient client1 = new HttpClient();
                    HttpResponseMessage response1 = await client1.SendAsync(request1);

                    // Verificar si la respuesta es exitosa (código 200)
                    if (response1.StatusCode == HttpStatusCode.OK)
                    {
                        // Leer el contenido de la respuesta
                        string content1 = await response1.Content.ReadAsStringAsync();
                        // Deserializar el contenido JSON a una lista de objetos Classficha1
                        var informes1 = JsonConvert.DeserializeObject<List<Classficha1>>(content1);
                        // Establecer la fuente de datos para informe1ListView
                        informe1ListView.ItemsSource = informes1;
                        Guardar datos en el archivo local
                        File.WriteAllText(rutaArchivo1, content1);
                    }
                }
                else //si no tiene internet
                {
                    // Leer datos desde el archivo local
                    string jsonGuardado = File.ReadAllText(rutaArchivo1);
                    var informes1 = JsonConvert.DeserializeObject<List<Classficha1>>(jsonGuardado);
                    // Filtrar informe1 por la carrera seleccionada
                    List<Classficha1> infoInforme1 = new List<Classficha1>();
                    foreach (var info in informes1)
                    {
                        if (info.id_carrera == idCarreraSeleccionada)
                        {
                            infoInforme1.Add(info);
                        }
                    }
                    // Establecer la fuente de datos para informe1ListView
                    informe1ListView.ItemsSource = infoInforme1;
                }
            }
            catch (Exception ex) //si se encuentra un error o una excepción entonces notificar al usurio de este
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }

            //informe 2
            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe2.json");

            try
            {
                // Verificar si hay conexión a Internet
                if (internet.TieneConexionInternet())
                {
                    // Construir la URL para obtener el informe2
                    string url2 = "https://adminuas-001-site3.gtempurl.com/api/Informe2/Consultar_Informe2?id_informe=1";

                    var request2 = new HttpRequestMessage();
                    request2.RequestUri = new Uri(url2);
                    request2.Method = HttpMethod.Get;

                    HttpClient client2 = new HttpClient();
                    HttpResponseMessage response2 = await client2.SendAsync(request2);

                    // Verificar si la respuesta es exitosa (código 200)
                    if (response2.StatusCode == HttpStatusCode.OK)
                    {
                        // Leer el contenido de la respuesta
                        string content2 = await response2.Content.ReadAsStringAsync();
                        // Deserializar el contenido JSON a una lista de objetos Classficha2
                        var informes2 = JsonConvert.DeserializeObject<List<Classficha2>>(content2);
                        // Establecer la fuente de datos para informe2ListView
                        informe2ListView.ItemsSource = informes2;
                        // Guardar datos en el archivo local
                        File.WriteAllText(rutaArchivo2, content2);
                    }
                }
                else //si no tiene internet entonces
                {
                    // Leer datos desde el archivo local
                    string jsonGuardado = File.ReadAllText(rutaArchivo2);
                    var informes2 = JsonConvert.DeserializeObject<List<Classficha2>>(jsonGuardado);
                    // Establecer la fuente de datos para informe2ListView
                    informe2ListView.ItemsSource = informes2;
                }
            }
            catch (Exception ex) //si se encuentra un error o una excepción entonces notificar al usurio de este
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }

            //informe 3
            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo3 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe3.json");

            try
            {
                // Verificar si hay conexión a Internet
                if (internet.TieneConexionInternet()) //si tiene internet entonces
                {
                    // Construir la URL para obtener el informe3 de la facultad seleccionada
                    string url3 = "https://adminuas-001-site3.gtempurl.com/api/Informe3/Consultar_Informe3?id_facultad=" + facultad_seleccionada;

                    var request3 = new HttpRequestMessage();
                    request3.RequestUri = new Uri(url3);
                    request3.Method = HttpMethod.Get;

                    HttpClient client3 = new HttpClient();
                    HttpResponseMessage response3 = await client3.SendAsync(request3);

                    // Verificar si la respuesta es exitosa (código 200)
                    if (response3.StatusCode == HttpStatusCode.OK)
                    {
                        // Leer el contenido de la respuesta
                        string content3 = await response3.Content.ReadAsStringAsync();
                        // Deserializar el contenido JSON a una lista de objetos Classficha3
                        var informes3 = JsonConvert.DeserializeObject<List<Classficha3>>(content3);
                        // Establecer la fuente de datos para informe3ListView
                        informe3ListView.ItemsSource = informes3;
                        // Guardar datos en el archivo local    
                        File.WriteAllText(rutaArchivo3, content3);
                    }
                }
                else //si no tiene internet entonces
                {
                    // Leer datos desde el archivo local
                    string jsonGuardado = File.ReadAllText(rutaArchivo3);
                    var informes3 = JsonConvert.DeserializeObject<List<Classficha3>>(jsonGuardado);
                    // Filtrar informe3 por la facultad seleccionada
                    List<Classficha3> infoInforme3 = new List<Classficha3>();

                    foreach (var info in informes3)
                    {
                        if (info.id_facultad == facultad_seleccionada)
                        {
                            infoInforme3.Add(info);

                        }
                    }
                    // Establecer la fuente de datos para informe3ListView
                    informe3ListView.ItemsSource = infoInforme3;
                }
            }
            catch (Exception ex) //si se encuentra un error o una excepción entonces notificar al usurio de este
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }

            //informe 4
            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo4 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe4.json");

            try
            {
                // Verificar si hay conexión a Internet
                if (internet.TieneConexionInternet()) //si tiene internet
                {
                    // Construir la URL para obtener el informe4 de la carrera seleccionada
                    string url4 = "https://adminuas-001-site3.gtempurl.com/api/Informe4/Consultar_Informe4?id_carrera=" + idCarreraSeleccionada;

                    var request4 = new HttpRequestMessage();
                    request4.RequestUri = new Uri(url4);
                    request4.Method = HttpMethod.Get;

                    HttpClient client4 = new HttpClient();
                    HttpResponseMessage response4 = await client4.SendAsync(request4);

                    // Verificar si la respuesta es exitosa (código 200)
                    if (response4.StatusCode == HttpStatusCode.OK)
                    {
                        // Leer el contenido de la respuesta
                        string content4 = await response4.Content.ReadAsStringAsync();
                        // Deserializar el contenido JSON a una lista de objetos Classficha4
                        var informes4 = JsonConvert.DeserializeObject<List<Classficha4>>(content4);
                        // Establecer la fuente de datos para informe4ListView
                        informe4ListView.ItemsSource = informes4;
                        // Guardar datos en el archivo local
                        File.WriteAllText(rutaArchivo4, content4);
                    }
                }
                else //si no hay internet
                {
                    // Leer datos desde el archivo local
                    string jsonGuardado = File.ReadAllText(rutaArchivo4);
                    var informes4 = JsonConvert.DeserializeObject<List<Classficha4>>(jsonGuardado);
                    // Filtrar informe4 por la carrera seleccionada
                    List<Classficha4> infoInforme4 = new List<Classficha4>();

                    foreach (var info in informes4)
                    {
                        if (info.id_carrera == idCarreraSeleccionada)
                        {
                            infoInforme4.Add(info);

                        }
                    }
                    // Establecer la fuente de datos para informe4ListView
                    informe4ListView.ItemsSource = infoInforme4;
                }
            }
            catch (Exception ex) //si se encuentra un error o una excepción entonces notificar al usurio de este
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }

            //informe 5
            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo5 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe5.json");

            try
            {
                // Verificar si hay conexión a Internet
                if (internet.TieneConexionInternet()) //si tiene internet
                {
                    // Construir la URL para obtener el informe5 de la carrera seleccionada
                    string url5 = "https://adminuas-001-site3.gtempurl.com/api/Informe5/Consultar_Informe5?id_carrera=" + idCarreraSeleccionada;

                    var request5 = new HttpRequestMessage();
                    request5.RequestUri = new Uri(url5);
                    request5.Method = HttpMethod.Get;

                    HttpClient client5 = new HttpClient();
                    HttpResponseMessage response5 = await client5.SendAsync(request5);

                    // Verificar si la respuesta es exitosa (código 200)
                    if (response5.StatusCode == HttpStatusCode.OK)
                    {
                        // Leer el contenido de la respuesta
                        string content5 = await response5.Content.ReadAsStringAsync();
                        // Deserializar el contenido JSON a una lista de objetos Classficha5
                        var informes5 = JsonConvert.DeserializeObject<List<Classficha5>>(content5);
                        // Establecer la fuente de datos para informe5ListView
                        informe5ListView.ItemsSource = informes5;
                        // Guardar datos en el archivo local
                        File.WriteAllText(rutaArchivo5, content5);
                    }

                }
                else //si no hay internet
                {
                    //Leer datos desde el archivo local
                    string jsonGuardado = File.ReadAllText(rutaArchivo5);
                    var informes5 = JsonConvert.DeserializeObject<List<Classficha5>>(jsonGuardado);
                    // Filtrar informe5 por la carrera seleccionada
                    List<Classficha5> infoInforme5 = new List<Classficha5>();

                    foreach (var info in informes5)
                    {
                        if (info.id_carrera == idCarreraSeleccionada)
                        {
                            infoInforme5.Add(info);
                        }
                    }
                    // Establecer la fuente de datos para informe5ListView
                    informe5ListView.ItemsSource = infoInforme5;
                }
            }
            catch (Exception ex) //si se encuentra un error o una excepción entonces notificar al usurio de este
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }
            
            //informe 6
            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo6 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe6.json");

            try
            {
                if (internet.TieneConexionInternet()) //si tiene initernet
                {
                    // Construir la URL para obtener el informe6 de la carrera seleccionada
                    string url6 = "https://adminuas-001-site3.gtempurl.com/api/Informe6/Consultar_Informe6?id_carrera=" + idCarreraSeleccionada;

                    var request6 = new HttpRequestMessage();
                    request6.RequestUri = new Uri(url6);
                    request6.Method = HttpMethod.Get;

                    HttpClient client6 = new HttpClient();
                    HttpResponseMessage response6 = await client6.SendAsync(request6);

                    // Verificar si la respuesta es exitosa (código 200)
                    if (response6.StatusCode == HttpStatusCode.OK)
                    {
                        // Leer el contenido de la respuesta
                        string content6 = await response6.Content.ReadAsStringAsync();
                        // Deserializar el contenido JSON a una lista de objetos Classficha6
                        var informes6 = JsonConvert.DeserializeObject<List<Classficha6>>(content6);
                        // Establecer la fuente de datos para informe6ListView
                        informe6ListView.ItemsSource = informes6;
                        // Guardar datos en el archivo local
                        File.WriteAllText(rutaArchivo6, content6);
                    }
                }
                else
                {
                    // Si no hay conexión a Internet, leer datos desde el archivo local
                    string jsonGuardado = File.ReadAllText(rutaArchivo6);
                    var informes6 = JsonConvert.DeserializeObject<List<Classficha6>>(jsonGuardado);
                    // Filtrar informe6 por la carrera seleccionada
                    List<Classficha6> infoInforme6 = new List<Classficha6>();

                    foreach (var info in informes6)
                    {
                        if (info.id_carrera == idCarreraSeleccionada)
                        {
                            infoInforme6.Add(info);
                        }
                    }
                    // Establecer la fuente de datos para informe6ListView
                    informe6ListView.ItemsSource = infoInforme6;
                }
            }
            catch (Exception ex) //si se encuentra un error o una excepción entonces notificar al usurio de este
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }


            //informe 7
            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo7 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe7.json");

            try
            {
                // Verificar si hay conexión a Internet
                if (internet.TieneConexionInternet())
                {
                    // Construir la URL para obtener el informe7 de la carrera seleccionada
                    string url7 = "https://adminuas-001-site3.gtempurl.com/api/Informe7/Consultar_Informe7?id_carrera=" + idCarreraSeleccionada;

                    var request7 = new HttpRequestMessage();
                    request7.RequestUri = new Uri(url7);
                    request7.Method = HttpMethod.Get;

                    HttpClient client7 = new HttpClient();
                    HttpResponseMessage response7 = await client7.SendAsync(request7);

                    // Verificar si la respuesta es exitosa (código 200)
                    if (response7.StatusCode == HttpStatusCode.OK)
                    {
                        // Leer el contenido de la respuesta
                        string content7 = await response7.Content.ReadAsStringAsync();
                        // Deserializar el contenido JSON a una lista de objetos Classficha7
                        var informes7 = JsonConvert.DeserializeObject<List<Classficha7>>(content7);
                        // Establecer la fuente de datos para informe7ListView
                        informe7ListView.ItemsSource = informes7;
                        // Guardar datos en el archivo local
                        File.WriteAllText(rutaArchivo7, content7);
                    }
                }
                else
                {
                    // Si no hay conexión a Internet, leer datos desde el archivo local
                    string jsonGuardado = File.ReadAllText(rutaArchivo7);
                    var informes7 = JsonConvert.DeserializeObject<List<Classficha7>>(jsonGuardado);
                    // Filtrar informe7 por la carrera seleccionada
                    List<Classficha7> infoInforme7 = new List<Classficha7>();

                    foreach (var info in informes7)
                    {
                        if (info.id_carrera == idCarreraSeleccionada)
                        {
                            infoInforme7.Add(info);
                        }
                    }
                    // Establecer la fuente de datos para informe7ListView
                    informe7ListView.ItemsSource = infoInforme7;
                }
            }
            catch (Exception ex) //si se encuentra un error o una excepción entonces notificar al usurio de este
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }
            
            
            //informe 8
            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo8 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe8.json");

            try
            {
                // Verificar si hay conexión a Internet
                if (internet.TieneConexionInternet())
                {
                    // Construir la URL para obtener el informe8 de la carrera seleccionada
                    string url8 = "https://adminuas-001-site3.gtempurl.com/api/Informe8/Consultar_Informe8?id_carrera=" + idCarreraSeleccionada;

                    var request8 = new HttpRequestMessage();
                    request8.RequestUri = new Uri(url8);
                    request8.Method = HttpMethod.Get;

                    HttpClient client8 = new HttpClient();
                    HttpResponseMessage response8 = await client8.SendAsync(request8);

                    // Verificar si la respuesta es exitosa (código 200)
                    if (response8.StatusCode == HttpStatusCode.OK)
                    {
                        // Leer el contenido de la respuesta
                        string content8 = await response8.Content.ReadAsStringAsync();
                        // Deserializar el contenido JSON a una lista de objetos Classficha8
                        var informes8 = JsonConvert.DeserializeObject<List<Classficha8>>(content8);
                        // Establecer la fuente de datos para informe8ListView
                        informe8ListView.ItemsSource = informes8;
                        // Guardar datos en el archivo local
                        File.WriteAllText(rutaArchivo8, content8);

                    }
                }
                else
                {
                    // Si no hay conexión a Internet, leer datos desde el archivo local
                    string jsonGuardado = File.ReadAllText(rutaArchivo8);
                    var informes8 = JsonConvert.DeserializeObject<List<Classficha8>>(jsonGuardado);
                    // Filtrar informe8 por la carrera seleccionada
                    List<Classficha8> infoInforme8 = new List<Classficha8>();

                    foreach (var info in informes8)
                    {
                        if (info.id_carrera == idCarreraSeleccionada)
                        {
                            infoInforme8.Add(info);
                        }
                    }
                    // Establecer la fuente de datos para informe8ListView
                    informe8ListView.ItemsSource = infoInforme8;
                }
            }
            catch (Exception ex) //si se encuentra un error o una excepción entonces notificar al usurio de este
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }
            

            //informe 9

            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo9 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe9.json");

            try
            {
                // Verificar si hay conexión a Internet
                if (internet.TieneConexionInternet())
                {
                    // Construir la URL para obtener el informe9 de la carrera seleccionada
                    string url9 = "https://adminuas-001-site3.gtempurl.com/api/Informe9/Consultar_Informe9?id_carrera=" + idCarreraSeleccionada;

                    var request9 = new HttpRequestMessage();
                    request9.RequestUri = new Uri(url9);
                    request9.Method = HttpMethod.Get;

                    HttpClient client9 = new HttpClient();
                    HttpResponseMessage response9 = await client9.SendAsync(request9);

                    // Verificar si la respuesta es exitosa (código 200)
                    if (response9.StatusCode == HttpStatusCode.OK)
                    {
                        // Leer el contenido de la respuesta
                        string content9 = await response9.Content.ReadAsStringAsync();
                        // Deserializar el contenido JSON a una lista de objetos Classficha9
                        var informes9 = JsonConvert.DeserializeObject<List<Classficha9>>(content9);
                        // Establecer la fuente de datos para informe9ListView
                        informe9ListView.ItemsSource = informes9;
                        // Guardar datos en el archivo local
                        File.WriteAllText(rutaArchivo9, content9);

                    }
                }
                else
                {
                    // Si no hay conexión a Internet, leer datos desde el archivo local
                    string jsonGuardado = File.ReadAllText(rutaArchivo9);
                    var informes9 = JsonConvert.DeserializeObject<List<Classficha9>>(jsonGuardado);
                    // Filtrar informe9 por la carrera seleccionada
                    List<Classficha9> infoInforme9 = new List<Classficha9>();

                    foreach (var info in informes9)
                    {
                        if (info.id_carrera == idCarreraSeleccionada)
                        {
                            infoInforme9.Add(info);
                        }
                    }
                    // Establecer la fuente de datos para informe9ListView
                    informe9ListView.ItemsSource = infoInforme9;
                }
            }
            catch (Exception ex) //si se encuentra un error o una excepción entonces notificar al usurio de este
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }
            

            //informe 10

            // Obtener la ruta del archivo en el sistema de archivos local
            /*string rutaArchivo10 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe10.json");

            try
            {
                if (internet.TieneConexionInternet())
                {
                    string url10 = "https://adminuas-001-site3.gtempurl.com/api/Informe10/Consultar_Informe10?id_carrera=" + idCarreraSeleccionada;

                    var request10 = new HttpRequestMessage();
                    request10.RequestUri = new Uri(url10);
                    request10.Method = HttpMethod.Get;

                    HttpClient client10 = new HttpClient();
                    HttpResponseMessage response10 = await client10.SendAsync(request10);

                    if (response10.StatusCode == HttpStatusCode.OK)
                    {
                        string content10 = await response10.Content.ReadAsStringAsync();
                        var informes10 = JsonConvert.DeserializeObject<List<Classficha10>>(content10);

                        informe10ListView.ItemsSource = informes10;
                        File.WriteAllText(rutaArchivo10, content10);

                    }
                }
                else
                {
                    string jsonGuardado = File.ReadAllText(rutaArchivo10);
                    var informes10 = JsonConvert.DeserializeObject<List<Classficha10>>(jsonGuardado);
                    List<Classficha10> infoInforme10 = new List<Classficha10>();

                    foreach (var info in informes10)
                    {
                        if (info.id_carrera == idCarreraSeleccionada)
                        {
                            infoInforme10.Add(info);
                        }
                    }
                    informe10ListView.ItemsSource = infoInforme10;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }*/
            
        }
    }
}