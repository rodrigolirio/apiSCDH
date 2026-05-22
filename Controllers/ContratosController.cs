using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCDH.Data;
using SCDH.Models;

namespace SCDH.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContratosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ContratosController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(
        [FromForm] string numeroContrato,
        [FromForm] string cpfCliente,
        [FromForm] decimal valorImovel,
        IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0)
            return BadRequest("Arquivo é obrigatório.");

        if (arquivo.Length > 5 * 1024 * 1024)
            return BadRequest("Arquivo excede 5MB.");

        if (Path.GetExtension(arquivo.FileName).ToLower() != ".pdf")
            return BadRequest("Apenas PDF é permitido.");

        var pastaDestino = Path.Combine(_env.WebRootPath, "documentos");
        Directory.CreateDirectory(pastaDestino);

        var nomeArquivo = $"{Guid.NewGuid()}.pdf";
        var caminhoCompleto = Path.Combine(pastaDestino, nomeArquivo);

        using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }

        var contrato = new ContratoHabitacional
        {
            Id = Guid.NewGuid(),
            NumeroContrato = numeroContrato,
            CpfCliente = cpfCliente,
            ValorImovel = valorImovel,
            CaminhoArquivo = caminhoCompleto
        };

        _context.Contratos.Add(contrato);
        await _context.SaveChangesAsync();

        return Ok(contrato);
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var contratos = await _context.Contratos.ToListAsync();
        return Ok(contratos);
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> Download(Guid id)
    {
        var contrato = await _context.Contratos.FindAsync(id);
        if (contrato == null) return NotFound();

        if (!System.IO.File.Exists(contrato.CaminhoArquivo))
            return NotFound("Arquivo físico não encontrado.");

        var bytes = await System.IO.File.ReadAllBytesAsync(contrato.CaminhoArquivo);
        return File(bytes, "application/pdf", $"{contrato.NumeroContrato}.pdf");
    }
}