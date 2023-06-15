using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace DocumentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextManagementController : ControllerBase
    {
        static string key = "3a0aa1a9674344a6a697e5f3ad77b3e8";
        static string endpoint = "https://documentmagement.cognitiveservices.azure.com/";

        [HttpPost("register")]
        public async Task<ActionResult> ReadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Read the uploaded file content into a stream
            using (Stream stream = file.OpenReadStream())
            {
                ComputerVisionClient client =
                  new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
                  { Endpoint = endpoint };

                // Read text from the uploaded file stream
                var textHeaders = await client.ReadInStreamAsync(stream);
                // After the request, get the operation location (operation ID)
                string operationLocation = textHeaders.OperationLocation;
                await Task.Delay(2000);

                // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
                // We only need the ID and not the full URL
                const int numberOfCharsInOperationId = 36;
                string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

                // Extract the text
                ReadOperationResult results;
                do
                {
                    results = await client.GetReadResultAsync(Guid.Parse(operationId));
                }
                while ((results.Status == OperationStatusCodes.Running ||
                    results.Status == OperationStatusCodes.NotStarted));

                // Display the found text.
                var textResults = results.AnalyzeResult.ReadResults;
                foreach (ReadResult page in textResults)
                {
                    foreach (Line line in page.Lines)
                    {
                        Console.WriteLine(line.Text);
                    }
                }
            }

            return Ok();
        }
    }
}
