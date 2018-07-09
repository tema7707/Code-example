using System;
using System.Collections.Generic;
using System.Globalization;

namespace ClassLibrary
{
    // Структура координат поездки
    public struct Сoordinates
    {
        public decimal longitude, latitude;
        // Строки с точкой как разделитель
        public string longitudeStr { get { return longitude.ToString(CultureInfo.InvariantCulture); } }
        public string latitudeStr { get { return latitude.ToString(CultureInfo.InvariantCulture); } }

        public Сoordinates(decimal longitude, decimal latitude)
        {
            this.longitude = longitude;
            this.latitude = latitude;
        }
    }

    // Класс поездки
    public class Trip
    {
        // данные поездки
        ulong id, trip_duration, dist_meters, wait_sec;
        string store_and_fwd_flag, vendor_id;
        DateTime pickup_datetime, dropoff_datetime;
        Сoordinates pickup_coordinates, dropoff_coordinates;
        // лист, содержащий номера ошибочных полей
        List<int> ErrorArgs = new List<int>();
        // Индексатор нужен для того, чтобы обращаться к свойствам по индексу (удобно при сортировке) 
        public string this[int index]
        {
            get
            {
                switch (index){
                    case 0: return Id;
                    case 1: return Vendor_id;
                    case 2: return Pickup_datetime;
                    case 3: return Dropoff_datetime;
                    case 4: return Pickup_longitude;
                    case 5: return Pickup_latitude;
                    case 6: return Dropoff_longitude;
                    case 7: return Dropoff_latitude;
                    case 8: return Store_and_fwd_flag;
                    case 9: return Trip_duration;
                    case 10: return Dist_meters;
                    case 11: return Wait_sec;
                    default: return "";
                }
            }
        }

