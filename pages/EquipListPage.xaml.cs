using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.Classes;
using WpfApp1.Models;

namespace WpfApp1.Pages
{
    public partial class EquipListPage : Page
    {
        public EquipListPage() { InitializeComponent(); Loaded += (s, e) => Load(); }

        private void Load(string search = "")
        {
            try
            {
                var list = EquipmentRepo.GetAll(search);
                Grid0.ItemsSource = list;
                SubTitle.Text = $"{list.Count} позиций в каталоге";
                S0.Text = list.Count.ToString();
                S1.Text = list.Count(x => x.ConditionLvl == "Новое").ToString();
                S2.Text = list.Count(x => x.ConditionLvl == "Требует ремонта").ToString();
                S3.Text = list.Any() ? $"₽{list.Average(x => (double)x.PricePerDay):N0}" : "—";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchBox_TextChanged(object s, TextChangedEventArgs e)
            => Load(SearchBox.Text);

        private void BtnRefresh_Click(object s, RoutedEventArgs e) => Load(SearchBox.Text);

        private void BtnAdd_Click(object s, RoutedEventArgs e)
            => AdminWindow.Instance.GoTo("EqAdd", null);

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is int id)
            {
                var page = new EquipFormPage(id);
                AdminWindow.Instance.AdminFrame.Navigate(page);
                AdminWindow.Instance.PageTitle.Text = "Снаряжение — редактировать";
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is int id)
            {
                var eq = EquipmentRepo.GetById(id);
                if (eq == null) return;
                var r = MessageBox.Show($"Удалить «{eq.Name}»?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes)
                {
                    try { EquipmentRepo.Delete(id); Load(SearchBox.Text); }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка удаления: " + ex.Message, "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Grid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Grid0.SelectedItem is Equipment eq)
            {
                var page = new EquipFormPage(eq.Id);
                AdminWindow.Instance.AdminFrame.Navigate(page);
                AdminWindow.Instance.PageTitle.Text = "Снаряжение — редактировать";
            }
        }
    }
}
