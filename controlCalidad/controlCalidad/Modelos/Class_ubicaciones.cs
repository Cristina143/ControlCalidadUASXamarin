using System;
using System.Collections.Generic;
using System.Text;

namespace controlCalidad.Modelos
{
    class Class_ubicaciones
    {
    }

    public class ClassZona
    {
        public int id_zona { get; set; }
        public string nombre { get; set; }
    }

    public class ClassFacultad
    {
        public int id_facultad { get; set; }
        public int id_zona { get; set; }
        public string nombre { get; set; }
    }

    public class ClassCarrera
    {
        public int id_carrera { get; set; }
        public string nombre { get; set; }
        public int id_facultad { get; set; }
    }
}
