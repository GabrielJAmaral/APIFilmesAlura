using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dtos;

public class UpdateFilmeDto
{
    [Required]
    [MaxLength(100, ErrorMessage = "Tamanho do título do filme é grande demais")]
    public string Titulo { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Tamanho do gênero é de no máximo 50 caracteres")]
    public string Genero { get; set; }

    [Required]
    [Range(70, 630, ErrorMessage = "Filme deve ter entre 70 à 630 minutos")]
    public int Duracao { get; set; }
}
