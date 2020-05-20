using BookLib.Entity;
using System;
using System.Collections.Generic;

namespace BookLib.Models
{
    public class BookingViewModel
    {
        public int? UserQueueNum { get; set; }
        public BookingStatus? UserBookStatus { get; set; }
        public bool IsUserWait { get; set; }
        public DateTime? Deadline { get; set; }
        public int CurrentUserId { get; set; }
        public BookingStatus CurrentStatus { get; set; }
        public int QueueNum { get; set; }
        public DateTime? AvailableDate { get; set; }
        public int? BookedUserId { get; set; }
        public Book Book { get; set; }
    }
}
