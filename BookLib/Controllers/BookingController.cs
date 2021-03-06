﻿using System;
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
        private readonly IUserService _userService;
        private readonly ILibraryService _libraryService;

        public BookingController(IBookService bookService, 
            IBookingService bookingService,
            IUserService userService,
            ILibraryService libraryService)
        {
            _bookService = bookService;
            _boookingService = bookingService;
            _userService = userService;
            _libraryService = libraryService;
        }
        [HttpGet]
        public IActionResult Book(int bookId)
        {
            var login = User.Identity.Name;
            var user = _userService.GetUser(login);
            _boookingService.RefreshBookStatus(bookId);
            var book = _bookService.GetBook(bookId);
            var lib = _libraryService.GetLibrary(book.LibraryId);
            var viewModel = new BookingViewModel()
            {
                IsUserWait = _boookingService.IsUserWait(bookId, user.Id),
                CurrentUserId = user.Id,
                Deadline = _boookingService.GetDeadLine(bookId, user.Id),
                Book = book,
                AvailableDate = _boookingService.GetAvailableDate(bookId, user.Id),
                BookedUserId = _boookingService.GetBookedUserId(bookId),
                CurrentStatus = _boookingService.GetBookStatus(bookId),
                QueueNum = _boookingService.GetQueueNum(bookId),
                UserBookStatus = _boookingService.GetUserBookStatus(bookId, user.Id),
                UserQueueNum = _boookingService.GetUserWaitingQueueNum(bookId, user.Id),
                LibName = lib != null? $"{lib.Name},{lib.Address}":string.Empty
            };
            return View(viewModel);
        }

        [HttpGet]
        public string MakeBooking(int bookId, int userId)
        {
            return _boookingService.ChangeBookingStatus(bookId, userId, Entity.BookingStatus.Booked);
        }

        [HttpGet]
        public string LeaveQueue(int bookId, int userId)
        {
            _boookingService.LeaveQueue(bookId, userId);
            _boookingService.RefreshBookStatusAfterQueueLeave(bookId);
            return "Вы покинули очередь";
        }

        [HttpGet]
        public string CancelBooking(int bookId, int userId)
        {
            _boookingService.RefreshBookStatus(bookId);
            return _boookingService.ChangeBookingStatus(bookId, userId, Entity.BookingStatus.Returned);
        }
    }
}