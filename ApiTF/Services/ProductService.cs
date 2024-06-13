using ApiTF.BaseDados;
using ApiTF.BaseDados.Models;
using ApiTF.Services.DTOs;
using ApiTF.Services.Exceptions;
using ApiTF.Services.Validate;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiTF.Services
{
    public class ProductService
    {
        private readonly TfDbContext _dbContext;
        private readonly StockLogService _stockLogService;
        private readonly IMapper _mapper;

        public ProductService(TfDbContext dbContext, StockLogService stockLogService, IMapper mapper)
        {
            _dbContext = dbContext;
            _stockLogService = stockLogService;
            _mapper = mapper;
        }

        public TbProduct Insert(ProductDTO dto)
        {
            if (!ProductValidate.IsValid(dto.Barcode, dto.Barcodetype))
            {
                throw new InvalidEntityException("Código de barras inválido.");
            }

            var entity = _mapper.Map<TbProduct>(dto);
            _dbContext.Add(entity);
            _dbContext.SaveChanges();

            _stockLogService.InsertStockLog(new StockLogDTO
            {
                Productid = entity.Id,
                Qty = entity.Stock,
                Createdat = DateTime.Now
            });

            return entity;
        }

        public TbProduct GetByDescription(string description)
        {
            var existingEntity = _dbContext.TbProducts.FirstOrDefault(c => c.Description == description);

            if (existingEntity == null)
            {
                throw new NotFoundException("Registro não existe");
            }
            return existingEntity;
        }

        public TbProduct GetByBarcode(string barcode)
        {
            var existingEntity = _dbContext.TbProducts.FirstOrDefault(c => c.Barcode == barcode);

            if (existingEntity == null)
            {
                throw new NotFoundException("Registro não existe");
            }
            return existingEntity;
        }

        public TbProduct GetById(int id)
        {
            var existingEntity = _dbContext.TbProducts.FirstOrDefault(c => c.Id == id);

            if (existingEntity == null)
            {
                throw new NotFoundException("Registro não existe");
            }
            return existingEntity;
        }

        public TbProduct Update(ProductDTO dto, int id)
        {
            var existingEntity = GetById(id);

            if (existingEntity == null)
            {
                throw new NotFoundException("Registro não existe");
            }

            if (!ProductValidate.IsValid(dto.Barcode, dto.Barcodetype))
            {
                throw new InvalidEntityException("Código de barras inválido.");
            }


            int oldStock = existingEntity.Stock;
            _mapper.Map(dto, existingEntity);
            _dbContext.Update(existingEntity);
            _dbContext.SaveChanges();

            return existingEntity;
        }

        public TbProduct AdjustStock(int productId, int qty)
        {
            var product = GetById(productId);

            if (product == null)
            {
                throw new NotFoundException("Produto não encontrado");
            }
            if (product.Stock + qty < 0)
            {
                throw new InsufficientStockException("Estoque insuficiente para realizar a operação.");
            }
            if (qty == 0)
            {
                throw new InvalidEntityException("A quantidade a atualizar deve ser diferente de zero.");
            }

            int oldStock = product.Stock;
            product.Stock += qty;
            _dbContext.Update(product);
            _dbContext.SaveChanges();

            _stockLogService.InsertStockLog(new StockLogDTO
            {
                Productid = product.Id,
                Qty = product.Stock - oldStock,
                Createdat = DateTime.Now
            });

            return product;
        }
    }
}
