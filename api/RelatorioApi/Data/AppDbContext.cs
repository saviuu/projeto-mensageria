using Microsoft.EntityFrameworkCore;
using RelatorioApi.Models;

namespace RelatorioApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Relatorio> Relatorios { get; set; }
}