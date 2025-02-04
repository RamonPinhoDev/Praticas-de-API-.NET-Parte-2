using APICatologo.Data;
using APICatologo.DTOs;
using APICatologo.DTOs.Mappins;
using APICatologo.Filter;
using APICatologo.Interfaces;
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
        private readonly IUnitOfWork _uof;

        public CategoriasController(IUnitOfWork uof)
        {
            _uof = uof;
        }



        [HttpGet]
        public  ActionResult<IEnumerable<CategoriasDTO>> Get()
        {

            var cat = _uof.CategoriaRepository.GetAll();

            var catDTO = cat.TocategoriaDtoList();




            return Ok(catDTO);

        }
       
        [HttpGet("{id:int}", Name = "ObterRota")]
        public IActionResult Get2(int id)
        {

          var cat = _uof.CategoriaRepository.Get(c=> c.CategoriaId == id);
           
            var catDTO = cat.ToCategoriaDto();
            
            return Ok(catDTO);

        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriasDTO> Put(int id, CategoriasDTO categoriasDTO)
        {
            var catego = categoriasDTO.ToCategoria();
            

            var cate = _uof.CategoriaRepository.Update(catego);
            _uof.Commit();
            var categs = cate.ToCategoriaDto();
            return Ok(cate);
        }

        [HttpPost]
        public ActionResult<CategoriasDTO> Post(CategoriasDTO categoriasDTO)
        {
           
           var categoria= categoriasDTO.ToCategoria();
            _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();
          
            return new CreatedAtRouteResult("ObterRota", new { Id = categoriasDTO.CategoriaId }, categoriasDTO);

        }


        [HttpDelete("{id:int}")]
        public ActionResult<CategoriasDTO> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);
           _uof.CategoriaRepository.Delete(categoria);
          
            _uof.Commit();
           var categoriasDTO= categoria.ToCategoriaDto();
            return Ok(categoriasDTO);



        }

    }
}
