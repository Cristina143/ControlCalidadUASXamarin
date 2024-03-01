using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace controlCalidad.Modelos
{
    public static class internet
    {
        public static bool TieneConexionInternet()
        {
            var current = Connectivity.NetworkAccess;
            return current == NetworkAccess.Internet;
        }
    }
    /*internal class internet
    {
    }*/
}
