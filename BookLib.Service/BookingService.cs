using BookLib.Data;
using BookLib.Entity;
using BookLib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLib.Service
{
    public class BookingService : IBookingService
    {
        private readonly BookLibContext _context;

        public BookingService(BookLibContext context)
        {
            _context = context;
        }
        public Queue ChangeBookingStatus(int bookId, int userId, BookingStatus bookingStatus)
        {
            throw new NotImplementedException();
        }

        public Queue SetWaitingPlace(int bookId, int userId) => CreateBooking(bookId, userId, BookingStatus.Waiting);

        public Queue CreateBooking(int bookId, int userId) => CreateBooking(bookId, userId, BookingStatus.Booked);

        private Queue CreateBooking(int bookId, int userId, BookingStatus bookingStatus)
        {
            var queue = new Queue()
            {
                BookId = bookId,
                UserId = userId,
                BookingStatus = bookingStatus,
                Deadline = DateTime.Now.AddDays(7)
            };
            _context.Queues.Add(queue);
            _context.SaveChanges();
            queue = _context.Queues.First(x => x.Deadline == queue.Deadline && x.BookId == bookId && x.UserId == userId);
            return queue;
        }

        public IEnumerable<Queue> GetBookQueues(int bookId)
        => _context.Queues
            .Where(x => x.BookId == bookId)
            .OrderByDescending(x => x.Deadline)
            .ToList();

        public BookingStatus GetUserStatus(int userId)
        {
            var queues = _context.Queues
                .Where(x => x.UserId == userId)
                .Select(x => x.BookingStatus)
                .Distinct()
                .ToList();

            if (queues.Contains(BookingStatus.Expired)) return BookingStatus.Expired;
            if (queues.Contains(BookingStatus.Waiting)) return BookingStatus.Waiting;
            if (queues.Contains(BookingStatus.Booked))  return BookingStatus.Booked;
            return BookingStatus.Returned;
        }

        public void RefreshStatuses()
        {
            var queues = _context.Queues.Where(x => x.BookingStatus == BookingStatus.Booked && x.Deadline<DateTime.Today);
            foreach (var queue in queues)
                queue.BookingStatus = BookingStatus.Expired;
            if (queues != null && queues.Any()) 
            {
                _context.Update(queues);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Queue> GetUserQueue(int userId)
            => _context.Queues.Where(x => x.UserId == userId)
            .OrderBy(x => x.BookingStatus).ToList();

        public int GetQueueNum(int bookId)
        => _context.Queues.Count(x => x.BookingStatus == BookingStatus.Waiting);

        public int? GetBookedUserId(int bookId)
        => _context
            .Queues
            .FirstOrDefault(x => x.BookingStatus == BookingStatus.Expired || x.BookingStatus == BookingStatus.Waiting)?
            .UserId;

        public DateTime? GetAvailableDate(int bookId)
        => _context
            .Queues
            .Where(x => x.BookingStatus == BookingStatus.Waiting)
            .OrderByDescending(x => x.BookingStatus)
            .FirstOrDefault()
            .Deadline;

        public BookingStatus GetBookStatus(int bookId)
        {
            var queues = _context.Queues
                .Where(x => x.BookId == bookId)
                .Select(x => x.BookingStatus)
                .Distinct()
                .ToList();
            if (!queues?.Any()==true) return BookingStatus.Returned;

            if (queues.Contains(BookingStatus.Expired)) return BookingStatus.Expired;
            if (queues.Contains(BookingStatus.Waiting)) return BookingStatus.Waiting;
            if (queues.Contains(BookingStatus.Booked)) return BookingStatus.Booked;
            return BookingStatus.Returned;
        }
    }
}
