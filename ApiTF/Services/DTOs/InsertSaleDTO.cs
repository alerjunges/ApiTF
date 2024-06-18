using System;

namespace ApiTF.Services.DTOs
{
    public class InsertSaleDTO
    {
        /// <summary>
        /// Código da venda (Um código único da venda, onde todos os items de uma venda, terão o mesmo código). Deve ser uma chave guid.
        /// </summary>
        public string Code { get; set; }


        /// <summary>
        /// Código do produto
        /// </summary>
        public int Productid { get; set; }


        /// <summary>
        /// Quantidade vendida
        /// </summary>
        public int Qty { get; set; }

    }
}
