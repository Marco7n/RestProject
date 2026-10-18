using BibliotecaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Data;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options) { }
    public DbSet<Livro> Livros => Set<Livro>();
    public DbSet<Emprestimo> Emprestimos => Set<Emprestimo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Emprestimo>().HasOne(e => e.Livro)
            .WithMany().HasForeignKey(e => e.LivroId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Emprestimo>().HasIndex(e => e.LivroId)
            .IsUnique().HasFilter("DataDevolucao IS NULL");
    }
}
