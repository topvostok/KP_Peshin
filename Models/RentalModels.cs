using System;

namespace WpfApp1.Models
{
    // в”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђ
    // 1. РЎРќРђР РЇР–Р•РќРР•
    // в”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђ
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string SizeValue { get; set; }
        public string ConditionLvl { get; set; }
        public decimal PricePerDay { get; set; }
        public int StockQty { get; set; }
        public string ImageUrl { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        public string PriceDisplay => $"в‚Ѕ{PricePerDay:N0}/РґРµРЅСЊ";
        public string StockDisplay => $"{StockQty} С€С‚.";
        public string CategoryIcon => Category == "Р›С‹Р¶Рё" ? "в›·" :
                                      Category == "РЎРЅРѕСѓР±РѕСЂРґ" ? "рџЏ‚" :
                                      Category == "РњР°СЃРєР°" ? "рџҐЅ" :
                                      Category == "РЁР»РµРј" ? "в›‘" :
                                      Category == "РљРѕРјРїР»РµРєС‚" ? "рџЋї" : "рџ“¦";
    }

    // в”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђ
    // 2. РљР›РР•РќРў
    // в”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђ
    public class RentalClient
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Passport { get; set; }
        public string SkillLevel { get; set; }
        public DateTime? BirthDate { get; set; }
        public int LoyaltyPts { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        public string BirthDisplay => BirthDate.HasValue ? BirthDate.Value.ToString("dd.MM.yyyy") : "вЂ”";
        public string LoyaltyDisplay => $"{LoyaltyPts} pts";
        public string SkillBadge => SkillLevel == "РќРѕРІРёС‡РѕРє" ? "рџџў" :
                                        SkillLevel == "Р›СЋР±РёС‚РµР»СЊ" ? "рџ”µ" :
                                        SkillLevel == "РџСЂРѕРґРІРёРЅСѓС‚С‹Р№" ? "рџџ " :
                                        SkillLevel == "РџСЂРѕС„РµСЃСЃРёРѕРЅР°Р»" ? "рџ”ґ" : "вљЄ";
    }

    // в”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђ
    // 3. РђР Р•РќР”Рђ
    // в”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђ
    public class Rental
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int EquipmentId { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime ReturnPlan { get; set; }
        public int DaysCount { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Deposit { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        // JOIN РїРѕР»СЏ
        public string ClientName { get; set; }
        public string EquipmentName { get; set; }

        public string TotalDisplay => $"в‚Ѕ{TotalPrice:N0}";
        public string DepositDisplay => $"в‚Ѕ{Deposit:N0}";
        public string RentDateDisplay => RentDate.ToString("dd.MM.yyyy");
        public string ReturnPlanDisplay => ReturnPlan.ToString("dd.MM.yyyy");
        public string StatusColor => Status == "РђРєС‚РёРІРЅР°" ? "#FF5DE0FF" :
                                          Status == "Р—Р°РІРµСЂС€РµРЅР°" ? "#FFA3FF5C" :
                                          Status == "РџСЂРѕСЃСЂРѕС‡РµРЅР°" ? "#FFFF5C3A" :
                                          Status == "РћС‚РјРµРЅРµРЅР°" ? "#FF666666" : "#FFFFFFFF";
    }

    // в”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђ
    // 4. Р’РћР—Р’Р РђРў
    // в”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђ
    public class Return
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime ReturnDate { get; set; }
        public string ConditionAfter { get; set; }
        public decimal PenaltyAmount { get; set; }
        public decimal RefundAmount { get; set; }
        public string InspectorName { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        // JOIN РїРѕР»СЏ
        public string ClientName { get; set; }
        public string EquipmentName { get; set; }

        public string ReturnDateDisplay => ReturnDate.ToString("dd.MM.yyyy");
        public string PenaltyDisplay => PenaltyAmount > 0 ? $"в‚Ѕ{PenaltyAmount:N0}" : "вЂ”";
        public string RefundDisplay => $"в‚Ѕ{RefundAmount:N0}";
        public string ConditionColor => ConditionAfter == "РћС‚Р»РёС‡РЅРѕ" ? "#FFA3FF5C" :
                                          ConditionAfter == "РҐРѕСЂРѕС€Рѕ" ? "#FF5DE0FF" :
                                          ConditionAfter == "РџРѕРІСЂРµР¶РґРµРЅРѕ" ? "#FFFF5C3A" :
                                          ConditionAfter == "РЈС‚РµСЂСЏРЅРѕ" ? "#FFFF1A1A" : "#FFFFFFFF";
    }
}
