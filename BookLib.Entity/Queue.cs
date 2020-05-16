using System;

namespace BookLib.Entity
{
    public class Queue
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime Deadline{ get; set; }
        public BookingStatus BookingStatus{ get; set; }
    }
}
