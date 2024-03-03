using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace controlCalidad.Modelos
{
    // Clase estática para verificar la conexión a Internet.
    public static class internet
    {
        // Método estático que devuelve true si hay conexión a Internet, de lo contrario, devuelve false.
        public static bool TieneConexionInternet()
        {
            var current = Connectivity.NetworkAccess; // Obtiene el estado de la conexión actual.
            return current == NetworkAccess.Internet; // Devuelve true si el estado de la conexión es Internet, de lo contrario, devuelve false.
        }
    }
   
}
