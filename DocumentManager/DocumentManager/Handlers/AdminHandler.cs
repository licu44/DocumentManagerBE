using DocumentManager.Data;
using DocumentManager.Models;
using DocumentManager.Services;

namespace DocumentManager.Handlers
{
    public class AdminHandler
    {
        private readonly AdminServices _adminServices;

        public AdminHandler(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }
        public List<AdminUserDto> GetAllUsers()
        {
            return _adminServices.GetAllUsers();
        }
        public async Task<UserStatus> AddOrUpdateUserStatusAsync(int userId, int feedbackStatusId, int authorizationStatusId, int engineeringStatusId)
        {
            return await _adminServices.AddOrUpdateUserStatusAsync(userId, feedbackStatusId, authorizationStatusId, engineeringStatusId);
        }
    }
}
