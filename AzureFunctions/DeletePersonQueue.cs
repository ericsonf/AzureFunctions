using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureFunctions
{
    public static class DeletePersonQueue
    {
        [FunctionName("DeletePersonQueue")]
        public static void Run(
            [QueueTrigger("DeletePerson", Connection = "AzureWebJobsStorage")]string queueItem,
            [Table("Person")]CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation($"DeletePersonQueue trigger function started.");

            JObject data = JsonConvert.DeserializeObject<JObject>(queueItem);
            var partitionKey = data?["partitionKey"].ToString();
            var rowKey = data?["rowKey"].ToString();

            var person = new DynamicTableEntity(partitionKey, rowKey);
            person.ETag = "*";

            var tableOperation = TableOperation.Delete(person);
            cloudTable.ExecuteAsync(tableOperation);

            log.LogInformation($"DeletePersonQueue trigger function finished.");
        }
    }
}
