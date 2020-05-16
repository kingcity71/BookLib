using BookLib.Entity;

namespace BookLib.Interface
{
    public interface IBookService
    {
        Book GetBook(int id);
        Book Create(Book book);
        Book Update(Book book);
    }
}
