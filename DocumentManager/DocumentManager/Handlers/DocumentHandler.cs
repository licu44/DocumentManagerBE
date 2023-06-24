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
            var linesList = (await ExtractText(file)).ToList();

            var documentText = string.Join(" ", linesList);

            UrbanCertificateDto certificateData = new UrbanCertificateDto();

            var certStart = documentText.IndexOf("Nr.") + "Nr.".Length;
            var certEnd = documentText.IndexOf(" din ");
            certificateData.Number = int.Parse(documentText.Substring(certStart, certEnd - certStart).Trim());

            var dateStart = documentText.IndexOf(" din ") + " din ".Length;
            var dateEnd = documentText.IndexOf("CERTIFICAT DE URBANISM");
            certificateData.Date = documentText.Substring(dateStart, dateEnd - dateStart).Trim();

            var projectStart = documentText.IndexOf("În scopul:") + "În scopul:".Length;
            var projectEnd = documentText.IndexOf("Ca urmare a Cererii adresate");
            certificateData.ProjectType = documentText.Substring(projectStart, projectEnd - projectStart).Trim();

            var userAdressStart = documentText.IndexOf("cu domiciliul in") + "cu domiciliul in".Length;
            var userAdressEnd = documentText.IndexOf("telefon/fax");
            certificateData.UserAdress = documentText.Substring(userAdressStart, userAdressEnd - userAdressStart).Trim();

            var projectAdressStart = documentText.IndexOf("situat în") + "situat în".Length;
            var projectAdressEnd = documentText.IndexOf("sau identificat prin CF");
            certificateData.ProjectAdress = documentText.Substring(projectAdressStart, projectAdressEnd - projectAdressStart).Trim();

            await _docServices.InsertUranCertificateData(certificateData, userId);

            return new OkObjectResult(certificateData);
        }
        
        public async Task<IActionResult> LandCertificate(IFormFile file, int userId)
        {
            var linesList = (await ExtractText(file)).ToList();

            LandCertificateDto certificateData = new LandCertificateDto();

            Match match = Regex.Match(linesList[linesList.FindIndex(line => line.Contains("Carte Funciara Nr."))], @"\d+");
            string CFNumber = match.Success ? match.Value : "";

            certificateData.CF = CFNumber;

            await _docServices.InsertLandCertificateData(certificateData, userId);

            return new OkObjectResult(certificateData);
        }
        public async Task<IActionResult> CadastralPlan(IFormFile file, int userId)
        {
            var linesList = (await ExtractText(file)).ToList();

            CadastralPlanDto cadastralPlanData = new CadastralPlanDto();

            Match match = Regex.Match(linesList[linesList.FindIndex(line => line.Contains("Teren: "))], @"\d+(\.\d+)?");
            string surface = match.Success ? match.Value : "";

            cadastralPlanData.Surface = surface;

            await _docServices.InsertCadastralPlanData(cadastralPlanData, userId);

            return new OkObjectResult(cadastralPlanData);
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
                case 1:
                    var idCardDto = JsonConvert.DeserializeObject<IdCardDto>(documentData.ToString());
                    await _docServices.UpdateIdCardData(idCardDto, userId);
                    break;
                case 2:
                    var urbanCertificateDto = JsonConvert.DeserializeObject<UrbanCertificateDto>(documentData.ToString());
                    await _docServices.UpdateUrbanCertificateData(urbanCertificateDto, userId);
                    break;
                case 3:
                    var landCertificateDto = JsonConvert.DeserializeObject<LandCertificateDto>(documentData.ToString());
                    await _docServices.UpdateLandCertificateData(landCertificateDto, userId);
                    break;
                case 4:
                    var cadastralPlanDto = JsonConvert.DeserializeObject<CadastralPlanDto>(documentData.ToString());
                    await _docServices.UpdateCadastralPlanData(cadastralPlanDto, userId);
                    break;

                default:
                    throw new ArgumentException($"Unsupported document type: {documentType}");
            }
        }


        public async Task<bool> GenerateDocuments(int userId)
        {
            try
            {
                var docs = await _docServices.GetAllDocumentDetails(userId);
                await _docServices.GenerateDocumentsForUser(userId);

                List<string> documentNames = new List<string> { "NotificareMediu", "CerereApaCanal", "CerereElectrica" };

                foreach (var documentName in documentNames)
                {
                    string newFilePath = $"E:\\dezvoltare personala\\LICENTA\\DocumentManagerBE\\DocumentManager\\DocumentManager\\Files\\Generated\\{documentName}-{userId}.docx";
                    string sourceFilePath = $"E:\\dezvoltare personala\\LICENTA\\DocumentManagerBE\\DocumentManager\\DocumentManager\\Files\\{documentName}.docx";

                    ReplaceTextInDocumentAndSaveAs(docs, sourceFilePath, newFilePath);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void ReplaceTextInDocumentAndSaveAs(Dictionary<string, string> docs, string sourceFilePath, string newFilePath)
        {
            // Make a copy of the document.
            System.IO.File.Copy(sourceFilePath, newFilePath, true);

            // Open the copy and make changes.
            using (WordprocessingDocument newDoc = WordprocessingDocument.Open(newFilePath, true))
            {
                var body = newDoc.MainDocumentPart.Document.Body;

                foreach (var text in body.Descendants<Text>())
                {
                    foreach (var doc in docs)
                    {
                        if (text.Text.Contains(doc.Key))
                        {
                            text.Text = text.Text.Replace(doc.Key, doc.Value);
                        }
                    }
                }

                newDoc.MainDocumentPart.Document.Save();
            }
        }
    }
}
