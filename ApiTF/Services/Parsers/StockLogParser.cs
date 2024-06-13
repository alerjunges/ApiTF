using ApiTF.BaseDados.Models;
using ApiTF.Services.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiTF.Services.Parsers
{
    public class StockLogParser
    {
        public static TbStockLog ToEntity(StockLogDTO dto)
        {
            return new TbStockLog
            {
                Productid = dto.Productid,
                Qty = dto.Qty,
                Createdat = System.DateTime.Now
            };
        }
    }
}
