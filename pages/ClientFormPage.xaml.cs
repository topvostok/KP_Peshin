using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Classes;
using WpfApp1.Models;

namespace WpfApp1.Pages
{
    public partial class ClientFormPage : Page
    {
        private int _id = 0;

        public ClientFormPage(int id = 0)
        {
            InitializeComponent();
            _id = id;
            Loaded += (s, e) => Init();
        }

        private void Init()
        {
            CbSkill.SelectedIndex = 1;
            if (_id > 0)
            {
                FormTitle.Text = "РЕДАКТИРОВАТЬ КЛИЕНТА"; // Исправлено
                BtnDelete.Visibility = Visibility.Visible;
                var cl = ClientRepo.GetById(_id);
                if (cl == null) return;
                TbName.Text = cl.FullName;
                TbPhone.Text = cl.Phone;
                TbEmail.Text = cl.Email;
                TbPassport.Text = cl.Passport;
                DpBirth.SelectedDate = cl.BirthDate;
                TbLoyalty.Text = cl.LoyaltyPts.ToString();
                TbNotes.Text = cl.Notes;
                SetCombo(CbSkill, cl.SkillLevel);
            }
        }

        private void SetCombo(ComboBox cb, string val)
        {
            foreach (ComboBoxItem item in cb.Items)
                if (item.Content?.ToString() == val) { cb.SelectedItem = item; return; }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbName.Text) || string.IsNullOrWhiteSpace(TbPhone.Text))
            {
                MessageBox.Show("Заполните ФИО и телефон.", "Ошибка");
                return;
            }

            if (!int.TryParse(TbLoyalty.Text, out int pts)) pts = 0;

            var cl = new RentalClient
            {
                Id = _id,
                FullName = TbName.Text.Trim(),
                Phone = TbPhone.Text.Trim(),
                Email = TbEmail.Text.Trim(),
                Passport = TbPassport.Text.Trim(),
                SkillLevel = (CbSkill.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Любитель",
                BirthDate = DpBirth.SelectedDate,
                LoyaltyPts = pts,
                Notes = TbNotes.Text.Trim(),
            };

            try
            {
                if (_id == 0) ClientRepo.Insert(cl);
                else ClientRepo.Update(cl);
                AdminWindow.Instance.GoTo("ClList", AdminWindow.Instance.NavClList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var r = MessageBox.Show("РЈРґР°Р»РёС‚СЊ РєР»РёРµРЅС‚Р°?", "РџРѕРґС‚РІРµСЂР¶РґРµРЅРёРµ",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (r == MessageBoxResult.Yes)
            {
                try { ClientRepo.Delete(_id); AdminWindow.Instance.GoTo("ClList", AdminWindow.Instance.NavClList); }
                catch (Exception ex)
                { MessageBox.Show("РћС€РёР±РєР°: " + ex.Message, "РћС€РёР±РєР°", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
            => AdminWindow.Instance.GoTo("ClList", AdminWindow.Instance.NavClList);
    }
}
