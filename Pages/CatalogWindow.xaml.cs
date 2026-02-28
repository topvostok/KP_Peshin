using System;
using System.Collections.Generic;
using System.Data.Common;
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
    /// <summary>
    /// Окно каталога — отображает товары одного раздела из БД.
    /// Открывается из главного окна по нажатию кнопки «Весь каталог».
    /// </summary>
    public partial class CatalogWindow : Window
    {
        // ── Параметры окна ─────────────────────────────────────────────────
        // Категория товаров (skis / snowboard / goggles / helmets)
        private readonly string _category;

        // Акцентный цвет раздела (передаётся из MainWindow)
        private readonly Color _accentColor;

        // ── Данные ────────────────────────────────────────────────────────
        // Полный список товаров из БД для текущей категории
        private List<AlpineProductModel> _allProducts = new List<AlpineProductModel>();

        // Активный фильтр (all / budget / mid / premium / instock)
        private string _activeFilter = "all";

        // ── Конструктор ───────────────────────────────────────────────────
        /// <summary>
        /// Создаёт окно каталога для указанной категории.
        /// </summary>
        /// <param name="category">Категория БД: skis | snowboard | goggles | helmets</param>
        /// <param name="accentColor">Акцентный цвет раздела для подсветки UI</param>
        /// <param name="tag">Тег (напр. "ГОРНЫЕ ЛЫЖИ")</param>
        /// <param name="titleLine1">Первая строка заголовка (напр. "ГОРНЫЕ")</param>
        /// <param name="titleLine2">Вторая строка заголовка (напр. "ЛЫЖИ")</param>
        /// <param name="bgImageUrl">URL фонового изображения раздела</param>
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

            // Применяем акцентный цвет ко всем кистям окна
            ApplyAccentColor(accentColor);

            // Заполняем тексты заголовка
            PageTagLabel.Text = tag;
            TitleLine1Label.Text = titleLine1;
            TitleLine2Label.Text = titleLine2;
            WindowCategoryLabel.Text = tag;

            // Загружаем фон раздела
            if (!string.IsNullOrEmpty(bgImageUrl))
                LoadBgImage(bgImageUrl);

            // Загрузка товаров из БД + привязка SearchBox
            Loaded += OnLoaded;
        }

        // ── Loaded ────────────────────────────────────────────────────────
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Анимация акцентной полосы при открытии
            var barAnim = new DoubleAnimation
            {
                From = 0,
                To = ActualHeight,
                Duration = TimeSpan.FromMilliseconds(600),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            AccentBar.BeginAnimation(HeightProperty, barAnim);

            // Загрузка товаров
            LoadProducts();
        }

        // ── Загрузка товаров из БД ────────────────────────────────────────
        private void LoadProducts()
        {
            StatusLabel.Text = "Загрузка данных из базы…";
            ItemCountLabel.Text = "Загрузка…";

            // Получаем список из DBConnection
            _allProducts = DBConnection.LoadProductsByCategory(_category);

            // Обновляем счётчики
            int inStockCount = _allProducts.Count(p => p.InStock);
            ItemCountLabel.Text = $"{_allProducts.Count} товаров";
            InStockCountLabel.Text = $"{inStockCount} в наличии";
            StatusLabel.Text = $"Загружено {_allProducts.Count} товаров";

            // Применяем активный фильтр (по умолчанию «Все»)
            ApplyFilter(_activeFilter);
        }

        // ── Фильтрация ────────────────────────────────────────────────────
        private void ApplyFilter(string filterTag)
        {
            _activeFilter = filterTag;
            IEnumerable<AlpineProductModel> filtered = _allProducts;

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
                default: // "all"
                    filtered = _allProducts;
                    break;
            }

            // Если есть текст в поиске — дополнительно фильтруем
            string search = SearchBox.Text?.Trim().ToLower() ?? "";
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(p =>
                    (p.Name ?? "").ToLower().Contains(search) ||
                    (p.Brand ?? "").ToLower().Contains(search));
            }

            var result = filtered.ToList();

            // Добавляем вычислимое свойство IsOutOfStock для XAML биндинга
            var vm = result.Select(p => new CatalogProductViewModel(p)).ToList();

            ProductsGrid.ItemsSource = vm;
            EmptyLabel.Visibility = vm.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            StatusLabel.Text = $"Показано {vm.Count} из {_allProducts.Count} товаров";

            // Обновляем подсветку активного фильтра
            UpdateFilterButtons(filterTag);
        }

        // ── Подсветка активного фильтра ───────────────────────────────────
        private void UpdateFilterButtons(string activeTag)
        {
            var buttons = new[]
            {
                (FilterAll,     "all"),
                (FilterBudget,  "budget"),
                (FilterMid,     "mid"),
                (FilterPremium, "premium"),
                (FilterInStock, "instock"),
            };

            foreach (var (btn, tag) in buttons)
            {
                bool isActive = tag == activeTag;
                btn.Background = isActive
                    ? new SolidColorBrush(Color.FromArgb(50, _accentColor.R, _accentColor.G, _accentColor.B))
                    : new SolidColorBrush(Color.FromArgb(20, 242, 242, 242));
                btn.Foreground = isActive
                    ? new SolidColorBrush(_accentColor)
                    : new SolidColorBrush(Color.FromArgb(136, 242, 242, 242));
            }
        }

        // ── Применение акцентного цвета ───────────────────────────────────
        private void ApplyAccentColor(Color c)
        {
            AnimateBrush(AccentBarBrush, c);
            AnimateBrush(LogoAccentBrush, c);
            AnimateBrush(LogoDotBrush, c);
            AnimateBrush(TagLineBrush, c);
            AnimateBrush(TagBrush, c);
            AnimateBrush(Title2Brush, c);
            AnimateBrush(TitleAccentBrush, c);
            AnimateBrush(InStockTextBrush, c);
            InStockBadgeBrush.Color = Color.FromArgb(26, c.R, c.G, c.B);
        }

        private static void AnimateBrush(SolidColorBrush brush, Color to)
        {
            brush.BeginAnimation(SolidColorBrush.ColorProperty,
                new ColorAnimation { To = to, Duration = TimeSpan.FromMilliseconds(500) });
        }

        // ── Загрузка фонового изображения ─────────────────────────────────
        private async void LoadBgImage(string url)
        {
        //    try
        //    {
        //        using var http = new System.Net.Http.HttpClient();
        //        var bytes = await http.GetByteArrayAsync(url);
        //        var bmp = new System.Windows.Media.Imaging.BitmapImage();
        //        bmp.BeginInit();
        //        bmp.StreamSource = new System.IO.MemoryStream(bytes);
        //        bmp.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
        //        bmp.EndInit();
        //        bmp.Freeze();
        //        Dispatcher.Invoke(() => BgImage.Source = bmp);
        //    }
        //    catch { /* молча игнорируем */ }
        }

        // ── Обработчики событий UI ────────────────────────────────────────

        /// <summary>Клик по кнопке-фильтру</summary>
        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
                ApplyFilter(tag);
        }

        /// <summary>Изменение текста поиска — скрываем/показываем плейсхолдер и фильтруем</summary>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchPlaceholder.Visibility =
                string.IsNullOrEmpty(SearchBox.Text) ? Visibility.Visible : Visibility.Collapsed;
            ApplyFilter(_activeFilter);
        }

        // ── Управление окном ──────────────────────────────────────────────
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

    // ─────────────────────────────────────────────────────────────────────
    //  ViewModel-обёртка для карточки товара (добавляет IsOutOfStock)
    // ─────────────────────────────────────────────────────────────────────
    /// <summary>
    /// Передаётся как DataContext для каждой карточки в ItemsControl.
    /// </summary>
    public class CatalogProductViewModel
    {
        public int Id { get; }
        public string Name { get; }
        public string Brand { get; }
        public string Category { get; }
        public decimal Price { get; }
        public string ImageUrl { get; }
        public string Description { get; }
        public bool InStock { get; }
        public int StockQty { get; }
        public string Specs { get; }

        // Для XAML: показывает бейдж «Нет в наличии»
        public bool IsOutOfStock => !InStock;
        // Отформатированная цена
        public string PriceDisplay => $"₽{Price:N0}";

        public CatalogProductViewModel(AlpineProductModel m)
        {
            Id = m.Id;
            Name = m.Name;
            Brand = m.Brand;
            Category = m.Category;
            Price = m.Price;
            ImageUrl = m.ImageUrl;
            Description = m.Description;
            InStock = m.InStock;
            StockQty = m.StockQty;
            Specs = m.Specs;
        }
    }
}