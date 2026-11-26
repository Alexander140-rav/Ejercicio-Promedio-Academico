namespace PromedioAcademico.Models
{
    public class PromedioModel
    {
        public double Calificacion1 { get; set; }
        public double Calificacion2 { get; set; }
        public double Calificacion3 { get; set; }

        public double CalcularPromedio()
        {
            return (Calificacion1 + Calificacion2 + Calificacion3) / 3.0;
        }
    }
}