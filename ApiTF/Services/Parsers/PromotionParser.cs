using System;
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
                Startdate = DateTime.SpecifyKind(dto.Startdate, DateTimeKind.Unspecified),
                Enddate = DateTime.SpecifyKind(dto.Enddate, DateTimeKind.Unspecified),
                Promotiontype = dto.Promotiontype,
                Productid = dto.Productid,
                Value = dto.Value
            };
        }
    }
}
