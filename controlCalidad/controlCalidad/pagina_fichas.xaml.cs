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
        private int facultad_seleccionada;
        public pagina_fichas()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            int carrera = ((App)Application.Current).CarreraPersona;
            string token = ((App)Application.Current).tokenPersona;
            int facultad = ((App)Application.Current).FacultadPersona;
            int zona = ((App)Application.Current).ZonaPersona;

            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "zonaFicha.json");

            try
            {

                if (internet.TieneConexionInternet())
                {
                    var request = new HttpRequestMessage();
                    request.RequestUri = new Uri("https://adminuas-001-site3.gtempurl.com/api/Zona/Consultar_Zona");
                    request.Method = HttpMethod.Get;
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<List<ClassZona>>(content);
                        Select_zona.ItemsSource = resultado;
                        // Guardar datos
                        File.WriteAllText(rutaArchivo, content);
                    }
                }
                else
                {
                    string jsonGuardado = File.ReadAllText(rutaArchivo);
                    var resp = JsonConvert.DeserializeObject<List<ClassZona>>(jsonGuardado);
                    Select_zona.ItemsSource = resp;
                }
                //if (carrera == 0)
                //{
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");

            }

        }

        private async void Select_zona_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Select_zona.SelectedItem != null)
            {
                Seject_carrera.ItemsSource = null;
                Seject_carrera.SelectedItem = null;
                Select_facultad.ItemsSource = null;
                ClassZona zonaSeleccionada = (ClassZona)Select_zona.SelectedItem;
                int idZonaSeleccionada = zonaSeleccionada.id_zona;
                // Obtener la ruta del archivo en el sistema de archivos local
                string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "facultadFicha.json");
                try
                {
                    if (internet.TieneConexionInternet())
                    {
                        var request = new HttpRequestMessage();
                        request.RequestUri = new Uri("https://adminuas-001-site3.gtempurl.com/api/Facultades/Consultar_Facultad");
                        request.Method = HttpMethod.Get;

                        HttpClient client = new HttpClient();
                        HttpResponseMessage response = await client.SendAsync(request);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            var facultades = JsonConvert.DeserializeObject<List<ClassFacultad>>(content);
                            // Filtrar las facultades por la zona seleccionada
                            var facultadesFiltradas = facultades.Where(f => f.id_zona == idZonaSeleccionada).ToList();

                            Select_facultad.ItemsSource = facultadesFiltradas;
                            // Guardar datos
                            File.WriteAllText(rutaArchivo, content);
                        }
                    }
                    else
                    {
                        string jsonGuardado = File.ReadAllText(rutaArchivo);
                        var resp = JsonConvert.DeserializeObject<List<ClassFacultad>>(jsonGuardado);
                        var facultadesFiltradas = resp.Where(f => f.id_zona == idZonaSeleccionada).ToList();
                        Select_facultad.ItemsSource = facultadesFiltradas;
                    }     
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    await DisplayAlert("Error de conexión", ex.Message, "OK");
                }
            }
        }

        private async void Select_facultad_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Select_facultad.SelectedItem != null)
            {
                ClassFacultad facultadSeleccionada = (ClassFacultad)Select_facultad.SelectedItem;
                int idFacultadSeleccionada = facultadSeleccionada.id_facultad;
                facultad_seleccionada = idFacultadSeleccionada;
                // Obtener la ruta del archivo en el sistema de archivos local
                string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "carreraFicha.json");

                try
                {
                    if (internet.TieneConexionInternet())
                    {
                        var request = new HttpRequestMessage();
                        request.RequestUri = new Uri("https://adminuas-001-site3.gtempurl.com/api/Carreras/Consultar_Carrera");
                        request.Method = HttpMethod.Get;

                        HttpClient client = new HttpClient();
                        HttpResponseMessage response = await client.SendAsync(request);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            var carreras = JsonConvert.DeserializeObject<List<ClassCarrera>>(content);

                            // Filtrar las carreras por la facultad seleccionada
                            var carrerasFiltradas = carreras.Where(c => c.id_facultad == idFacultadSeleccionada).ToList();

                            Seject_carrera.ItemsSource = carrerasFiltradas;
                            // Guardar datos
                            File.WriteAllText(rutaArchivo, content);
                        }
                    }
                    else
                    {
                        string jsonGuardado = File.ReadAllText(rutaArchivo);
                        var resp = JsonConvert.DeserializeObject<List<ClassCarrera>>(jsonGuardado);
                        var carrerasFiltradas = resp.Where(c => c.id_facultad == idFacultadSeleccionada).ToList();
                        Seject_carrera.ItemsSource = carrerasFiltradas;
                    }       
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    await DisplayAlert("Error de conexión", ex.Message, "OK");
                }
            }
        }

        private async void Seject_carrera_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClassCarrera carreraSeleccionada = (ClassCarrera)Seject_carrera.SelectedItem;
            int idCarreraSeleccionada = carreraSeleccionada.id_carrera;

            //informe 1

            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe1.json");

            try
            {
                if (internet.TieneConexionInternet())
                {
                    string url1 = "https://adminuas-001-site3.gtempurl.com/api/Informe/Consultar_Informe1?id_carrera=" + idCarreraSeleccionada;

                    var request1 = new HttpRequestMessage();
                    request1.RequestUri = new Uri(url1);
                    request1.Method = HttpMethod.Get;

                    HttpClient client1 = new HttpClient();
                    HttpResponseMessage response1 = await client1.SendAsync(request1);

                    if (response1.StatusCode == HttpStatusCode.OK)
                    {
                        string content1 = await response1.Content.ReadAsStringAsync();
                        var informes1 = JsonConvert.DeserializeObject<List<Classficha1>>(content1);

                        informe1ListView.ItemsSource = informes1;
                        File.WriteAllText(rutaArchivo1, content1);
                    }
                }
                else
                {
                    string jsonGuardado = File.ReadAllText(rutaArchivo1);
                    var informes1 = JsonConvert.DeserializeObject<List<Classficha1>>(jsonGuardado);
                    List<Classficha1> infoInforme1 = new List<Classficha1>();
                    foreach (var info in informes1)
                    {
                        if (info.id_carrera == idCarreraSeleccionada)
                        {
                            infoInforme1.Add(info);
                        }
                    }
                    informe1ListView.ItemsSource = infoInforme1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }

            //informe 2
            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe2.json");

            try
            {
                if (internet.TieneConexionInternet())
                {
                    string url2 = "https://adminuas-001-site3.gtempurl.com/api/Informe2/Consultar_Informe2?id_informe=1";

                    var request2 = new HttpRequestMessage();
                    request2.RequestUri = new Uri(url2);
                    request2.Method = HttpMethod.Get;

                    HttpClient client2 = new HttpClient();
                    HttpResponseMessage response2 = await client2.SendAsync(request2);

                    if (response2.StatusCode == HttpStatusCode.OK)
                    {
                        string content2 = await response2.Content.ReadAsStringAsync();
                        var informes2 = JsonConvert.DeserializeObject<List<Classficha2>>(content2);

                        informe2ListView.ItemsSource = informes2;
                        File.WriteAllText(rutaArchivo2, content2);
                    }
                }
                else
                {
                    string jsonGuardado = File.ReadAllText(rutaArchivo2);
                    var informes2 = JsonConvert.DeserializeObject<List<Classficha2>>(jsonGuardado);
                    informe2ListView.ItemsSource = informes2;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }

            //informe 3
            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo3 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe3.json");

            try
            {
                if (internet.TieneConexionInternet())
                {
                    string url3 = "https://adminuas-001-site3.gtempurl.com/api/Informe3/Consultar_Informe3?id_facultad=" + facultad_seleccionada;

                    var request3 = new HttpRequestMessage();
                    request3.RequestUri = new Uri(url3);
                    request3.Method = HttpMethod.Get;

                    HttpClient client3 = new HttpClient();
                    HttpResponseMessage response3 = await client3.SendAsync(request3);

                    if (response3.StatusCode == HttpStatusCode.OK)
                    {
                        string content3 = await response3.Content.ReadAsStringAsync();
                        var informes3 = JsonConvert.DeserializeObject<List<Classficha3>>(content3);

                        informe3ListView.ItemsSource = informes3;
                        File.WriteAllText(rutaArchivo3, content3);
                    }
                }
                else
                {
                    string jsonGuardado = File.ReadAllText(rutaArchivo3);
                    var informes3 = JsonConvert.DeserializeObject<List<Classficha3>>(jsonGuardado);
                    List<Classficha3> infoInforme3 = new List<Classficha3>();

                    foreach (var info in informes3)
                    {
                        if (info.id_facultad == facultad_seleccionada)
                        {
                            infoInforme3.Add(info);

                        }
                    }
                    informe3ListView.ItemsSource = infoInforme3;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }

            //informe 4
            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo4 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe4.json");

            try
            {
                if (internet.TieneConexionInternet())
                {
                    string url4 = "https://adminuas-001-site3.gtempurl.com/api/Informe4/Consultar_Informe4?id_carrera=" + idCarreraSeleccionada;

                    var request4 = new HttpRequestMessage();
                    request4.RequestUri = new Uri(url4);
                    request4.Method = HttpMethod.Get;

                    HttpClient client4 = new HttpClient();
                    HttpResponseMessage response4 = await client4.SendAsync(request4);

                    if (response4.StatusCode == HttpStatusCode.OK)
                    {
                        string content4 = await response4.Content.ReadAsStringAsync();
                        var informes4 = JsonConvert.DeserializeObject<List<Classficha4>>(content4);

                        informe4ListView.ItemsSource = informes4;
                        File.WriteAllText(rutaArchivo4, content4);
                    }
                }
                else
                {
                    string jsonGuardado = File.ReadAllText(rutaArchivo4);
                    var informes4 = JsonConvert.DeserializeObject<List<Classficha4>>(jsonGuardado);
                    List<Classficha4> infoInforme4 = new List<Classficha4>();

                    foreach (var info in informes4)
                    {
                        if (info.id_carrera == idCarreraSeleccionada)
                        {
                            infoInforme4.Add(info);

                        }
                    }
                    informe4ListView.ItemsSource = infoInforme4;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }

            //informe 5
            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo5 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe5.json");

            try
            {
                if (internet.TieneConexionInternet())
                {
                    string url5 = "https://adminuas-001-site3.gtempurl.com/api/Informe5/Consultar_Informe5?id_carrera=" + idCarreraSeleccionada;

                    var request5 = new HttpRequestMessage();
                    request5.RequestUri = new Uri(url5);
                    request5.Method = HttpMethod.Get;

                    HttpClient client5 = new HttpClient();
                    HttpResponseMessage response5 = await client5.SendAsync(request5);

                    if (response5.StatusCode == HttpStatusCode.OK)
                    {
                        string content5 = await response5.Content.ReadAsStringAsync();
                        var informes5 = JsonConvert.DeserializeObject<List<Classficha5>>(content5);

                        informe5ListView.ItemsSource = informes5;
                        File.WriteAllText(rutaArchivo5, content5);
                    }

                }
                else
                {
                    string jsonGuardado = File.ReadAllText(rutaArchivo5);
                    var informes5 = JsonConvert.DeserializeObject<List<Classficha5>>(jsonGuardado);
                    List<Classficha5> infoInforme5 = new List<Classficha5>();

                    foreach (var info in informes5)
                    {
                        if (info.id_carrera == idCarreraSeleccionada)
                        {
                            infoInforme5.Add(info);
                        }
                    }
                    informe5ListView.ItemsSource = infoInforme5;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }
            
            //informe 6
            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo6 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe6.json");

            try
            {
                if (internet.TieneConexionInternet())
                {
                    string url6 = "https://adminuas-001-site3.gtempurl.com/api/Informe6/Consultar_Informe6?id_carrera=" + idCarreraSeleccionada;

                    var request6 = new HttpRequestMessage();
                    request6.RequestUri = new Uri(url6);
                    request6.Method = HttpMethod.Get;

                    HttpClient client6 = new HttpClient();
                    HttpResponseMessage response6 = await client6.SendAsync(request6);

                    if (response6.StatusCode == HttpStatusCode.OK)
                    {
                        string content6 = await response6.Content.ReadAsStringAsync();
                        var informes6 = JsonConvert.DeserializeObject<List<Classficha6>>(content6);

                        informe6ListView.ItemsSource = informes6;
                        File.WriteAllText(rutaArchivo6, content6);
                    }
                }
                else
                {
                    string jsonGuardado = File.ReadAllText(rutaArchivo6);
                    var informes6 = JsonConvert.DeserializeObject<List<Classficha6>>(jsonGuardado);
                    List<Classficha6> infoInforme6 = new List<Classficha6>();

                    foreach (var info in informes6)
                    {
                        if (info.id_carrera == idCarreraSeleccionada)
                        {
                            infoInforme6.Add(info);
                        }
                    }
                    informe6ListView.ItemsSource = infoInforme6;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }


            //informe 7
            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo7 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe7.json");

            try
            {
                if (internet.TieneConexionInternet())
                {
                    string url7 = "https://adminuas-001-site3.gtempurl.com/api/Informe7/Consultar_Informe7?id_carrera=" + idCarreraSeleccionada;

                    var request7 = new HttpRequestMessage();
                    request7.RequestUri = new Uri(url7);
                    request7.Method = HttpMethod.Get;

                    HttpClient client7 = new HttpClient();
                    HttpResponseMessage response7 = await client7.SendAsync(request7);

                    if (response7.StatusCode == HttpStatusCode.OK)
                    {
                        string content7 = await response7.Content.ReadAsStringAsync();
                        var informes7 = JsonConvert.DeserializeObject<List<Classficha7>>(content7);

                        informe7ListView.ItemsSource = informes7;
                        File.WriteAllText(rutaArchivo7, content7);
                    }
                }
                else
                {
                    string jsonGuardado = File.ReadAllText(rutaArchivo7);
                    var informes7 = JsonConvert.DeserializeObject<List<Classficha7>>(jsonGuardado);
                    List<Classficha7> infoInforme7 = new List<Classficha7>();

                    foreach (var info in informes7)
                    {
                        if (info.id_carrera == idCarreraSeleccionada)
                        {
                            infoInforme7.Add(info);
                        }
                    }
                    informe7ListView.ItemsSource = infoInforme7;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }
            
            
            //informe 8
            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo8 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe8.json");

            try
            {
                if (internet.TieneConexionInternet())
                {
                    string url8 = "https://adminuas-001-site3.gtempurl.com/api/Informe8/Consultar_Informe8?id_carrera=" + idCarreraSeleccionada;

                    var request8 = new HttpRequestMessage();
                    request8.RequestUri = new Uri(url8);
                    request8.Method = HttpMethod.Get;

                    HttpClient client8 = new HttpClient();
                    HttpResponseMessage response8 = await client8.SendAsync(request8);

                    if (response8.StatusCode == HttpStatusCode.OK)
                    {
                        string content8 = await response8.Content.ReadAsStringAsync();
                        var informes8 = JsonConvert.DeserializeObject<List<Classficha8>>(content8);

                        informe8ListView.ItemsSource = informes8;
                        File.WriteAllText(rutaArchivo8, content8);

                    }
                }
                else
                {
                    string jsonGuardado = File.ReadAllText(rutaArchivo8);
                    var informes8 = JsonConvert.DeserializeObject<List<Classficha8>>(jsonGuardado);
                    List<Classficha8> infoInforme8 = new List<Classficha8>();

                    foreach (var info in informes8)
                    {
                        if (info.id_carrera == idCarreraSeleccionada)
                        {
                            infoInforme8.Add(info);
                        }
                    }
                    informe8ListView.ItemsSource = infoInforme8;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }
            

            //informe 9

            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo9 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "informe9.json");

            try
            {
                if (internet.TieneConexionInternet())
                {
                    string url9 = "https://adminuas-001-site3.gtempurl.com/api/Informe9/Consultar_Informe9?id_carrera=" + idCarreraSeleccionada;

                    var request9 = new HttpRequestMessage();
                    request9.RequestUri = new Uri(url9);
                    request9.Method = HttpMethod.Get;

                    HttpClient client9 = new HttpClient();
                    HttpResponseMessage response9 = await client9.SendAsync(request9);

                    if (response9.StatusCode == HttpStatusCode.OK)
                    {
                        string content9 = await response9.Content.ReadAsStringAsync();
                        var informes9 = JsonConvert.DeserializeObject<List<Classficha9>>(content9);

                        informe9ListView.ItemsSource = informes9;
                        File.WriteAllText(rutaArchivo9, content9);

                    }
                }
                else
                {
                    string jsonGuardado = File.ReadAllText(rutaArchivo9);
                    var informes9 = JsonConvert.DeserializeObject<List<Classficha9>>(jsonGuardado);
                    List<Classficha9> infoInforme9 = new List<Classficha9>();

                    foreach (var info in informes9)
                    {
                        if (info.id_carrera == idCarreraSeleccionada)
                        {
                            infoInforme9.Add(info);
                        }
                    }
                    informe9ListView.ItemsSource = infoInforme9;
                }
            }
            catch (Exception ex)
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