using AutoMapper;
using ApiTF.BaseDados.Models;
using ApiTF.Services.DTOs;

namespace ApiTF.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDTO, TbProduct>().ReverseMap();
            CreateMap<PromotionDTO, TbPromotion>().ReverseMap();
            CreateMap<SaleDTO, TbSale>().ReverseMap();
            CreateMap<StockLogDTO, TbStockLog>().ReverseMap();
            CreateMap<InsertSaleDTO, TbSale>().ReverseMap();
            //CreateMap<SaleReportDTO, SaleDTO>();

        }
    }
}