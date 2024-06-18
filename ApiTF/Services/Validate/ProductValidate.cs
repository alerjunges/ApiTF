using System.Linq;

namespace ApiTF.Services.Validate
{
    public class ProductValidate
    {
        public static bool IsValid(string barcode, string barcodeType)
        {
            // Validar código de barras com base no tipo
            switch (barcodeType.ToUpper())
            {
                case "EAN-13":
                    return ValidateEAN13(barcode);
                case "DUN-14":
                    return ValidateDUN14(barcode);
                case "UPC":
                    return ValidateUPC(barcode);
                case "CODE 11":
                    return ValidateCode11(barcode);
                case "CODE 39":
                    return ValidateCode39(barcode);
                default:
                    return false;
            }
        }

        private static bool ValidateEAN13(string barcode)
        {
            // Implementar validação para EAN-13
            return barcode.Length == 13 && long.TryParse(barcode, out _);
        }

        private static bool ValidateDUN14(string barcode)
        {
            // Implementar validação para DUN-14
            return barcode.Length == 14 && long.TryParse(barcode, out _);
        }

        private static bool ValidateUPC(string barcode)
        {
            // Implementar validação para UPC
            return barcode.Length == 12 && long.TryParse(barcode, out _);
        }

        private static bool ValidateCode11(string barcode)
        {
            // Implementar validação para CODE 11
            return barcode.All(c => char.IsDigit(c) || c == '-' || c == '*');
        }

        private static bool ValidateCode39(string barcode)
        {
            // Implementar validação para CODE 39
            return barcode.All(c => char.IsLetterOrDigit(c) || "-.$/+% ".Contains(c));
        }
    }
}

