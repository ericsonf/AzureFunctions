using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureFunctions
{
    public class Person : TableEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public Person() { }

        public static explicit operator Person(DynamicTableEntity v)
        {
            throw new NotImplementedException();
        }
    }
}
