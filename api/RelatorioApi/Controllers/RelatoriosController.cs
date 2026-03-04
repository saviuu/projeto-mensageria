using Microsoft.AspNetCore.Mvc;
using RelatorioApi.Data;
using RelatorioApi.Models;
using RelatorioApi.DTOs;
using RelatorioApi.Services;

namespace RelatorioApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RelatoriosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IRabbitMqService _rabbitMqService;

    public RelatoriosController(AppDbContext context, IRabbitMqService rabbitMqService)
    {
        _context = context;
        _rabbitMqService = rabbitMqService;
    }

    [HttpPost]
    public async Task<IActionResult> SolicitarRelatorio([FromBody] SolicitacaoRelatorioDto request)
    {
        var relatorio = new Relatorio
        {
            NomeSolicitante = request.NomeSolicitante
        };

        _context.Relatorios.Add(relatorio);
        await _context.SaveChangesAsync();

        var mensagem = new { RelatorioId = relatorio.Id };
        await _rabbitMqService.PublicarMensagemAsync(mensagem);

        return Accepted(new 
        { 
            Mensagem = "Solicitação recebida e enviada para processamento.",
            relatorio.Id 
        });
    }
}