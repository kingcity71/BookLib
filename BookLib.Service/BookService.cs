using BookLib.Data;
using BookLib.Entity;
using BookLib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public Book GetBook(int id)
        {
            var book = _context.Books.FirstOrDefault(x => x.Id == id);
            return book;
        }

        public Book Update(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
            var bookUpdated = _context.Books.FirstOrDefault(x => x.Id == book.Id);
            return bookUpdated;
        }
    }
}
