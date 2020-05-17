using BookLib.Entity;
using System.Collections.Generic;

namespace BookLib.Interface
{
    public interface IBookService
    {
        Book GetBook(int id);
        int GetBookCount(string searchKey);
        Book Create(Book book);
        Book Update(Book book);
        IEnumerable<Book> GetBooks(int take, int skip, string searchKey);
    }
}
