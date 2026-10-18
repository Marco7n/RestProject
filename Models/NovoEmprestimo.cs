using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Models;

public class NovoEmprestimo
{
    [Range(1, int.MaxValue)]
    public int LivroId { get; set; }
    [Required, MaxLength(100)]
    public string NomeLeitor { get; set; } = "";
}
