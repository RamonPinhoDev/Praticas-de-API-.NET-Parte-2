using APICatologo.Data;
using APICatologo.DTOs;
using APICatologo.DTOs.Mappins;
using APICatologo.Interfaces;
using APICatologo.Models;
using APICatologo.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

namespace APICatologo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly  IProdutosRepository _repositoryProduto;
    private readonly IUnitOfWork _uof;

    private readonly IMapper _mapper;



    public ProdutosController(IProdutosRepository repositoryProduto, IUnitOfWork uof, IMapper  mapper)
    {

        _repositoryProduto = repositoryProduto;
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet("filter/preço/pagination")]
    public async Task< ActionResult<IEnumerable<ProdutosDTO>>> GeprodutoPrecoFiltro([FromQuery] ProdutosFiltroPreco produtosFiltroPreçoParamters)
    {var produtos = await _repositoryProduto.GetProdutosFiltroPrecoAsync(produtosFiltroPreçoParamters);

        var metadados = new { produtos.TotalCount,
        produtos.PageSize,
            produtos.CurrentPage,
            produtos.TotalPages,
            produtos.HasNext,
            produtos.HasPrevious
        };Response.Headers.Append("X-pagnation", JsonConvert.SerializeObject(metadados));

        var produtosDTO = _mapper.Map<IEnumerable<ProdutosDTO>>(produtos);
        return Ok(produtosDTO);
    }
    [HttpGet("Pagination")]
    public async Task<ActionResult<IEnumerable<ProdutosDTO>>> Get([FromQuery] ProdutosParametrs paramters)
        {
        var produtos = await _uof.ProdutosRepository.GetProdutosAsync(paramters);
        var metaDados = new 
        {
            produtos.TotalCount,
            produtos.PageSize,
            produtos.CurrentPage,
            produtos.TotalPages,
            produtos.HasNext,
            produtos.HasPrevious,
        };
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaDados));
        var ProdutosDTO = _mapper.Map<IEnumerable<ProdutosDTO>>(produtos);
        return Ok(ProdutosDTO);
        }

    [HttpGet("GetCategorias/{id:int}", Name = "produtosId")]
    public async Task<ActionResult<IEnumerable<ProdutosDTO>>> GetProdutos(int id)
    {
        var pro = await _repositoryProduto.GetProdutosPorCategoriaAsync(id);
        return Ok(pro);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutosDTO>>> Get()
    {
        var produtos = await _uof.ProdutosRepository.GetAllAsync();
        if (produtos  == null) { return BadRequest(); }
       var  produtosDTO = _mapper.Map<IEnumerable<ProdutosDTO>>(produtos);
        return Ok(produtosDTO);

    }

    [HttpPatch]
    public async Task<ActionResult<ProdutoDTOUpdateResponse>> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
    {
        if (patchProdutoDTO == null) 
        { return NotFound(); }

        var produto = await _uof.ProdutosRepository.GetAsync(p=> p.ProdutoId == id);

        var produdoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);
        patchProdutoDTO.ApplyTo(produdoUpdateRequest, ModelState);

        if (!ModelState.IsValid /*|| TryValidateModel(produdoUpdateRequest)*/)
        { 
            return BadRequest(ModelState);
        }

        _mapper.Map(produdoUpdateRequest, produto);

        _uof.ProdutosRepository.Update(produto);
        _uof.CommitAsync();

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));

    }



    [HttpGet("GetProduto/{id:int}", Name = "ObterProduto")]

    public async Task<ActionResult<ProdutosDTO> >GetById(int id)   
    {
        var produtos = await _uof.ProdutosRepository.GetAsync(p=> p.ProdutoId == id);
        var produtosDTO = _mapper.Map<ProdutosDTO>(produtos);
         return Ok  (produtosDTO);    
    }

    [HttpPost]
    public async Task<ActionResult<ProdutosDTO> >Post(ProdutosDTO produtoDTO)
    {
        var produto = _mapper.Map<Produto>(produtoDTO);
      var novoproduto = _uof.ProdutosRepository.Create(produto);
        _uof.CommitAsync();
        var produtocinvertido = _mapper.Map<Produto>(novoproduto);
        return new CreatedAtRouteResult("ObterProduto", new {Id= novoproduto.ProdutoId}, produtocinvertido);

    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProdutosDTO> >Put(int id, ProdutosDTO produtoDTO)
    {
        var produto = _mapper.Map<Produto>(produtoDTO);
        var pro =  _uof.ProdutosRepository.Update(produto);
      //var atualizado =  _repository.Update(produto);
      //  if (atualizado) {
      //  return Ok();
      //  }
      //  else {return StatusCode(500, "Nao encontrado!"); }
      _uof.CommitAsync();
        var proDTO = _mapper.Map<ProdutosDTO>(pro);

        return Ok(proDTO);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProdutosDTO>> Delete(int id)
    {
        var pro =  await _uof.ProdutosRepository.GetAsync(p=> p.ProdutoId == id);
        _uof.ProdutosRepository.Delete(pro);
        _uof.CommitAsync();
        var proDTO = _mapper.Map<ProdutosDTO>(pro);
        return Ok(proDTO);
       //var produto = _repository.Delete(Id);
       // if (produto)
       // {
       //     return Ok(produto);
       // }
       // else {
       //     return StatusCode(500, "Nao encontrado!");
       // }
    }
}