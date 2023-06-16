using DocumentManager.Data;
using DocumentManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Services
{
    public class UserServices
    {
        private readonly DataContext _dbContext;
        public UserServices(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int InsertUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            _dbContext.UserRoles.AddAsync(new UserRole {UserId = user.Id, RoleId = 1 });
            _dbContext.SaveChanges();

            return _dbContext.SaveChanges();
        }

        public UserData SelectUser(string username)
        {
            var result = (from u in _dbContext.Users
                           join ur in _dbContext.UserRoles on u.Id equals ur.UserId
                           join r in _dbContext.Roles on ur.RoleId equals r.Id
                           where u.Username == username
                          select new UserData
                          {
                              User = u,
                              Role = r.RoleName
                          }).FirstOrDefault();

            return result;
        }
    }
}
