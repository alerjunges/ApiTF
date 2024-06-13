using ApiTF.BaseDados.Models;
using ApiTF.Services.DTOs;

namespace ApiTF.Services.Parsers
{
    public static class PromotionParser
    {
        public static TbPromotion ToEntity(PromotionDTO dto)
        {
            return new TbPromotion
            {
                Id = dto.Id,
                Startdate = dto.Startdate,
                Enddate = dto.Enddate,
                Promotiontype = dto.Promotiontype,
                Productid = dto.Productid,
                Value = dto.Value
            };
        }
    }
}
