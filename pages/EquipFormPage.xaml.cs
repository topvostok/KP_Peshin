using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Classes;
using WpfApp1.Models;

namespace WpfApp1.Pages
{
    public partial class EquipFormPage : Page
    {
        private int _id = 0;

        public EquipFormPage(int id = 0)
        {
            InitializeComponent();
            _id = id;

            // Устанавливаем значения по умолчанию
            CbCategory.SelectedIndex = 0; // Лыжи
            CbCondition.SelectedIndex = 1; // Хорошее

            // Если редактируем - загружаем данные
            if (_id > 0)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            try
            {
                FormTitle.Text = "РЕДАКТИРОВАТЬ СНАРЯЖЕНИЕ";
                BtnDelete.Visibility = Visibility.Visible;

                var eq = EquipmentRepo.GetById(_id);
                if (eq == null)
                {
                    MessageBox.Show("Снаряжение не найдено!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Заполняем поля
                TbName.Text = eq.Name;
                TbBrand.Text = eq.Brand;
                TbSize.Text = eq.SizeValue;
                TbPrice.Text = eq.PricePerDay.ToString();
                TbStock.Text = eq.StockQty.ToString();
                TbImage.Text = eq.ImageUrl;
                TbNotes.Text = eq.Notes;

                // Выбираем категорию
                SelectComboBoxItem(CbCategory, eq.Category);

                // Выбираем состояние
                SelectComboBoxItem(CbCondition, eq.ConditionLvl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectComboBoxItem(ComboBox comboBox, string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Content.ToString() == text)
                {
                    comboBox.SelectedItem = item;
                    return;
                }
            }
        }

        private string GetSelectedComboBoxText(ComboBox comboBox)
        {
            if (comboBox.SelectedItem == null) return "";

            if (comboBox.SelectedItem is ComboBoxItem item)
            {
                return item.Content.ToString();
            }

            return comboBox.SelectedItem.ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка обязательных полей
                if (string.IsNullOrWhiteSpace(TbName.Text))
                {
                    MessageBox.Show("Введите название!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TbBrand.Text))
                {
                    MessageBox.Show("Введите бренд!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверка цены
                if (!decimal.TryParse(TbPrice.Text, out decimal price))
                {
                    MessageBox.Show("Введите корректную цену!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверка количества
                if (!int.TryParse(TbStock.Text, out int stock))
                {
                    MessageBox.Show("Введите корректное количество!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Получаем значения из ComboBox
                string category = GetSelectedComboBoxText(CbCategory);
                string condition = GetSelectedComboBoxText(CbCondition);

                // Создаем объект
                var eq = new Equipment
                {
                    Id = _id,
                    Name = TbName.Text.Trim(),
                    Brand = TbBrand.Text.Trim(),
                    Category = category,
                    ConditionLvl = condition,
                    SizeValue = string.IsNullOrWhiteSpace(TbSize.Text) ? null : TbSize.Text.Trim(),
                    PricePerDay = price,
                    StockQty = stock,
                    ImageUrl = string.IsNullOrWhiteSpace(TbImage.Text) ? null : TbImage.Text.Trim(),
                    Notes = string.IsNullOrWhiteSpace(TbNotes.Text) ? null : TbNotes.Text.Trim()
                };

                // Сохраняем
                if (_id == 0)
                {
                    EquipmentRepo.Insert(eq);
                    MessageBox.Show("Снаряжение добавлено!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    EquipmentRepo.Update(eq);
                    MessageBox.Show("Снаряжение обновлено!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Возвращаемся к списку
                AdminWindow.Instance.GoTo("EqList", AdminWindow.Instance.NavEqList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_id == 0) return;

            var result = MessageBox.Show("Удалить это снаряжение?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    EquipmentRepo.Delete(_id);
                    MessageBox.Show("Снаряжение удалено!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    AdminWindow.Instance.GoTo("EqList", AdminWindow.Instance.NavEqList);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow.Instance.GoTo("EqList", AdminWindow.Instance.NavEqList);
        }
    }
}