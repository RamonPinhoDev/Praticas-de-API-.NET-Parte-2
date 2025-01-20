using APICatologo.Data;
using APICatologo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICatologo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

   

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _context.Produtos.AsNoTracking().ToList();
        if (produtos == null)
        {
            return NotFound("Produtos não encontrados");
        }
        return produtos;

    }
    //[HttpGet("PrimeiroProduto")]
    //[HttpGet("Teste")]
    //[HttpGet("/PrimeiroProduto")]
    [HttpGet("{name:alpha:length(5)}")]

    public ActionResult<Produto> GetPrimeiro()
    {
        var produtos = _context.Produtos.FirstOrDefault();
        if (produtos == null)
        {
            return NotFound("Produtos não encontrados");
        }
        return produtos;

    }
    //[HttpGet("{Id}/{Params=Ramon}", Name ="ObterProduto")]

    [HttpGet("{Id:int:min(1)}", Name = "ObterProduto")]

    public async Task<ActionResult<Produto>> GetAsync(int Id, [BindRequired] string Ramon)   
    {
        var r = Ramon;
        var produtos = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == Id);
        if(produtos == null)
        {
            return NotFound("Produto não encontrado");
        }

         return Ok  (produtos);    
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto == null)
        {
            return BadRequest();
        }
        _context.Produtos.Add(produto);
        _context.SaveChanges();
        return new CreatedAtRouteResult("Obterproduto", new {Id=produto.ProdutoId}, produto);

    }

    [HttpPut("{Id:int}")]
    public ActionResult Put(int Id, Produto produto)
    { 
        if(produto == null)
        {
            return BadRequest();
        }

        _context.Produtos.Entry(produto).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok();
    }

    [HttpDelete("{Id:int}")]
    public ActionResult Delete(int Id)
    {
        var produto = _context.Produtos.FirstOrDefault(p=> p.ProdutoId == Id);
        if(produto == null)
        {
            return NotFound();

        }

        _context.Remove(produto);
        _context.SaveChanges();

        return Ok(produto);
    }
}