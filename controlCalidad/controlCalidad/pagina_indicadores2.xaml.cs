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

namespace controlCalidad
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pagina_indicadores2 : ContentPage
    {
        private List<ClassIndicador> preguntas;
        public pagina_indicadores2()
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

            try
            {
                /*if (carrera == 0)
                {
                    var request = new HttpRequestMessage();
                    request.RequestUri = new Uri("https://marianitaaa123-001-site1.etempurl.com/api/Carreras/Consultar_Zona");
                    request.Method = HttpMethod.Get;
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<List<ClassZona>>(content);
                    }
                }
                else
                {*/
                // facultad nueva
                List<ClassFacultad> facultadSeleccionada = new List<ClassFacultad>();
                var requestfacu = new HttpRequestMessage();
                requestfacu.RequestUri = new Uri("https://adminuas-001-site3.gtempurl.com/api/Facultades/Consultar_Facultad");
                requestfacu.Method = HttpMethod.Get;
                HttpClient clientfacu = new HttpClient();
                HttpResponseMessage responsefacu = await clientfacu.SendAsync(requestfacu);
                if (responsefacu.StatusCode == HttpStatusCode.OK)
                {
                    string contentfacu = await responsefacu.Content.ReadAsStringAsync();
                    var facultadesfacu = JsonConvert.DeserializeObject<List<ClassFacultad>>(contentfacu);
                    // Filtrar las facultades por la zona seleccionada
                    var facultadesFiltradas = facultadesfacu.Where(f => f.id_facultad == facultad).ToList();
                    Select_facultad.ItemsSource = facultadesFiltradas;
                }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }
        }

        private async void Select_facultad_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClassFacultad facultadSeleccionada = (ClassFacultad)Select_facultad.SelectedItem;
            int idFacultadSeleccionada = facultadSeleccionada.id_facultad;

            try
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

                    Select_carrera.ItemsSource = carrerasFiltradas;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }
        }

        private async void Select_carrera_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClassCarrera carreraSeleccionada = (ClassCarrera)Select_carrera.SelectedItem;
            int idCarreraSeleccionada = carreraSeleccionada.id_carrera;
            try
            {
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
                string RequestApi = "https://adminuas-001-site3.gtempurl.com/api/Respuestas/Consultar_Pregunta";
                HttpClient client = new HttpClient();
                var json = JsonConvert.SerializeObject(ind);
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(RequestApi, contentJson);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    preguntas = JsonConvert.DeserializeObject<List<ClassIndicador>>(content);

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

        private async void Select_indicador_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Select_indicador.SelectedItem != null && preguntas != null && preguntas.Count > 0)
            {
                //valuacion
                var selectedQuestion = preguntas[Select_indicador.SelectedIndex];
                lblIndicador.Text = $"{selectedQuestion.eje}-{selectedQuestion.categoria}-{selectedQuestion.indicador}";
                var margen = selectedQuestion.margen;

                // Rellenar el Entry valoracion con la información de la pregunta seleccionada
                valoracion.Text = selectedQuestion.valuacion;

                //primera tabla
                ClassIndicador indicadorSeleccionada = (ClassIndicador)Select_indicador.SelectedItem;
                int idIndicadorSeleccionada = indicadorSeleccionada.id_pregunta;
                try
                {
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
                    string RequestApi = "https://adminuas-001-site3.gtempurl.com/api/Recomendaciones/Consultar_Recomendacion";
                    HttpClient client = new HttpClient();
                    var json = JsonConvert.SerializeObject(rec);
                    var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(RequestApi, contentJson);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        List<ClassRecomendacion> recomendaciones = JsonConvert.DeserializeObject<List<ClassRecomendacion>>(content);

                        foreach (var recomendacion in recomendaciones)
                        {
                            DateTime fecha = DateTime.Parse(recomendacion.fecha_limite);
                            string fechaFormateada = fecha.ToString("yyyy-MM-dd");
                            recomendacion.fecha_limite = fechaFormateada;

                            DateTime fecha_amarillo = DateTime.Parse(recomendacion.semaforo_ama);
                            string fecha_amarilloFormateada = fecha_amarillo.ToString("yyyy-MM-dd");
                            recomendacion.semaforo_ama = fecha_amarilloFormateada;

                            DateTime fecha_rojo = DateTime.Parse(recomendacion.semaforo_rojo);
                            string fecha_rojoFormateada = fecha_rojo.ToString("yyyy-MM-dd");
                            recomendacion.semaforo_rojo = fecha_rojoFormateada;

                            DateTime fechaActual = DateTime.Now;
                            TimeSpan diferencia = fecha - fechaActual;
                            int diasDiferencia = (int)diferencia.TotalDays;
                            recomendacion.dias_diferencia = diasDiferencia;


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
                                recomendacion.FondoColor = Color.LightGreen;
                            }

                            /*if (recomendacion.cumplido == false)
                            {
                                if (fechaActual > fecha_rojo && fechaActual < fecha) //ponerse en color rojo si la fecha actual es mayor (es decir que ya paso) que la fecha puesta para el rojo pero menor que la fecha limite (que no ha pasado)
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
                                }
                            }
                            else if (recomendacion.cumplido == true) //ponerse en color verde si ya se cumplio
                            {
                                recomendacion.FondoColor = Color.LightGreen;
                            }*/





                            /*if (diasDiferencia <= 15 && diasDiferencia > 0 && recomendacion.porcentaje_metas < 100)
                            {
                                recomendacion.FondoColor = Color.LightCoral;
                            }
                            else if (diasDiferencia > 15 && diasDiferencia <= 30 && recomendacion.porcentaje_metas < 100)
                            {
                                recomendacion.FondoColor = Color.Yellow;
                            }
                            else if (diasDiferencia < 0)
                            {
                                if (recomendacion.porcentaje_metas < 100)
                                {
                                    recomendacion.FondoColor = Color.LightCoral;
                                }
                                else
                                {
                                    recomendacion.FondoColor = Color.LightGreen;//greenyellow
                                }
                            }
                            else
                            {
                                // Color por defecto si no se cumplen las condiciones anteriores
                                recomendacion.FondoColor = Color.LightGreen; //darkseagreen
                            }*/
                        }
                        recomendacionesListView.ItemsSource = recomendaciones;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    await DisplayAlert("Error de conexión", ex.Message, "OK");
                }

                //segunda tabla
                try
                {
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

                    string RequestApiCumplimiento = "https://adminuas-001-site3.gtempurl.com/api/Cumplimiento/Consultar_Cumplimiento";
                    HttpClient clientCumplimiento = new HttpClient();
                    var jsonCumplimiento = JsonConvert.SerializeObject(cumplimiento);
                    var contentJsonCumplimiento = new StringContent(jsonCumplimiento, Encoding.UTF8, "application/json");
                    HttpResponseMessage responseCumplimiento = await clientCumplimiento.PostAsync(RequestApiCumplimiento, contentJsonCumplimiento);
                    if (responseCumplimiento.StatusCode == HttpStatusCode.OK)
                    {
                        string contentCumplimiento = await responseCumplimiento.Content.ReadAsStringAsync();
                        List<ClassCumplimiento> cumplimientos = JsonConvert.DeserializeObject<List<ClassCumplimiento>>(contentCumplimiento);

                        foreach (var cumplimir in cumplimientos)
                        {
                            DateTime fecha = DateTime.Parse(cumplimir.fecha);
                            string fechaFormateada = fecha.ToString("yyyy-MM-dd");
                            cumplimir.fecha = fechaFormateada;
                        }

                        cumplimientoListView.ItemsSource = cumplimientos;
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