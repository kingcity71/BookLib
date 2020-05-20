using BookLib.Entity;
using BookLib.Interface;
using BookLib.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookLib.Controllers
{
    public class AdminController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;

        public AdminController(IBookingService bookingService, IUserService userService, IBookService bookService)
        {
            _bookingService = bookingService;
            _userService = userService;
            _bookService = bookService;
        }
        public IActionResult BookQueue(int bookId)
        {
            _bookingService.RefreshBookStatus(bookId);

            var queues = _bookingService.GetBookQueues(bookId);
            var book = _bookService.GetBook(bookId);
            var viewModel = new BookQueuesViewModel() { Queues = new Dictionary<Queue, User>() };

            viewModel.BookName = book.Name;
            viewModel.Id = book.Id;

            foreach (var queue in queues)
                viewModel.Queues.Add(queue, _userService.GetUser(queue.UserId));

            return View(viewModel);
        }
    
        public IActionResult UserQuery(int userId)
        {
            var queues = _bookingService.GetUserQueue(userId);
            var user = _userService.GetUser(userId);
            var viewModel = new UserQueryViewModel() 
            { Queues = new Dictionary<Queue, Book>() };

            viewModel.UserName = user.FullName;
            
            foreach (var queue in queues)
            {
                viewModel.Queues.Add(queue, _bookService.GetBook(queue.BookId));
                _bookingService.RefreshBookStatus(queue.BookId);
            }
                
            return View(viewModel);
        }
    }
}