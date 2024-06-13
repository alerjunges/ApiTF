using ApiTF.BaseDados.Models;
using ApiTF.Services;
using ApiTF.Services.DTOs;
using ApiTF.Services.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiTF.Services
{
    public class SalesService
    {
        private readonly TfDbContext _dbContext;
        private readonly ProductService _productService;
        private readonly PromotionService _promotionService;
        private readonly StockLogService _stockLogService;
        private readonly IMapper _mapper;

        public SalesService(TfDbContext dbContext, ProductService productService, PromotionService promotionService, StockLogService stockLogService, IMapper mapper)
        {
            _dbContext = dbContext;
            _productService = productService;
            _promotionService = promotionService;
            _stockLogService = stockLogService;
            _mapper = mapper;
        }

        public IEnumerable<TbSale> Insert(List<SaleDTO> dtoList)
        {
            var sales = new List<TbSale>();
            var currentTime = DateTime.Now;
            var code = Guid.NewGuid().ToString();

            foreach (var dto in dtoList)
            {
                ValidateProduct(dto.Productid, dto.Qty);

                var promotions = _promotionService.GetActivePromotions(dto.Productid);

                var product = _productService.GetById(dto.Productid);
                decimal unitPrice = product.Price;
                decimal originalPrice = unitPrice;
                foreach (var promotion in promotions)
                {
                    unitPrice = ApplyPromotion(unitPrice, promotion);
                }

                decimal totalDiscount = originalPrice - unitPrice;

                AdjustStockAndLog(dto.Productid, dto.Qty);

                var sale = _mapper.Map<TbSale>(dto);
                sale.Code = code;
                sale.Price = unitPrice;
                sale.Discount = totalDiscount;
                sale.Createat = currentTime;

                _dbContext.Add(sale);
                sales.Add(sale);
            }

            _dbContext.SaveChanges();

            return sales;
        }

        public TbSale GetByCode(string code)
        {
            var existingEntity = _dbContext.TbSales.FirstOrDefault(c => c.Code == code);
            if (existingEntity == null)
            {
                throw new NotFoundException("Nenhuma venda encontrada para este código");
            }
            return existingEntity;
        }

        public List<SalesReportDTO> GenerateSalesReport(DateTime startDate, DateTime endDate)
        {
            ValidateReportDates(startDate, endDate);

            var query = GetSalesReport(startDate, endDate);

            if (!query.Any())
            {
                throw new NotFoundException("Nenhuma venda encontrada para o período.");
            }

            return query.ToList();
        }

        private IEnumerable<SalesReportDTO> GetSalesReport(DateTime startDate, DateTime endDate)
        {
            var query = from sale in _dbContext.TbSales
                        join product in _dbContext.TbProducts on sale.Productid equals product.Id
                        where sale.Createat >= startDate && sale.Createat < endDate.AddDays(1)
                        select new SalesReportDTO
                        {
                            SaleCode = sale.Code,
                            ProductDescription = product.Description,
                            Price = sale.Price,
                            Quantity = sale.Qty,
                            SaleDate = sale.Createat
                        };
            return query;
        }

        private decimal ApplyPromotion(decimal price, TbPromotion promotion)
        {
            return promotion.Promotiontype switch
            {
                0 => price * (1 - promotion.Value / 100),
                1 => price - promotion.Value,
                _ => price,
            };
        }

        private void AdjustStockAndLog(int productId, int quantity)
        {
            var product = _productService.GetById(productId);
            if (product == null)
            {
                throw new NotFoundException("Produto não existe");
            }

            if (product.Stock < quantity)
            {
                throw new InsufficientStockException("Estoque insuficiente para a movimentação");
            }

            _productService.AdjustStock(productId, -quantity);

            var stockLogDto = new StockLogDTO
            {
                Productid = productId,
                Qty = -quantity,
                Createdat = DateTime.Now
            };
            _stockLogService.InsertStockLog(stockLogDto);
        }

        private void ValidateProduct(int productId, int quantity)
        {
            var product = _productService.GetById(productId);
            if (product == null)
            {
                throw new NotFoundException("Produto não existe");
            }

            if (product.Stock < quantity)
            {
                throw new InsufficientStockException("Estoque insuficiente para a movimentação");
            }
        }

        private void ValidateReportDates(DateTime startDate, DateTime endDate)
        {
            if (startDate == default || endDate == default)
            {
                throw new BadRequestException("As datas de início e fim são obrigatórias.");
            }
            if (endDate < startDate)
            {
                throw new InvalidEntityException("A data de fim não pode ser anterior à data de início.");
            }
        }
    }
}
