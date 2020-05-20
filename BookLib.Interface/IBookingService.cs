using BookLib.Entity;
using System;
using System.Collections.Generic;

namespace BookLib.Interface
{
    public interface IBookingService
    {
        string ChangeBookingStatus(int bookId, int userId, BookingStatus bookingStatus);

        Queue CreateBooking(int bookId, int userId);

        IEnumerable<Queue> GetBookQueues(int bookId);

        BookingStatus GetUserStatus(int userId);
        IEnumerable<Queue> GetUserQueue(int userId);

        Queue SetWaitingPlace(int bookId, int userId);

        void LeaveQueue(int bookId, int userId);
        void RefreshBookStatusAfterQueueLeave(int bookId);
        
        void RefreshBookStatus(int bookId);
        void RefreshStatuses();


        int GetQueueNum(int bookId);
        int? GetBookedUserId(int bookId);
        DateTime? GetAvailableDate(int bookId, int userId);
        BookingStatus GetBookStatus(int bookId);
        BookingStatus? GetUserBookStatus(int bookId, int userId);
        DateTime? GetDeadLine(int bookId,int userId);
        int? GetUserWaitingQueueNum(int bookId, int userId);
        bool IsUserWait(int bookId, int userId);
    }
}
