using DocumentManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Services
{
    public class StatusServices
    {
        private readonly DataContext _dbContext;
        public StatusServices(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<AuthorizationStatus>> GetAllAuthorizationStatusesAsync()
        {
            return await _dbContext.AuthorizationStatuses.ToListAsync();
        }

        public async Task<List<EngineeringStatus>> GetAllEngineeringStatusesAsync()
        {
            return await _dbContext.EngineeringStatuses.ToListAsync();
        }

        public async Task<List<FeedbackStatus>> GetAllFeedbackStatusesAsync()
        {
            return await _dbContext.FeedbackStatuses.ToListAsync();
        }
        public async Task<UserStatus> GetUserStatusAsync(int userId)
        {
            return await _dbContext.UserStatuses.FirstOrDefaultAsync(us => us.UserId == userId);
        }

    }
}
