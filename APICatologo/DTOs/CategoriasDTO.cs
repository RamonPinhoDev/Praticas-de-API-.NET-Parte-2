using System.ComponentModel.DataAnnotations;

namespace APICatologo.DTOs
{
    public class CategoriasDTO
    {
        public int CategoriaId { get; set; }
        [StringLength(80)]
        public string? Nome { get; set; }
        [StringLength(80)]
        [Required]
        public string? ImagemUrl { get; set; }
    }
}
