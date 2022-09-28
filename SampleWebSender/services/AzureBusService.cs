using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SampleShared.models;
using Microsoft.Azure.ServiceBus;
using System.Text.Json;
using System.Text;

namespace SampleWebSender.services
{
    public class AzureBusService : IAzureBusService
    {
        private readonly IConfiguration _configuration;
        public AzureBusService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendMessageAsync(Person personMessage, string queueName)
        {
            // getting connection string from config file
            var connectionString = _configuration.GetConnectionString("AzureServiceBusConnection");
            // initializing Queue
            var qClient = new QueueClient(connectionString, queueName);
            // convert the person obj to json
            var msgBody = JsonSerializer.Serialize(personMessage);
            // initialize the Queue message
            var msg = new Message(Encoding.UTF8.GetBytes(msgBody));
            // send it to the client
            await qClient.SendAsync(msg);
        }
    }
}