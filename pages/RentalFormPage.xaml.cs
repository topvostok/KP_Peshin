using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Classes;
using WpfApp1.Models;

namespace WpfApp1.Pages
{
    public partial class RentalFormPage : Page
    {
        private int _id = 0;

        public RentalFormPage(int id = 0)
        {
            InitializeComponent();
            _id = id;
            Loaded += (s, e) => Init();
        }

        private void Init()
        {
            try
            {
                // Заполняем комбобоксы
                CbClient.ItemsSource = ClientRepo.GetAll();
                CbEquip.ItemsSource = EquipmentRepo.GetAll();
                CbStatus.SelectedIndex = 0; // Активна по умолчанию

                // Устанавливаем даты по умолчанию
                DpStart.SelectedDate = DateTime.Today;
                DpEnd.SelectedDate = DateTime.Today.AddDays(1);

                // Если редактируем существующую аренду
                if (_id > 0)
                {
                    FormTitle.Text = "РЕДАКТИРОВАТЬ АРЕНДУ";
                    BtnDelete.Visibility = Visibility.Visible;

                    var rn = RentalRepo.GetById(_id);
                    if (rn == null) return;

                    // Выбираем клиента в комбобоксе
                    foreach (var item in CbClient.Items)
                    {
                        if (item is RentalClient cl && cl.Id == rn.ClientId)
                        {
                            CbClient.SelectedItem = item;
                            break;
                        }
                    }

                    // Выбираем снаряжение в комбобоксе
                    foreach (var item in CbEquip.Items)
                    {
                        if (item is Equipment eq && eq.Id == rn.EquipmentId)
                        {
                            CbEquip.SelectedItem = item;
                            break;
                        }
                    }

                    // Устанавливаем даты
                    DpStart.SelectedDate = rn.RentDate;
                    DpEnd.SelectedDate = rn.ReturnPlan;

                    // Заполняем остальные поля
                    TbDeposit.Text = rn.Deposit.ToString();
                    TbNotes.Text = rn.Notes;

                    // Выбираем статус
                    SetCombo(CbStatus, rn.Status);

                    // Пересчитываем итоговую сумму
                    RecalcTotal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetCombo(ComboBox cb, string val)
        {
            foreach (ComboBoxItem item in cb.Items)
            {
                if (item.Content?.ToString() == val)
                {
                    cb.SelectedItem = item;
                    return;
                }
            }
        }

        private void CbEquip_Changed(object sender, SelectionChangedEventArgs e)
        {
            RecalcTotal();
        }

        private void DpDate_Changed(object sender, SelectionChangedEventArgs e)
        {
            RecalcTotal();
        }

        private void RecalcTotal()
        {
            try
            {
                if (DpStart.SelectedDate == null || DpEnd.SelectedDate == null) return;

                // Вычисляем количество дней
                int days = (int)(DpEnd.SelectedDate.Value - DpStart.SelectedDate.Value).TotalDays;
                if (days < 1) days = 1;
                TbDays.Text = days.ToString();

                // Вычисляем итоговую сумму
                if (CbEquip.SelectedItem is Equipment eq)
                {
                    decimal total = eq.PricePerDay * days;
                    TbTotal.Text = total.ToString("N0");
                }
            }
            catch (Exception ex)
            {
                // Игнорируем ошибки вычисления
                System.Diagnostics.Debug.WriteLine($"Ошибка расчета: {ex.Message}");
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка обязательных полей
                if (CbClient.SelectedItem == null)
                {
                    MessageBox.Show("Выберите клиента.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (CbEquip.SelectedItem == null)
                {
                    MessageBox.Show("Выберите снаряжение.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (DpStart.SelectedDate == null)
                {
                    MessageBox.Show("Укажите дату начала аренды.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (DpEnd.SelectedDate == null)
                {
                    MessageBox.Show("Укажите дату возврата.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверка дат
                if (DpEnd.SelectedDate.Value < DpStart.SelectedDate.Value)
                {
                    MessageBox.Show("Дата возврата не может быть раньше даты начала.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Парсинг чисел
                if (!decimal.TryParse(TbDeposit.Text, out decimal dep))
                {
                    dep = 0;
                }

                if (!int.TryParse(TbDays.Text, out int days))
                {
                    days = (int)(DpEnd.SelectedDate.Value - DpStart.SelectedDate.Value).TotalDays;
                    if (days < 1) days = 1;
                }

                // Убираем пробелы из суммы для парсинга
                string totalText = TbTotal.Text.Replace(" ", "").Replace(",", "");
                if (!decimal.TryParse(totalText, out decimal total))
                {
                    // Если не удалось распарсить, пересчитываем
                    if (CbEquip.SelectedItem is Equipment eq)
                    {
                        total = eq.PricePerDay * days;
                    }
                    else
                    {
                        total = 0;
                    }
                }

                // Создаем объект аренды
                var rn = new Rental
                {
                    Id = _id,
                    ClientId = ((RentalClient)CbClient.SelectedItem).Id,
                    EquipmentId = ((Equipment)CbEquip.SelectedItem).Id,
                    RentDate = DpStart.SelectedDate.Value,
                    ReturnPlan = DpEnd.SelectedDate.Value,
                    DaysCount = days,
                    TotalPrice = total,
                    Deposit = dep,
                    Status = (CbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Активна",
                    Notes = TbNotes.Text?.Trim() ?? ""
                };

                // Сохраняем в БД
                if (_id == 0)
                {
                    RentalRepo.Insert(rn);
                    MessageBox.Show("Аренда успешно добавлена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    RentalRepo.Update(rn);
                    MessageBox.Show("Аренда успешно обновлена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Возвращаемся к списку
                AdminWindow.Instance.GoTo("RnList", AdminWindow.Instance.NavRnList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    "Вы уверены, что хотите удалить эту аренду?\nЭто действие нельзя отменить.",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    RentalRepo.Delete(_id);
                    MessageBox.Show("Аренда успешно удалена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    AdminWindow.Instance.GoTo("RnList", AdminWindow.Instance.NavRnList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow.Instance.GoTo("RnList", AdminWindow.Instance.NavRnList);
        }
    }
}