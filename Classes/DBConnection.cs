using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using MySql.Data.MySqlClient;
using WpfApp1.Models;

namespace WpfApp1.Classes
{
    /// <summary>
    /// Класс для работы с базой данных горнолыжного магазина Alpine.
    /// Поддерживает: загрузку товаров по категориям, поиск, фильтрацию.
    /// </summary>
    public class DBConnection
    {
        // ── Строка подключения ─────────────────────────────────────────────
        // Замените uid/pwd/database на ваши реальные данные
        public static string ConnectionString =

            "server=127.0.0.1;port=3306;uid=root;pwd=;database=alpine_shop;";

        // ── Кэш последней загрузки (чтобы не дёргать БД лишний раз) ───────
        private static readonly Dictionary<string, List<AlpineProductModel>> _cache =
            new Dictionary<string, List<AlpineProductModel>>();

        // ─────────────────────────────────────────────────────────────────
        //  БАЗОВЫЙ МЕТОД — выполняет SELECT и возвращает reader
        // ─────────────────────────────────────────────────────────────────
        /// <summary>
        /// Открывает соединение, выполняет запрос и возвращает MySqlDataReader.
        /// Соединение НУЖНО закрыть вручную после чтения.
        /// </summary>
        public static MySqlDataReader Query(string sql, MySqlConnection connection)
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                var command = new MySqlCommand(sql, connection);
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения запроса:\n{ex.Message}",
                                "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        // ─────────────────────────────────────────────────────────────────
        //  ЗАГРУЗКА ТОВАРОВ ПО КАТЕГОРИИ
        // ─────────────────────────────────────────────────────────────────
        /// <summary>
        /// Загружает список товаров из таблицы alpine_products по категории.
        /// Категории: skis | snowboard | goggles | helmets
        /// </summary>
        public static List<AlpineProductModel> LoadProductsByCategory(string category)
        {
            var list = new List<AlpineProductModel>();

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();

                    // Параметризованный запрос — защита от SQL-инъекций
                    string sql = "SELECT id, name, brand, category, price, image_url, " +
                                 "description, in_stock, stock_qty, specs " +
                                 "FROM alpine_products " +
                                 "WHERE category = @category " +
                    "ORDER BY price ASC";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@category", category);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(ReadProduct(reader));
                            }
                        }
                    }
                }

                // Обновляем кэш
                _cache[category] = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка загрузки товаров (категория: {category}):\n{ex.Message}",
                    "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return list;
        }

        // ─────────────────────────────────────────────────────────────────
        //  ЗАГРУЗКА ВСЕХ ТОВАРОВ
        // ─────────────────────────────────────────────────────────────────
        /// <summary>
        /// Загружает все товары из таблицы alpine_products.
        /// </summary>
        public static List<AlpineProductModel> LoadAllProducts()
        {
            var list = new List<AlpineProductModel>();

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT id, name, brand, category, price, image_url, " +
                                 "description, in_stock, stock_qty, specs " +
                                 "FROM alpine_products " +
                                 "ORDER BY category, price ASC";

                    using (var cmd = new MySqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(ReadProduct(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки всех товаров:\n{ex.Message}",
                                "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return list;
        }

        // ─────────────────────────────────────────────────────────────────
        //  ПОИСК ТОВАРОВ ПО НАЗВАНИЮ / БРЕНДУ
        // ─────────────────────────────────────────────────────────────────
        /// <summary>
        /// Ищет товары по подстроке в названии или бренде, с фильтром по категории.
        /// Если category = null — поиск по всем категориям.
        /// </summary>
        public static List<AlpineProductModel> SearchProducts(string searchText, string category = null)
        {
            var list = new List<AlpineProductModel>();

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT id, name, brand, category, price, image_url, " +
                                 "description, in_stock, stock_qty, specs " +
                                 "FROM alpine_products " +
                                 "WHERE (name LIKE @search OR brand LIKE @search) ";

                    // Добавляем фильтр по категории, если указана
                    if (!string.IsNullOrEmpty(category))
                        sql += "AND category = @category ";

                    sql += "ORDER BY price ASC";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", $"%{searchText}%");
                        if (!string.IsNullOrEmpty(category))
                            cmd.Parameters.AddWithValue("@category", category);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                list.Add(ReadProduct(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска товаров:\n{ex.Message}",
                                "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return list;
        }

        // ─────────────────────────────────────────────────────────────────
        //  ФИЛЬТР ПО ЦЕНЕ
        // ─────────────────────────────────────────────────────────────────
        /// <summary>
        /// Загружает товары категории в диапазоне цен.
        /// </summary>
        public static List<AlpineProductModel> LoadByPriceRange(
            string category, decimal minPrice, decimal maxPrice)
        {
            var list = new List<AlpineProductModel>();

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT id, name, brand, category, price, image_url, " +
                                 "description, in_stock, stock_qty, specs " +
                                 "FROM alpine_products " +
                                 "WHERE category = @category " +
                                 "AND price BETWEEN @minPrice AND @maxPrice " +
                                 "ORDER BY price ASC";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@category", category);
                        cmd.Parameters.AddWithValue("@minPrice", minPrice);
                        cmd.Parameters.AddWithValue("@maxPrice", maxPrice);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                list.Add(ReadProduct(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации по цене:\n{ex.Message}",
                                "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return list;
        }

        // ─────────────────────────────────────────────────────────────────
        //  ФИЛЬТР «ТОЛЬКО В НАЛИЧИИ»
        // ─────────────────────────────────────────────────────────────────
        /// <summary>
        /// Загружает товары, которые есть в наличии (in_stock = 1).
        /// </summary>
        public static List<AlpineProductModel> LoadInStock(string category)
        {
            var list = new List<AlpineProductModel>();

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT id, name, brand, category, price, image_url, " +
                                 "description, in_stock, stock_qty, specs " +
                                 "FROM alpine_products " +
                                 "WHERE category = @category AND in_stock = 1 " +
                                 "ORDER BY price ASC";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@category", category);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                list.Add(ReadProduct(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров в наличии:\n{ex.Message}",
                                "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return list;
        }

        // ─────────────────────────────────────────────────────────────────
        //  ПРОВЕРКА СОЕДИНЕНИЯ
        // ─────────────────────────────────────────────────────────────────
        /// <summary>
        /// Проверяет, удаётся ли подключиться к БД.
        /// Возвращает true при успехе.
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не удалось подключиться к базе данных.\n" +
                    $"Проверьте строку подключения:\n\n{ConnectionString}\n\n" +
                    $"Ошибка: {ex.Message}",
                    "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }

        // ─────────────────────────────────────────────────────────────────
        //  ПРИВАТНЫЙ ХЕЛПЕР — читает одну строку reader → AlpineProductModel
        // ─────────────────────────────────────────────────────────────────
        private static AlpineProductModel ReadProduct(MySqlDataReader reader)
        {
            return new AlpineProductModel
            {
                Id = reader.GetInt32("id"),
                Name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader.GetString("name"),
                Brand = reader.IsDBNull(reader.GetOrdinal("brand")) ? "" : reader.GetString("brand"),
                Category = reader.IsDBNull(reader.GetOrdinal("category")) ? "" : reader.GetString("category"),
                Price = reader.IsDBNull(reader.GetOrdinal("price")) ? 0 : reader.GetDecimal("price"),
                ImageUrl = reader.IsDBNull(reader.GetOrdinal("image_url")) ? "" : reader.GetString("image_url"),
                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description"),
                InStock = !reader.IsDBNull(reader.GetOrdinal("in_stock")) && reader.GetBoolean("in_stock"),
                StockQty = reader.IsDBNull(reader.GetOrdinal("stock_qty")) ? 0 : reader.GetInt32("stock_qty"),
                Specs = reader.IsDBNull(reader.GetOrdinal("specs")) ? "" : reader.GetString("specs"),
            };
        }
    }
}