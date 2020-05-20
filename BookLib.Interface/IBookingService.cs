using BookLib.Entity;
using System;
using System.Collections.Generic;

namespace BookLib.Interface
{
    public interface IBookingService
    {
        string ChangeBookingStatus(int bookId, int userId, BookingStatus bookingStatus);

        Queue CreateBooking(int bookId, int userId);

        DateTime? GetAvailableDate(int bookId, int userId);

        int? GetBookedUserId(int bookId);

        IEnumerable<Queue> GetBookQueues(int bookId);

        BookingStatus GetBookStatus(int bookId);

        DateTime? GetDeadLine(int bookId, int userId);

        int GetQueueNum(int bookId);

        BookingStatus? GetUserBookStatus(int bookId, int userId);

        IEnumerable<Queue> GetUserQueue(int userId);

        BookingStatus GetUserStatus(int userId);
        int? GetUserWaitingQueueNum(int bookId, int userId);

        bool IsUserWait(int bookId, int userId);

        void LeaveQueue(int bookId, int userId);

        void RefreshBookStatus(int bookId);

        void RefreshBookStatusAfterQueueLeave(int bookId);

        void RefreshStatuses();

        Queue SetWaitingPlace(int bookId, int userId);
    }
}
