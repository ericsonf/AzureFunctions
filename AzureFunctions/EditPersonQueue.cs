using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace AzureFunctions
{
    public static class EditPersonQueue
    {
        [FunctionName("EditPersonQueue")]
        public static void Run(
            [QueueTrigger("EditPerson", Connection = "AzureWebJobsStorage")]string queueItem,
            [Table("Person")]CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation($"EditPersonQueue trigger function started.");

            JObject data = JsonConvert.DeserializeObject<JObject>(queueItem);
            var partitionKey = data?["partitionKey"].ToString();
            var rowKey = data?["rowKey"].ToString();
            var name = data?["name"].ToString();
            var email = data?["email"].ToString();

            var person = new Person();
            person.PartitionKey = partitionKey;
            person.RowKey = rowKey;
            person.Name = name;
            person.Email = email;

            var tableOperation = TableOperation.InsertOrReplace(person);
            cloudTable.ExecuteAsync(tableOperation);

            log.LogInformation($"EditPersonQueue trigger function finished.");
        }
    }
}
