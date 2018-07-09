using System;
using System.Collections.Generic;
using System.Globalization;

namespace ClassLibrary
{
    // Класс, координирующий работу всех остальных классов 
    public static class Jarvis
    {
        
        static Manager manager = new Manager();
        public static List<Trip> trips { get { return manager.trips; } }
        // Можно ли сохранить в файл, к которому мы обращались в последний раз
        public static bool CanSave { get { return String.IsNullOrEmpty(CSVProcessor.path) ? false : true; } }
        // Название популярной и непопулярной компаний
        public static string MostPopular { get { return manager.MostPopularCompany(); }}
        public static string LeastPopular { get { return manager.LeastPopularCompany(); } }
        // Возвращает названия всех компаний
        public static List<string> Сompanies
        {
            get
            {
                List<string> names = new List<string>();
                foreach (TaxiVendor vendor in manager.vendors)
                    if (vendor.name != "Error") names.Add(vendor.name);
                return names;
            }
        }

        // Очистка данных
        public static void NewFile()
        {
            CSVProcessor.path = String.Empty;
            manager = new Manager();
        }

        // Подключение к файлу и заполнение данных о компаниях
        public static void Connect(string path)
        {
            CSVProcessor.Connect(path);
            manager = new Manager(CSVProcessor.lines);
        }

        // Обновление списка компаний
        public static void Synchronization(int index)
        {
            // Сортируем, если выбрано название колонки для сортировки
            if (index != -1 && index != 12)
            trips.Sort((Trip x, Trip y) =>
            {
                // Ошибочные значения группируем вначале списка 
                if (x[index] == "Error" && y[index] == "Error") return 0;
                if (!(x[index] == "Error") && y[index] == "Error") return 1;
                if (x[index] == "Error" && !(y[index] == "Error")) return -1;
                // Определённая сортировка для каждого типов
                switch (index)
                { 
                    // Дата
                    case 2: case 3: return DateTime.Parse(x[index]).CompareTo(DateTime.Parse(y[index]));
                    // Строка
                    case 1: case 8: return x[index].CompareTo(y[index]);
                    // Число
                    default: return decimal.Parse(x[index], CultureInfo.InvariantCulture).CompareTo(decimal.Parse(y[index],
                                                                                                CultureInfo.InvariantCulture));
                }
            });
            manager.Refresh();
        }

        // Сохранения
        public static void Save()
        {
            CSVProcessor.Save();
        }
        public static void SaveAs(string str)
        {
            CSVProcessor.path = str;
            CSVProcessor.Save();
        }

        // Добавление строк в файл
        public static void AddToFile(string str)
        {
            CSVProcessor.path = str;
            CSVProcessor.AddTo();
        }

        // Удадение информации о поездке
        public static void Dellete(Trip tr)
        {
            manager.Dellete(tr);
        }
    }
}
