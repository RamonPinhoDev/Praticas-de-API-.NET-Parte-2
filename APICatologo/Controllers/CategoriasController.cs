using APICatologo.Data;
using APICatologo.Filter;
using APICatologo.Models;
using APICatologo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatologo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

    
        private readonly ILogger _logger;
        public CategoriasController(AppDbContext context, IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }


        [HttpGet("Configuracoes")]
        public string PegarChavesDeConfiguracoes()
        {
            var chave1 = _configuration["chave1"];
            var chave2 = _configuration["chave2"];
            var secao1 = _configuration["secao1:chave2"];

            return $"chave1: {chave1}, chave2: {chave2}, secao1:{secao1}";
        }
        //forma antiga de se usar from services
        [HttpGet("ComFromService/{nome}")]
        public string GetComFromServices([FromServices] IMeuServoco meuServoco, string Ramon)
        {


            return meuServoco.Saudacao(Ramon);
        }

        //forma mais atual de se usar from services
        [HttpGet("SemfromServices/ {nome}")]
        public string GetSemFromservices(IMeuServoco meuServoco, string Ramon)
        {

            return meuServoco.Saudacao(Ramon);
        }
        [HttpGet]

        [ServiceFilter(typeof(ApiLogginFilter))]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get()
        {

            return await _context.Categorias.AsNoTracking().ToListAsync();


        }
        [HttpGet("Produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCat()
        {
            _logger.LogInformation("================= api=======Produtos");
            try
            {
                throw new DataMisalignedException();
                //return _context.Categorias.Include(c => c.Produtos).ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um priblema ao tratar a sua solicitação");
                //return StatusCode(500,"Ocorreu um priblema ao tratar a sua solicitação");

            }
        }
        [HttpGet("{id:int}", Name = "ObterRota")]
        public IActionResult Get(int id)
        {
           // _logger.LogInformation($"=========== api=======Produtos id: {id}");
            throw new Exception("Error ao retornar categoria pelo id");
            //string[] teste = null;
            //if (teste.Length > 0)
            //{

            //}
            //try
            //{
            //    var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
            //    if (categoria == null)
            //    {
            //        _logger.LogInformation("================= api=======Deu errado os planos");
            //        return BadRequest($"id: {id} nao encontrado");
                   
            //    }
            //    return Ok(categoria);
            //}
            //catch (Exception)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);

            //}




        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (categoria == null) { return BadRequest(); }
            _context.Categorias.Entry(categoria).State = EntityState.Modified;

            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return new CreatedAtRouteResult("ObterRota", new { Id = categoria.CategoriaId }, categoria);

        }


        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {

            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
            if (categoria != null) { return NotFound(); }
            _context.Remove(categoria);


            _context.SaveChanges();
            return Ok(categoria);



        }

    }
}
