using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace controlCalidad
{
    public partial class App : Application
    {

        public static MasterDetailPage MasterDet { get; set; }

        // Variables para almacenar información sobre la carrera, facultad, zona y token del usuario.
        public int CarreraPersona;
        public int FacultadPersona;
        public int ZonaPersona;
        public string tokenPersona;
        public string NombrePersona;

        public int IdCarreraElegida;
        public int IdFacultadElegida;
        public int IdZonaElegida;

        public string NombreCarreraElegida;
        public string NombreFacultadElegida;
        public string NombreZonaElegida;

        public App()
        {
            InitializeComponent();

            if (Application.Current.Properties.ContainsKey("tokenPersona"))
            {
                Application.Current.Properties["tokenPersona"] = null; // O cualquier valor inicial que desees
            }
            else
            {
                Application.Current.Properties.Add("tokenPersona", null); // O cualquier valor inicial que desees
            }

            // Define la página principal de la aplicación.
            // En este caso, se configura para que la página de inicio sea una instancia de la clase "pagina_login".
            //MainPage = new MainPage();
            MainPage = new NavigationPage(new pagina_login());

        }

        protected override void OnStart()
        {
            bool usuarioYaInicioSesion = VerificarSesion();

            if (usuarioYaInicioSesion)
            {
                // Si el usuario ya ha iniciado sesión, configuramos MainPage como tu MasterDetailPage.
                ConfigurarMasterDetailPage();
            }
            else
            {

            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private bool VerificarSesion()
        {
            // Aquí implementa la lógica para verificar si el usuario ya ha iniciado sesión.
            // Puede ser a través de la existencia de un token de sesión, un estado de autenticación en la configuración, etc.
            // Devuelve true si el usuario ya ha iniciado sesión, false en caso contrario.
            // Ejemplo:
            // return !string.IsNullOrEmpty(App.Current.Properties["TokenSesion"] as string);
            //return !string.IsNullOrEmpty(App.Current.Properties["tokenPersona"] as string);
            if (App.Current.Properties["tokenPersona"] != null)
            {
                return true;
            }else { return false; }
        }

        private void ConfigurarMasterDetailPage()
        {
            // Aquí configuramos MainPage como tu MasterDetailPage.
            var masterPage = new Master(); // Define tu MasterPage aquí.
            var detailNavigation = new NavigationPage(new Pagina_indicadores()); // La página inicial en el Detail, envuelta en NavigationPage.
            MasterDet = new MasterDetailPage
            {
                Master = masterPage,
                Detail = detailNavigation
            };

            MainPage = MasterDet;
        }

    }
}
