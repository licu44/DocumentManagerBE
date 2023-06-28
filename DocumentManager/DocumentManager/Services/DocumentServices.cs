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
        public async Task<IdCard> GetdCardData(int userId)
        {
            IdCard result = _dbContext.IdCards.FirstOrDefault(id => id.UserId == userId);
            return result;
        }
        public async Task<UrbanCertificate> GetUrbanCertificateData(int userId)
        {
            var result = _dbContext.UrbanCertificates.FirstOrDefault(id => id.UserId == userId);
            return result;
        }
        public async Task<LandCertificate> GetLandCertificateData(int userId)
        {
            var result = _dbContext.LandCertificates.FirstOrDefault(id => id.UserId == userId);
            return result;
        }
        public async Task<CadastralPlan> GetCadastralPlanData(int userId)
        {
            var result = _dbContext.CadastralPlans.FirstOrDefault(id => id.UserId == userId);
            return result;
        }

        public async Task<IEnumerable<GeneratedDocumentsDto>> GetGeneratedUserDocumentsAsync(int userId)
        {
            var documentTypes = await _dbContext.GenerateDocTypes
                 .ToListAsync();

            var userDocs = await _dbContext.UserGeneratedDocs
                 .Where(ud => ud.UserId == userId)
                 .ToListAsync();

            var documentDtos = documentTypes.Select(d => {
                var userDoc = userDocs.FirstOrDefault(ud => ud.TypeId == d.Id);
                return new GeneratedDocumentsDto
                {
                    DocId = d.Id,
                    DocName = d.Type,
                    CreationDate = userDoc?.CreationDate,
                };
            }).ToList();

            return documentDtos;
        }
        public async Task<object> GetGeneratedDocumentsAsync()
        {
            var documentTypes = await _dbContext.GenerateDocTypes
                 .ToListAsync();

            return documentTypes;
        }
        public async Task InsertGeneratedDocumentPath(int userId, int typeId, string filePath)
        {
            var userGeneratedDoc = new UserGeneratedDoc
            {
                UserId = userId,
                TypeId = typeId,
                CreationDate = DateTime.Now,
                WordDocumentPath = filePath
            };

            _dbContext.UserGeneratedDocs.Add(userGeneratedDoc);
            await _dbContext.SaveChangesAsync();
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
        public async Task InsertUranCertificateData(UrbanCertificateDto certificateData, int userId)
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
        public async Task InsertLandCertificateData(LandCertificateDto certificateData, int userId)
        {
            var newLandCertificate = new LandCertificate
            {
                UserId = userId,
                CF = certificateData.CF,
            };

            _dbContext.LandCertificates.Add(newLandCertificate);

            var newUserDoc = new UserDoc
            {
                UserId = userId,
                TypeId = 3,
                CreationDate = DateTime.Now,
                Status = "UPLOADED",
            };

            _dbContext.UserDocs.Add(newUserDoc);

            await _dbContext.SaveChangesAsync();
        }
        public async Task InsertCadastralPlanData(CadastralPlanDto certificateData, int userId)
        {
            var newCadastralPlan = new CadastralPlan
            {
                UserId = userId,
                Surface = certificateData.Surface,
            };

            _dbContext.CadastralPlans.Add(newCadastralPlan);

            var newUserDoc = new UserDoc
            {
                UserId = userId,
                TypeId = 4,
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
        public async Task UpdateUrbanCertificateData(UrbanCertificateDto urbanCertificateData, int userId)
        {
            var urbanCertificate = await _dbContext.UrbanCertificates.FirstOrDefaultAsync(i => i.UserId == userId);

            if (urbanCertificate != null)
            {
                urbanCertificate.UserId = userId;
                urbanCertificate.ProjectAdress = urbanCertificateData.ProjectAdress;
                urbanCertificate.UserAdress = urbanCertificateData.UserAdress;
                urbanCertificate.Number = urbanCertificateData.Number;
                urbanCertificate.Date = urbanCertificateData.Date;
                urbanCertificate.ProjectType = urbanCertificateData.ProjectType;

                _dbContext.UrbanCertificates.Update(urbanCertificate);

                var userDoc = await _dbContext.UserDocs
                    .FirstOrDefaultAsync(ud => ud.UserId == userId && ud.TypeId == 2);

                if (userDoc != null)
                {
                    userDoc.Status = "VERIFIED";
                    _dbContext.UserDocs.Update(userDoc);
                }
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"No Document found for user with ID {userId}");
            }
        }
        public async Task UpdateLandCertificateData(LandCertificateDto landCertificateData, int userId)
        {
            var landCertificate = await _dbContext.LandCertificates.FirstOrDefaultAsync(i => i.UserId == userId);

            if (landCertificate != null)
            {
                landCertificate.UserId = userId;
                landCertificate.CF = landCertificateData.CF;

                _dbContext.LandCertificates.Update(landCertificate);

                var userDoc = await _dbContext.UserDocs
                    .FirstOrDefaultAsync(ud => ud.UserId == userId && ud.TypeId == 3);

                if (userDoc != null)
                {
                    userDoc.Status = "VERIFIED";
                    _dbContext.UserDocs.Update(userDoc);
                }
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"No Document found for user with ID {userId}");
            }
        }
        public async Task UpdateCadastralPlanData(CadastralPlanDto cadastralPlanData, int userId)
        {
            var cadastralPlan = await _dbContext.CadastralPlans.FirstOrDefaultAsync(i => i.UserId == userId);

            if (cadastralPlan != null)
            {
                cadastralPlan.UserId = userId;
                cadastralPlan.Surface = cadastralPlanData.Surface;

                _dbContext.CadastralPlans.Update(cadastralPlan);

                var userDoc = await _dbContext.UserDocs
                    .FirstOrDefaultAsync(ud => ud.UserId == userId && ud.TypeId == 4);

                if (userDoc != null)
                {
                    userDoc.Status = "VERIFIED";
                    _dbContext.UserDocs.Update(userDoc);
                }
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"No Document found for user with ID {userId}");
            }
        }

        public async Task GenerateDocumentsForUser(int userId)
        {
            
        }
        public async Task<Dictionary<string, string>> GetAllDocumentDetails(int userId)
        {
            var docs = new Dictionary<string, string>();

            var idCard = await _dbContext.IdCards.FirstOrDefaultAsync(i => i.UserId == userId);
            if (idCard != null)
            {
                docs["IdCardLastName"] = idCard.LastName;
                docs["IdCardFirstName"] = idCard.FirstName;
                docs["IdCardCNP"] = idCard.CNP;
                docs["IdCardSeries"] = idCard.Series;
                docs["IdCardNumber"] = idCard.Number;
            }

            var urbanCertificate = await _dbContext.UrbanCertificates.FirstOrDefaultAsync(i => i.UserId == userId);
            if (urbanCertificate != null)
            {
                docs["UrbanCertificateUserAddress"] = urbanCertificate.UserAdress;
                docs["UrbanCertificateProjectAddress"] = urbanCertificate.ProjectAdress;
                docs["UrbanCertificateNumber"] = urbanCertificate.Number.ToString();
                docs["UrbanCertificateDate"] = urbanCertificate.Date;
                docs["UrbanCertificateProjectType"] = urbanCertificate.ProjectType;
            }

            var landCertificate = await _dbContext.LandCertificates.FirstOrDefaultAsync(i => i.UserId == userId);
            if (landCertificate != null)
            {
                docs["LandCertificateCF"] = landCertificate.CF;
            }

            var cadastralPlan = await _dbContext.CadastralPlans.FirstOrDefaultAsync(i => i.UserId == userId);
            if (cadastralPlan != null)
            {
                docs["CadastralPlanSurface"] = cadastralPlan.Surface;
            }

            return docs;
        }


    }
}
