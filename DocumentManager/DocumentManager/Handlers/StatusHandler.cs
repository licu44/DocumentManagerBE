using DocumentManager.Models;
using DocumentManager.Services;

namespace DocumentManager.Handlers
{
    public class StatusHandler
    {
        private readonly StatusServices _statusServices;

        public StatusHandler(StatusServices statusServices)
        {
            _statusServices = statusServices;
        }
        public async Task<object> GetUserStatusAndAllStatusesAsync(int userId)
        {
            var userStatus = await _statusServices.GetUserStatusAsync(userId);

            return new
            {
                UserStatus = userStatus,
                AuthorizationStatuses = await _statusServices.GetAllAuthorizationStatusesAsync(),
                EngineeringStatuses = await _statusServices.GetAllEngineeringStatusesAsync(),
                FeedbackStatuses = await _statusServices.GetAllFeedbackStatusesAsync()
            };
        }

        public async Task<object> GetAllStatusesAsync()
        {
            return new
            {
                AuthorizationStatuses = await _statusServices.GetAllAuthorizationStatusesAsync(),
                EngineeringStatuses = await _statusServices.GetAllEngineeringStatusesAsync(),
                FeedbackStatuses = await _statusServices.GetAllFeedbackStatusesAsync()
            };
        }
    }
}
