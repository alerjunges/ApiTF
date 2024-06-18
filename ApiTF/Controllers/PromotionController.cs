using ApiTF.Services.DTOs;
using ApiTF.Services;
using ApiTF.Services.Exceptions;
using ApiTF.BaseDados.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace ApiTF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly PromotionService _service;

        public PromotionController(PromotionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Adiciona uma nova promoção.
        /// </summary>
        /// <param name="promotion">As informações da nova promoção.</param>
        /// <returns>Os detalhes do promoção criada.</returns>
        /// <response code="200">Promoção criada com sucesso</response>
        /// <response code="400">Os dados fornecidos são inválidos.</response>
        /// <response code="422">Campos obrigatórios não foram preenchidos.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPost]
        public ActionResult<TbPromotion> Insert(PromotionDTO promotion)
        {
            try
            {
                var entity = _service.Insert(promotion);
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
        /// <param name="id">O ID da promoção a ser atualizado.</param>
        /// <param name="dto">Os novos dados da promoção.</param>
        /// <returns>Os detalhes da promoção atualizado.</returns>
        /// <response code="200">Promoção atualizada com sucesso</response>
        /// <response code="400">Os dados fornecidos são inválidos.</response>
        /// <response code="422">Campos obrigatórios não foram preenchidos para a atualização.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPut("{id}")]
        public ActionResult<TbPromotion> Update(int id, PromotionDTO dto)
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
        /// Obtém as promoções de um produto dentro de um determinado período.
        /// </summary>
        /// <param name="productId">O ID do produto.</param>
        /// <param name="startDate">A data de início do período.</param>
        /// <param name="endDate">A data de término do período.</param>
        /// <returns>Uma lista de promoções para o produto no período especificado.</returns>
        /// <response code="200">Promoção consultada com sucesso</response>
        /// <response code="400">Os dados fornecidos são inválidos.</response>
        /// <response code="404">Indica que o ID do produto não foi encontrado ou nenhuma promoção foi encontrada para o período especificado.</response>
        /// <response code="500">Erro interno do servidor.</response>

        [HttpGet("promotion/{productId}")]
        public ActionResult<TbPromotion> GetByPromotion(int productId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var entity = _service.GetByPromotion(productId, startDate, endDate);
                return Ok(entity);
            }
            catch (InvalidEntityException e)
            {
                return BadRequest(e.Message);
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

