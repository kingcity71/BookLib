using BookLib.Entity;
namespace BookLib.Interface
{
    public interface IUserService
    {
        User GetUser(int id);
        User GetUser(string login);
        User Register(User user);
    }
}
