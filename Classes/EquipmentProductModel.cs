namespace WpfApp1.Models
{
    /// <summary>
    /// Модель товара из таблицы Equipment для отображения в каталоге магазина
    /// </summary>
    public class EquipmentProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string SizeValue { get; set; }
        public decimal Price { get; set; } // Используем price_per_day как цену продажи
        public string ImageUrl { get; set; }
        public string Description { get; set; } // Используем notes как описание
        public int StockQty { get; set; }

        // Для отображения в UI
        public bool InStock => StockQty > 0;
        public string PriceDisplay => $"₽{Price:N0}";
        public string Specs => SizeValue ?? "";
        public bool IsOutOfStock => !InStock;
    }
}