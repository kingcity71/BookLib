using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookLib.Entity;
using BookLib.Interface;
using BookLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookLib.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;
        private readonly ILibraryService _libraryService;

        public BookController(IBookService bookService, 
            IUserService userService,
            IBookingService bookingService,
            ILibraryService libraryService)
        {
            _bookService = bookService;
            _userService = userService;
            _bookingService = bookingService;
            _libraryService = libraryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new BookViewModel()
            {
                Libs = _libraryService.GetLibraries()
                .Select(x => new SelectListItem()
                {
                    Text = $"{x.Name}, {x.Address}",
                    Value = x.Id.ToString()
                })
            };
            return View(viewModel);
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
            bookViewModel.Libs = _libraryService.GetLibraries()
                .Select(x => new SelectListItem()
                {
                    Text = $"{x.Name}, {x.Address}",
                    Selected = bookViewModel.LibId==x.Id,
                    Value = x.Id.ToString()
                });
            return View(bookViewModel);
        }
        
        [HttpGet]
        public IActionResult Update(int id)
        {
            var book = _bookService.GetBook(id);
            var bookViewModel = MapBook(book);
            bookViewModel.Libs = _libraryService.GetLibraries()
                .Select(x => new SelectListItem()
                {
                    Text = $"{x.Name}, {x.Address}",
                    Selected = book.LibraryId == x.Id,
                    Value = x.Id.ToString()
                });
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
            bookViewModel.Libs = _libraryService.GetLibraries()
                .Select(x => new SelectListItem()
                {
                    Text = $"{x.Name}, {x.Address}",
                    Selected = bookViewModel.LibId == x.Id,
                    Value = x.Id.ToString()
                });
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
                var book = _bookService.GetBook(queue.BookId);
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
                PhotoBase64 = bookViewModel.PhotoBase64,
                LibraryId = bookViewModel.LibId
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
                PhotoBase64 = book.PhotoBase64,
                LibId = book.LibraryId
            };
        }
    }
}