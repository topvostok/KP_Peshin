using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp1.Classes;
using WpfApp1.Pages;

namespace WpfApp1
{
    public partial class AdminWindow : Window
    {
        public static AdminWindow Instance { get; private set; }

        private Button _activeNav;

        public AdminWindow()
        {
            InitializeComponent();
            Instance = this;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Check DB
            bool ok = DatabaseHelper.TestConnection(out string err);
            DbDotBrush.Color = ok
                ? Color.FromRgb(0xA3, 0xFF, 0x5C)
                : Color.FromRgb(0xFF, 0x5C, 0x3A);
            DbStatusTxt.Text = ok ? "БД подключена" : "Нет подключения";

            // Start on equipment list
            GoTo("EqList", NavEqList);
        }

        public void GoTo(string page, Button navBtn = null)
        {
            if (_activeNav != null)
                _activeNav.Style = (Style)FindResource("BtnNav");

            if (navBtn != null)
            {
                navBtn.Style = (Style)FindResource("BtnNavActive");
                _activeNav = navBtn;
            }

            switch (page)
            {
                case "EqList": AdminFrame.Navigate(new EquipListPage()); PageTitle.Text = "Снаряжение — список"; break;
                case "EqAdd": AdminFrame.Navigate(new EquipFormPage()); PageTitle.Text = "Снаряжение — добавить"; break;
                case "ClList": AdminFrame.Navigate(new ClientListPage()); PageTitle.Text = "Клиенты — список"; break;
                case "ClAdd": AdminFrame.Navigate(new ClientFormPage()); PageTitle.Text = "Клиенты — добавить"; break;
                case "RnList": AdminFrame.Navigate(new RentalListPage()); PageTitle.Text = "Аренды — список"; break;
                case "RnAdd": AdminFrame.Navigate(new RentalFormPage()); PageTitle.Text = "Аренды — новая"; break;
                case "RetList": AdminFrame.Navigate(new ReturnListPage()); PageTitle.Text = "Возвраты — список"; break;
                case "RetAdd": AdminFrame.Navigate(new ReturnFormPage()); PageTitle.Text = "Возвраты — оформить"; break;
            }
        }

        private void NavBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
                GoTo(tag, btn);
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();
        private void BtnMin_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void BtnMax_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }
}
