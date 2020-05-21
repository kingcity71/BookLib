using BookLib.Entity;
using System;
using System.Collections.Generic;

namespace BookLib.Models
{
    public class BookingViewModel
    {
        public DateTime? AvailableDate { get; set; }
        public Book Book { get; set; }
        public int? BookedUserId { get; set; }
        public BookingStatus CurrentStatus { get; set; }
        public int CurrentUserId { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsUserWait { get; set; }
        public string LibName { get; set; }
        public int QueueNum { get; set; }
        public BookingStatus? UserBookStatus { get; set; }
        public int? UserQueueNum { get; set; }
    }
}
