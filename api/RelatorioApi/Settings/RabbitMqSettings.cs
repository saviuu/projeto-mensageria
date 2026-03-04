namespace RelatorioApi.Settings;

public class RabbitMqSettings
{
    public const string SectionName = "RabbitMq";
    
    public string HostName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}