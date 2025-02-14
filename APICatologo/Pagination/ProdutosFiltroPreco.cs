namespace APICatologo.Pagination
{
    public class ProdutosFiltroPreco : QueryStringParamters
    {
        public decimal ? Preco { get; set; }
        public string? PrecoCriterio {  get; set; }
    }
}
