using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace controlCalidad
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            // Configuración del MainPage como MasterDetailPage
            // Se establece la página Master como la clase Master
            this.Master = new Master();
            // Se establece la página Detail como la clase Detail dentro de un NavigationPage
            this.Detail = new NavigationPage(new Detail());
            // Se establece la instancia de MainPage como la propiedad MasterDet de la clase App
            App.MasterDet = this;
        }
    }
}
