/* Автор: Широков Артемий
 * Контрольное домашнее задание
 * Вариант: 15
 */
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ClassLibrary;
using Microsoft.Win32;
using System.Collections.Generic;

namespace KDZ2
{
    // Логика взаимодействия для MainWindow.xaml
    public partial class MainWindow : Window
    {
        // Название компании, по которому происходит фильтрация
        string FilterName = "все";

        public MainWindow()
        {
            InitializeComponent();
        }

        // Загрузка даблицы при запуске приложения
        private void table_Loaded(object sender, RoutedEventArgs e)
        {
            table.ItemsSource = Jarvis.trips;
            SortName.SelectedIndex = 12;
            // скрываем и преставляем столбцы
            table.Columns[1].Visibility = Visibility.Hidden;
            table.Columns[0].Visibility = Visibility.Hidden;
            table.Columns[0].DisplayIndex = table.Columns.Count - 1;
        }

        // Добавление новой поездки 
        private void AddLine_Click(object sender, RoutedEventArgs e)
        {
            Jarvis.trips.Insert(0, new Trip());
            Refresh();
        }

        // Удаление данных о поездке
        private void DellLine_Click(object sender, RoutedEventArgs e)
        {
            // Получаем поездку
            Trip order = (Trip)((Button)sender).DataContext;
            // удаляем её
            Jarvis.Dellete(order);
            Refresh();
        }

        // Показ информации по двойному клику
        private void table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (table.IsReadOnly) {
                try
                {
                    Trip item = table.SelectedItem as Trip;
                    MessageBox.Show(String.Format(" Id: {0}\n Vendor_id: {1}\n Pickup_datetime: {2}\n Dropoff_datetime: {3}\n" +
                    " Pickup_longitude: {4}\n Pickup_latitude: {5}\n Dropoff_longitude: {6}\n Dropoff_latitude: {7}\n" +
                    " Store_and_fwd_flag: {8}\n Trip_duration: {9}\n Dist_meters: {10}\n Wait_sec: {11}", item.Id, item.Vendor_id,
                    item.Pickup_datetime, item.Dropoff_datetime, item.Pickup_longitude, item.Pickup_latitude, item.Dropoff_longitude,
                    item.Dropoff_latitude, item.Store_and_fwd_flag, item.Trip_duration, item.Dist_meters, item.Wait_sec), "Информация о поездке");
                }
                catch {
                    // Если пользователь кликает не на ячейку таблицы
                    MessageBox.Show("Кликните дважды на ячейку для просмотра информации.", "Подсказка");
                }
            }
        }

        // Красим строки при изменении
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            // Строки могут быть пустые
            try
            {
                e.Row.Background = ((Trip)(e.Row.DataContext)).IsError ? e.Row.Background = new SolidColorBrush(Colors.Yellow) :
                        new SolidColorBrush(Colors.White);
            }
            catch { }
        }

        // Сохранить как
        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
            SaveFileDialog myDialog = new SaveFileDialog();
            myDialog.Filter = "Данные(*.csv;)|*.csv;";
            myDialog.CheckFileExists = true;
            if (myDialog.ShowDialog() == true)
            {
                Jarvis.SaveAs(myDialog.FileName);
                MessageBox.Show("Информация сохранена", "Сообщение");
            }
        }

        // Переключение между режимами чтения и редактирования
        private void Edit_button_Click(object sender, RoutedEventArgs e)
        {
            if (table.IsReadOnly)
            {
                Edit_button.Header = "Режим редактирования";
                table.CanUserAddRows = false;
            }
            else
                Edit_button.Header = "Режим чтения";

            table.IsReadOnly ^= true;
            table.Columns[0].Visibility = table.IsReadOnly ? Visibility.Hidden : Visibility.Visible;
        }

        // Скрыть/показать кнопку "Сохранить"
        private void File_Click(object sender, RoutedEventArgs e)
        {
            Save.Visibility = Jarvis.CanSave ? Visibility.Visible : Visibility.Collapsed;
        }

        // Сохранение в файл, к которому мы обращались в последний раз
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
            Jarvis.Save();
            MessageBox.Show("Информация сохранена", "Сообщение");
        }

        // Выход из программы
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите выйти?", "Выход", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) Close();
        }

        // Открытие файла
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            SortName.SelectedIndex = 12;
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Данные(*.csv;)|*.csv;";
            myDialog.CheckFileExists = true;
            if (myDialog.ShowDialog() == true)
            {
                Jarvis.Connect(myDialog.FileName);
                Refresh();
            }
        }

        // Добавление строк из таблицы к файлу
        private void AddToFile_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Данные(*.csv;)|*.csv;";
            myDialog.CheckFileExists = true;
            if (myDialog.ShowDialog() == true)
            {
                Jarvis.AddToFile(myDialog.FileName);
                MessageBox.Show("Строки из даблицы добавлены в файл", "Сообщение");
            }
        }

        // Обновление данных
        private void Refresh_click(object sender, RoutedEventArgs e)
        {
            Refresh();
            MessageBox.Show("Таблица обновлена", "Сообщение");
        }

        // Создание пустой таблицы
        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            Jarvis.NewFile();
            Refresh();
            MessageBox.Show("Пустая таблица создана ", "Сообщение");
        }

        // О программе
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Программа представлять собой небольшую информационно-справочную систему, основанную" +
                " на данных о поездках на такси в центре Мехико.\n Автор: Широков Артемий, студент НИУ ВШЭ, группы БПИ173.",
                "О программе");
        }

        //Обновление данных в таблице
        void Refresh()
        {
            Jarvis.Synchronization(SortName.SelectedIndex);
            // Вывод названий компаний
            MostPopular.Content = Jarvis.MostPopular == String.Empty ? String.Empty : "Самая популярная компания: " + Jarvis.MostPopular;
            LeastPopular.Content = Jarvis.LeastPopular == String.Empty ? String.Empty : "Самая непопулярная компания: " + Jarvis.LeastPopular;
            Filtration();
            // Отображение столбцов
            table.Columns[1].Visibility = Visibility.Hidden;
            table.Columns[0].Visibility = table.IsReadOnly ? Visibility.Hidden : Visibility.Visible;
            table.Columns[0].DisplayIndex = table.Columns.Count - 1;
            // Обновление
            table.Items.Refresh();
        }

        void Filtration()
        {
            // Обновление фильтра
            FilterName = Filter.SelectedItem != null ? Filter.SelectedItem.ToString() : "все";
            Filter.Items.Clear();
            // Добавление названий всех компаний в фильтр
            foreach (string s in Jarvis.Сompanies)
                Filter.Items.Add(s);
            Filter.Items.Add("все");
            Filter.Items.Refresh();
            Filter.SelectedItem = FilterName;
            // Фильтрация
            if (FilterName != "все")
            {
                List<Trip> FilterList = new List<Trip>();
                foreach (Trip t in Jarvis.trips)
                    if (t.Vendor_id == FilterName) FilterList.Add(t);
                table.ItemsSource = FilterList;
            }
            else
                table.ItemsSource = Jarvis.trips;
        }

        // Изменение высоты строк
        private void Density1_Click(object sender, MouseButtonEventArgs e) { table.RowHeight = 20;}
        private void Density2_Click(object sender, MouseButtonEventArgs e) { table.RowHeight = 25;}
        private void Density3_Click(object sender, MouseButtonEventArgs e) { table.RowHeight = 35;}
    }
}
