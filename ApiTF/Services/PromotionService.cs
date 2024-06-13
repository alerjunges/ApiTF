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
    public class PromotionService
    {
        private readonly TfDbContext _dbContext;
        private readonly IMapper _mapper;

        public PromotionService(TfDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public TbPromotion Insert(PromotionDTO dto)
        {
            if (!PromotionValidate.Validate(dto))
                return null;

            ProductExists(dto.Productid);

            var entity = _mapper.Map<TbPromotion>(dto);

            _dbContext.Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public TbPromotion Update(PromotionDTO dto, int id)
        {
            PromotionExists(id);
            ProductExists(dto.Productid);

            if (!PromotionValidate.Validate(dto))
                return null;

            var existingEntity = GetById(id);

            _mapper.Map(dto, existingEntity);

            _dbContext.Update(existingEntity);
            _dbContext.SaveChanges();

            return existingEntity;
        }

        public TbPromotion GetById(int id)
        {
            var existingEntity = _dbContext.TbPromotions.FirstOrDefault(c => c.Id == id);
            if (existingEntity == null)
            {
                throw new NotFoundException("Registro não existe");
            }
            return existingEntity;
        }

        public IEnumerable<TbPromotion> GetByPromotion(int productId, DateTime startDate, DateTime endDate)
        {
            ValidateDates(startDate, endDate);
            ProductExists(productId);

            var promotion = _dbContext.TbPromotions
                .Where(c => c.Productid == productId && startDate >= c.Startdate && endDate <= c.Enddate)
                .ToList();

            if (!promotion.Any())
            {
                throw new NotFoundException("Nenhuma promoção encontrada para o período especificado.");
            }

            return promotion;
        }
        public List<TbPromotion> GetActivePromotions(int productId)
        {
            ProductExists(productId);

            var currentDate = DateTime.Now;

            var promotion = _dbContext.TbPromotions
                .Where(p => p.Productid == productId
                            && p.Startdate <= currentDate
                            && p.Enddate >= currentDate)
                .OrderBy(p => p.Promotiontype)
                .ToList();

            if (!promotion.Any())
            {
                throw new NotFoundException("Nenhuma promoção ativa encontrada para o produto especificado.");
            }

            return promotion;
        }
        private void ProductExists(int productId)
        {
            var productExists = _dbContext.TbProducts.Any(p => p.Id == productId);
            if (!productExists)
            {
                throw new NotFoundException("Produto não encontrado.");
            }
        }
        private void PromotionExists(int promotionId)
        {
            var promotionExists = GetById(promotionId);
            if (promotionExists == null)
            {
                throw new NotFoundException("Promoção não encontrada.");
            }
        }
        private void ValidateDates(DateTime startDate, DateTime endDate)
        {
            if (startDate == default(DateTime) || endDate == default(DateTime))
            {
                throw new InvalidEntityException("A data de início e a data de fim não podem ser vazias.");
            }
            if (endDate < startDate)
            {
                throw new InvalidEntityException("A data de fim não pode ser anterior à data de início.");
            }
        }
    }
}

