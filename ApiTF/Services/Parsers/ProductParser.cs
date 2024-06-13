using ApiTF.BaseDados.Models;
using ApiTF.Services.DTOs;

namespace ApiTF.Services.Parsers
{
    public class ProductParser
    {
        public static TbProduct ToEntity(ProductDTO dto)
        {
            return new TbProduct
            {
                Description = dto.Description,
                Barcode = dto.Barcode,
                Barcodetype = dto.Barcodetype,
                Stock = dto.Stock,
                Price = dto.Price,
                Costprice = dto.Costprice,
            };
        }
    }
}
