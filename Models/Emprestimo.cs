namespace BibliotecaApi.Models;

public class Emprestimo
{
    public int Id { get; set; }
    public int LivroId { get; set; }
    public Livro Livro { get; set; } = null!;
    public string NomeLeitor { get; set; } = "";
    public DateTime DataEmprestimo { get; set; }
    public DateTime? DataDevolucao { get; set; }
}
