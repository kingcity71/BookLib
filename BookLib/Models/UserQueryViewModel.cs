using BookLib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLib.Models
{
    public class UserQueryViewModel
    {
        public string UserName { get; set; }
        public Dictionary<Queue, Book> Queues { get; set; }
    }
}
