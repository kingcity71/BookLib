using BookLib.Data;
using BookLib.Entity;
using BookLib.Interface;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public Book Update(Book book)
        {
            throw new NotImplementedException();
        }
    }
}
