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
            var productExists = _dbContext.TbProducts.Any(p => p.Id == productId);

            if (!productExists)
            {
                throw new NotFoundException("Produto não encontrado.");
            }

            var logs = from log in _dbContext.TbStockLogs
                       where log.Productid == productId
                       select new StockLogResultDTO
                       {
                           Date = log.Createdat,
                           Barcode = log.Product.Barcode,
                           Description = log.Product.Description,
                           Quantity = log.Qty,
                           Product = new ProductDetailsDTO
                           {
                               Id = log.Product.Id,
                               Description = log.Product.Description,
                               Barcode = log.Product.Barcode,
                               BarcodeType = log.Product.Barcodetype,
                               Stock = log.Product.Stock,
                               Price = log.Product.Price,
                               CostPrice = log.Product.Costprice
                           }
                       };

            if (!logs.Any())
            {
                throw new NotFoundException("Nenhum log encontrado para o produto.");
            }

            return logs.ToList();
        }
    }
}
