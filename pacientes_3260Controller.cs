using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApiDets.Modelo;
using WebApiDets.Data;

[ApiController]
[Route("api/[controller]")]
public class PacientesmisaelController : ControllerBase
{
    private readonly AppDbContext _context;
    private static int contador = 1;
    private static readonly string[] medicosValidos = { "MED-1010", "MED-2020", "MED-3030", "MED-4040", "MED-5050" };

    public PacientesmisaelController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult RegistrarPaciente([FromBody] Pacientesmisael paciente)
    {
        if (!medicosValidos.Contains(paciente.MedicoResponsable))
            return Unauthorized("Carnet inválido");

        paciente.IdPaciente = $"PAC-2026-{contador:D3}";
        contador++;

        if (paciente.NivelGravedad == 5)
        {
            int criticos = _context.Pacientesmisael.Count(p => p.NivelGravedad == 5 && p.Estado == "En espera");
            if (criticos >= 5)
                return BadRequest("Capacidad máxima alcanzada. Redirección inmediata a otro hospital sugerida");
        }

        paciente.Estado = "En espera";
        _context.Pacientesmisael.Add(paciente);
        _context.SaveChanges();

        return CreatedAtAction(nameof(ObtenerPacientes), new { id = paciente.IdPaciente }, paciente);
    }

    [HttpGet]
    public IActionResult ObtenerPacientes()
    {
        var lista = _context.Pacientesmisael.ToList();

        // Ordenamiento manual (Burbuja)
        for (int i = 0; i < lista.Count - 1; i++)
        {
            for (int j = 0; j < lista.Count - i - 1; j++)
            {
                if (lista[j].NivelGravedad < lista[j + 1].NivelGravedad)
                {
                    var temp = lista[j];
                    lista[j] = lista[j + 1];
                    lista[j + 1] = temp;
                }
            }
        }

        return Ok(lista);
    }
}
