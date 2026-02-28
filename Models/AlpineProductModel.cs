namespace WpfApp1.Models
{
    /// <summary>
    /// Модель товара из базы данных Alpine
    /// </summary>
    public class AlpineProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }   // skis / snowboard / goggles / helmets
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool InStock { get; set; }
        public int StockQty { get; set; }
        public string Specs { get; set; }      // доп. характеристики

        // Удобное отображение цены
        public string PriceDisplay => $"₽{Price:N0}";
    }
}