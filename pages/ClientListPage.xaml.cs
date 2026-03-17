using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.Classes;
using WpfApp1.Models;

namespace WpfApp1.Pages
{
    public partial class ClientListPage : Page
    {
        public ClientListPage() { InitializeComponent(); Loaded += (s, e) => Load(); }

        private void Load(string search = "")
        {
            try
            {
                var list = ClientRepo.GetAll(search);
                Grid0.ItemsSource = list;
                SubTitle.Text = $"{list.Count} РєР»РёРµРЅС‚РѕРІ Р·Р°СЂРµРіРёСЃС‚СЂРёСЂРѕРІР°РЅРѕ";
                S0.Text = list.Count.ToString();
                S1.Text = list.Count(x => x.SkillLevel == "РџСЂРѕС„РµСЃСЃРёРѕРЅР°Р»").ToString();
                S2.Text = list.Count(x => x.SkillLevel == "РќРѕРІРёС‡РѕРє").ToString();
                S3.Text = list.Any() ? $"{list.Average(x => x.LoyaltyPts):N0} pts" : "вЂ”";
            }
            catch (Exception ex)
            {
                MessageBox.Show("РћС€РёР±РєР° Р·Р°РіСЂСѓР·РєРё: " + ex.Message, "РћС€РёР±РєР°",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchBox_TextChanged(object s, TextChangedEventArgs e) => Load(SearchBox.Text);
        private void BtnRefresh_Click(object s, RoutedEventArgs e) => Load(SearchBox.Text);
        private void BtnAdd_Click(object s, RoutedEventArgs e)
            => AdminWindow.Instance.GoTo("ClAdd", null);

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is int id)
            {
                AdminWindow.Instance.AdminFrame.Navigate(new ClientFormPage(id));
                AdminWindow.Instance.PageTitle.Text = "РљР»РёРµРЅС‚С‹ вЂ” СЂРµРґР°РєС‚РёСЂРѕРІР°С‚СЊ";
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is int id)
            {
                var cl = ClientRepo.GetById(id);
                if (cl == null) return;
                var r = MessageBox.Show($"РЈРґР°Р»РёС‚СЊ РєР»РёРµРЅС‚Р° В«{cl.FullName}В»?", "РџРѕРґС‚РІРµСЂР¶РґРµРЅРёРµ",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes)
                {
                    try { ClientRepo.Delete(id); Load(SearchBox.Text); }
                    catch (Exception ex)
                    { MessageBox.Show("РћС€РёР±РєР°: " + ex.Message, "РћС€РёР±РєР°", MessageBoxButton.OK, MessageBoxImage.Error); }
                }
            }
        }

        private void Grid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Grid0.SelectedItem is RentalClient cl)
            {
                AdminWindow.Instance.AdminFrame.Navigate(new ClientFormPage(cl.Id));
                AdminWindow.Instance.PageTitle.Text = "РљР»РёРµРЅС‚С‹ вЂ” СЂРµРґР°РєС‚РёСЂРѕРІР°С‚СЊ";
            }
        }
    }
}
