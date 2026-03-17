using System;
using MySql.Data.MySqlClient;

namespace WpfApp1.Classes
{
    public static class DatabaseHelper
    {
        // ───── НАСТРОЙКИ ПОДКЛЮЧЕНИЯ ─────────────────────────────
        private const string Server = "127.0.0.1";
        private const string Port = "3306";
        private const string Database = "AlpineRental";
        private const string User = "root";
        private const string Password = ""; // ваш пароль, если есть
        // ─────────────────────────────────────────────────────────

        public static string ConnectionString =>
            $"server={Server};port={Port};uid={User};pwd={Password};database={Database};charset=utf8mb4;";

        public static MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        public static bool TestConnection(out string error)
        {
            error = string.Empty;
            try
            {
                using (GetConnection()) return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}