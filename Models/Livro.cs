using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Models;

public class Livro
{
    public int Id { get; set; }
    [Required, MaxLength(150)]
    public string Titulo { get; set; } = "";
    [Required, MaxLength(100)]
    public string Autor { get; set; } = "";
    [Range(1, 9999)]
    public int Ano { get; set; }
}
