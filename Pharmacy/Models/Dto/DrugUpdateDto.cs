using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Models.Dto
{
    public class DrugUpdateDto
    {
        public string Name { get; set; } = null!;

        public decimal SellingPrice { get; set; }

        public decimal PurchasingPrice { get; set; }

        public string? Barcode { get; set; }
        public string DrugType { get; set; } = null!;
        public DateOnly? ExpirationDate { get; set; }

        public int ShelfAmount { get; set; }

        public int StoredAmount { get; set; }

        public int LowAmount { get; set; }

        public int SubAmountQuantity { get; set; }
        public List<string>? Tags { get; set; }
    }
   
}  
