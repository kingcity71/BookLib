using BookLib.Entity;

namespace BookLib.Interface
{
    public interface IBookService
    {
        Book GetBook(int id);
        Book Create(int id);
        Book Update(int id);
    }
}
