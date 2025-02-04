using APICatologo.Data;
using APICatologo.DTOs;
using APICatologo.DTOs.Mappins;
using APICatologo.Interfaces;
using APICatologo.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

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


    [HttpGet("GetCategorias/{id:int}", Name = "produtosId")]
    public ActionResult<IEnumerable<ProdutosDTO>> GetProdutos(int id)
    {
        var pro = _repositoryProduto.GetProdutosPorCategoria(id);
        return Ok(pro);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProdutosDTO>> Get()
    {
        var produtos = _uof.ProdutosRepository.GetAll();
        if (produtos  == null) { return BadRequest(); }
       var  produtosDTO = _mapper.Map<IEnumerable<ProdutosDTO>>(produtos);
        return Ok(produtosDTO);

    }

    [HttpPatch]
    public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
    {
        if (patchProdutoDTO == null) 
        { return NotFound(); }

        var produto = _uof.ProdutosRepository.Get(p=> p.ProdutoId == id);

        var produdoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);
        patchProdutoDTO.ApplyTo(produdoUpdateRequest, ModelState);

        if (!ModelState.IsValid /*|| TryValidateModel(produdoUpdateRequest)*/)
        { 
            return BadRequest(ModelState);
        }

        _mapper.Map(produdoUpdateRequest, produto);

        _uof.ProdutosRepository.Update(produto);
        _uof.Commit();

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));

    }



    [HttpGet("GetProduto/{id:int}", Name = "ObterProduto")]

    public ActionResult<ProdutosDTO> GetById(int id)   
    {
        var produtos = _uof.ProdutosRepository.Get(p=> p.ProdutoId == id);
        var produtosDTO = _mapper.Map<ProdutosDTO>(produtos);
         return Ok  (produtosDTO);    
    }

    [HttpPost]
    public ActionResult<ProdutosDTO> Post(ProdutosDTO produtoDTO)
    {
        var produto = _mapper.Map<Produto>(produtoDTO);
      var novoproduto = _uof.ProdutosRepository.Create(produto);
        _uof.Commit();
        var produtocinvertido = _mapper.Map<Produto>(novoproduto);
        return new CreatedAtRouteResult("ObterProduto", new {Id= novoproduto.ProdutoId}, produtocinvertido);

    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutosDTO> Put(int id, ProdutosDTO produtoDTO)
    {
        var produto = _mapper.Map<Produto>(produtoDTO);
        var pro = _uof.ProdutosRepository.Update(produto);
      //var atualizado =  _repository.Update(produto);
      //  if (atualizado) {
      //  return Ok();
      //  }
      //  else {return StatusCode(500, "Nao encontrado!"); }
      _uof.Commit();
        var proDTO = _mapper.Map<ProdutosDTO>(pro);

        return Ok(proDTO);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutosDTO> Delete(int id)
    {
        var pro = _uof.ProdutosRepository.Get(p=> p.ProdutoId == id);
        _uof.ProdutosRepository.Delete(pro);
        _uof.Commit();
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