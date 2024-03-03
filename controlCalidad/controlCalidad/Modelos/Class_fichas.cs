using System;
using System.Collections.Generic;
using System.Text;

namespace controlCalidad.Modelos
{
    internal class Class_fichas
    {
    }

    // Clase para representar informe de tipo 1
    public class Classficha1
    {
        public int id_informe { get; set; }
        public int id_carrera { get; set; }
        public string lugar_fecha { get; set; }
    }

    // Clase para representar informe de tipo 2
    public class Classficha2
    {
        public int id_informe { get; set; }
        public string nombre { get; set; }
        public string mision { get; set; }
        public string vision { get; set; }
        public string politicas { get; set; }
        public string lineas_estrategicas { get; set; }
    }

    // Clase para representar informe de tipo 3
    public class Classficha3
    {
        public int id_informe { get; set; }
        public int id_facultad { get; set; }
        public string nombre { get; set; }
        public string campus { get; set; }
        public string fecha_inicio { get; set; }
        public string mision { get; set; }
        public string vision { get; set; }
        public string objetivos_estrategicos { get; set; }
    }

    // Clase para representar informe de tipo 4
    public class Classficha4
    {
        public int id_informe { get; set; }
        public int id_carrera { get; set; }

        public string documento_oficial { get; set; }
        public string numero_rvoe { get; set; }
        public string fecha_rvoe { get; set; }
        public string instituto_rvoe { get; set; }
        public string mision { get; set; }
        public string vision { get; set; }
        public string objetivos_estrategicos { get; set; }

    }

    // Clase para representar informe de tipo 5
    public class Classficha5
    {
        public int id_informe { get; set; }
        public int id_carrera { get; set; }
        public int tiempo_completo { get; set; }
        public float tiempo_completo_por { get; set; }
        public int tres_cuartos { get; set; }
        public float tres_cuartos_por { get; set; }
        public int medio_tiempo { get; set; }
        public float medio_tiempo_por { get; set; }
        public int asignatura { get; set; }
        public float asignatura_por { get; set; }
        public int tsu { get; set; }
        public float tsU_por { get; set; }
        public int pa { get; set; }
        public float pA_por { get; set; }
        public int l { get; set; }
        public float l_por { get; set; }
        public int e { get; set; }
        public float e_por { get; set; }
        public int m { get; set; }
        public float m_por { get; set; }
        public int d { get; set; }
        public float d_por { get; set; }
        public int total_1 { get; set; }
        public int total_2 { get; set; }
    }

    // Clase para representar informe de tipo 6
    public class Classficha6
    {
        public int id_informe { get; set; }
        public int id_carrera { get; set; }
        public int ultima_hombres_nuevo { get; set; }
        public float ultima_hombres_nuevo_por { get; set; }
        public int ultima_mujeres_nuevo { get; set; }
        public float ultima_mujeres_nuevo_por { get; set; }
        public int ultima_subtotal_nuevo { get; set; }
        public float ultima_subtotal_nuevo_por { get; set; }
        public int ultima_hombres_reingreso { get; set; }
        public float ultima_hombres_reingreso_por { get; set; }
        public int ultima_mujeres_reingreso { get; set; }
        public float ultima_mujeres_reingreso_por { get; set; }
        public int ultima_subtotal_reingreso { get; set; }
        public float ultima_subtotal_reingreso_por { get; set; }
        public int penul_hombres_nuevo { get; set; }
        public float penul_hombres_nuevo_por { get; set; }
        public int penul_mujeres_nuevo { get; set; }
        public float penul_mujeres_nuevo_por { get; set; }
        public int penul_subtotal_nuevo { get; set; }
        public float penul_subtotal_nuevo_por { get; set; }
        public int penul_hombres_reingreso { get; set; }
        public float penul_hombres_reingreso_por { get; set; }
        public int penul_mujeres_reingreso { get; set; }
        public float penul_mujeres_reingreso_por { get; set; }
        public int penul_subtotal_reingreso { get; set; }
        public float penul_subtotal_reingreso_por { get; set; }
        public int ante_hombres_nuevo { get; set; }
        public float ante_hombres_nuevo_por { get; set; }
        public int ante_mujeres_nuevo { get; set; }
        public float ante_mujeres_nuevo_por { get; set; }
        public int ante_subtotal_nuevo { get; set; }
        public float ante_subtotal_nuevo_por { get; set; }
        public int ante_hombres_reingreso { get; set; }
        public float ante_hombres_reingreso_por { get; set; }
        public int ante_mujeres_reingreso { get; set; }
        public float ante_mujeres_reingreso_por { get; set; }
        public int ante_subtotal_reingreso { get; set; }
        public float ante_subtotal_reingreso_por { get; set; }
        public int hombres_ultima { get; set; }
        public int mujeres_ultima { get; set; }
        public int suma_ultima { get; set; }
        public float hombres_ultima_por { get; set; }
        public float mujeres_ultima_por { get; set; }
        public int hombres_penul { get; set; }
        public int mujeres_penul { get; set; }
        public int suma_penul { get; set; }
        public float hombres_penul_por { get; set; }
        public float mujeres_penul_por { get; set; }
        public int hombres_ante { get; set; }
        public int mujeres_ante { get; set; }
        public int suma_ante { get; set; }
        public float hombres_ante_por { get; set; }
        public float mujeres_ante_por { get; set; }
    }

