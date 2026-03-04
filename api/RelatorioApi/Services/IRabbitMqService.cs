namespace RelatorioApi.Services;

public interface IRabbitMqService
{
    public Task PublicarMensagemAsync(object mensagem);
}