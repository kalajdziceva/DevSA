using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ReservationSystem.Model
{
    class Hotel
    {
        public static readonly int MaxRooms = 4; // per floor
        public static readonly int MaxFloors = 2;
        public static List<Room> Rooms { get; set; } = new List<Room>(MaxRooms * MaxFloors);

        List<Reservation> Reservations { get; set; } = new List<Reservation>();



        /*
         * 01-10
         * 05 - 09 t
         * 
         * 09 - 11 t
         *  
         *  
         *  12 - 15
         *  
         *  20 - 30
         */

        public bool IsRoomAvailable(int roomNum, DateTime periodFrom, DateTime periodTo)
        {


            foreach (var r in Reservations.Where(x => x.Room.RoomNumber == roomNum && x.ActiveReservation == true))
            {
                //if (r.ReservationFrom <= periodFrom && (r.ReservationTo > periodFrom || r.ReservationTo >= periodTo))
                if (periodFrom < r.ReservationTo && r.ReservationFrom < periodTo)
                {
                    return false;
                }
            }
            return true;
        }

        public Room GetFirstAvailableRoom(DateTime periodFrom, DateTime periodTo)
        {
            return Reservations.OrderBy(x => x.ReservationTo).FirstOrDefault(x => IsRoomAvailable(x.Room.RoomNumber, periodFrom, periodTo))?.Room;
        }

        public void AddRoom(int roomNum, int floor, decimal pricePerDay)
        {
            if (MaxFloors < 0 ||floor > MaxFloors)
            {
                throw new ArgumentOutOfRangeException("Incorrect floor");
            }

            if (Rooms.Capacity == Rooms.Count)
            {
                throw new OverflowException("Max capacity of rooms reached");
            }

            Rooms.Add(new Room { RoomNumber = roomNum, Floor = floor, InFunction = true, PricePerDay = pricePerDay }) ;
        }

        public void AddToFirstAvailRoom(string firstName, string lastName, string id, int age, DateTime periodFrom, DateTime periodTo, decimal discount)
        {
            Room availableRoom = GetFirstAvailableRoom(periodFrom, periodTo);

            if (availableRoom != null)
            {
                Client client = new Client { Firstname = firstName, LastName = lastName, IdCardNum = id, Age = age };
                Reservation reservation = new Reservation
                {
                    Room = availableRoom,
                    Client = client,
                    ReservationFrom = periodFrom,
                    ReservationTo = periodTo,
                    Discount = discount,
                    ActiveReservation = true
                    
                };
                Reservations.Add(reservation);
            } else
            {
                throw 
                    new Exception("No available rooms for client in this period: " + 
                    periodFrom.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) +
                    " - " +
                    periodTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            }
        }


        public void AddReservationForRoom(int roomNum, string firstName, string lastName, string id, int age, DateTime periodFrom, DateTime periodTo, decimal discount) {
            if (IsRoomAvailable(roomNum, periodFrom, periodTo))
            {
                Room room = Rooms.Where(x => x.RoomNumber == roomNum).FirstOrDefault();
                Client client = new Client { Firstname = firstName, LastName = lastName, IdCardNum = id, Age = age };
                Reservation reservation = new Reservation
                {
                    Room = room,
                    Client = client,
                    ReservationFrom = periodFrom,
                    ReservationTo = periodTo,
                    Discount = discount,
                    ActiveReservation = true
                };
                Reservations.Add(reservation);
            } else { 
            throw
                new Exception("No available rooms for the room" + roomNum + " in this period: " +
                periodFrom.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) +
                " - " +
                periodTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            }
        }


            public List<Reservation> GetAllReservations()
        {
            return Reservations;
        }

        public void DisplayRoomsInformation()
        {
            Console.WriteLine(new string('+', 40));
            Console.WriteLine("Total number of rooms: " + Rooms.Count);
            foreach (var r in Rooms)
            {
                Console.WriteLine(new string('=', 40));
                Console.WriteLine(r);
            }
            Console.WriteLine(new string('+', 40));
        }


    }
}
