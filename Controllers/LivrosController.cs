using BibliotecaApi.Data;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Controllers;

[ApiController]
[Route("api/livros")]
public class LivrosController : ControllerBase
{
    private readonly BibliotecaContext db;
    public LivrosController(BibliotecaContext db) { this.db = db; }

    [HttpGet]
    public async Task<ActionResult<List<Livro>>> Listar()
    {
        return await db.Livros.AsNoTracking().OrderBy(l => l.Titulo).ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Livro>> Buscar(int id)
    {
        var livro = await db.Livros.FindAsync(id);
        if (livro == null) return NotFound();
        return livro;
    }

    [HttpPost]
    public async Task<ActionResult<Livro>> Cadastrar(Livro livro)
    {
        livro.Id = 0;
        db.Livros.Add(livro);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Buscar), new { id = livro.Id }, livro);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, Livro dados)
    {
        var livro = await db.Livros.FindAsync(id);
        if (livro == null) return NotFound();
        livro.Titulo = dados.Titulo;
        livro.Autor = dados.Autor;
        livro.Ano = dados.Ano;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var livro = await db.Livros.FindAsync(id);
        if (livro == null) return NotFound();
        if (await db.Emprestimos.AnyAsync(e => e.LivroId == id))
            return Conflict(new { mensagem = "O livro possui empréstimos registrados." });
        db.Livros.Remove(livro);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
