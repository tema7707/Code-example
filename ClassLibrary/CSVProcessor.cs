using System.IO;

namespace ClassLibrary
{
    // Класс для работы с файлом
    public static class CSVProcessor
    {
        public static string[] lines;
        public static string path;

        // Подключение
        public static void Connect(string str)
        {
            path = str;
            // Чтение строк
            string[] strs = File.ReadAllLines(path);
            lines = new string[strs.Length-1];
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = strs[i+1];
                //Удаляем кавычки
                lines[i] = lines[i].Replace("\"", "");
            }
        }

        // Сохранение в файл
        public static void Save()
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                // Название полей
                writer.WriteLine("\"id\",\"vendor_id\",\"pickup_datetime\",\"dropoff_datetime\",\"pickup_longitude\",\"pickup_latitude\"," +
                        "\"dropoff_longitude\",\"dropoff_latitude\",\"store_and_fwd_flag\",\"trip_duration\",\"dist_meters\",\"wait_sec\"");
                foreach (Trip trip in Jarvis.trips)
                    {
                        writer.WriteLine(trip.ToString());
                    }
            }
        }

        // Добавление в файл
        public static void AddTo()
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                foreach (Trip trip in Jarvis.trips)
                {
                    writer.WriteLine(trip.ToString());
                }
            }
        }
    }
}
