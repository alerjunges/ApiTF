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

        public TbSale Insert(InsertSaleDTO dto)
        {
            var product = _productService.GetById(dto.Productid);

            if (product == null)
            {
                throw new NotFoundException("Produto não existe");
            }

            if (product.Stock < dto.Qty)
            {
                throw new InsufficientStockException("Estoque insuficiente para a movimentação");
            }

            var activePromotions = _promotionService.GetActivePromotions(dto.Productid);

            // Calculate the final price after applying promotions
            decimal unitPrice = product.Price;
            decimal discountedPrice = unitPrice;
            foreach (var promotion in activePromotions)
            {
                discountedPrice = ApplyPromotion(discountedPrice, promotion);
            }

            decimal totalOriginalPrice = unitPrice;
            decimal totalDiscountedPrice = discountedPrice;
            decimal totalDiscount = totalOriginalPrice - totalDiscountedPrice;

            var sale = _mapper.Map<TbSale>(dto);
            sale.Price = totalOriginalPrice; // The original total price
            sale.Discount = totalDiscount; // Total discount applied
            sale.Createat = DateTime.Now;

            product.Stock -= dto.Qty;

            _dbContext.Entry(product).State = EntityState.Modified;
            _dbContext.TbSales.Add(sale);
            _dbContext.SaveChanges();

            _stockLogService.InsertStockLog(new StockLogDTO
            {
                Productid = sale.Productid,
                Qty = -sale.Qty,
                Createdat = DateTime.Now
            });

            return sale;
        }

        private decimal CalculateFinalPrice(decimal basePrice, IEnumerable<TbPromotion> promotions, int quantity)
        {
            decimal discountedPrice = basePrice;
            foreach (var promo in promotions)
            {
                discountedPrice = ApplyPromotion(discountedPrice, promo);
            }
            return discountedPrice * quantity;
        }

        public TbSale GetByCode(string code)
        {
            var existingEntity = _dbContext.TbSales.Include(c => c.Product).FirstOrDefault(c => c.Code == code);
            if (existingEntity == null)
            {
                throw new NotFoundException("Nenhuma venda encontrada para este código");
            }
            return existingEntity;
        }

        public List<SalesReportDTO> GetSalesReport(DateTime startDate, DateTime endDate)
        {
            ValidateReportDates(startDate, endDate);

            var sales = _dbContext.TbSales
                .Include(s => s.Product)
                .Where(s => s.Createat >= startDate && s.Createat < endDate.AddDays(1))
                .ToList();

            if (!sales.Any())
            {
                throw new NotFoundException("Nenhuma venda encontrada para o período.");
            }

            var report = sales.Select(sale => new SalesReportDTO
            {
                SaleCode = sale.Code,
                ProductDescription = sale.Product.Description,
                Price = sale.Price,
                Quantity = sale.Qty,
                SaleDate = sale.Createat
            }).ToList();

            return report;
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
