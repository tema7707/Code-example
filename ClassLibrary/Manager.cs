using System.Collections.Generic;

namespace ClassLibrary
{
    // Класс, управляющий всеми таксомоторными компаниями
    public class Manager
    {
        public List<TaxiVendor> vendors = new List<TaxiVendor>();
        public List<Trip> trips = new List<Trip>();

        public Manager() { }

        public Manager(string[] lines)
        {
            vendors.Clear();
            // Формируем список компаний
            foreach (string line in lines)
            {
                string[] info = line.Split(',');

                if (info.Length == 12)
                {
                    bool New = true;
                    foreach (TaxiVendor vendor in vendors)
                        if (info[1] == vendor.name)
                        {
                            trips.Add(new Trip(info));
                            vendor.AddTrip(trips[trips.Count-1]);
                            New = false;
                        }
                    if (New)
                    {
                        // Создание новой компании
                        vendors.Add(new TaxiVendor(info[1]));
                        trips.Add(new Trip(info));
                        vendors[vendors.Count - 1].AddTrip(trips[trips.Count - 1]);
                    }
                }
            }
        }

        // Обновление данных о компаниях
        public void Refresh()
        {
            vendors.Clear();
            for (int i = 0; i < trips.Count; i++)
            {
                bool IsNewVendor = true;
                for (int j = 0; j < vendors.Count; j++)
                {
                    if (trips[i].Vendor_id == vendors[j].name)
                    {
                        IsNewVendor = false;
                        vendors[j].AddTrip(trips[i]);
                    }
                }
                if (IsNewVendor)
                {
                    // Добавление новой
                    vendors.Add(new TaxiVendor(trips[i].Vendor_id));
                    vendors[vendors.Count - 1].AddTrip(trips[i]);
                }
            }
        }

        // Удаление данных о поездке
        public void Dellete(Trip tr)
        {
            trips.Remove(tr);
            foreach (TaxiVendor vendor in vendors)
            {
                if (vendor.name == tr.Vendor_id)
                    vendor.DellTrip(tr);
            }
        }

        // Получение самой популярной компании
        public string MostPopularCompany()
        {
            string MostPopular = "";
            int max = int.MinValue;
            foreach (TaxiVendor vendor in vendors)
                if (vendor.CorrectTrips > max && vendor.CorrectTrips != 0)
                {
                    max = vendor.CorrectTrips;
                    MostPopular = vendor.name;
                }
            return MostPopular;
        }

        // Получение самой непопулярной компании
        public string LeastPopularCompany()
        {
            string LeastPopular = "";
            int min = int.MaxValue;
            foreach (TaxiVendor vendor in vendors)
                if (vendor.CorrectTrips < min && vendor.CorrectTrips != 0)
                {
                    min = vendor.CorrectTrips;
                    LeastPopular = vendor.name;
                }
            return LeastPopular;
        }
    }
}
