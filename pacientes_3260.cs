namespace WebApiDets.Modelo
{
    public class Pacientesmisael
    {
        public string IdPaciente { get; set; } // PAC-2026-XXX
        public int NivelGravedad { get; set; } // 1 a 5
        public string Estado { get; set; } // En espera, Atendido, Derivado
        public string MedicoResponsable { get; set; } // Carnet
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
    }
}