    // Clase para representar informe de tipo 7
    public class Classficha7
    {
        public int id_informe { get; set; }
        public int id_carrera { get; set; }
        public int ultima_estudiantes_ingresados { get; set; }
        public int ultima_decersion { get; set; }
        public float ultima_indice_decersion { get; set; }
        public int ultima_reprobacion { get; set; }
        public float ultima_indice_reprobacion { get; set; }
        public float ultima_egresados { get; set; }
        public float ultima_indice_eficiencia { get; set; }
        public int ultima_titulados { get; set; }
        public float ultima_indice_titulacion { get; set; }
        public float ultima_indice_neto { get; set; }
        public int penul_estudiantes_ingresados { get; set; }
        public int penul_decersion { get; set; }
        public float penul_indice_decersion { get; set; }
        public int penul_reprobacion { get; set; }
        public float penul_indice_reprobacion { get; set; }
        public float penul_egresados { get; set; }
        public float penul_indice_eficiencia { get; set; }
        public int penul_titulados { get; set; }
        public float penul_indice_titulacion { get; set; }
        public float penul_indice_neto { get; set; }
        public int ante_estudiantes_ingresados { get; set; }
        public int ante_decersion { get; set; }
        public float ante_indice_decersion { get; set; }
        public int ante_reprobacion { get; set; }
        public float ante_indice_reprobacion { get; set; }
        public float ante_egresados { get; set; }
        public float ante_indice_eficiencia { get; set; }
        public int ante_titulados { get; set; }
        public float ante_indice_titulacion { get; set; }
        public float ante_indice_neto { get; set; }
    }

    // Clase para representar informe de tipo 8
    public class Classficha8
    {
        public int id_informe { get; set; }
        public int id_carrera { get; set; }
        public string nombre { get; set; }
        public string cargo { get; set; }
    }

    // Clase para representar informe de tipo 9
    public class Classficha9
    {
        public int id_informe { get; set; }
        public int id_carrera { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string telefonos { get; set; }
    }

    // Clase para representar informe de tipo 10
    public class Classficha10
    {
        public int id_informe { get; set; }
        public int id_carrera { get; set; }
        public string ultima_generacion { get; set; }
        public int ultima_estudiantes_egresados { get; set; }
        public int ultima_alumnos_examen { get; set; }
        public int ultima_indice_aplicacion { get; set; }
        public int ultima_alumnos_aprobados { get; set; }
        public int ultima_indice_aprobacion { get; set; }
        public string penul_generacion { get; set; }
        public int penul_estudiantes_egresados { get; set; }
        public int penul_alumnos_examen { get; set; }
        public int penul_indice_aplicacion { get; set; }
        public int penul_alumnos_aprobados { get; set; }
        public int penul_indice_aprobacion { get; set; }
        public string ante_generacion { get; set; }
        public int ante_estudiantes_egresados { get; set; }
        public int ante_alumnos_examen { get; set; }
        public int ante_indice_aplicacion { get; set; }
        public int ante_alumnos_aprobados { get; set; }
        public int ante_indice_aprobacion { get; set; }
    }

}
