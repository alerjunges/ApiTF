using ApiTF.BaseDados.Models;
using ApiTF.Services.DTOs;
using ApiTF.Services.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiTF.Services
{
    public class StockLogService
    {
        private readonly TfDbContext _dbContext;
        private readonly IMapper _mapper;

        public StockLogService(TfDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public TbStockLog InsertStockLog(StockLogDTO dto)
        {
            var entity = _mapper.Map<TbStockLog>(dto);
            _dbContext.TbStockLogs.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public List<StockLogResultDTO> GetStockLogByProductId(int productId)
        {
            var product = _dbContext.TbProducts
                .Include(p => p.TbStockLogs)
                .FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                throw new NotFoundException("Produto não encontrado.");
            }

            var logs = product.TbStockLogs.Select(log => new StockLogResultDTO
            {
                Date = log.Createdat,
                Barcode = product.Barcode,
                Description = product.Description,
                Quantity = log.Qty,
                Product = new ProductDetailsDTO
                {
                    Id = product.Id,
                    Description = product.Description,
                    Barcode = product.Barcode,
                    BarcodeType = product.Barcodetype,
                    Stock = product.Stock,
                    Price = product.Price,
                    CostPrice = product.Costprice
                }
            }).ToList();

            if (!logs.Any())
            {
                throw new NotFoundException("Nenhum log encontrado para o produto.");
            }

            return logs;
        }
    }
}
