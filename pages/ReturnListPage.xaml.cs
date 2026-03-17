using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.Classes;
using WpfApp1.Models;

namespace WpfApp1.Pages
{
    public partial class ReturnListPage : Page
    {
        public ReturnListPage() { InitializeComponent(); Loaded += (s, e) => Load(); }

        private void Load(string search = "")
        {
            try
            {
                var list = ReturnRepo.GetAll(search);
                Grid0.ItemsSource = list;
                SubTitle.Text = $"{list.Count} возвратов оформлено";
                S0.Text = list.Count.ToString();
                S1.Text = list.Count(x => x.ConditionAfter == "Отлично").ToString();
                S2.Text = list.Count(x => x.ConditionAfter == "Повреждено" || x.ConditionAfter == "Утеряно").ToString();
                S3.Text = list.Any() ? $"₽{list.Sum(x => (double)x.PenaltyAmount):N0}" : "—";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchBox_TextChanged(object s, TextChangedEventArgs e) => Load(SearchBox.Text);
        private void BtnRefresh_Click(object s, RoutedEventArgs e) => Load(SearchBox.Text);
        private void BtnAdd_Click(object s, RoutedEventArgs e)
            => AdminWindow.Instance.GoTo("RetAdd", null);

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is int id)
            {
                AdminWindow.Instance.AdminFrame.Navigate(new ReturnFormPage(id));
                AdminWindow.Instance.PageTitle.Text = "Возвраты — редактировать";
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is int id)
            {
                var r = MessageBox.Show("Удалить запись о возврате?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes)
                {
                    try { ReturnRepo.Delete(id); Load(SearchBox.Text); }
                    catch (Exception ex)
                    { MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
                }
            }
        }

        private void Grid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Grid0.SelectedItem is Return ret)
            {
                AdminWindow.Instance.AdminFrame.Navigate(new ReturnFormPage(ret.Id));
                AdminWindow.Instance.PageTitle.Text = "Возвраты — редактировать";
            }
        }
    }
}
