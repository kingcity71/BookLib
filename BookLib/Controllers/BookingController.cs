using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLib.Interface;
using BookLib.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookLib.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IBookingService _boookingService;

        public BookingController(IBookService bookService, IBookingService bookingService)
        {
            _bookService = bookService;
            _boookingService = bookingService;
        }
        [HttpGet]
        public IActionResult Book(int bookId)
        {
            var viewModel = new BookingViewModel()
            {
                Book = _bookService.GetBook(bookId),

            };
            return View();
        }

        [HttpGet]
        public IActionResult MakeBooking(int bookId)
        {
            return View();
        }
    }
}