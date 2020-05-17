using BookLib.Entity;
using System.Collections.Generic;

namespace BookLib.Models
{
    public class BookingViewModel
    {
        public IEnumerable<Queue> Queues { get; set; }
        public Book Book { get; set; }
    }
}
