using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PromedioAcademico.Models;

namespace PromedioAcademico.ViewModels
{
    public class PromedioViewModel : INotifyPropertyChanged
    {
        private string _calificacion1Text = string.Empty;
        private string _calificacion2Text = string.Empty;
        private string _calificacion3Text = string.Empty;
        private string _promedioResultado = string.Empty;
        private string _nivelResultado = string.Empty;
        private string _mensajeError = string.Empty;

        public string Calificacion1Text
        {
            get => _calificacion1Text;
            set { _calificacion1Text = value; OnPropertyChanged(); }
        }

        public string Calificacion2Text
        {
            get => _calificacion2Text;
            set { _calificacion2Text = value; OnPropertyChanged(); }
        }

        public string Calificacion3Text
        {
            get => _calificacion3Text;
            set { _calificacion3Text = value; OnPropertyChanged(); }
        }

        public string PromedioResultado
        {
            get => _promedioResultado;
            set { _promedioResultado = value; OnPropertyChanged(); }
        }

        public string NivelResultado
        {
            get => _nivelResultado;
            set { _nivelResultado = value; OnPropertyChanged(); }
        }

        public string MensajeError
        {
            get => _mensajeError;
            set { _mensajeError = value; OnPropertyChanged(); }
        }

        public ICommand CalcularPromedioCommand { get; }

        public PromedioViewModel()
        {
            CalcularPromedioCommand = new Command(Calcular);
        }

        private void Calcular()
        {
            // Limpiar estado previo para evitar mostrar resultados anteriores si hay error
            PromedioResultado = string.Empty;
            NivelResultado = string.Empty;
            MensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(Calificacion1Text) ||
                string.IsNullOrWhiteSpace(Calificacion2Text) ||
                string.IsNullOrWhiteSpace(Calificacion3Text))
            {
                MensajeError = "Todos los campos son obligatorios.";
                return;
            }

            if (!double.TryParse(Calificacion1Text, out double c1) ||
                !double.TryParse(Calificacion2Text, out double c2) ||
                !double.TryParse(Calificacion3Text, out double c3))
            {
                MensajeError = "Ingresa únicamente valores numéricos válidos.";
                return;
            }

            if (c1 < 0 || c1 > 10 || c2 < 0 || c2 > 10 || c3 < 0 || c3 > 10)
            {
                MensajeError = "Las calificaciones deben estar entre 0 y 10.";
                return;
            }

            var modelo = new PromedioModel
            {
                Calificacion1 = c1,
                Calificacion2 = c2,
                Calificacion3 = c3
            };

            double promedioExacto = modelo.CalcularPromedio();

            // Determinación de categoría con el promedio completo (sin redondear)
            string nivel;
            if (promedioExacto < 7.0)
                nivel = "No acreditado";
            else if (promedioExacto < 8.0)
                nivel = "Suficiente";
            else if (promedioExacto < 9.0)
                nivel = "Bueno";
            else
                nivel = "Excelente";

            // Formato a dos decimales solo al mostrar
            PromedioResultado = $"Promedio: {promedioExacto:F2}";
            NivelResultado = $"Nivel: {nivel}";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}