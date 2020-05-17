using BookLib.Data;
using BookLib.Entity;
using BookLib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLib.Service
{
    public class UserService : IUserService
    {
        private readonly BookLibContext _context;

        public UserService(BookLibContext context)
        {
            _context = context;
        }
        public User GetUser(int id)
        => _context.Users.FirstOrDefault(x => x.Id == id);
        

        public User GetUser(string login)
        => _context.Users.FirstOrDefault(x=>x.Login.ToLower()==login.ToLower());

        public User Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return GetUser(user.Login);
        }
    }
}
