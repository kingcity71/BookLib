using BookLib.Entity;

namespace BookLib.Interface
{
    public interface IBookingService
    {
        Queue CreateBooking(int bookId, int userId);
        Queue ChangeBookingStatus(int bookId, int userId, BookingStatus bookingStatus);
    }
}
