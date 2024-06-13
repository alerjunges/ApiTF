using ApiTF.BaseDados.Models;
using ApiTF.Services;
using ApiTF.Services.DTOs;
using ApiTF.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApiTF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockLogController : ControllerBase
    {
        private readonly StockLogService _service;

        public StockLogController(StockLogService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtém os logs de um determinado produto.
        /// </summary>
        /// <param name="productId">O ID do produto a ser consultado.</param>
        /// <returns>A lista de logs do produto.</returns>
        /// <response code="200">Logs do produto consultado com sucesso.</response>
        /// <response code="404">Logs do produto não encontrados.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet("{productId}/logs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StockLogResultDTO>> GetLogByProductId(int productId)
        {
            try
            {
                var logs = _service.GetStockLogByProductId(productId);
                return Ok(logs);
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}
