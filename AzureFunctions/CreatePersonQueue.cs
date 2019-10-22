using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureFunctions
{
    public static class CreatePersonQueue
    {
        [FunctionName("CreatePersonQueue")]
        public static void Run(
            [QueueTrigger("CreatePerson", Connection = "AzureWebJobsStorage")]string queueItem, 
            [Table("Person")]CloudTable cloudTable, 
            ILogger log)
        {
            log.LogInformation($"CreatePersonQueue trigger function started.");

            JObject data = JsonConvert.DeserializeObject<JObject>(queueItem);
            var name = data?["name"].ToString();
            var email = data?["email"].ToString();

            var person = new Person();
            person.PartitionKey = "Person";
            person.RowKey = Guid.NewGuid().ToString();
            person.Name = name;
            person.Email = email;

            var tableOperation = TableOperation.Insert(person);
            cloudTable.ExecuteAsync(tableOperation);

            log.LogInformation($"CreatePersonQueue trigger function finished.");
        }
    }
}
