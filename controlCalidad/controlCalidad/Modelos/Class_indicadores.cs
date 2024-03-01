using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace controlCalidad.Modelos
{
    internal class Class_indicadores
    {
    }

    public class ClassIndicador
    {
        public int id_pregunta { get; set; }
        public int id_carrera { get; set; }
        public int eje { get; set; }
        public int categoria { get; set; }
        public int indicador { get; set; }
        public string nombre { get; set; }
        public string valuacion { get; set; }
        public int margen { get; set; }
    }

    public class ClassRecomendacion
    {
        public int id_recomendacion { get; set; }
        public int id_pregunta { get; set; }
        public string nombre { get; set; }
        public string accion { get; set; }
        public string responsable { get; set; }
        public string objetivos { get; set; }
        public int porcentaje_metas { get; set; }
        public string fecha_limite { get; set; }
        public string semaforo_ama { get; set; }
        public string semaforo_rojo { get; set; }
        public Boolean cumplido { get; set; }
        public int dias_diferencia { get; set; }
        public Color FondoColor { get; set; }
    }

    public class ClassCumplimiento
    {
        public int id_cumplimiento { get; set; }
        public int id_pregunta { get; set; }
        public int id_recomendacion { get; set; }
        public string acciones_realizadas { get; set; }
        public string fecha { get; set; }
        public int meta_alcanzada { get; set; }
        public string documentos { get; set; }
    }
}
