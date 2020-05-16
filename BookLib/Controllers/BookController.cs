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

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
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
                _bookService.Create(book);
            }
            return View(bookViewModel);
        }

        private Book MapBook(BookViewModel bookViewModel)
        {
            var book = new Book()
            {
                Id = bookViewModel.Id,
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
    }
}