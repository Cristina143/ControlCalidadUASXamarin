using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace controlCalidad
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Master : ContentPage
    {
        public Master()
        {
            InitializeComponent();
        }

        // Método invocado cuando se hace clic en el botón de Indicadores
        private async void btnIndicadores_Clicked(object sender, EventArgs e)
        {
            // Oculta el menú desplegable (master)
            App.MasterDet.IsPresented = false;
            // Crea una nueva página de navegación para la sección de Indicadores y la muestra
            var paginaIndicadores = new NavigationPage(new Pagina_indicadores());
            await App.MasterDet.Detail.Navigation.PushAsync(paginaIndicadores);
        }

        // Método invocado cuando se hace clic en el botón de Fichas
        private async void btnFichas_Clicked(object sender, EventArgs e)
        {
            // Oculta el menú desplegable (master)
            App.MasterDet.IsPresented = false;
            // Crea una nueva página de navegación para la sección de Fichas y la muestra
            var paginaFichas = new NavigationPage(new pagina_fichas());
            await App.MasterDet.Detail.Navigation.PushAsync(paginaFichas);
        }

        // Método invocado cuando se hace clic en el botón de Cerrar Sesión
        private void btnCerrar_Clicked(object sender, EventArgs e)
        {
            // Restablece todas las variables de sesión a sus valores predeterminados
            ((App)Application.Current).CarreraPersona = 0;
            ((App)Application.Current).FacultadPersona = 0;
            ((App)Application.Current).ZonaPersona = 0;
            ((App)Application.Current).tokenPersona = null;
            ((App)Application.Current).NombrePersona = null;
            ((App)Application.Current).IdCarreraElegida = 0;
            ((App)Application.Current).IdFacultadElegida = 0;
            ((App)Application.Current).IdZonaElegida = 0;
            ((App)Application.Current).NombreCarreraElegida = null;
            ((App)Application.Current).NombreFacultadElegida = null;
            ((App)Application.Current).NombreZonaElegida= null;
            // Redirige a la página de inicio de sesión
            App.Current.MainPage = new pagina_login();
        }
    }
}