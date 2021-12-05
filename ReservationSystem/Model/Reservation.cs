using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ReservationSystem.Model
{
    class Reservation
    {
        public Room Room { get; set; }
        public Client Client { get; set; }

        public DateTime ReservationFrom { get; set; }
        public  DateTime ReservationTo { get; set; }

        public decimal Discount { get; set; } // expressed in % from 0 to 100%

        public bool ActiveReservation { get; set; } = true;

        public decimal CalculatePrice()
        {
            decimal totalPrice = Room.PricePerDay * (decimal)ReservationTo.Subtract(ReservationFrom).TotalDays;

            var totalDays = ReservationTo.Subtract(ReservationFrom).TotalDays;


            return totalPrice - (totalPrice * (Discount / 100));
        }


        public override String ToString() 
        {
            return
                    "Room number: " + Room.RoomNumber + "\n" +
                    "Client information: " + Client.Firstname + " " + Client.LastName + " ,id: " + Client.IdCardNum + "\n" +
                    "Total price with discount: " + this.CalculatePrice().ToString("0.##") + "\n" +
                    "Period from " + ReservationFrom.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " to " + ReservationTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "\n" +
                    "Discount: " + Discount + "%" + "\n" +
                    new string('-', 80);
        }


    }
}
