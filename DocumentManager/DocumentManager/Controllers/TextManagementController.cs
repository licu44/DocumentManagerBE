using DocumentManager.Data;
using DocumentManager.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class TextManagementController : ControllerBase
    {
        private readonly DocumentHandler _documentHandler;

        public TextManagementController(DocumentHandler documentHandler)
        {
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
        [HttpGet("documentFields/{userId}/{documentType}")]
        public async Task<object> GetDocumentFields(int userId, int documentType)
        {
            var result = await _documentHandler.GetDocumentFields(userId, documentType);
            return result;
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
            var path = $"E:\\dezvoltare personala\\LICENTA\\DocumentManagerBE\\DocumentManager\\DocumentManager\\Files\\Generated\\{documentName}-{userId}.docx";

            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            await using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            return File(memory, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", Path.GetFileName(path));
        }



    }
}
