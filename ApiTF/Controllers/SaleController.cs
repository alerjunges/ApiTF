using ApiTF.BaseDados.Models;
using ApiTF.Services;
using ApiTF.Services.DTOs;
using ApiTF.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApiTF.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly SalesService _service;

        public SalesController(SalesService service)
        {
            _service = service;
        }

        /// <summary>
        /// Insere uma nova venda.
        /// </summary>
        /// <param name="sale">A lista de dados da venda.</param>
        /// <returns>Os detalhes da venda registrada.</returns>
        /// <response code="200">Venda criada com sucesso</response>
        /// <response code="400">Erro de validação nos dados da venda ou que o estoque é insuficiente.</response>
        /// <response code="404">Venda não encontrada.</response>
        /// <response code="500">Erro interno do servidor.</response>

        [HttpPost]
        public ActionResult<TbSale> Insert(InsertSaleDTO sale)
        {
            try
            {
                var entity = _service.Insert(sale);
                return Ok(entity);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidEntityException e)
            {
                return BadRequest(e.Message);
            }
            catch (InsufficientStockException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }


        /// <summary>
        /// Obtém uma venda pelo código.
        /// </summary>
        /// <param name="code">O código da venda a ser obtida.</param>
        /// <returns>A venda solicitada.</returns>
        /// <response code="200">Venda consultada com sucesso</response>
        /// <response code="404">Venda não encontrada.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet("{code}")]
        public IActionResult GetByCode(string code)
        {
            try
            {
                var sale = _service.GetByCode(code);
                return Ok(sale);
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

        /// <summary>
        /// Obtém um relatório de vendas por período.
        /// </summary>
        /// <param name="startDate">A data de início do período.</param>
        /// <param name="endDate">A data de fim do período.</param>
        /// <returns>Uma lista de relatórios de vendas agrupados por código da venda.</returns>
        /// <response code="200">Venda consultada com sucesso</response>
        /// <response code="400">Os dados fornecidos são inválidos.</response>
        /// <response code="404">Venda não encontrada.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet("report")]
        public ActionResult<List<SalesReportDTO>> GenerateSaleReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                var report = _service.GetSalesReport(startDate, endDate);
                return Ok(report);
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
