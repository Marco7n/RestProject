using BibliotecaApi.Data;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Controllers;

[ApiController]
[Route("api/emprestimos")]
public class EmprestimosController : ControllerBase
{
    private readonly BibliotecaContext db;
    public EmprestimosController(BibliotecaContext db) { this.db = db; }

    [HttpGet]
    public async Task<ActionResult<List<Emprestimo>>> Listar()
    {
        return await db.Emprestimos.AsNoTracking().Include(e => e.Livro)
            .OrderByDescending(e => e.Id).ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Emprestimo>> Buscar(int id)
    {
        var emprestimo = await db.Emprestimos.Include(e => e.Livro)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (emprestimo == null) return NotFound();
        return emprestimo;
    }

    [HttpPost]
    public async Task<ActionResult<Emprestimo>> Cadastrar(NovoEmprestimo dados)
    {
        var livro = await db.Livros.FindAsync(dados.LivroId);
        if (livro == null) return NotFound(new { mensagem = "Livro não encontrado." });
        if (await db.Emprestimos.AnyAsync(e => e.LivroId == dados.LivroId && e.DataDevolucao == null))
            return Conflict(new { mensagem = "O livro já está emprestado." });
        var emprestimo = new Emprestimo
        {
            LivroId = dados.LivroId,
            Livro = livro,
            NomeLeitor = dados.NomeLeitor,
            DataEmprestimo = DateTime.UtcNow
        };
        db.Emprestimos.Add(emprestimo);
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException erro) when (erro.InnerException is SqliteException { SqliteExtendedErrorCode: 2067 })
        {
            return Conflict(new { mensagem = "O livro já está emprestado." });
        }
        return CreatedAtAction(nameof(Buscar), new { id = emprestimo.Id }, emprestimo);
    }

    [HttpPut("{id:int}/devolucao")]
    public async Task<IActionResult> Devolver(int id)
    {
        var emprestimo = await db.Emprestimos.FindAsync(id);
        if (emprestimo == null) return NotFound();
        if (emprestimo.DataDevolucao == null)
        {
            emprestimo.DataDevolucao = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
        return NoContent();
    }
}
