using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using WpfApp1.Models;

namespace WpfApp1.Classes
{
    // в•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђ
    //  EQUIPMENT REPOSITORY
    // в•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђ
    public static class EquipmentRepo
    {
        public static List<Equipment> GetAll(string search = "")
        {
            var list = new List<Equipment>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT * FROM Equipment WHERE 1=1";
                if (!string.IsNullOrWhiteSpace(search))
                    sql += " AND (name LIKE @s OR brand LIKE @s OR category LIKE @s)";
                sql += " ORDER BY category, name";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (!string.IsNullOrWhiteSpace(search))
                        cmd.Parameters.AddWithValue("@s", $"%{search}%");
                    using (var r = cmd.ExecuteReader())
                        while (r.Read()) list.Add(MapEquip(r));
                }
            }
            return list;
        }

        public static Equipment GetById(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new MySqlCommand("SELECT * FROM Equipment WHERE id=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var r = cmd.ExecuteReader())
                    return r.Read() ? MapEquip(r) : null;
            }
        }

        public static void Insert(Equipment e)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new MySqlCommand(
                "INSERT INTO Equipment (name,category,brand,size_value,condition_lvl,price_per_day,stock_qty,image_url,notes)" +
                " VALUES (@n,@cat,@br,@sz,@cond,@price,@qty,@img,@notes)", conn))
            {
                BindEquip(cmd, e);
                cmd.ExecuteNonQuery();
            }
        }

        public static void Update(Equipment e)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new MySqlCommand(
                "UPDATE Equipment SET name=@n,category=@cat,brand=@br,size_value=@sz," +
                "condition_lvl=@cond,price_per_day=@price,stock_qty=@qty,image_url=@img,notes=@notes WHERE id=@id", conn))
            {
                BindEquip(cmd, e);
                cmd.Parameters.AddWithValue("@id", e.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public static void Delete(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new MySqlCommand("DELETE FROM Equipment WHERE id=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private static void BindEquip(MySqlCommand cmd, Equipment e)
        {
            cmd.Parameters.AddWithValue("@n", e.Name);
            cmd.Parameters.AddWithValue("@cat", e.Category);
            cmd.Parameters.AddWithValue("@br", e.Brand);
            cmd.Parameters.AddWithValue("@sz", (object)e.SizeValue ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@cond", e.ConditionLvl ?? "РҐРѕСЂРѕС€РµРµ");
            cmd.Parameters.AddWithValue("@price", e.PricePerDay);
            cmd.Parameters.AddWithValue("@qty", e.StockQty);
            cmd.Parameters.AddWithValue("@img", (object)e.ImageUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@notes", (object)e.Notes ?? DBNull.Value);
        }

        private static Equipment MapEquip(MySqlDataReader r) => new Equipment
        {
            Id = r.GetInt32("id"),
            Name = r.GetString("name"),
            Category = r.GetString("category"),
            Brand = r.GetString("brand"),
            SizeValue = r.IsDBNull(r.GetOrdinal("size_value")) ? null : r.GetString("size_value"),
            ConditionLvl = r.GetString("condition_lvl"),
            PricePerDay = r.GetDecimal("price_per_day"),
            StockQty = r.GetInt32("stock_qty"),
            ImageUrl = r.IsDBNull(r.GetOrdinal("image_url")) ? null : r.GetString("image_url"),
            Notes = r.IsDBNull(r.GetOrdinal("notes")) ? null : r.GetString("notes"),
            CreatedAt = r.GetDateTime("created_at"),
        };
    }

    public static class ClientRepo
    {
        public static List<RentalClient> GetAll(string search = "")
        {
            var list = new List<RentalClient>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT * FROM Clients WHERE 1=1";
                if (!string.IsNullOrWhiteSpace(search))
                    sql += " AND (full_name LIKE @s OR phone LIKE @s OR email LIKE @s)";
                sql += " ORDER BY full_name";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (!string.IsNullOrWhiteSpace(search))
                        cmd.Parameters.AddWithValue("@s", $"%{search}%");
                    using (var r = cmd.ExecuteReader())
                        while (r.Read()) list.Add(MapClient(r));
                }
            }
            return list;
        }

        public static RentalClient GetById(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new MySqlCommand("SELECT * FROM Clients WHERE id=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var r = cmd.ExecuteReader())
                    return r.Read() ? MapClient(r) : null;
            }
        }

        public static void Insert(RentalClient c)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new MySqlCommand(
                "INSERT INTO Clients (full_name,phone,email,passport,skill_level,birth_date,loyalty_pts,notes)" +
                " VALUES (@fn,@ph,@em,@pp,@sk,@bd,@lp,@notes)", conn))
            {
                BindClient(cmd, c);
                cmd.ExecuteNonQuery();
            }
        }

        public static void Update(RentalClient c)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new MySqlCommand(
                "UPDATE Clients SET full_name=@fn,phone=@ph,email=@em,passport=@pp," +
                "skill_level=@sk,birth_date=@bd,loyalty_pts=@lp,notes=@notes WHERE id=@id", conn))
            {
                BindClient(cmd, c);
                cmd.Parameters.AddWithValue("@id", c.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public static void Delete(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new MySqlCommand("DELETE FROM Clients WHERE id=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private static void BindClient(MySqlCommand cmd, RentalClient c)
        {
            cmd.Parameters.AddWithValue("@fn", c.FullName);
            cmd.Parameters.AddWithValue("@ph", c.Phone);
            cmd.Parameters.AddWithValue("@em", (object)c.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@pp", (object)c.Passport ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@sk", c.SkillLevel ?? "Р›СЋР±РёС‚РµР»СЊ");
            cmd.Parameters.AddWithValue("@bd", c.BirthDate.HasValue ? (object)c.BirthDate.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@lp", c.LoyaltyPts);
            cmd.Parameters.AddWithValue("@notes", (object)c.Notes ?? DBNull.Value);
        }

        private static RentalClient MapClient(MySqlDataReader r) => new RentalClient
        {
            Id = r.GetInt32("id"),
            FullName = r.GetString("full_name"),
            Phone = r.GetString("phone"),
            Email = r.IsDBNull(r.GetOrdinal("email")) ? null : r.GetString("email"),
            Passport = r.IsDBNull(r.GetOrdinal("passport")) ? null : r.GetString("passport"),
            SkillLevel = r.GetString("skill_level"),
            BirthDate = r.IsDBNull(r.GetOrdinal("birth_date")) ? (DateTime?)null : r.GetDateTime("birth_date"),
            LoyaltyPts = r.GetInt32("loyalty_pts"),
            Notes = r.IsDBNull(r.GetOrdinal("notes")) ? null : r.GetString("notes"),
            CreatedAt = r.GetDateTime("created_at"),
        };
    }

    // в•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђ
    //  RENTAL REPOSITORY
    // в•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђ
    public static class RentalRepo
    {
        public static List<Rental> GetAll(string search = "")
        {
            var list = new List<Rental>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql =
                    "SELECT r.*, c.full_name AS client_name, e.name AS equip_name " +
                    "FROM Rentals r " +
                    "JOIN Clients c ON r.client_id=c.id " +
                    "JOIN Equipment e ON r.equipment_id=e.id WHERE 1=1";
                if (!string.IsNullOrWhiteSpace(search))
                    sql += " AND (c.full_name LIKE @s OR e.name LIKE @s OR r.status LIKE @s)";
                sql += " ORDER BY r.rent_date DESC";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (!string.IsNullOrWhiteSpace(search))
                        cmd.Parameters.AddWithValue("@s", $"%{search}%");
                    using (var r = cmd.ExecuteReader())
                        while (r.Read()) list.Add(MapRental(r));
                }
            }
            return list;
        }

        public static Rental GetById(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new MySqlCommand(
                "SELECT r.*, c.full_name AS client_name, e.name AS equip_name " +
                "FROM Rentals r JOIN Clients c ON r.client_id=c.id " +
                "JOIN Equipment e ON r.equipment_id=e.id WHERE r.id=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var r = cmd.ExecuteReader())
                    return r.Read() ? MapRental(r) : null;
            }
        }

        public static void Insert(Rental rn)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new MySqlCommand(
                "INSERT INTO Rentals (client_id,equipment_id,rent_date,return_plan,days_count,total_price,deposit,status,notes)" +
                " VALUES (@ci,@ei,@rd,@rp,@dc,@tp,@dep,@st,@notes)", conn))
            {
                BindRental(cmd, rn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void Update(Rental rn)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new MySqlCommand(
                "UPDATE Rentals SET client_id=@ci,equipment_id=@ei,rent_date=@rd,return_plan=@rp," +
                "days_count=@dc,total_price=@tp,deposit=@dep,status=@st,notes=@notes WHERE id=@id", conn))
            {
                BindRental(cmd, rn);
                cmd.Parameters.AddWithValue("@id", rn.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public static void Delete(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new MySqlCommand("DELETE FROM Rentals WHERE id=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private static void BindRental(MySqlCommand cmd, Rental rn)
        {
            cmd.Parameters.AddWithValue("@ci", rn.ClientId);
            cmd.Parameters.AddWithValue("@ei", rn.EquipmentId);
            cmd.Parameters.AddWithValue("@rd", rn.RentDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@rp", rn.ReturnPlan.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@dc", rn.DaysCount);
            cmd.Parameters.AddWithValue("@tp", rn.TotalPrice);
            cmd.Parameters.AddWithValue("@dep", rn.Deposit);
            cmd.Parameters.AddWithValue("@st", rn.Status ?? "РђРєС‚РёРІРЅР°");
            cmd.Parameters.AddWithValue("@notes", (object)rn.Notes ?? DBNull.Value);
        }

        private static Rental MapRental(MySqlDataReader r) => new Rental
        {
            Id = r.GetInt32("id"),
            ClientId = r.GetInt32("client_id"),
            EquipmentId = r.GetInt32("equipment_id"),
            RentDate = r.GetDateTime("rent_date"),
            ReturnPlan = r.GetDateTime("return_plan"),
            DaysCount = r.GetInt32("days_count"),
            TotalPrice = r.GetDecimal("total_price"),
            Deposit = r.GetDecimal("deposit"),
            Status = r.GetString("status"),
            Notes = r.IsDBNull(r.GetOrdinal("notes")) ? null : r.GetString("notes"),
            CreatedAt = r.GetDateTime("created_at"),
            ClientName = r.GetString("client_name"),
            EquipmentName = r.GetString("equip_name"),
        };
    }

    // в•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђ
    //  RETURN REPOSITORY
    // в•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђв•ђ
    public static class ReturnRepo
    {
        public static List<Return> GetAll(string search = "")
        {
            var list = new List<Return>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql =
                    "SELECT ret.*, c.full_name AS client_name, e.name AS equip_name " +
                    "FROM Returns ret " +
                    "JOIN Rentals rn ON ret.rental_id=rn.id " +
                    "JOIN Clients c ON rn.client_id=c.id " +
                    "JOIN Equipment e ON rn.equipment_id=e.id WHERE 1=1";
                if (!string.IsNullOrWhiteSpace(search))
                    sql += " AND (c.full_name LIKE @s OR e.name LIKE @s OR ret.condition_after LIKE @s)";
                sql += " ORDER BY ret.return_date DESC";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (!string.IsNullOrWhiteSpace(search))
                        cmd.Parameters.AddWithValue("@s", $"%{search}%");
                    using (var r = cmd.ExecuteReader())
                        while (r.Read()) list.Add(MapReturn(r));
                }
            }
            return list;
        }

        public static Return GetById(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new MySqlCommand(
                "SELECT ret.*, c.full_name AS client_name, e.name AS equip_name " +
                "FROM Returns ret JOIN Rentals rn ON ret.rental_id=rn.id " +
                "JOIN Clients c ON rn.client_id=c.id " +
                "JOIN Equipment e ON rn.equipment_id=e.id WHERE ret.id=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var r = cmd.ExecuteReader())
                    return r.Read() ? MapReturn(r) : null;
            }
        }

        public static void Insert(Return ret)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                using (var cmd = new MySqlCommand(
                    "INSERT INTO Returns (rental_id,return_date,condition_after,penalty_amount,refund_amount,inspector_name,notes)" +
                    " VALUES (@rid,@rd,@ca,@pa,@ra,@ins,@notes)", conn))
                {
                    BindReturn(cmd, ret);
                    cmd.ExecuteNonQuery();
                }
                // РџРѕРјРµС‡Р°РµРј Р°СЂРµРЅРґСѓ РєР°Рє Р·Р°РІРµСЂС€С‘РЅРЅСѓСЋ
                using (var cmd2 = new MySqlCommand("UPDATE Rentals SET status='Р—Р°РІРµСЂС€РµРЅР°' WHERE id=@id", conn))
                {
                    cmd2.Parameters.AddWithValue("@id", ret.RentalId);
                    cmd2.ExecuteNonQuery();
                }
            }
        }

        public static void Update(Return ret)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new MySqlCommand(
                "UPDATE Returns SET rental_id=@rid,return_date=@rd,condition_after=@ca," +
                "penalty_amount=@pa,refund_amount=@ra,inspector_name=@ins,notes=@notes WHERE id=@id", conn))
            {
                BindReturn(cmd, ret);
                cmd.Parameters.AddWithValue("@id", ret.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public static void Delete(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new MySqlCommand("DELETE FROM Returns WHERE id=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private static void BindReturn(MySqlCommand cmd, Return ret)
        {
            cmd.Parameters.AddWithValue("@rid", ret.RentalId);
            cmd.Parameters.AddWithValue("@rd", ret.ReturnDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@ca", ret.ConditionAfter ?? "РҐРѕСЂРѕС€Рѕ");
            cmd.Parameters.AddWithValue("@pa", ret.PenaltyAmount);
            cmd.Parameters.AddWithValue("@ra", ret.RefundAmount);
            cmd.Parameters.AddWithValue("@ins", (object)ret.InspectorName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@notes", (object)ret.Notes ?? DBNull.Value);
        }

        private static Return MapReturn(MySqlDataReader r) => new Return
        {
            Id = r.GetInt32("id"),
            RentalId = r.GetInt32("rental_id"),
            ReturnDate = r.GetDateTime("return_date"),
            ConditionAfter = r.GetString("condition_after"),
            PenaltyAmount = r.GetDecimal("penalty_amount"),
            RefundAmount = r.GetDecimal("refund_amount"),
            InspectorName = r.IsDBNull(r.GetOrdinal("inspector_name")) ? null : r.GetString("inspector_name"),
            Notes = r.IsDBNull(r.GetOrdinal("notes")) ? null : r.GetString("notes"),
            CreatedAt = r.GetDateTime("created_at"),
            ClientName = r.GetString("client_name"),
            EquipmentName = r.GetString("equip_name"),
        };
    }
}
