using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace AzureFunctions
{
    public static class GetPerson
    {
        [FunctionName("GetPerson")]
        public static async Task<Person> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("Person")]CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation("GetPerson function started a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            JObject data = JsonConvert.DeserializeObject<JObject>(requestBody);
            var partitionKey = data?["partitionKey"].ToString();
            var rowKey = data?["rowKey"].ToString();

            TableOperation person = TableOperation.Retrieve<Person>(partitionKey, rowKey);
            TableResult result = await cloudTable.ExecuteAsync(person);

            log.LogInformation("GetPerson function finished a request.");

            return (Person)result.Result;
        }
    }
}