        #region Свойства
        // Наличие ошибки
        public bool IsError {
            get { return ErrorArgs.Count == 0 ? false : true; }
        }
        // Номер
        public string Id {
            set {
                if (ulong.TryParse(value, out id))
                    ErrorArgs.Remove(0);
                else
                    ErrorArgs.Add(0);
            }
            get { return ErrorArgs.Contains(0) ? "Error" : id.ToString(); }
        }
        // Название компании
        public string Vendor_id
        {
            set {
                if (String.IsNullOrEmpty(value) || value == "Error")
                {
                    ErrorArgs.Add(1);
                }
                else
                {
                    ErrorArgs.Remove(1);
                    vendor_id = value;
                }
            }
            get { return ErrorArgs.Contains(1) ? "Error" : vendor_id; }
        }
        // Дата и время начала поездки
        public string Pickup_datetime
        {
            set {
                if (DateTime.TryParse(value, out pickup_datetime))
                    ErrorArgs.Remove(2);
                else
                    ErrorArgs.Add(2);
            }
            get { return ErrorArgs.Contains(2) ? "Error" : pickup_datetime.ToString(); }
        }
        // Дата и время конца поездки
        public string Dropoff_datetime
        {
            set
            {
                if (DateTime.TryParse(value, out dropoff_datetime))
                    ErrorArgs.Remove(3);
                else
                    ErrorArgs.Add(3);
            }
            get { return ErrorArgs.Contains(3) ? "Error" : dropoff_datetime.ToString(); }
        }
        // Координаты ширины начала поездки (в промежутке [-180;180])
        public string Pickup_longitude {
            set {
                if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out pickup_coordinates.longitude)
                                                                            && Math.Abs(pickup_coordinates.longitude) <= 180)
                    ErrorArgs.Remove(4);
                else
                    ErrorArgs.Add(4);
            }
            get { return ErrorArgs.Contains(4) ? "Error" : pickup_coordinates.longitudeStr; }
        }
        // Координаты долготы начала поездки (в промежутке [-90;90])
        public string Pickup_latitude {
            set
            {
                if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out pickup_coordinates.latitude) && Math.Abs(pickup_coordinates.latitude) <= 90)
                    ErrorArgs.Remove(5);
                else
                    ErrorArgs.Add(5);
            }
            get { return ErrorArgs.Contains(5) ? "Error" : pickup_coordinates.latitudeStr; }
        }
        // Координаты ширины конца поездки (в промежутке [-180;180])
        public string Dropoff_longitude {
            set
            {
                if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out dropoff_coordinates.longitude) && Math.Abs(dropoff_coordinates.longitude) <= 180)
                    ErrorArgs.Remove(6);
                else
                    ErrorArgs.Add(6);
            }
            get { return ErrorArgs.Contains(6) ? "Error" : dropoff_coordinates.longitudeStr; }
        }
        // Координаты долготы конца поездки (в промежутке [-90;90])
        public string Dropoff_latitude
        {
            set
            {
                if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out dropoff_coordinates.latitude) && Math.Abs(dropoff_coordinates.latitude) <= 90)
                    ErrorArgs.Remove(7);
                else
                    ErrorArgs.Add(7);
            }
            get { return ErrorArgs.Contains(7) ? "Error" : dropoff_coordinates.latitudeStr; }
        }
        // Была ли запись поездки сохранена в памяти транспортного средства перед отправкой поставщику
        public string Store_and_fwd_flag
        {
            set {
                if (String.IsNullOrEmpty(value) || value == "Error" || (value != "N" && value != "Y"))
                {
                    ErrorArgs.Add(8);
                }
                else
                {
                    store_and_fwd_flag = value;
                    ErrorArgs.Remove(8);
                }
            }
            get { return ErrorArgs.Contains(8) ? "Error" : store_and_fwd_flag; }
        }
        // Продолжительность поездки
        public string Trip_duration
        {
            set
            {
                if (ulong.TryParse(value, out trip_duration))
                    ErrorArgs.Remove(9);
                else if (!ErrorArgs.Contains(2) && !ErrorArgs.Contains(3))
                {
                    trip_duration = (ulong)(dropoff_datetime - pickup_datetime).TotalSeconds;
                    ErrorArgs.Remove(9);
                }
                else
                    ErrorArgs.Add(9);
            }
            get { return ErrorArgs.Contains(9) ? "Error" : trip_duration.ToString(); }
        }
        // Длина поездки
        public string Dist_meters
        {
            set
            {
                if (ulong.TryParse(value, out dist_meters))
                    ErrorArgs.Remove(10);
                else
                    ErrorArgs.Add(10);
            }
            get { return ErrorArgs.Contains(10) ? "Error" : dist_meters.ToString(); }
        }
        // Время ожидания
        public string Wait_sec
        {
            set
            {
                if (ulong.TryParse(value, out wait_sec))
                    ErrorArgs.Remove(11);
                else
                    ErrorArgs.Add(11);
            }
            get { return ErrorArgs.Contains(11) ? "Error" : wait_sec.ToString(); }
        }
        #endregion
        
        // Конструктор, создающий обьект по умолчанию
        public Trip()
        {
            ErrorArgs.Add(1);
            ErrorArgs.Add(8);
        }

        // Конструктор, создающий объект из массива строк
        public Trip(string[] info)
        {
            Id = info[0];
            Vendor_id = info[1];
            Pickup_datetime = info[2];
            Dropoff_datetime = info[3];
            Pickup_longitude = info[4];
            Pickup_latitude = info[5];
            Dropoff_longitude = info[6];
            Dropoff_latitude = info[7];
            Store_and_fwd_flag = info[8];
            Trip_duration = info[9];
            Dist_meters = info[10];
            Wait_sec = info[11];
        }

        // Формирует строку
        public override string ToString()
        {
            string s = string.Format($"{Id},\"{Vendor_id}\",\"{Pickup_datetime}\",\"{Dropoff_datetime}\"," +
                $"{Pickup_longitude},{Pickup_latitude},{Dropoff_longitude},{Dropoff_latitude},\"" +
                $"{Store_and_fwd_flag}\",{Trip_duration},{Dist_meters},{Wait_sec}");
            // Не записываем в файл Error
            return s.Replace("Error", "");
        }
    }
}
