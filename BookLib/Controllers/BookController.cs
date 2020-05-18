using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookLib.Entity;
using BookLib.Interface;
using BookLib.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookLib.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;
        public BookController(IBookService bookService, IUserService userService, IBookingService bookingService)
        {
            _bookService = bookService;
            _userService = userService;
            _bookingService = bookingService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = MapBook(bookViewModel);
                book = _bookService.Create(book);
                return Redirect("/book/library?page=1&key=");
            }
            return View(bookViewModel);
        }
        
        [HttpGet]
        public IActionResult Update(int id)
        {
            var book = _bookService.GetBook(id);
            var bookViewModel = MapBook(book);
            return View(bookViewModel);
        }
        [HttpPost]
        public IActionResult Update(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = MapBook(bookViewModel);
                book = _bookService.Update(book);
                bookViewModel = MapBook(book);
                return Redirect("/book/library?page=1&key=");
            }
            return View(bookViewModel);
        }

        [HttpGet]
        public IActionResult MyBooks()
        {
            var login = User.Identity.Name;
            var user = _userService.GetUser(login);
            _bookingService.RefreshStatuses();
            var userBooks = new List<UserBook>();
            var userQueues = _bookingService.GetUserQueue(user.Id);
            foreach(var queue in userQueues)
            {
                var book = _bookService.GetBook(queue.Id);
                userBooks.Add(new UserBook()
                {
                    Id = book.Id,
                    Deadline = queue.Deadline,
                    Name = book.Name,
                    Status = queue.BookingStatus
                });
            }
            return View(userBooks);
        }

        [HttpGet]
        public IActionResult Library(int page, string key)
        {
            var countOnPage = 10;
            var books = _bookService.GetBooks(countOnPage, countOnPage * (page - 1), key??string.Empty);
            ViewBag.BookCount = _bookService.GetBookCount(key??string.Empty);
            ViewBag.CurrentPage = page;
            return View(new LibraryViewModel() {Books=books, Key=key ?? string.Empty });
        }
        
        private Book MapBook(BookViewModel bookViewModel)
        {
            var book = new Book()
            {
                Id = bookViewModel.Id??0,
                Name = bookViewModel.Name,
                Description = bookViewModel.Description,
                PhotoBase64 = bookViewModel.PhotoBase64
            };

            if (bookViewModel.Photo != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(bookViewModel.Photo.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)bookViewModel.Photo.Length);
                }
                book.PhotoBase64 = $"data:image/jpeg;base64, {Convert.ToBase64String(imageData)}";
            }
            return book;
        }
        private BookViewModel MapBook(Book book)
        {
            return new BookViewModel()
            {
                Id = book.Id,
                Description = book.Description,
                Name = book.Name,
                PhotoBase64 = book.PhotoBase64
            };
        }
    }
}