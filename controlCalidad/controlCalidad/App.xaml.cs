using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace controlCalidad
{
    public partial class App : Application
    {
        public int CarreraPersona;
        public int FacultadPersona;
        public int ZonaPersona;
        public string tokenPersona;

        public App()
        {
            InitializeComponent();

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
