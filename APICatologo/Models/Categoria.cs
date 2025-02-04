using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatologo.Models;
// com a nova função NET 6 ponto e virgula substitui as chaves do addscopeds
[Table("Categorias")]
public class Categoria
{
    //É  uma boa prática porque é responsabilidade da classe onde você define a propriedade do tipo coleção inicializar essa coleção.
    public Categoria()
    {
        var produto = new Collection<Produto>();
    }
    [Key]
    public int CategoriaId { get; set; }
    [StringLength(80)]
    public string? Nome { get; set; }
    [StringLength(80)]
    [Required]
    public string? ImagemUrl { get; set; }
    [JsonIgnore]
    public IEnumerable<Produto> Produtos { get; set; }

    
}
