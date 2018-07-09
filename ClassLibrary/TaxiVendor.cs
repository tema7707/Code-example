using System.Collections.Generic;

namespace ClassLibrary
{
    // Класс таксомоторной компании
    public class TaxiVendor : Trip
    {
        public string name;
        // Информация о поездках
        List<Trip> trips = new List<Trip>();

        // Количество поездок
        public int CorrectTrips
        {
            get
            {
                int count = 0;
                foreach (Trip trip in trips)
                    if (!trip.IsError) count++;
                return count;
            }
        }

        public TaxiVendor(string name)
        {
            this.name = name;
        }

        // Добавление поездки 
        public void AddTrip(Trip tr)
        {
            trips.Add(tr);
        }

        // Удаление поездки 
        public void DellTrip(Trip tr)
        {
            trips.Remove(tr);
        }
    }
}
