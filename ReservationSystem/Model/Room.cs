using System;
using System.Collections.Generic;
using System.Text;

namespace ReservationSystem.Model
{
    class Room
    {
        public int RoomNumber { get; set; }
        public decimal PricePerDay { get; set; }
        public int Floor { get; set; }

        public bool InFunction { get; set; }

        public override string ToString()
        {
            return
                  "Room number: " + RoomNumber + "\n" +
                  "Price per day: " + PricePerDay.ToString("0.##") + "\n" + 
                  "Floor: " + Floor + "\n" +
                  "InFunction: " + InFunction;
        }
    }
}
