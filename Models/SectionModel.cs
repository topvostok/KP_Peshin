using System.Collections.Generic;
using System.Windows.Media;

namespace WpfApp1.Models
{
    public class SectionModel
    {
        public string Tag { get; set; }
        public string TitleLine1 { get; set; }
        public string TitleLine2 { get; set; }
        public string Description { get; set; }
        public Color AccentColor { get; set; }
        public string CtaText { get; set; }
        public string BgImageUrl { get; set; }
        public string SegImageUrl { get; set; }

        // For the circle segment display
        public string SegLabel { get; set; }
        public string SegIconPath { get; set; }   // Geometry path data for icon

        public List<ProductModel> Products { get; set; } = new List<ProductModel>();

        // Derived
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
