using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using WpfApp1.Models;

namespace WpfApp1.Classes
{
    /// <summary>
    /// Репозиторий для работы с таблицей Equipment как с товарами магазина
    /// </summary>
    public static class EquipmentShopRepository
    {
        private static readonly Dictionary<string, List<EquipmentProductModel>> _cache =
            new Dictionary<string, List<EquipmentProductModel>>();

        public static List<EquipmentProductModel> GetProductsByCategory(string category)
        {
            // Маппинг категорий из UI в категории БД
            string dbCategory;
            switch (category)
            {
                case "skis":
                    dbCategory = "Лыжи";
                    break;
                case "snowboard":
                    dbCategory = "Сноуборды";
                    break;
                case "goggles":
                    dbCategory = "Маски";
                    break;
                case "helmets":
                    dbCategory = "Шлемы";
                    break;
                default:
                    dbCategory = category;
                    break;
            }

            // Проверяем кэш
            if (_cache.ContainsKey(dbCategory))
                return _cache[dbCategory];

            var list = new List<EquipmentProductModel>();

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"SELECT id, name, category, brand, size_value, 
                                   price_per_day, image_url, notes, stock_qty
                                   FROM Equipment 
                                   WHERE category = @category 
                                   ORDER BY price_per_day ASC";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@category", dbCategory);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(MapToProduct(reader));
                            }
                        }
                    }
                }

                _cache[dbCategory] = list;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Ошибка загрузки товаров: {ex.Message}",
                    "Ошибка",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }

            return list;
        }

        public static List<EquipmentProductModel> GetAllProducts()
        {
            var list = new List<EquipmentProductModel>();

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"SELECT id, name, category, brand, size_value, 
                                   price_per_day, image_url, notes, stock_qty
                                   FROM Equipment 
                                   WHERE category IN ('Лыжи', 'Сноуборды', 'Маски', 'Шлемы')
                                   ORDER BY category, price_per_day ASC";

                    using (var cmd = new MySqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(MapToProduct(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Ошибка загрузки всех товаров: {ex.Message}",
                    "Ошибка",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }

            return list;
        }

        public static List<EquipmentProductModel> SearchProducts(string searchText, string category = null)
        {
            var list = new List<EquipmentProductModel>();

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"SELECT id, name, category, brand, size_value, 
                                   price_per_day, image_url, notes, stock_qty
                                   FROM Equipment 
                                   WHERE (name LIKE @search OR brand LIKE @search OR notes LIKE @search) ";

                    if (!string.IsNullOrEmpty(category))
                    {
                        string dbCategory;
                        switch (category)
                        {
                            case "skis": dbCategory = "Лыжи"; break;
                            case "snowboard": dbCategory = "Сноуборды"; break;
                            case "goggles": dbCategory = "Маски"; break;
                            case "helmets": dbCategory = "Шлемы"; break;
                            default: dbCategory = category; break;
                        }
                        sql += "AND category = @category ";
                    }

                    sql += "ORDER BY price_per_day ASC";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", $"%{searchText}%");
                        if (!string.IsNullOrEmpty(category))
                        {
                            string dbCategory;
                            switch (category)
                            {
                                case "skis": dbCategory = "Лыжи"; break;
                                case "snowboard": dbCategory = "Сноуборды"; break;
                                case "goggles": dbCategory = "Маски"; break;
                                case "helmets": dbCategory = "Шлемы"; break;
                                default: dbCategory = category; break;
                            }
                            cmd.Parameters.AddWithValue("@category", dbCategory);
                        }

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                list.Add(MapToProduct(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Ошибка поиска товаров: {ex.Message}",
                    "Ошибка",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }

            return list;
        }

        public static List<EquipmentProductModel> GetByPriceRange(string category, decimal minPrice, decimal maxPrice)
        {
            string dbCategory;
            switch (category)
            {
                case "skis": dbCategory = "Лыжи"; break;
                case "snowboard": dbCategory = "Сноуборды"; break;
                case "goggles": dbCategory = "Маски"; break;
                case "helmets": dbCategory = "Шлемы"; break;
                default: dbCategory = category; break;
            }

            var list = new List<EquipmentProductModel>();

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"SELECT id, name, category, brand, size_value, 
                                   price_per_day, image_url, notes, stock_qty
                                   FROM Equipment 
                                   WHERE category = @category 
                                   AND price_per_day BETWEEN @minPrice AND @maxPrice 
                                   ORDER BY price_per_day ASC";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@category", dbCategory);
                        cmd.Parameters.AddWithValue("@minPrice", minPrice);
                        cmd.Parameters.AddWithValue("@maxPrice", maxPrice);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                list.Add(MapToProduct(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Ошибка фильтрации: {ex.Message}",
                    "Ошибка",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }

            return list;
        }

        public static List<EquipmentProductModel> GetInStock(string category)
        {
            string dbCategory;
            switch (category)
            {
                case "skis": dbCategory = "Лыжи"; break;
                case "snowboard": dbCategory = "Сноуборды"; break;
                case "goggles": dbCategory = "Маски"; break;
                case "helmets": dbCategory = "Шлемы"; break;
                default: dbCategory = category; break;
            }

            var list = new List<EquipmentProductModel>();

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"SELECT id, name, category, brand, size_value, 
                                   price_per_day, image_url, notes, stock_qty
                                   FROM Equipment 
                                   WHERE category = @category AND stock_qty > 0 
                                   ORDER BY price_per_day ASC";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@category", dbCategory);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                list.Add(MapToProduct(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Ошибка загрузки товаров в наличии: {ex.Message}",
                    "Ошибка",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }

            return list;
        }

        public static EquipmentProductModel GetProductById(int id)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"SELECT id, name, category, brand, size_value, 
                                   price_per_day, image_url, notes, stock_qty
                                   FROM Equipment 
                                   WHERE id = @id";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                return MapToProduct(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Ошибка загрузки товара: {ex.Message}",
                    "Ошибка",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }

            return null;
        }

        public static void ClearCache()
        {
            _cache.Clear();
        }

        private static EquipmentProductModel MapToProduct(MySqlDataReader reader)
        {
            return new EquipmentProductModel
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                Category = reader.GetString("category"),
                Brand = reader.GetString("brand"),
                SizeValue = reader.IsDBNull(reader.GetOrdinal("size_value")) ? "" : reader.GetString("size_value"),
                Price = reader.GetDecimal("price_per_day"),
                ImageUrl = reader.IsDBNull(reader.GetOrdinal("image_url")) ? "" : reader.GetString("image_url"),
                Description = reader.IsDBNull(reader.GetOrdinal("notes")) ? "" : reader.GetString("notes"),
                StockQty = reader.GetInt32("stock_qty")
            };
        }
    }
}