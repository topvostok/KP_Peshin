using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Classes;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        // ── Data ───────────────────────────────────────────────
        private readonly List<SectionModel> _sections = new List<SectionModel>
        {
            new SectionModel
            {
                Tag          = "ГОРНЫЕ ЛЫЖИ",
                TitleLine1   = "ГОРНЫЕ",
                TitleLine2   = "ЛЫЖИ",
                Description  = "Профессиональные горные лыжи для любого рельефа и стиля катания. От крутых трасс до мягкого пухляка — снаряжение уровня чемпионов.",
                AccentColor  = Color.FromRgb(0x5D, 0xE0, 0xFF),
                CtaText      = "Весь каталог лыж",
                BgImageUrl   = "https://static.tildacdn.com/tild3338-6236-4234-a434-353232316336/photo_2025-03-11_15-.jpg",
                SegImageUrl  = "https://static.tildacdn.com/tild3338-6236-4234-a434-353232316336/photo_2025-03-11_15-.jpg",
                SegLabel     = "ЛЫЖИ",
                Products     = new List<ProductModel>
                {
                    new ProductModel { ImageUrl = "https://avatars.mds.yandex.net/get-mpic/18718263/2a0000019ac964f4cd616ffcadf9bc318515/orig", Name = "Atomic Redster G9",    Price = "₽54 990" },
                    new ProductModel { ImageUrl = "https://avatars.mds.yandex.net/get-mpic/1605421/2a00000194bca77d4214fb2af2364ac9532d/orig", Name = "Rossignol Hero Elite", Price = "₽48 500" },
                    new ProductModel { ImageUrl = "https://ir.ozone.ru/s3/multimedia-1-b/w1200/7247468963.jpg", Name = "Salomon S/Race SL",   Price = "₽61 000" },
                }
            },
            new SectionModel
            {
                Tag          = "СНОУБОРДЫ",
                TitleLine1   = "СНОУ-",
                TitleLine2   = "БОРДЫ",
                Description  = "Фрирайд, трассы, парк — найди свою доску из 180+ моделей. Скорость, контроль и стиль от ведущих мировых марок.",
                AccentColor  = Color.FromRgb(0xFF, 0x5C, 0x3A),
                CtaText      = "Выбрать сноуборд",
                BgImageUrl   = "https://images.unsplash.com/photo-1519415943484-9fa1873496d4?auto=format&fit=crop&w=1600&q=90",
                SegImageUrl  = "https://images.unsplash.com/photo-1519415943484-9fa1873496d4?auto=format&fit=crop&w=900&q=85",
                SegLabel     = "СНОУБОРД",
                Products     = new List<ProductModel>
                {
                    new ProductModel { ImageUrl = "https://images.unsplash.com/photo-1519415943484-9fa1873496d4?auto=format&fit=crop&w=400&h=300&q=85", Name = "Burton Custom",       Price = "₽72 000" },
                    new ProductModel { ImageUrl = "https://images.unsplash.com/photo-1478401676105-21a85e0cebf4?auto=format&fit=crop&w=400&h=300&q=85", Name = "Jones Mind Expander", Price = "₽65 500" },
                    new ProductModel { ImageUrl = "https://images.unsplash.com/photo-1545126913-a647e4893f89?auto=format&fit=crop&w=400&h=300&q=85", Name = "Lib Tech T.Rice",     Price = "₽59 900" },
                }
            },
            new SectionModel
            {
                Tag          = "МАСКИ И ОЧКИ",
                TitleLine1   = "ГОРНО-",
                TitleLine2   = "ЛЫЖНЫЕ МАСКИ",
                Description  = "Максимальная видимость в любую погоду. Антифог-покрытие, сферические линзы и широкое поле зрения для безопасного спуска.",
                AccentColor  = Color.FromRgb(0xA3, 0xFF, 0x5C),
                CtaText      = "Подобрать маску",
                BgImageUrl   = "https://images.unsplash.com/photo-1546961342-ea5f62d96a91?auto=format&fit=crop&w=1600&q=90",
                SegImageUrl  = "https://images.unsplash.com/photo-1546961342-ea5f62d96a91?auto=format&fit=crop&w=900&q=85",
                SegLabel     = "МАСКИ",
                Products     = new List<ProductModel>
                {
                    new ProductModel { ImageUrl = "https://images.unsplash.com/photo-1546961342-ea5f62d96a91?auto=format&fit=crop&w=400&h=300&q=85", Name = "Oakley Flight Pro", Price = "₽18 900" },
                    new ProductModel { ImageUrl = "https://images.unsplash.com/photo-1611442530474-4db6adcc37b2?auto=format&fit=crop&w=400&h=300&q=85", Name = "Smith 4D MAG",      Price = "₽22 500" },
                    new ProductModel { ImageUrl = "https://images.unsplash.com/photo-1605152822754-b2ea8b69e0ab?auto=format&fit=crop&w=400&h=300&q=85", Name = "Anon M4 MIPS",     Price = "₽15 700" },
                }
            },
            new SectionModel
            {
                Tag          = "ШЛЕМЫ",
                TitleLine1   = "ГОРНО-",
                TitleLine2   = "ЛЫЖНЫЕ ШЛЕМЫ",
                Description  = "MIPS-технология, регулируемая вентиляция и интеграция маски. Защита, которой можно доверять на самых крутых трассах.",
                AccentColor  = Color.FromRgb(0xC8, 0x7D, 0xFF),
                CtaText      = "Выбрать шлем",
                BgImageUrl   = "https://images.unsplash.com/photo-1567013127542-490d757e6349?auto=format&fit=crop&w=1600&q=90",
                SegImageUrl  = "https://images.unsplash.com/photo-1567013127542-490d757e6349?auto=format&fit=crop&w=900&q=85",
                SegLabel     = "ШЛЕМЫ",
                Products     = new List<ProductModel>
                {
                    new ProductModel { ImageUrl = "https://images.unsplash.com/photo-1567013127542-490d757e6349?auto=format&fit=crop&w=400&h=300&q=85", Name = "Uvex Fierce MIPS",  Price = "₽24 900" },
                    new ProductModel { ImageUrl = "https://images.unsplash.com/photo-1580048915913-4f8f5cb481c4?auto=format&fit=crop&w=400&h=300&q=85", Name = "POC Skull X SPIN",  Price = "₽31 500" },
                    new ProductModel { ImageUrl = "https://images.unsplash.com/photo-1564466809058-bf4114d55352?auto=format&fit=crop&w=400&h=300&q=85", Name = "Giro Range MIPS",   Price = "₽19 800" },
                }
            },
        };

        // ── State ──────────────────────────────────────────────
        private int _current = 0;
        private double _rotation = 0;
        private bool _busy = false;

        // Dot controls
        private readonly List<Border> _dots = new List<Border>();

        // Segment grids (for click & active tint)
        private readonly Grid[] _segs;
        private readonly Rectangle[] _segTints;
        private readonly Image[] _segImgs;

        // Http client for image loading
        private static readonly HttpClient _http = new HttpClient();

        // ── Constructor ────────────────────────────────────────
        public MainWindow()
        {
            InitializeComponent();

            _segs = new[] { Seg0, Seg1, Seg2, Seg3 };
            _segTints = new[] { SegTint0, SegTint1, SegTint2, SegTint3 };
            _segImgs = new[] { SegImg0, SegImg1, SegImg2, SegImg3 };

            Loaded += OnLoaded;
            MouseWheel += OnMouseWheel;
            SizeChanged += OnSizeChanged;

            // Segment click handlers
            for (int i = 0; i < 4; i++)
            {
                int idx = i;
                _segs[i].MouseLeftButtonDown += (s, e) => { if (!_busy) GoTo(idx); };
            }
        }

        // ── Loaded ─────────────────────────────────────────────
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            BuildDots();
            PositionCircle();
            LoadSegmentImages();
            ApplySection(0, animate: false);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e) => PositionCircle();

        // ── Circle Positioning ─────────────────────────────────
        private void PositionCircle()
        {
            double panelW = RightPanel.ActualWidth > 0 ? RightPanel.ActualWidth : 600;
            double panelH = RightPanel.ActualHeight > 0 ? RightPanel.ActualHeight : 750;

            double circleSize = 1700;
            double leftEdge = panelW * 0.22;
            double centerX = leftEdge + circleSize / 2;
            double centerY = panelH / 2;

            CircleContainer.Width = circleSize;
            CircleContainer.Height = circleSize;

            Canvas.SetLeft(CircleContainer, centerX - circleSize / 2);
            Canvas.SetTop(CircleContainer, centerY - circleSize / 2);

            CircleCanvas.Width = panelW;
            CircleCanvas.Height = panelH;
        }

        // ── Build page indicator dots ──────────────────────────
        private void BuildDots()
        {
            PageDotsPanel.Children.Clear();
            _dots.Clear();

            for (int i = 0; i < _sections.Count; i++)
            {
                int idx = i;
                var dot = new Border
                {
                    Height = 5,
                    CornerRadius = new CornerRadius(3),
                    Cursor = Cursors.Hand,
                    Margin = new Thickness(0, 0, 8, 0),
                    Width = i == 0 ? 26 : 5,
                    Background = i == 0
                        ? new SolidColorBrush(_sections[0].AccentColor)
                        : new SolidColorBrush(Color.FromArgb(50, 242, 242, 242)),
                };
                dot.MouseLeftButtonDown += (s, e) => { if (!_busy) GoTo(idx); };
                _dots.Add(dot);
                PageDotsPanel.Children.Add(dot);
            }
        }

        // ── Section navigation ─────────────────────────────────
        private void GoTo(int idx)
        {
            if (_busy || idx == _current) return;
            _busy = true;

            int diff = idx - _current;
            if (diff > 2) diff -= 4;
            if (diff < -2) diff += 4;
            _rotation -= diff * 90.0;

            var rotAnim = new DoubleAnimation
            {
                To = _rotation,
                Duration = TimeSpan.FromMilliseconds(900),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };
            SpinnerRotate.BeginAnimation(RotateTransform.AngleProperty, rotAnim);

            AnimateOverlay(() =>
            {
                _current = idx;
                ApplySection(idx, animate: true);
                UpdateDots(idx);
                UpdateActiveSegment(idx);
                _busy = false;
            });
        }

        // ── Apply section data to UI ───────────────────────────
        private void ApplySection(int idx, bool animate)
        {
            var sec = _sections[idx];

            SetBrushColor(AccentBarBrush, sec.AccentColor);
            SetBrushColor(LogoAccentBrush, sec.AccentColor);
            SetBrushColor(LogoDotBrush, sec.AccentColor);
            SetBrushColor(TagLineBrush, sec.AccentColor);
            SetBrushColor(TagBrush, sec.AccentColor);
            SetBrushColor(Title2Brush, sec.AccentColor);
            SetBrushColor(HubNumBrush, sec.AccentColor);
            SetBrushColor(CtaBrush, sec.AccentColor);

            var barAnim = new DoubleAnimation
            {
                To = RootGrid.ActualHeight * 0.60,
                Duration = TimeSpan.FromMilliseconds(700),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            AccentBar.BeginAnimation(FrameworkElement.HeightProperty, barAnim);

            PageTag.Text = sec.Tag;
            TitleLine1.Text = sec.TitleLine1;
            TitleLine2.Text = sec.TitleLine2;
            PageDesc.Text = sec.Description;
            CtaButton.Content = sec.CtaText;
            HubNumber.Text = (idx + 1).ToString("D2");

            ProductList.ItemsSource = sec.Products;

            LoadImageAsync(sec.BgImageUrl, bmp =>
            {
                if (BgImage0.Opacity > 0.5)
                {
                    BgImage1.Source = bmp;
                    FadeImage(BgImage1, 0, 1, 800);
                    FadeImage(BgImage0, 1, 0, 800);
                }
                else
                {
                    BgImage0.Source = bmp;
                    FadeImage(BgImage0, 0, 1, 800);
                    FadeImage(BgImage1, 1, 0, 800);
                }
            });

            if (animate)
            {
                SlideIn(Title1Transform, 80, 0, 0);
                SlideIn(Title2Transform, 80, 0, 50);
                FadeSlideIn(PageDesc, 0, 1, 120);
                FadeSlideIn(ProductList, 0, 1, 200);
                FadeSlideInPanel(CtaButton.Parent as StackPanel, 0, 1, 300);
            }
            else
            {
                Title1Transform.Y = 0;
                Title2Transform.Y = 0;
                PageDesc.Opacity = 1;
                ProductList.Opacity = 1;
            }
        }

        // ── Image Loading ──────────────────────────────────────
        private void LoadSegmentImages()
        {
            for (int i = 0; i < _sections.Count; i++)
            {
                int idx = i;
                LoadImageAsync(_sections[i].SegImageUrl, bmp =>
                {
                    _segImgs[idx].Source = bmp;
                });
            }
        }

        private async void LoadImageAsync(string url, Action<BitmapImage> callback)
        {
            try
            {
                var bytes = await _http.GetByteArrayAsync(url);
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new System.IO.MemoryStream(bytes);
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                bmp.Freeze();
                Dispatcher.Invoke(() => callback(bmp));
            }
            catch
            {
                // Silently ignore
            }
        }

        // ── UI Update Helpers ──────────────────────────────────
        private static void SetBrushColor(SolidColorBrush brush, Color color)
        {
            var anim = new ColorAnimation
            {
                To = color,
                Duration = TimeSpan.FromMilliseconds(500)
            };
            brush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
        }

        private void UpdateDots(int idx)
        {
            for (int i = 0; i < _dots.Count; i++)
            {
                bool active = i == idx;
                var wAnim = new DoubleAnimation
                {
                    To = active ? 26 : 5,
                    Duration = TimeSpan.FromMilliseconds(400),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
                };
                _dots[i].BeginAnimation(FrameworkElement.WidthProperty, wAnim);
                _dots[i].Background = active
                    ? new SolidColorBrush(_sections[idx].AccentColor)
                    : new SolidColorBrush(Color.FromArgb(50, 242, 242, 242));
            }
        }

        private void UpdateActiveSegment(int idx)
        {
            for (int i = 0; i < 4; i++)
            {
                bool active = i == idx;
                var opAnim = new DoubleAnimation
                {
                    To = active ? 0.12 : 0,
                    Duration = TimeSpan.FromMilliseconds(500)
                };
                _segTints[i].BeginAnimation(UIElement.OpacityProperty, opAnim);

                var imgAnim = new DoubleAnimation
                {
                    To = active ? 0.75 : 0.50,
                    Duration = TimeSpan.FromMilliseconds(500)
                };
                _segImgs[i].BeginAnimation(UIElement.OpacityProperty, imgAnim);
            }
        }

        // ── Transition Overlay ─────────────────────────────────
        private void AnimateOverlay(Action onMidpoint)
        {
            var fadeIn = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            fadeIn.Completed += (s, e) =>
            {
                onMidpoint();

                var fadeOut = new DoubleAnimation
                {
                    To = 0,
                    Duration = TimeSpan.FromMilliseconds(300)
                };
                TransitionOverlay.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            };
            TransitionOverlay.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        // ── Animation Helpers ──────────────────────────────────
        private static void SlideIn(TranslateTransform t, double from, double to, int delayMs)
        {
            t.Y = from;
            var anim = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromMilliseconds(650),
                BeginTime = TimeSpan.FromMilliseconds(delayMs),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            t.BeginAnimation(TranslateTransform.YProperty, anim);
        }

        private static void FadeSlideIn(UIElement el, double from, double to, int delayMs)
        {
            el.Opacity = from;
            var anim = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromMilliseconds(600),
                BeginTime = TimeSpan.FromMilliseconds(delayMs),
            };
            el.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        private static void FadeSlideInPanel(StackPanel el, double from, double to, int delayMs)
        {
            if (el == null) return;
            FadeSlideIn(el, from, to, delayMs);
        }

        private static void FadeImage(Image img, double from, double to, int durationMs)
        {
            var anim = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromMilliseconds(durationMs),
            };
            img.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        // ── Event Handlers ─────────────────────────────────────
        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (!_busy) GoTo((_current + 3) % 4);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (!_busy) GoTo((_current + 1) % 4);
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_busy) return;
            if (e.Delta < 0) GoTo((_current + 1) % 4);
            else GoTo((_current + 3) % 4);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (_busy) return;
            if (e.Key == Key.Right || e.Key == Key.Down) GoTo((_current + 1) % 4);
            if (e.Key == Key.Left || e.Key == Key.Up) GoTo((_current + 3) % 4);
        }

        private void CtaButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentSection = _sections[_current];

                // Определяем категорию для БД
                string dbCategory;
                if (currentSection.Tag == "ГОРНЫЕ ЛЫЖИ")
                    dbCategory = "skis";
                else if (currentSection.Tag == "СНОУБОРДЫ")
                    dbCategory = "snowboard";
                else if (currentSection.Tag == "МАСКИ И ОЧКИ")
                    dbCategory = "goggles";
                else if (currentSection.Tag == "ШЛЕМЫ")
                    dbCategory = "helmets";
                else
                    dbCategory = "skis";

                // Проверяем подключение к БД
                if (!DatabaseHelper.TestConnection(out _))
                {
                    var result = MessageBox.Show(
                        "Не удалось подключиться к базе данных.\n\nХотите открыть демо-режим с тестовыми данными?",
                        "Ошибка подключения",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        OpenDemoCatalog(currentSection);
                    }
                    return;
                }

                // Проверяем, есть ли товары в БД
                var testProducts = EquipmentShopRepository.GetProductsByCategory(dbCategory);
                if (testProducts == null || testProducts.Count == 0)
                {
                    var result = MessageBox.Show(
                        $"В базе данных нет товаров для категории \"{currentSection.Tag}\".\n\nХотите открыть демо-режим?",
                        "Нет данных",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information);

                    if (result == MessageBoxResult.Yes)
                    {
                        OpenDemoCatalog(currentSection);
                    }
                    return;
                }

                // Открываем каталог с данными из БД
                var catalogWin = new CatalogWindow(
                    dbCategory,
                    currentSection.AccentColor,
                    currentSection.Tag,
                    currentSection.TitleLine1,
                    currentSection.TitleLine2,
                    currentSection.BgImageUrl)
                {
                    Owner = this
                };
                catalogWin.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии каталога: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenDemoCatalog(SectionModel section)
        {
            try
            {
                var demoWin = new CatalogWindow(
                    "demo",
                    section.AccentColor,
                    section.Tag + " (ДЕМО)",
                    section.TitleLine1,
                    section.TitleLine2,
                    section.BgImageUrl)
                {
                    Owner = this
                };
                demoWin.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии демо-режима: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Открываем подбор по параметрам...", "Alpine Shop",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void BtnAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            var adminWin = new AdminWindow();
            adminWin.Show();
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