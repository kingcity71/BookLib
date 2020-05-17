using BookLib.Data;
using BookLib.Entity;
using BookLib.Interface;
using System.Collections.Generic;
using System.Linq;

namespace BookLib.Service
{
    public class BookService : IBookService
    {
        private readonly BookLibContext _context;

        public BookService(BookLibContext context)
        {
            _context = context;
        }
        public Book Create(Book book)
        {
            var bookEntity = _context.Add(book);
            _context.SaveChanges();
            return bookEntity.Entity;
        }

        public Book GetBook(int id) => _context.Books.FirstOrDefault(x => x.Id == id);
        public int GetBookCount(string searchKey) => _context.Books.Where(x => x.Name.ToLower().Contains(searchKey.ToLower().Trim())).Count();

        public IEnumerable<Book> GetBooks(int take, int skip, string searchKey)
        => _context.Books
            .Where(x => x.Name.ToLower().Contains(searchKey.ToLower().Trim()))
            .Skip(skip)
            .Take(take)
            .ToList();

        public Book Update(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
            var bookUpdated = _context.Books.FirstOrDefault(x => x.Id == book.Id);
            return bookUpdated;
        }
    }
}