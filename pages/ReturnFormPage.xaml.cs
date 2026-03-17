using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Classes;
using WpfApp1.Models;

namespace WpfApp1.Pages
{
    // Helper для отображения аренды в ComboBox
    public class RentalComboItem
    {
        public int    Id          { get; set; }
        public string DisplayText { get; set; }
    }

    public partial class ReturnFormPage : Page
    {
        private int _id = 0;

        public ReturnFormPage(int id = 0)
        {
            InitializeComponent();
            _id = id;
            Loaded += (s, e) => Init();
        }

        private void Init()
        {
            // Показываем только активные и просроченные аренды
            var rentals = RentalRepo.GetAll();
            var items = new List<RentalComboItem>();
            foreach (var rn in rentals)
            {
                if (rn.Status == "Активна" || rn.Status == "Просрочена")
                    items.Add(new RentalComboItem
                    {
                        Id = rn.Id,
                        DisplayText = $"#{rn.Id} — {rn.ClientName} / {rn.EquipmentName} ({rn.RentDateDisplay})"
                    });
            }
            CbRental.ItemsSource = items;
            CbCondition.SelectedIndex = 0;
            DpReturn.SelectedDate = DateTime.Today;

            if (_id > 0)
            {
                FormTitle.Text = "РЕДАКТИРОВАТЬ ВОЗВРАТ";
                BtnDelete.Visibility = Visibility.Visible;
                var ret = ReturnRepo.GetById(_id);
                if (ret == null) return;

                // Add current rental even if completed
                var allRentals = RentalRepo.GetAll();
                foreach (var rn in allRentals)
                {
                    if (rn.Id == ret.RentalId && !items.Exists(x => x.Id == rn.Id))
                        items.Insert(0, new RentalComboItem
                        {
                            Id = rn.Id,
                            DisplayText = $"#{rn.Id} — {rn.ClientName} / {rn.EquipmentName} ({rn.RentDateDisplay})"
                        });
                }
                CbRental.ItemsSource = null;
                CbRental.ItemsSource = items;

                foreach (RentalComboItem item in CbRental.Items)
                    if (item.Id == ret.RentalId) { CbRental.SelectedItem = item; break; }

                DpReturn.SelectedDate = ret.ReturnDate;
                SetCombo(CbCondition, ret.ConditionAfter);
                TbPenalty.Text   = ret.PenaltyAmount.ToString();
                TbRefund.Text    = ret.RefundAmount.ToString();
                TbInspector.Text = ret.InspectorName;
                TbNotes.Text     = ret.Notes;
            }
        }

        private void SetCombo(ComboBox cb, string val)
        {
            foreach (ComboBoxItem item in cb.Items)
                if (item.Content?.ToString() == val) { cb.SelectedItem = item; return; }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CbRental.SelectedItem == null)
            { MessageBox.Show("Выберите аренду.", "Ошибка"); return; }

            if (DpReturn.SelectedDate == null)
            { MessageBox.Show("Укажите дату возврата.", "Ошибка"); return; }

            if (!decimal.TryParse(TbPenalty.Text, out decimal pen)) pen = 0;
            if (!decimal.TryParse(TbRefund.Text,  out decimal ref_)) ref_ = 0;

            var ret = new Return
            {
                Id             = _id,
                RentalId       = ((RentalComboItem)CbRental.SelectedItem).Id,
                ReturnDate     = DpReturn.SelectedDate.Value,
                ConditionAfter = (CbCondition.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Хорошо",
                PenaltyAmount  = pen,
                RefundAmount   = ref_,
                InspectorName  = TbInspector.Text.Trim(),
                Notes          = TbNotes.Text.Trim(),
            };

            try
            {
                if (_id == 0) ReturnRepo.Insert(ret);
                else          ReturnRepo.Update(ret);
                AdminWindow.Instance.GoTo("RetList", AdminWindow.Instance.NavRetList);
            }
            catch (Exception ex)
            { MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var r = MessageBox.Show("Удалить возврат?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (r == MessageBoxResult.Yes)
            {
                try { ReturnRepo.Delete(_id); AdminWindow.Instance.GoTo("RetList", AdminWindow.Instance.NavRetList); }
                catch (Exception ex)
                { MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
            => AdminWindow.Instance.GoTo("RetList", AdminWindow.Instance.NavRetList);
    }
}
