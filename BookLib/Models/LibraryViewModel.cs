using BookLib.Entity;
using System.Collections.Generic;

namespace BookLib.Models
{
    public class LibraryViewModel
    {
        public string Key { get; set; }
        public IEnumerable<Book> Books{ get; set; }
    }
}
