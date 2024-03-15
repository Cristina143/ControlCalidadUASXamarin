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
using System.IO;
using Xamarin.Essentials;

namespace controlCalidad
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Pagina_indicadores : ContentPage
    {
        private List<ClassIndicador> preguntas;
        public Pagina_indicadores()
        {
            InitializeComponent();
        }

        int idcarreraElegida = ((App)Application.Current).IdCarreraElegida;
        int idfacultadElegida = ((App)Application.Current).IdFacultadElegida;
        int idzonaElegida = ((App)Application.Current).IdZonaElegida;


        string nombrecarreraElegida = ((App)Application.Current).NombreCarreraElegida;
        string nombrefacultadElegida = ((App)Application.Current).NombreFacultadElegida;
        string nombrezonaElegida = ((App)Application.Current).NombreZonaElegida;

        // Método que se ejecuta al cargar la página
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Obtener datos del usuario almacenados en la aplicación
            //int carrera = ((App)Application.Current).CarreraPersona;
            //string token = ((App)Application.Current).tokenPersona;
            //int facultad = ((App)Application.Current).FacultadPersona;
            //int zona =((App)Application.Current).ZonaPersona;

            

            // Obtener la ruta del archivo en el sistema de archivos local
            string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "zona.json");
            

            try
            {
                if (internet.TieneConexionInternet())
                {
                    // El dispositivo tiene acceso a Internet

                    var request = new HttpRequestMessage();
                    request.RequestUri = new Uri("https://adminuas-001-site3.gtempurl.com/api/Zona/Consultar_Zona");
                    request.Method = HttpMethod.Get;
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);
                    // Verificar si la respuesta del servidor es exitosa (código 200 OK)
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // Leer el contenido de la respuesta
                        string content = await response.Content.ReadAsStringAsync();
                        // Deserializar el contenido JSON a una lista de objetos ClassZona
                        var resultado = JsonConvert.DeserializeObject<List<ClassZona>>(content);
                        // Asignar la lista de zonas al origen de datos del Picker
                        Select_zona.ItemsSource = resultado;
                        // Guardar los datos en el archivo local
                        File.WriteAllText(rutaArchivo, content);

                    }
                }
                else
                {
                    // El dispositivo no tiene acceso a Internet
                    // Leer los datos almacenados localmente en caso de no haber conexión
                    string jsonGuardado = File.ReadAllText(rutaArchivo);
                    var resp = JsonConvert.DeserializeObject<List<ClassZona>>(jsonGuardado);
                    Select_zona.ItemsSource = resp;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");

            }

        }

        // Método que se ejecuta cuando se selecciona una opción en el Picker de zona
        private async void Select_zona_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado una zona
            if (Select_zona.SelectedItem != null)
            {
                // Obtener la zona seleccionada del Picker
                ClassZona zonaSeleccionada = (ClassZona)Select_zona.SelectedItem;
                int idZonaSeleccionada = zonaSeleccionada.id_zona;
                ((App)Application.Current).IdZonaElegida = idZonaSeleccionada;
                ((App)Application.Current).NombreZonaElegida = zonaSeleccionada.nombre;

                // Obtener la ruta del archivo en el sistema de archivos local
                string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "facultad.json");
                try
                {
                    // Verificar si el dispositivo tiene conexión a Internet
                    if (internet.TieneConexionInternet())
                    {
                        // Realizar una solicitud HTTP para obtener las facultades por la zona seleccionada
                        var request = new HttpRequestMessage();
                        request.RequestUri = new Uri("https://adminuas-001-site3.gtempurl.com/api/Facultades/Consultar_Facultad");
                        request.Method = HttpMethod.Get;

                        HttpClient client = new HttpClient();
                        HttpResponseMessage response = await client.SendAsync(request);

                        // Verificar si la respuesta del servidor es exitosa (código 200 OK)
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Leer el contenido de la respuesta
                            string content = await response.Content.ReadAsStringAsync();
                            // Deserializar las facultades desde el JSON
                            var facultades = JsonConvert.DeserializeObject<List<ClassFacultad>>(content);
                            // Filtrar las facultades por la zona seleccionada
                            var facultadesFiltradas = facultades.Where(f => f.id_zona == idZonaSeleccionada).ToList();
                            // Asignar las facultades filtradas al origen de datos del Picker de facultades
                            Select_facultad.ItemsSource = facultadesFiltradas;
                            // Guardar datos en el archivo local
                            File.WriteAllText(rutaArchivo, content);
                        }
                    }
                    else
                    {
                        // El dispositivo no tiene conexión a Internet
                        // Leer los datos almacenados localmente en caso de no haber conexión
                        string jsonGuardado = File.ReadAllText(rutaArchivo);
                        var resp = JsonConvert.DeserializeObject<List<ClassFacultad>>(jsonGuardado);
                        // Filtrar las facultades por la zona seleccionada
                        var facultadesFiltradas = resp.Where(f => f.id_zona == idZonaSeleccionada).ToList();
                        // Asignar las facultades filtradas al origen de datos del Picker de facultades
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

        // Método que se ejecuta cuando se selecciona una opción en el Picker de facultad
        private async void Select_facultad_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado una facultad
            if (Select_facultad.SelectedItem != null)
            {
                // Obtener la facultad seleccionada del Picker
                ClassFacultad facultadSeleccionada = (ClassFacultad)Select_facultad.SelectedItem;
                int idFacultadSeleccionada = facultadSeleccionada.id_facultad;
                
                ((App)Application.Current).IdFacultadElegida = idFacultadSeleccionada;
                ((App)Application.Current).NombreFacultadElegida = facultadSeleccionada.nombre;

                // Obtener la ruta del archivo en el sistema de archivos local
                string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "carrera.json");

                try
                {
                    // Verificar si el dispositivo tiene conexión a Internet
                    if (internet.TieneConexionInternet())
                    {
                        // Realizar una solicitud HTTP para obtener las carreras por la facultad seleccionada
                        var request = new HttpRequestMessage();
                        request.RequestUri = new Uri("https://adminuas-001-site3.gtempurl.com/api/Carreras/Consultar_Carrera");
                        request.Method = HttpMethod.Get;

                        HttpClient client = new HttpClient();
                        HttpResponseMessage response = await client.SendAsync(request);

                        // Verificar si la respuesta del servidor es exitosa (código 200 OK)
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Leer el contenido de la respuesta
                            string content = await response.Content.ReadAsStringAsync();
                            // Deserializar las carreras desde el JSON
                            var carreras = JsonConvert.DeserializeObject<List<ClassCarrera>>(content);

                            // Filtrar las carreras por la facultad seleccionada
                            var carrerasFiltradas = carreras.Where(c => c.id_facultad == idFacultadSeleccionada).ToList();

                            // Asignar las carreras filtradas al origen de datos del Picker de carreras
                            Seject_carrera.ItemsSource = carrerasFiltradas;
                            // Guardar datos en el archivo local
                            File.WriteAllText(rutaArchivo, content);
                        }
                    }
                    else
                    {
                        // El dispositivo no tiene conexión a Internet
                        // Leer los datos almacenados localmente en caso de no haber conexión
                        string jsonGuardado = File.ReadAllText(rutaArchivo);
                        var resp = JsonConvert.DeserializeObject<List<ClassCarrera>>(jsonGuardado);
                        // Filtrar las carreras por la facultad seleccionada
                        var carrerasFiltradas = resp.Where(c => c.id_facultad == idFacultadSeleccionada).ToList();
                        // Asignar las carreras filtradas al origen de datos del Picker de carreras
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

        // Método que se ejecuta cuando se selecciona una opción en el Picker de carrera
        private async void Seject_carrera_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado una carrera
            if (Seject_carrera.SelectedItem != null)
            {
                // Obtener la carrera seleccionada del Picker
                ClassCarrera carreraSeleccionada = (ClassCarrera)Seject_carrera.SelectedItem;
                int idCarreraSeleccionada = carreraSeleccionada.id_carrera;

                ((App)Application.Current).IdCarreraElegida = idCarreraSeleccionada;
                ((App)Application.Current).NombreCarreraElegida = carreraSeleccionada.nombre;

                // Obtener la ruta del archivo en el sistema de archivos local
                string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "pregunta.json");

                try
                {
                    // Verificar si el dispositivo tiene conexión a Internet
                    if (internet.TieneConexionInternet())
                    {
                        // Crear un objeto de la clase ClassIndicador con la carrera seleccionada
                        ClassIndicador ind = new ClassIndicador
                        {
                            id_pregunta = 0,
                            id_carrera = idCarreraSeleccionada,
                            eje = 0,
                            categoria = 0,
                            indicador = 0,
                            nombre = "",
                            valuacion = "",
                            margen = 0,
                        };
                        // Realizar una solicitud HTTP para obtener las preguntas por la carrera seleccionada
                        string RequestApi = "https://adminuas-001-site3.gtempurl.com/api/Respuestas/Consultar_Pregunta";
                        HttpClient client = new HttpClient();
                        var json = JsonConvert.SerializeObject(ind);
                        var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync(RequestApi, contentJson);
                        // Verificar si la respuesta del servidor es exitosa (código 200 OK)
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Leer el contenido de la respuesta
                            string content = await response.Content.ReadAsStringAsync();
                            // Deserializar las preguntas desde el JSON
                            preguntas = JsonConvert.DeserializeObject<List<ClassIndicador>>(content);
                            // Asignar las preguntas al origen de datos del Picker de indicadores
                            Select_indicador.ItemsSource = preguntas;
                            Select_indicador.ItemDisplayBinding = new Binding("nombre");
                            // Guardar datos en el archivo local
                            File.WriteAllText(rutaArchivo, content);

                        }
                    }
                    else
                    {
                        // El dispositivo no tiene conexión a Internet
                        // Leer los datos almacenados localmente en caso de no haber conexión
                        string jsonGuardado = File.ReadAllText(rutaArchivo);
                        preguntas = JsonConvert.DeserializeObject<List<ClassIndicador>>(jsonGuardado);
                        // Asignar las preguntas al origen de datos del Picker de indicadores
                        Select_indicador.ItemsSource = preguntas;
                        Select_indicador.ItemDisplayBinding = new Binding("nombre");
                     
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    await DisplayAlert("Error de conexión", ex.Message, "OK");
                }
            }
        }

        // Método que se ejecuta cuando se selecciona una opción en el Picker de indicadores
        private async void Select_indicador_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado un indicador y si hay preguntas disponibles
            if (Select_indicador.SelectedItem != null && preguntas != null && preguntas.Count > 0)
            {
                //valuacion
                // Obtener la pregunta seleccionada del Picker
                var selectedQuestion = preguntas[Select_indicador.SelectedIndex];
                //lblIndicador.Text = $"{selectedQuestion.eje}-{selectedQuestion.categoria}-{selectedQuestion.indicador}";
                // Mostrar el nombre de la pregunta en un Label
                lblIndicador.Text = selectedQuestion.nombre;
                // Obtener el margen de la pregunta
                var margen = selectedQuestion.margen;

                // Rellenar el Entry valoracion con la información de la pregunta seleccionada
                valoracion.Text = selectedQuestion.valuacion;

                //primera tabla
                ClassIndicador indicadorSeleccionada = (ClassIndicador)Select_indicador.SelectedItem;
                // Obtener el ID de la pregunta seleccionada
                int idIndicadorSeleccionada = indicadorSeleccionada.id_pregunta;

                // Obtener la ruta del archivo en el sistema de archivos local para la tabla de recomendaciones
                string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "recomendacion.json");

                try
                {
                    // Verificar si el dispositivo tiene conexión a Internet
                    if (internet.TieneConexionInternet())
                    {
                        // Crear un objeto de la clase ClassRecomendacion con la pregunta seleccionada
                        ClassRecomendacion rec = new ClassRecomendacion
                        {
                            id_recomendacion = 0,
                            id_pregunta = idIndicadorSeleccionada,
                            nombre = "",
                            accion = "",
                            responsable = "",
                            objetivos = "",
                            porcentaje_metas = 0,
                            fecha_limite = "2023-11-21",
                            semaforo_ama = "2023-11-27",
                            semaforo_rojo = "2023-11-27",
                            cumplido = true,
                        };

                        // Realizar una solicitud HTTP para obtener las recomendaciones por la pregunta seleccionada
                        string RequestApi = "https://adminuas-001-site3.gtempurl.com/api/Recomendaciones/Consultar_Recomendacion";
                        HttpClient client = new HttpClient();
                        var json = JsonConvert.SerializeObject(rec);
                        var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync(RequestApi, contentJson);
                        // Verificar si la respuesta del servidor es exitosa (código 200 OK)
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Leer el contenido de la respuesta
                            string content = await response.Content.ReadAsStringAsync();
                            // Deserializar las recomendaciones desde el JSON
                            List<ClassRecomendacion> recomendaciones = JsonConvert.DeserializeObject<List<ClassRecomendacion>>(content);
                            // Procesar las recomendaciones y aplicar colores según las condiciones
                            foreach (var recomendacion in recomendaciones)
                            {
                                // Formatear fechas
                                DateTime fecha = DateTime.Parse(recomendacion.fecha_limite);
                                string fechaFormateada = fecha.ToString("yyyy-MM-dd");
                                recomendacion.fecha_limite = fechaFormateada;

                                DateTime fecha_amarillo = DateTime.Parse(recomendacion.semaforo_ama);
                                string fecha_amarilloFormateada = fecha_amarillo.ToString("yyyy-MM-dd");
                                recomendacion.semaforo_ama = fecha_amarilloFormateada;

                                DateTime fecha_rojo = DateTime.Parse(recomendacion.semaforo_rojo);
                                string fecha_rojoFormateada = fecha_rojo.ToString("yyyy-MM-dd");
                                recomendacion.semaforo_rojo = fecha_rojoFormateada;

                                // Calcular la diferencia en días entre la fecha actual y la fecha límite
                                DateTime fechaActual = DateTime.Now;
                                TimeSpan diferencia = fecha - fechaActual;
                                int diasDiferencia = (int)diferencia.TotalDays;
                                recomendacion.dias_diferencia = diasDiferencia;

                                // Aplicar colores según las condiciones
                                if (recomendacion.cumplido == false)
                                {
                                    if (diasDiferencia > margen)
                                    {
                                        recomendacion.FondoColor = Color.Yellow;
                                    }
                                    else if (diasDiferencia <= margen)
                                    {
                                        recomendacion.FondoColor = Color.LightCoral;
                                    }
                                }
                                else if (recomendacion.cumplido == true) //ponerse en color verde si ya se cumplio
                                {
                                    // Ponerse en color verde si ya se cumplió
                                    recomendacion.FondoColor = Color.LightGreen;
                                }




                                /*if (fechaActual > fecha_rojo && fechaActual < fecha) //ponerse en color rojo si la fecha actual es mayor (es decir que ya paso) que la fecha puesta para el rojo pero menor que la fecha limite (que no ha pasado)
                                {
                                    recomendacion.FondoColor = Color.LightCoral;
                                }
                                else if (fechaActual > fecha_rojo && fechaActual > fecha) //ponerse en color rojo si la fecha actual es mayor (es decir que ya paso) que la fecha puesta para el rojo y mayor que la fecha limite (que ya ha pasado)
                                {
                                    recomendacion.FondoColor = Color.LightCoral;
                                }
                                else if (fechaActual < fecha_rojo && fechaActual >= fecha_amarillo) //ponerse en color amarillo si la fecha actual es menor (es decir que no ha pasado) que la fecha puesta para el rojo pero mayor o igual que la fecha en amarillo (que ya ha pasado)
                                {
                                    recomendacion.FondoColor = Color.Yellow;
                                }
                                else if (fechaActual < fecha_amarillo) //ponerse en color verde si la fecha actual es menor (es decir que no ha pasado) que la fecha puesta para el amarillo
                                {
                                    recomendacion.FondoColor = Color.LightGreen;
                                }*/

                                /*if (diasDiferencia <= 15 && diasDiferencia > 0 && recomendacion.porcentaje_metas < 100)
                                {
                                    recomendacion.FondoColor = Color.IndianRed;
                                }
                                else if (diasDiferencia > 15 && diasDiferencia <= 30 && recomendacion.porcentaje_metas < 100)
                                {
                                    recomendacion.FondoColor = Color.Yellow;
                                }
                                else if (diasDiferencia < 0)
                                {
                                    if (recomendacion.porcentaje_metas < 100)
                                    {
                                        recomendacion.FondoColor = Color.IndianRed;
                                    }
                                    else
                                    {
                                        recomendacion.FondoColor = Color.PaleGreen;//greenyellow
                                    }
                                }
                                else
                                {
                                    // Color por defecto si no se cumplen las condiciones anteriores
                                    recomendacion.FondoColor = Color.PaleGreen; //darkseagreen
                                }*/
                            }
                            // Asignar las recomendaciones al ListView y guardar datos en el archivo local
                            recomendacionesListView.ItemsSource = recomendaciones;
                            // Guardar datos
                            File.WriteAllText(rutaArchivo, content);
                        }
                    }
                    else
                    {
                        // El dispositivo no tiene conexión a Internet
                        // Leer los datos almacenados localmente en caso de no haber conexión
                        string jsonGuardado = File.ReadAllText(rutaArchivo);
                        var resp = JsonConvert.DeserializeObject<List<ClassRecomendacion>>(jsonGuardado);
                        // Filtrar las recomendaciones por la pregunta seleccionada
                        List<ClassRecomendacion> recomendacionPregunta = new List<ClassRecomendacion>();
                        // Aplicar colores según las condiciones
                        foreach (var recomendacion in resp)
                        {
                            if (recomendacion.id_pregunta == idIndicadorSeleccionada)
                            {
                                // Formatear fechas
                                DateTime fecha = DateTime.Parse(recomendacion.fecha_limite);
                                string fechaFormateada = fecha.ToString("yyyy-MM-dd");
                                recomendacion.fecha_limite = fechaFormateada;

                                DateTime fecha_amarillo = DateTime.Parse(recomendacion.semaforo_ama);
                                string fecha_amarilloFormateada = fecha_amarillo.ToString("yyyy-MM-dd");
                                recomendacion.semaforo_ama = fecha_amarilloFormateada;

                                DateTime fecha_rojo = DateTime.Parse(recomendacion.semaforo_rojo);
                                string fecha_rojoFormateada = fecha_rojo.ToString("yyyy-MM-dd");
                                recomendacion.semaforo_rojo = fecha_rojoFormateada;

                                // Calcular la diferencia en días entre la fecha actual y la fecha límite
                                DateTime fechaActual = DateTime.Now;
                                TimeSpan diferencia = fecha - fechaActual;
                                int diasDiferencia = (int)diferencia.TotalDays;
                                recomendacion.dias_diferencia = diasDiferencia;

                                // Aplicar colores según las condiciones
                                if (recomendacion.cumplido == false)
                                {
                                    if (diasDiferencia > margen)
                                    {
                                        recomendacion.FondoColor = Color.Yellow;
                                    }
                                    else if (diasDiferencia <= margen)
                                    {
                                        recomendacion.FondoColor = Color.LightCoral;
                                    }
                                }
                                else if (recomendacion.cumplido == true) //ponerse en color verde si ya se cumplio
                                {
                                    // Ponerse en color verde si ya se cumplió
                                    recomendacion.FondoColor = Color.LightGreen;
                                }

                                recomendacionPregunta.Add( recomendacion);
                            }
                        }
                        // Asignar las recomendaciones al ListView
                        recomendacionesListView.ItemsSource = recomendacionPregunta;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    await DisplayAlert("Error de conexión", ex.Message, "OK");
                }

                //segunda tabla

                // Obtener la ruta del archivo en el sistema de archivos local
                string rutaArchivo1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "cumplimiento.json");

                try
                {
                    // Verificar si el dispositivo tiene conexión a Internet
                    if (internet.TieneConexionInternet())
                    {
                        // Crear un objeto de la clase ClassCumplimiento con la pregunta seleccionada
                        ClassCumplimiento cumplimiento = new ClassCumplimiento
                        {
                            id_cumplimiento = 0,
                            id_pregunta = idIndicadorSeleccionada,
                            id_recomendacion = 0,
                            acciones_realizadas = "",
                            fecha = "2023-11-21",
                            meta_alcanzada = 0,
                            documentos = "",
                        };

                        // Realizar una solicitud HTTP para obtener los cumplimientos por la pregunta seleccionada
                        string RequestApiCumplimiento = "https://adminuas-001-site3.gtempurl.com/api/Cumplimiento/Consultar_Cumplimiento";
                        HttpClient clientCumplimiento = new HttpClient();
                        var jsonCumplimiento = JsonConvert.SerializeObject(cumplimiento);
                        var contentJsonCumplimiento = new StringContent(jsonCumplimiento, Encoding.UTF8, "application/json");
                        HttpResponseMessage responseCumplimiento = await clientCumplimiento.PostAsync(RequestApiCumplimiento, contentJsonCumplimiento);
                        // Verificar si la respuesta del servidor es exitosa (código 200 OK)
                        if (responseCumplimiento.StatusCode == HttpStatusCode.OK)
                        {
                            // Leer el contenido de la respuesta
                            string contentCumplimiento = await responseCumplimiento.Content.ReadAsStringAsync();
                            // Deserializar los cumplimientos desde el JSON
                            var cumplimientos = JsonConvert.DeserializeObject<List<ClassCumplimiento>>(contentCumplimiento);
                            // Formatear fechas
                            foreach (var cumplimir in cumplimientos)
                            {
                                DateTime fecha = DateTime.Parse(cumplimir.fecha);
                                string fechaFormateada = fecha.ToString("yyyy-MM-dd");
                                cumplimir.fecha = fechaFormateada;
                            }
                            // Asignar los cumplimientos al ListView y guardar datos en el archivo local
                            cumplimientoListView.ItemsSource = cumplimientos;
                            File.WriteAllText(rutaArchivo1, contentCumplimiento);
                        }
                    }
                    else
                    {
                        // El dispositivo no tiene conexión a Internet
                        // Leer los datos almacenados localmente en caso de no haber conexión
                        string jsonGuardado = File.ReadAllText(rutaArchivo1);
                        var resp = JsonConvert.DeserializeObject<List<ClassCumplimiento>>(jsonGuardado);
                        // Filtrar los cumplimientos por la pregunta seleccionada
                        List<ClassCumplimiento> cumplimientoPregunta = new List<ClassCumplimiento>();
                        // Formatear fechas
                        foreach (var cumplimir in resp)
                        {
                            if (cumplimir.id_pregunta == idIndicadorSeleccionada)
                            {
                                DateTime fecha = DateTime.Parse(cumplimir.fecha);
                                string fechaFormateada = fecha.ToString("yyyy-MM-dd");
                                cumplimir.fecha = fechaFormateada;
                                cumplimientoPregunta.Add(cumplimir);
                            }


                                
                        }
                        // Asignar los cumplimientos al ListView
                        cumplimientoListView.ItemsSource = cumplimientoPregunta;
                    }   
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    await DisplayAlert("Error de conexión", ex.Message, "OK");
                }
            }
        }
    }
}