﻿using DocumentManager.Data;
using DocumentManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Services
{
    public class AdminServices
    {
        private readonly DataContext _dbContext;
        public AdminServices(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<AdminUserDto> GetAllUsers()
        {
            var users = _dbContext.Users
                .Include(u => u.UserStatuses)
                .ThenInclude(s => s.FeedbackStatus)
                .Include(u => u.UserStatuses)
                .ThenInclude(s => s.AuthorizationStatus)
                .Include(u => u.UserStatuses)
                .ThenInclude(s => s.EngineeringStatus)
            .ToList();

            return users.Select(user => new AdminUserDto
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserStatus = user.UserStatuses.Select(status => new UserStatusDTO
                {
                    FeedbackId = status.FeddbackId,
                    AuthorizationId = status.AuthorizationId,
                    EngineeringId = status.EngineeringId
                }).FirstOrDefault() ?? new UserStatusDTO() // Use FirstOrDefault to get only one UserStatusDTO or a new empty one if none exists
            }).ToList();
        }
        public async Task<UserStatus> AddOrUpdateUserStatusAsync(int userId, int feedbackStatusId, int authorizationStatusId, int engineeringStatusId)
        {
            var existingUserStatus = await _dbContext.UserStatuses.FirstOrDefaultAsync(us => us.UserId == userId);

            if (existingUserStatus == null)
            {
                var newUserStatus = new UserStatus
                {
                    UserId = userId,
                    FeddbackId = feedbackStatusId,
                    AuthorizationId = authorizationStatusId,
                    EngineeringId = engineeringStatusId
                };
                _dbContext.UserStatuses.Add(newUserStatus);
            }
            else
            {
                existingUserStatus.FeddbackId = feedbackStatusId;
                existingUserStatus.AuthorizationId = authorizationStatusId;
                existingUserStatus.EngineeringId = engineeringStatusId;
            }

            await _dbContext.SaveChangesAsync();

            return await _dbContext.UserStatuses.FirstOrDefaultAsync(us => us.UserId == userId);

        }
    }
}
