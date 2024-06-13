using System;

namespace ApiTF.Services.DTOs
{
    public class StockLogDTO
    {
        /// <summary>
        /// Identificador do produto
        /// </summary>
        public int Productid { get; set; }

        /// <summary>
        /// Quantidade movimentada. Se estiver adicionando, deve ser positivo, se tiver retirando / vendendo, deve ser negativo
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// Data da movimentação
        /// </summary>
        public DateTime Createdat { get; set; }
    }
}
