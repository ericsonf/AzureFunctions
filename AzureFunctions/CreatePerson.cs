using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AzureFunctions
{
    public static class CreatePerson
    {
        [FunctionName("CreatePerson")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequest req, 
            [Queue("serverlesspersonqueue", Connection = "AzureWebJobsStorage")]IAsyncCollector<string> queueItem, 
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function started a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            await queueItem.AddAsync(requestBody);

            log.LogInformation("C# HTTP trigger function finished a request.");

            return new OkObjectResult($"Obrigado! Seu registro já esta sendo processado.");
        }
    }
}
