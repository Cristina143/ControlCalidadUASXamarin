using System;
using System.Collections.Generic;
using System.Text;

namespace controlCalidad.Modelos
{
    public class Class_login
    {
        public int id_usuario { get; set; }
        public int id_carrera { get; set; }
        public int id_facultad { get; set; }
        public int id_zona { get; set; }
        public string correo { get; set; }
        public string clave { get; set; }
        public string nombre { get; set; }
        public string puesto { get; set; }
        public string fecha_alta { get; set; }
    }

    public class loginResponse
    {
        public string Token { get; set; }
        public List<Class_login> Usuario { get; set; }
    }
}
