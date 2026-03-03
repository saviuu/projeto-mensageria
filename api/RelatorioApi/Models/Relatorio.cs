namespace RelatorioApi.Models;

public class Relatorio
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string NomeSolicitante { get; set; } = string.Empty;
    public string Status { get; set; } = "Pendente"; // Status possíveis: Pendente, Processando, Concluido, Erro
    public DateTime DataSolicitacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataConclusao { get; set; }
}