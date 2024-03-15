﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace controlCalidad
{
    public partial class App : Application
    {
        // Variables para almacenar información sobre la carrera, facultad, zona y token del usuario.
        public int CarreraPersona;
        public int FacultadPersona;
        public int ZonaPersona;
        public string tokenPersona;

        public int IdCarreraElegida;
        public int IdFacultadElegida;
        public int IdZonaElegida;

        public string NombreCarreraElegida;
        public string NombreFacultadElegida;
        public string NombreZonaElegida;

        public App()
        {
            InitializeComponent();

            // Define la página principal de la aplicación.
            // En este caso, se configura para que la página de inicio sea una instancia de la clase "pagina_login".
            MainPage = new MainPage();
            MainPage = new NavigationPage(new pagina_login());

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
