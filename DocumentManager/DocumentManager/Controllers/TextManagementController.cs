using DocumentManager.Data;
using DocumentManager.Handlers;
using DocumentManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace DocumentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextManagementController : ControllerBase
    {
        private readonly DocumentServices _documentServices;
        private readonly DocumentHandler _documentHandler;

        public TextManagementController(DocumentServices documentServices, DocumentHandler documentHandler)
        {
            _documentServices = documentServices;
            _documentHandler = documentHandler;
        }

        [HttpPost("processFile")]
        public async Task<IActionResult> ProcessFile(IFormFile file, int userId, string documentType)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (userId==null)
            {
                return BadRequest("No ID provided.");
            }

            if (string.IsNullOrWhiteSpace(documentType))
            {
                return BadRequest("No key provided.");
            }

            // Redirect based on key
            switch (documentType)
            {
                case "1":
                    return await _documentHandler.IdCard(file, userId);
                case "2":
                    return await _documentHandler.UrbanCertificate(file, userId);
                case "3": 
                    return await _documentHandler.LandCertificate(file, userId);
                case "4":
                    return await _documentHandler.CadastralPlan(file, userId);
                default:
                    return BadRequest($"Invalid key: {documentType}");
            }
        }
        [HttpPut("update/{userId}/{documentType}")]
        public async Task<IActionResult> UpdateDocument(int userId, int documentType, [FromBody] object documentData)
        {
            try
            {
                await _documentHandler.UpdateDocument(userId, documentType, documentData);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }

        [HttpGet("{userId}/documents")]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetUserDocuments(int userId)
        {
            //for table
            var documentDtos = await _documentHandler.GetUserDocuments(userId);
            return Ok(documentDtos);
        }

        [HttpGet("{userId}/documents/generated")]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetGeneratedUserDocuments(int userId)
        {
            //for table
            var documentDtos = await _documentHandler.GetGeneratedUserDocuments(userId);
            return Ok(documentDtos);
        }
        [HttpGet("{userId}/generate")]
        public async Task<IActionResult> GetGenerateDocumentsTrigger(int userId)
        {
            var generationSuccess = await _documentHandler.GenerateDocuments(userId);

            if (!generationSuccess)
            {
                return StatusCode(500, "Document generation failed.");
            }

            var documentDtos = await _documentHandler.GetGeneratedUserDocuments(userId);
            return Ok(documentDtos);
        }
        [HttpGet("{userId}/{documentName}/download")]
        public async Task<IActionResult> DownloadDocument(int userId, string documentName)
        {
            // construct the file path
            var path = $"E:\\dezvoltare personala\\LICENTA\\DocumentManagerBE\\DocumentManager\\DocumentManager\\Files\\Generated\\{documentName}-{userId}.docx";

            // check if file exists
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            // get file stream
            var memory = new MemoryStream();
            await using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            // provide download
            return File(memory, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", Path.GetFileName(path));
        }



    }
}
