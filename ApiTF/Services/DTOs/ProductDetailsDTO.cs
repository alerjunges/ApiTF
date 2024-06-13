namespace ApiTF.Services.DTOs
{
    public class ProductDetailsDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public string BarcodeType { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
    }
}
