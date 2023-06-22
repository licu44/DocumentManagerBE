using DocumentManager.Services;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DocumentManager.Data;
using System.Text.RegularExpressions;
using DocumentManager.Models;
using DocumentFormat.OpenXml.Packaging;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel;
using Newtonsoft.Json;

namespace DocumentManager.Handlers
{
    public class DocumentHandler
    {
        static string key = "3a0aa1a9674344a6a697e5f3ad77b3e8";
        static string endpoint = "https://documentmagement.cognitiveservices.azure.com/";
        private readonly DocumentServices _docServices;

        public DocumentHandler(DocumentServices docServices)
        {
            _docServices = docServices;
        }

        public async Task<IEnumerable<DocumentDto>> GetUserDocuments(int userId)
        {
            var documentDtos = await _docServices.GetUserDocumentsAsync(userId);
            return documentDtos;
        }
        public async Task<IEnumerable<DocumentDto>> GetGeneratedUserDocuments(int userId)
        {
            var documentDtos = await _docServices.GetGeneratedUserDocumentsAsync(userId);
            return documentDtos;
        }

        public async Task<IActionResult> IdCard(IFormFile file, int userId)
        {
            var linesList = (await ExtractText(file)).ToList(); // Convert to List first
            string idLine = linesList.FirstOrDefault(line => line.StartsWith("IDROU"));
            string seriaLine = linesList.FirstOrDefault(line => line.StartsWith("SERIA"));
            string cnpLine = linesList.FirstOrDefault(line => line.StartsWith("CNP"));

            int addressLineIndex = linesList.FindIndex(line => line.StartsWith("Domiciliu")) + 1;
            string addressLine = linesList[addressLineIndex];

            if (string.IsNullOrEmpty(idLine) || string.IsNullOrEmpty(seriaLine) || string.IsNullOrEmpty(cnpLine) || string.IsNullOrEmpty(addressLine))
            {
                throw new Exception("Required line not found.");
            }

            IdCardDto idData = new IdCardDto();

            idLine = idLine.Replace("IDROU", "");

            var splitIdLine = idLine.Split(new string[] { "<<" }, StringSplitOptions.RemoveEmptyEntries);

            idData.LastName = splitIdLine[0].Trim();

            idData.FirstName = splitIdLine[1].Replace("<", "-").TrimEnd('-').Trim();

            var seriaParts = seriaLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            idData.Series = seriaParts[1];
            idData.Number = seriaParts[3];

            var cnpParts = cnpLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            idData.CNP = cnpParts[1];

            idData.Address = addressLine.Trim();

            await _docServices.InsertIdCardData(idData, userId);

            return new OkObjectResult(idData);
        }

        public async Task<IActionResult> UrbanCertificate(IFormFile file, int userId)
        {
            var linesList = (await ExtractText(file)).ToList(); // Convert to List first

            UrbanCertificateDto certificateData = new UrbanCertificateDto();

            await _docServices.InsertUrabCertificateData(certificateData, userId);

            return new OkObjectResult(certificateData);
        }



        public async Task<IEnumerable<string>> ExtractText(IFormFile file)
        {
            var linesText = new List<string>();

            using (Stream stream = file.OpenReadStream())
            {
                ComputerVisionClient client =
                    new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
                    { Endpoint = endpoint };

                var textHeaders = await client.ReadInStreamAsync(stream);
                string operationLocation = textHeaders.OperationLocation;
                await Task.Delay(2000);

                const int numberOfCharsInOperationId = 36;
                string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

                ReadOperationResult results;
                do
                {
                    results = await client.GetReadResultAsync(Guid.Parse(operationId));
                }
                while ((results.Status == OperationStatusCodes.Running ||
                        results.Status == OperationStatusCodes.NotStarted));

                var textResults = results.AnalyzeResult.ReadResults;
                foreach (ReadResult page in textResults)
                {
                    foreach (Line line in page.Lines)
                    {
                        linesText.Add(line.Text);
                    }
                }
            }

            return linesText;
        }
        public async Task UpdateDocument(int userId, int documentType, object documentData)
        {
            switch (documentType)
            {
                case 1: // IdCard
                    var idCardDto = JsonConvert.DeserializeObject<IdCardDto>(documentData.ToString());
                    await _docServices.UpdateIdCardData(idCardDto, userId);
                    break;
                // case 2: // OtherDocumentType
                //     var otherDto = JsonConvert.DeserializeObject<OtherDocumentDto>(documentData.ToString());
                //     await _documentServices.UpdateOtherDocumentData(otherDto, userId);
                //     break;
                default:
                    throw new ArgumentException($"Unsupported document type: {documentType}");
            }
        }


        public async Task<bool> GenerateDocuments(int userId)
        {
            // This is just an example, replace with actual logic
            try
            {
                AllDocumentDetailsDto result = await _docServices.GetAllDocumentDetails(userId);
                await _docServices.GenerateDocumentsForUser(userId);

                ReplaceTextInDocumentAndSaveAs("oldText", "newText", "E:\\dezvoltare personala\\LICENTA\\DocumentManagerBE\\DocumentManager\\DocumentManager\\Files\\Generated\\doc.docx");
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void ReplaceTextInDocumentAndSaveAs(string oldText, string newText, string newFilePath)
        {
            string sourceFilePath = "E:\\dezvoltare personala\\LICENTA\\DocumentManagerBE\\DocumentManager\\DocumentManager\\Files\\TestReplace.docx";

            // Make a copy of the document.
            System.IO.File.Copy(sourceFilePath, newFilePath, true);

            // Open the copy and make changes.
            using (WordprocessingDocument newDoc = WordprocessingDocument.Open(newFilePath, true))
            {
                var body = newDoc.MainDocumentPart.Document.Body;

                foreach (var text in body.Descendants<Text>())
                {
                    if (text.Text.Contains(oldText))
                    {
                        text.Text = text.Text.Replace(oldText, newText);
                    }
                }

                newDoc.MainDocumentPart.Document.Save();
            }
        }

    }
}
