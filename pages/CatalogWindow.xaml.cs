using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WpfApp1.Classes;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class CatalogWindow : Window
    {
        private readonly string _category;
        private readonly Color _accentColor;
        private List<EquipmentProductModel> _allProducts = new List<EquipmentProductModel>();
        private string _activeFilter = "all";

        public CatalogWindow(
            string category,
            Color accentColor,
            string tag,
            string titleLine1,
            string titleLine2,
            string bgImageUrl)
        {
            InitializeComponent();

            _category = category;
            _accentColor = accentColor;

            ApplyAccentColor(accentColor);

            PageTagLabel.Text = tag;
            TitleLine1Label.Text = titleLine1;
            TitleLine2Label.Text = titleLine2;
            WindowCategoryLabel.Text = tag;

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var barAnim = new DoubleAnimation
            {
                From = 0,
                To = ActualHeight,
                Duration = TimeSpan.FromMilliseconds(600),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            AccentBar.BeginAnimation(HeightProperty, barAnim);

            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                StatusLabel.Text = "Загрузка данных из базы…";
                ItemCountLabel.Text = "Загрузка…";

                // Загружаем товары из таблицы Equipment
                _allProducts = EquipmentShopRepository.GetProductsByCategory(_category);

                int inStockCount = _allProducts.Count(p => p.InStock);
                ItemCountLabel.Text = $"{_allProducts.Count} товаров";
                InStockCountLabel.Text = $"{inStockCount} в наличии";
                StatusLabel.Text = $"Загружено {_allProducts.Count} товаров";

                ApplyFilter(_activeFilter);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilter(string filterTag)
        {
            _activeFilter = filterTag;
            IEnumerable<EquipmentProductModel> filtered = _allProducts;

            switch (filterTag)
            {
                case "budget":
                    filtered = _allProducts.Where(p => p.Price < 30000);
                    break;
                case "mid":
                    filtered = _allProducts.Where(p => p.Price >= 30000 && p.Price <= 60000);
                    break;
                case "premium":
                    filtered = _allProducts.Where(p => p.Price > 60000);
                    break;
                case "instock":
                    filtered = _allProducts.Where(p => p.InStock);
                    break;
                default:
                    filtered = _allProducts;
                    break;
            }

            string search = SearchBox.Text?.Trim().ToLower() ?? "";
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(p =>
                    (p.Name ?? "").ToLower().Contains(search) ||
                    (p.Brand ?? "").ToLower().Contains(search) ||
                    (p.Description ?? "").ToLower().Contains(search));
            }

            var result = filtered.ToList();
            ProductsGrid.ItemsSource = result;
            EmptyLabel.Visibility = result.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            StatusLabel.Text = $"Показано {result.Count} из {_allProducts.Count} товаров";

            UpdateFilterButtons(filterTag);
        }

        private void UpdateFilterButtons(string activeTag)
        {
            var buttons = new[]
            {
                (FilterAll, "all"),
                (FilterBudget, "budget"),
                (FilterMid, "mid"),
                (FilterPremium, "premium"),
                (FilterInStock, "instock"),
            };

            foreach (var (btn, tag) in buttons)
            {
                if (btn == null) continue;

                bool isActive = tag == activeTag;
                btn.Background = isActive
                    ? new SolidColorBrush(Color.FromArgb(50, _accentColor.R, _accentColor.G, _accentColor.B))
                    : new SolidColorBrush(Color.FromArgb(20, 242, 242, 242));
                btn.Foreground = isActive
                    ? new SolidColorBrush(_accentColor)
                    : new SolidColorBrush(Color.FromArgb(136, 242, 242, 242));
            }
        }

        private void ApplyAccentColor(Color c)
        {
            try
            {
                AnimateBrush(AccentBarBrush, c);
                AnimateBrush(LogoAccentBrush, c);
                AnimateBrush(LogoDotBrush, c);
                AnimateBrush(TagLineBrush, c);
                AnimateBrush(TagBrush, c);
                AnimateBrush(Title2Brush, c);
                AnimateBrush(TitleAccentBrush, c);
                AnimateBrush(InStockTextBrush, c);

                if (InStockBadgeBrush != null)
                    InStockBadgeBrush.Color = Color.FromArgb(26, c.R, c.G, c.B);
            }
            catch { /* Игнорируем ошибки анимации */ }
        }

        private static void AnimateBrush(SolidColorBrush brush, Color to)
        {
            if (brush == null) return;

            brush.BeginAnimation(SolidColorBrush.ColorProperty,
                new ColorAnimation { To = to, Duration = TimeSpan.FromMilliseconds(500) });
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
                ApplyFilter(tag);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchPlaceholder.Visibility =
                string.IsNullOrEmpty(SearchBox.Text) ? Visibility.Visible : Visibility.Collapsed;
            ApplyFilter(_activeFilter);
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();
        private void BtnMinimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }
    }
}