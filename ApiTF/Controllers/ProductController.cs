using Microsoft.AspNetCore.Mvc;
using ApiTF.Services.DTOs;
using ApiTF.Services;
using ApiTF.Services.Exceptions;
using ApiTF.BaseDados.Models;
using System;
using AutoMapper;

namespace ApiTF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Adiciona um novo produto.
        /// </summary>
        /// <param name="product">As informações do novo produto.</param>
        /// <returns>Os detalhes do produto criado.</returns>
        /// <response code="200">Produto criado com sucesso</response>
        /// <response code="400">Os dados fornecidos são inválidos.</response>
        /// <response code="422">Campos obrigatórios não foram preenchidos.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPost]
        public ActionResult<TbProduct> Insert(ProductDTO product)
        {
            try
            {
                var entity = _service.Insert(product);
                return Ok(entity);
            }
            catch (InvalidEntityException e)
            {
                return StatusCode(422, new { error = e.Message });
            }
            catch (BadRequestException e)
            {
                return StatusCode(400, new { error = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        /// <summary>
        /// Atualiza um produto existente pelo ID.
        /// </summary>
        /// <param name="id">O ID do produto a ser atualizado.</param>
        /// <param name="dto">Os novos dados do produto.</param>
        /// <returns>Os detalhes do produto atualizado.</returns>
        /// <response code="200">Produto atualizado com sucesso</response>
        /// <response code="400">Os dados fornecidos são inválidos.</response>
        /// <response code="422">Campos obrigatórios não foram preenchidos para a atualização.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPut("{id}")]
        public ActionResult<TbProduct> Update(int id, ProductDTO dto)
        {
            try
            {
                var entity = _service.Update(dto, id);
                return Ok(entity);
            }
            catch (InvalidEntityException e)
            {
                return StatusCode(422, new { error = e.Message });
            }
            catch (BadRequestException e)
            {
                return StatusCode(400, new { error = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        /// <summary>
        /// Atualiza o estoque pelo ID.
        /// </summary>
        /// <param name="qty">Quantidade a ser atualizada.</param>
        /// <param name="productId">ID do produto a ser atualizado.</param>
        /// <returns>O estoque do produto atualizado.</returns>
        /// <response code="200">Estoque atualizado com sucesso</response>
        /// <response code="400">Os dados fornecidos são inválidos.</response>
        /// <response code="422">Campos obrigatórios não foram preenchidos para a atualização.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPut("/ajustestock/{productId}")]
        public ActionResult<TbProduct> AdjustStock(int qty, int productId)
        {
            try
            {
                var entity = _service.AdjustStock(productId, qty);
                return Ok(entity);
            }
            catch (InvalidEntityException e)
            {
                return StatusCode(422, new { error = e.Message });
            }
            catch (BadRequestException e)
            {
                return StatusCode(400, new { error = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        /// <summary>
        /// Obtém um produto pela descrição.
        /// </summary>
        /// <param name="description">A descrição do produto.</param>
        /// <returns>Os detalhes do produto correspondente.</returns>
        /// <response code="200">Produto consultado com sucesso</response>
        /// <response code="422">Campo obrigatório não foi preenchido para a consulta.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet("/description/{description}")]
        public ActionResult<TbProduct> GetByDescription(string description)
        {
            try
            {
                var entity = _service.GetByDescription(description);
                return Ok(entity);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidEntityException e)
            {
                return StatusCode(422, new { error = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        /// <summary>
        /// Obtém um produto pelo código de barras.
        /// </summary>
        /// <param name="barcode">O código de barras do produto.</param>
        /// <returns>Os detalhes do produto correspondente.</returns>
        /// <response code="200">Produto consultado com sucesso</response>
        /// <response code="404">Produto não encontrado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet("/barcode/{barcode}")]
        public ActionResult<TbProduct> GetByBarcode(string barcode)
        {
            try
            {
                var entity = _service.GetByBarcode(barcode);
                return Ok(entity);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}