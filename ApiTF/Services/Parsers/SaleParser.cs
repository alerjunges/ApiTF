using ApiTF.BaseDados.Models;
using ApiTF.Services.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiTF.Services.Parsers
{
    public class SaleParser
    {
        public static TbSale ToEntity(SaleDTO dto)
        {
            return new TbSale
            {
               Code = dto.Code,
               Createat = System.DateTime.Now,
               Productid = dto.Productid,
               Price = dto.Price,
               Qty = dto.Qty,
               Discount = dto.Discount

            };
        }
    }
}
