using BookLib.Entity;
using BookLib.Interface;
using System;

namespace BookLib.Service
{
    public class BookingService : IBookingService
    {
        public Queue ChangeBookingStatus(int bookId, int userId, BookingStatus bookingStatus)
        {
            throw new NotImplementedException();
        }

        public Queue CreateBooking(int bookId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
