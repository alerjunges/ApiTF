using ApiTF.Services.DTOs;
using ApiTF.Services.Exceptions;

namespace ApiTF.Services.Validate
{
    public static class PromotionValidate
    {
        public static bool Validate(PromotionDTO promotionDTO)
        {
            if (promotionDTO.Startdate > promotionDTO.Enddate)
            {
                throw new InvalidEntityException("A data de início da promoção não pode ser posterior ao término.");
            }

            if (promotionDTO.Promotiontype < 0 || promotionDTO.Promotiontype > 1)
            {
                throw new InvalidEntityException("Tipo de promoção inválido.");
            }

            if (promotionDTO.Productid <= 0)
            {
                throw new InvalidEntityException("O ID do produto associado à promoção é obrigatório.");
            }

            if (promotionDTO.Value <= 0)
            {
                throw new InvalidEntityException("O valor da promoção deve ser maior que zero.");
            }

            return true;
        }
    }
}
