using Microsoft.EntityFrameworkCore;
using MinimalApiProject.Models;

public class AppDbContext : DbContext
{
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Tarefa> Tarefa { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=teste.db");
    }
}