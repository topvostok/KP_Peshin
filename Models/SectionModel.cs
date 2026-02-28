using System.Collections.Generic;
using System.Windows.Media;

namespace WpfApp1.Models
{
    /// <summary>
    /// Модель раздела главного экрана (лыжи / сноуборд / маски / шлемы).
    /// </summary>
    public class SectionModel
    {
        // Тег раздела (напр. "ГОРНЫЕ ЛЫЖИ")
        public string Tag { get; set; }

        // Двухстрочный заголовок
        public string TitleLine1 { get; set; }
        public string TitleLine2 { get; set; }

        // Описание раздела
        public string Description { get; set; }

        // Акцентный цвет раздела
        public Color AccentColor { get; set; }

        // Текст кнопки CTA
        public string CtaText { get; set; }

        // URL фонового изображения
        public string BgImageUrl { get; set; }

        // URL изображения для сегмента круга
        public string SegImageUrl { get; set; }

        // Подпись сегмента круга
        public string SegLabel { get; set; }

        // SVG-путь иконки сегмента
        public string SegIconPath { get; set; }

        // Категория в базе данных (skis | snowboard | goggles | helmets)
        // Используется при открытии окна каталога
        public string DbCategory { get; set; }

        // Товары превью (3 штуки на главном экране, из жёстко прописанных данных)
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();

        // ── Вычислимые свойства ──────────────────────────────────────────
        public SolidColorBrush AccentBrush => new SolidColorBrush(AccentColor);

        public Color AccentTintColor
        {
            get
            {
                var c = AccentColor;
                return Color.FromArgb(40, c.R, c.G, c.B);
            }
        }

        public SolidColorBrush AccentTintBrush => new SolidColorBrush(AccentTintColor);
    }
}