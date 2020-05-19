using BookLib.Entity;
using System;
using System.Collections.Generic;

namespace BookLib.Models
{
    public class BookingViewModel
    {
        public BookingStatus CurrentStatus { get; set; }
        public int QueueNum { get; set; }
        public DateTime? AvailableDate { get; set; }
        public int? BookedUserId { get; set; }
        public Book Book { get; set; }
    }
}
