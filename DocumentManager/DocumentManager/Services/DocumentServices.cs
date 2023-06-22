using DocumentManager.Data;
using DocumentManager.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DocumentManager.Services
{
    public class DocumentServices
    {
        private readonly DataContext _dbContext;
        public DocumentServices(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<DocumentDto>> GetUserDocumentsAsync(int userId)
        {
            var documentTypes = await _dbContext.DocumentTypes
                 .ToListAsync();

            var userDocs = await _dbContext.UserDocs
                 .Where(ud => ud.UserId == userId)
                 .ToListAsync();

            var documentDtos = documentTypes.Select(d => {
                var userDoc = userDocs.FirstOrDefault(ud => ud.TypeId == d.Id);
                return new DocumentDto
                {
                    DocId = d.Id,
                    DocName = d.DocName,
                    CreationDate = userDoc?.CreationDate,
                    Status = userDoc?.Status,
                };
            }).ToList();

            return documentDtos;
        }
        public async Task<IEnumerable<DocumentDto>> GetGeneratedUserDocumentsAsync(int userId)
        {
            var documentTypes = await _dbContext.DocumentTypes
                 .ToListAsync();

            var userDocs = await _dbContext.UserDocs
                 .Where(ud => ud.UserId == userId)
                 .ToListAsync();

            var documentDtos = documentTypes.Select(d => {
                var userDoc = userDocs.FirstOrDefault(ud => ud.TypeId == d.Id);
                return new DocumentDto
                {
                    DocId = d.Id,
                    DocName = d.DocName,
                    CreationDate = userDoc?.CreationDate,
                    Status = userDoc?.Status,
                };
            }).ToList();

            return documentDtos;
        }
        public async Task InsertIdCardData(IdCardDto idData, int userId)
        {
            var newIdCard = new IdCard
            {
                UserId = userId,
                LastName = idData.LastName,
                FirstName = idData.FirstName,
                CNP = idData.CNP,
                Series = idData.Series,
                Number = idData.Number,
                Address = idData.Address,
            };

            _dbContext.IdCards.Add(newIdCard);

            var newUserDoc = new UserDoc
            {
                UserId = userId,
                TypeId = 1,
                CreationDate = DateTime.Now,
                Status = "UPLOADED",
            };

            _dbContext.UserDocs.Add(newUserDoc);

            await _dbContext.SaveChangesAsync();
        }
        public async Task InsertUrabCertificateData(UrbanCertificateDto certificateData, int userId)
        {
            var newUrbanCertificate = new UrbanCertificate
            {
                UserId = userId,
                Number = certificateData.Number,
                Date = certificateData.Date,
                UserAdress = certificateData.UserAdress,
                ProjectAdress = certificateData.ProjectAdress,
                ProjectType = certificateData.ProjectType
            };

            _dbContext.UrbanCertificates.Add(newUrbanCertificate);

            var newUserDoc = new UserDoc
            {
                UserId = userId,
                TypeId = 2,
                CreationDate = DateTime.Now,
                Status = "UPLOADED",
            };

            _dbContext.UserDocs.Add(newUserDoc);

            await _dbContext.SaveChangesAsync();
        }
        
        public async Task UpdateIdCardData(IdCardDto idData, int userId)
        {
            var idCard = await _dbContext.IdCards.FirstOrDefaultAsync(i => i.UserId == userId);

            if (idCard != null)
            {
                idCard.LastName = idData.LastName;
                idCard.FirstName = idData.FirstName;
                idCard.CNP = idData.CNP;
                idCard.Series = idData.Series;
                idCard.Number = idData.Number;
                idCard.Address = idData.Address;

                _dbContext.IdCards.Update(idCard);

                var userDoc = await _dbContext.UserDocs
                    .FirstOrDefaultAsync(ud => ud.UserId == userId && ud.TypeId == 1);

                if (userDoc != null)
                {
                    userDoc.Status = "VERIFIED";
                    _dbContext.UserDocs.Update(userDoc);
                }
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"No IdCard found for user with ID {userId}");
            }
        }
        public async Task GenerateDocumentsForUser(int userId)
        {
            // Implement the logic to generate the documents here.
            // This is a placeholder for your actual implementation.
        }
        public async Task GetDocumentDetails(int userId)
        {
            // Implement the logic to generate the documents here.
            // This is a placeholder for your actual implementation.
        }
        public async Task<AllDocumentDetailsDto> GetAllDocumentDetails(int userId)
        {
            AllDocumentDetailsDto allDocumentDetails = new AllDocumentDetailsDto();

            var idCard = await _dbContext.IdCards.FirstOrDefaultAsync(i => i.UserId == userId);
            if (idCard != null)
            {
                allDocumentDetails.IdCardDetails = new IdCardDetailsDto
                {
                    LastName = idCard.LastName,
                    FirstName = idCard.FirstName,
                    CNP = idCard.CNP,
                    Series = idCard.Series,
                    Number = idCard.Number,
                    Address = idCard.Address,
                };
            }

            // ...

            return allDocumentDetails;
        }


    }
}
