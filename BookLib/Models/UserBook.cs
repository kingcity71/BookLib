using BookLib.Entity;
using System;

namespace BookLib.Models
{
    public class UserBook
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public BookingStatus Status { get; set; }
    }
}
