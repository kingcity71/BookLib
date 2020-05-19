using BookLib.Entity;
using System;
using System.Collections.Generic;

namespace BookLib.Interface
{
    public interface IBookingService
    {
        Queue ChangeBookingStatus(int bookId, int userId, BookingStatus bookingStatus);

        Queue CreateBooking(int bookId, int userId);

        IEnumerable<Queue> GetBookQueues(int bookId);

        BookingStatus GetUserStatus(int userId);
        IEnumerable<Queue> GetUserQueue(int userId);

        Queue SetWaitingPlace(int bookId, int userId);

        void RefreshStatuses();


        int GetQueueNum(int bookId);
        int? GetBookedUserId(int bookId);
        DateTime? GetAvailableDate(int bookId);
        BookingStatus GetBookStatus(int bookId);
    }
}
