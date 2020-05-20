using BookLib.Entity;
using System.Collections.Generic;

namespace BookLib.Models
{
    public class BookQueuesViewModel
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public Dictionary<Queue, User> Queues {get;set;}
    }
}