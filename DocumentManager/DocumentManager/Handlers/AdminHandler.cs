using DocumentFormat.OpenXml.Spreadsheet;
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
        public async Task<List<GenerateDocType>> GetGeneratedDocTypes()
        {
            return await _adminServices.GetGeneratedDocTypesAsync();
        }
        public async Task<bool> DeleteGeneratedDocType(int id)
        {
            var fileName = await _adminServices.GetDocumentFileNameById(id);

            if (!string.IsNullOrEmpty(fileName) && await _adminServices.DeleteGeneratedDocTypeAsync(id))
            {
                var filePath = $"E:\\dezvoltare personala\\LICENTA\\DocumentManagerBE\\DocumentManager\\DocumentManager\\Files\\{fileName}";
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                return true;
            }

            return false;
        }


        public async Task<string> SaveDocument(IFormFile file)
        {
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileExtension = Path.GetExtension(file.FileName);

            if (!string.Equals(fileExtension, ".docx", StringComparison.OrdinalIgnoreCase))
            {
                // Handle invalid file extension
                throw new Exception("Invalid file extension. Only .docx files are allowed.");
            }

            string filePath = Path.Combine("E:\\dezvoltare personala\\LICENTA\\DocumentManagerBE\\DocumentManager\\DocumentManager\\Files", fileName + fileExtension);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            await _adminServices.SaveDocumentName(fileName);

            return fileName;
        }


        public async Task<bool> UpdateGeneratedDocRestricted(int id, bool restricted)
        {
            return await _adminServices.UpdateGeneratedDocRestrictedAsync(id, restricted);
        }




    }
}
