using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureFunctions
{
    public static class QueuePerson
    {
        [FunctionName("QueuePerson")]
        public static void Run(
            [QueueTrigger("serverlesspersonqueue", Connection = "AzureWebJobsStorage")]string queueItem, 
            [Table("serverlesspersontable")]CloudTable cloudTable, 
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function started.");

            string name = null;
            string email = null;

            JObject data = JsonConvert.DeserializeObject<JObject>(queueItem);
            name = name ?? data?["name"].ToString();
            email = email ?? data?["email"].ToString();

            var person = new Person();
            person.PartitionKey = "Pessoas";
            person.RowKey = Guid.NewGuid().ToString();
            person.Name = name;
            person.Email = email;

            var tableOperation = TableOperation.Insert(person);
            cloudTable.ExecuteAsync(tableOperation);

            log.LogInformation($"C# Queue trigger function finished.");
        }
    }
}
