using ReservationSystem.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ReservationSystem
{
    class Program
    {
        static void Main(string[] args)
        {

            Hotel hotel = new Hotel();

            for (int i = 1; i <= Hotel.MaxFloors; i++)
            {
                for (int j = 1; j <= Hotel.MaxRooms; j++)
                {
                    hotel.AddRoom(((i-1) * Hotel.MaxRooms) + j , i, 100);
                }
            }

            Client Jack = new Client { Firstname = "Black", LastName = "Jack", Age = 30, IdCardNum = "0x100" };
            Client Ana = new Client { Firstname = "Ana", LastName = "Cyborg", Age = 20, IdCardNum = "0x200" };
            Client John = new Client { Firstname = "John", LastName = "Foo", Age = 35, IdCardNum = "0x300" };

            Reservation RJack = new Reservation { Client = Jack, ReservationFrom = new DateTime(2010, 10, 01), ReservationTo = new DateTime(2010, 10, 11), ActiveReservation = true, Discount = 10, Room = Hotel.Rooms.ElementAt(0) };
            Reservation RAna = new Reservation { Client = Ana, ReservationFrom = new DateTime(2010, 10, 20), ReservationTo = new DateTime(2010, 11, 01), ActiveReservation = true, Discount = 15, Room = Hotel.Rooms.ElementAt(0) };
            Reservation RJohn = new Reservation { Client = John, ReservationFrom = new DateTime(2010, 11, 01), ReservationTo = new DateTime(2010, 11, 05), ActiveReservation = true, Discount = 0, Room = Hotel.Rooms.ElementAt(1) };

            hotel.GetAllReservations().Add(RJack);
            hotel.GetAllReservations().Add(RAna);
            hotel.GetAllReservations().Add(RJohn);


            hotel.AddReservationForRoom(2, "Michael", "Jackson", "0x400", 60, new DateTime(2010, 10, 11), new DateTime(2010, 10, 20), 0);

            bool exit = false;

            do
            {

                PrintMenu();

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            MakeReservationForFreeAvailableRoom(hotel);
                            break;

                        case "2":
                            MakeReservationForRoomNum(hotel);

                            break;

                        case "3":
                            ListAllReservations(hotel);
                            break;

                        case "4":
                            CancelReservation(hotel);
                            break;

                        case "5":
                            ListAvailableRoomsForPeriod(hotel);
                            break;

                        case "6":

                            hotel.DisplayRoomsInformation();

                            break;

                        case "7":
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Invalid option");
                            PrintMenu();
                            break;
                    }
                } catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);

                }


            } while (exit == false);


        }

        private static void ListAvailableRoomsForPeriod(Hotel hotel)
        {
            Console.Write("Period from (yyyy-MM-dd format): ");
            DateTime periodFrom = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            Console.Write("Period to (yyyy-MM-dd format): ");
            DateTime periodTo = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            foreach(var room in Hotel.Rooms)
            {
                if (hotel.IsRoomAvailable(room.RoomNumber, periodFrom, periodTo))
                {
                    Console.WriteLine(new string('*', 80));
                    Console.WriteLine("Room " + room.RoomNumber + " is available for period " +
                        periodFrom.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) +
                        " - " +
                        periodTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

                    Console.WriteLine(room);
                }
            }
        }

        private static void MakeReservationForFreeAvailableRoom(Hotel hotel)
        {
            Console.WriteLine("Enter reservation details:");

            Console.Write("Room num: ");
            int roomNum = int.Parse(Console.ReadLine());

            Console.Write("Period from (yyyy-MM-dd format): ");
            DateTime periodFrom = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            Console.Write("Period to (yyyy-MM-dd format): ");
            DateTime periodTo = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            bool available = hotel.IsRoomAvailable(roomNum, periodFrom, periodTo);

            if (available == false)
            {
                Console.WriteLine("Room " + roomNum + " is unavailable for that period");
                return;
            } 

            Console.Write("Firstname: ");
            string firstName = Console.ReadLine();

            Console.Write("LastName: ");
            string lastName = Console.ReadLine();

            Console.Write("Id card: ");
            string id = Console.ReadLine();

            Console.Write("Age: ");
            int age = int.Parse(Console.ReadLine());

            Console.Write("Discount: ");
            decimal discount = decimal.Parse(Console.ReadLine());


            hotel.AddReservationForRoom(roomNum, firstName, lastName, id, age, periodFrom, periodTo, discount);
     

            }


        private static void MakeReservationForRoomNum(Hotel hotel)
        {
            Console.WriteLine("Enter reservation details:");

            Console.Write("Firstname: ");
            string firstName = Console.ReadLine();

            Console.Write("LastName: ");
            string lastName = Console.ReadLine();

            Console.Write("Id card: ");
            string id = Console.ReadLine();

            Console.Write("Age: ");
            int age = int.Parse(Console.ReadLine());

            Console.Write("Period from (yyyy-MM-dd format): ");
            DateTime periodFrom = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            Console.Write("Period to (yyyy-MM-dd format): ");
            DateTime periodTo = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            Console.Write("Discount: ");
            decimal discount = decimal.Parse(Console.ReadLine());


            hotel.AddToFirstAvailRoom(firstName, lastName, id, age, periodFrom, periodTo, discount);


        }

        private static void ListAllReservations(Hotel hotel)
        {
            Console.WriteLine(new string('+', 80));
            Console.WriteLine("Total number of reservations: " + hotel.GetAllReservations().Where(x => x.ActiveReservation == true).ToList().Count);

            foreach (var r in hotel.GetAllReservations().Where(x => x.ActiveReservation == true))
            {
                Console.WriteLine(r);
            }
        }

        private static void CancelReservation(Hotel hotel)
        {
            Console.Write("Type room number for list of reservations for that room: ");
            int roomNum = int.Parse(Console.ReadLine());

            Console.WriteLine("Room " + roomNum + " reservations: ");
            int i = 1;


            foreach (var r in hotel.GetAllReservations().Where(x => x.Room.RoomNumber == roomNum && x.ActiveReservation == true).OrderBy(x => x.ReservationFrom))
            {
                Console.WriteLine("Reservation number ---> " + i);
                Console.WriteLine(r);
                i++;
            }

            Console.Write("Enter number of reservation you want to cancel for this room: ");
            int cancelReservation = int.Parse(Console.ReadLine());

            if (cancelReservation > 0 && cancelReservation <= hotel.GetAllReservations().Where(x => x.Room.RoomNumber == roomNum && x.ActiveReservation == true).OrderBy(x => x.ReservationFrom).Count())
            {
                hotel.GetAllReservations().Where(x => x.Room.RoomNumber == roomNum && x.ActiveReservation == true).OrderBy(x => x.ReservationFrom).ElementAt(cancelReservation-1).ActiveReservation = false;
                Console.WriteLine("Reservation canceled");
            } else
            {
                Console.WriteLine("Invalid cancelation");
            }






        }

        private static void PrintMenu()
        {
            Console.WriteLine(new string('-', 90));
            Console.WriteLine("Hotel reservation system menu:");
            Console.WriteLine("1. Make reservation for free available room");
            Console.WriteLine("2. Make reservation for specific room");
            Console.WriteLine("3. List all reservations");
            Console.WriteLine("4. Cancel reservation");
            Console.WriteLine("5. Get available rooms for period");
            Console.WriteLine("6. Print all rooms and their prices");
            Console.WriteLine("7. Exit");
        }
    }
}
