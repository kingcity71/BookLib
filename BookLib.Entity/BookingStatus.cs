namespace BookLib.Entity
{
    public enum BookingStatus
    {
        Waiting,//в ожидании своей очереди
        Booked,//держит книгу на руках сейчас
        Expired,//просрочил сдачу
        Returned//вернул
    }
}