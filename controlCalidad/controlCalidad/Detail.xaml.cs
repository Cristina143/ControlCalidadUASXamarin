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
    public partial class Detail : ContentPage
    {
        public Detail()
        {
            InitializeComponent();
            lblusuario.Text = ((App)Application.Current).NombrePersona; //Escribe el valor que se encuentra en la variable general NombrePersona (que corresponde al nombre de usuario) en el texto del label, para mostrar en pantalla
        }
    }
}