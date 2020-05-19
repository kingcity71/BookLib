﻿using BookLib.Data;
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

        public string ChangeBookingStatus(int bookId, int userId, BookingStatus bookingStatus)
        {
            if (BookingStatus.Booked == bookingStatus)
            {
                if (_context.Queues.Any(x => x.BookingStatus == BookingStatus.Expired && x.UserId == userId))
                    return "У вас есть просроченные сдачи, вы не можете брать новые книги";
                if (_context.Queues.Any(x => x.BookingStatus == BookingStatus.Waiting && x.BookId == bookId))
                {
                    var targetDate = _context
                        .Queues
                        .Where(x => x.BookingStatus == BookingStatus.Waiting && x.BookId == bookId)
                        .OrderByDescending(x => x.Deadline)
                        .First()
                        .Deadline
                        .AddDays(7);
                    _context.Queues.Add(new Queue() { Deadline = targetDate, BookId = bookId, UserId = userId, BookingStatus = BookingStatus.Waiting });
                    _context.SaveChanges();
                    return "Вы успешно заняли место в очереди";
                }
                if (_context.Queues.Any(x => x.BookingStatus == BookingStatus.Expired && x.BookId == bookId))
                {
                    var targetDate = DateTime.Today.AddDays(7);
                    _context.Queues.Add(new Queue() { Deadline = targetDate, BookId = bookId, UserId = userId, BookingStatus = BookingStatus.Waiting });
                    _context.SaveChanges();
                    return "Вы успешно заняли место в очереди";
                }
                if (_context.Queues.Any(x => x.BookingStatus == BookingStatus.Booked && x.BookId == bookId))
                {
                    var targetDate = _context.Queues.FirstOrDefault(x => x.BookingStatus == BookingStatus.Booked && x.BookId == bookId).Deadline;
                    _context.Queues.Add(new Queue() { Deadline = targetDate, BookId = bookId, UserId = userId, BookingStatus = BookingStatus.Waiting });
                    _context.SaveChanges();
                    return "Вы успешно заняли место в очереди";
                }
                _context.Queues.Add(new Queue()
                {
                    BookId = bookId,
                    BookingStatus = BookingStatus.Booked,
                    Deadline = DateTime.Today.AddDays(7),
                    UserId = userId
                });
                _context.SaveChanges();
                return $"Вы взяли книгу до {DateTime.Today.AddDays(7).ToShortDateString()}";
            }
            //if (BookingStatus.Returned== bookingStatus)
            //{

            //}
            return string.Empty;
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
            if (queues.Contains(BookingStatus.Booked)) return BookingStatus.Booked;
            return BookingStatus.Returned;
        }
        //обновление времени в очередях
        public void RefreshBookStatus(int bookId)
        {
            var queue = _context
                .Queues
                .FirstOrDefault(x => bookId == x.BookId && x.BookingStatus == BookingStatus.Booked && x.Deadline < DateTime.Today);
            if (queue != null)
            {
                queue.BookingStatus = BookingStatus.Expired;
                _context.Queues.Update(queue);
                _context.SaveChanges();
            }
            //если нет просроченых - выход
            if (!_context.Queues.Any(x => x.BookId == bookId && x.BookingStatus == BookingStatus.Expired)) return;

            var queues = _context.Queues
                .Where(x => x.BookId == bookId && x.BookingStatus == BookingStatus.Waiting)
                .OrderBy(x => x.Deadline)
                .ToList();

            //если нет ожидающих - выход
            if (!queues.Any()) return;
            //если у первого времени ожидающего == через неделю - выход
            if (queues.FirstOrDefault().Deadline == DateTime.Today.AddDays(7)) return;

            var currentDeadline = DateTime.Today.AddDays(7);

            foreach (var queueT in queues)
            {
                queueT.Deadline = currentDeadline;
                currentDeadline = currentDeadline.AddDays(7);
                _context.Queues.Update(queue);
                _context.SaveChanges();

            }
        }

        //обновление дат очереди после ухода юзера из очереди
        public void RefreshBookStatusAfterQueueLeave(int bookId)
        {
            var queue = _context.Queues
                .Where(x => x.BookId == bookId && BookingStatus.Waiting == x.BookingStatus)
                .OrderBy(x=>x.Deadline)
                .ToList();
            if (!queue.Any()) return;
            var targetDate = 
                _context.Queues.Any(x=>bookId==x.BookId && x.BookingStatus == BookingStatus.Expired)?
                DateTime.Today.AddDays(7)
                :_context.Queues.FirstOrDefault(x => x.BookingStatus == BookingStatus.Booked).Deadline.AddDays(7);
            foreach(var q in queue)
            {
                q.Deadline = targetDate;
                targetDate = targetDate.AddDays(7);
                _context.Queues.Update(q);
                _context.SaveChanges();
            }
        }


        public void RefreshStatuses()
        {
            var queues = _context.Queues.Where(x => x.BookingStatus == BookingStatus.Booked && x.Deadline < DateTime.Today);
            foreach (var queue in queues)
                queue.BookingStatus = BookingStatus.Expired;
            if (queues != null && queues.Any())
            {
                _context.Update(queues);
                _context.SaveChanges();
            }
            foreach (var queue in queues)
                RefreshBookStatus(queue.BookId);
        }

        public IEnumerable<Queue> GetUserQueue(int userId)
            => _context.Queues.Where(x => x.UserId == userId)
            .OrderBy(x => x.BookingStatus).ToList();

        public int GetQueueNum(int bookId)
        {
            var num = _context.Queues.Count(x =>x.BookId==bookId && x.BookingStatus == BookingStatus.Waiting);
            if (num == 0) num = _context.Queues.Count(x => x.BookId == bookId && x.BookingStatus == BookingStatus.Expired);
            if (num == 0) num = _context.Queues.Count(x => x.BookId == bookId && x.BookingStatus == BookingStatus.Booked);
            return num;
        }

        public int? GetBookedUserId(int bookId)
        => _context
            .Queues
            .FirstOrDefault(x => x.BookId == bookId && (x.BookingStatus == BookingStatus.Expired || x.BookingStatus == BookingStatus.Booked))?
            .UserId;

        public DateTime? GetAvailableDate(int bookId, int userId)
        {
            var queues = _context
              .Queues
              .Where(x => x.BookingStatus == BookingStatus.Waiting && x.BookId == bookId)
              .OrderByDescending(x => x.BookingStatus).ToList();

            if (queues.Any(x => x.UserId == userId))
                return queues.FirstOrDefault(x => x.UserId == userId)?.Deadline;
            if (queues.Any())
                return queues.FirstOrDefault()?.Deadline;
            if (_context
              .Queues.Any(x => x.BookId == bookId && x.BookingStatus == BookingStatus.Expired))
                return DateTime.Today.AddDays(7);
            return _context
              .Queues
              .FirstOrDefault(x => x.BookingStatus == BookingStatus.Booked && x.BookId == bookId)
              ?.Deadline;
        }
        public BookingStatus GetBookStatus(int bookId)
        {
            var queues = _context.Queues
                .Where(x => x.BookId == bookId)
                .Select(x => x.BookingStatus)
                .Distinct()
                .ToList();
            if (!queues?.Any() == true) return BookingStatus.Returned;

            if (queues.Contains(BookingStatus.Expired)) return BookingStatus.Expired;
            if (queues.Contains(BookingStatus.Waiting)) return BookingStatus.Waiting;
            if (queues.Contains(BookingStatus.Booked)) return BookingStatus.Booked;
            return BookingStatus.Returned;
        }
        public DateTime? GetDeadLine(int bookId, int userId)
            => _context.Queues.FirstOrDefault(x => x.UserId == userId && x.BookId == bookId
            && (x.BookingStatus == BookingStatus.Booked || x.BookingStatus == BookingStatus.Expired))
            ?.Deadline;

        public bool IsUserWait(int bookId, int userId)
            => _context.Queues.Any(x => x.BookId == bookId && userId == x.UserId && BookingStatus.Waiting == x.BookingStatus);

        public void LeaveQueue(int bookId, int userId)
        {
            var queue = _context.Queues.FirstOrDefault(x => x.BookId == bookId && userId == x.UserId);
            _context.Queues.Remove(queue);
            _context.SaveChanges();
        }
    }
}
