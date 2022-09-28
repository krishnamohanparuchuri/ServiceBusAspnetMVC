using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using SampleShared.models;

namespace SampleAppReceiver
{
    class Program
    {
        const string connString = "Endpoint=sb://samplewebservicebusapi.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LF9HEaWgLhx+WDWSeFGZpGmsoNx6Q1/CzM036fe0vZg=";

        static IQueueClient qClient;
        static async Task Main(string[] args)
        {
            qClient = new QueueClient(connString, "personqueue");

            var msgOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false,
            };

            qClient.RegisterMessageHandler(ProcessMessageAsync, msgOptions);

            Console.ReadLine();

            await qClient.CloseAsync();

        }
        private static async Task ProcessMessageAsync(Message message, CancellationToken token)
        {
            var jsonBody = Encoding.UTF8.GetString(message.Body);
            var personObj = JsonSerializer.Deserialize<Person>(jsonBody);

            Console.WriteLine($"First Name: {personObj.FirstName}");
            Console.WriteLine($"Last Name: {personObj.LastName}");
            Console.WriteLine($"Email: {personObj.Email}");

            //updating the queue that the message has been updated Successfully
            await qClient.CompleteAsync(message.SystemProperties.LockToken);

        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine($"Something went wrong , {args.Exception}");
            return Task.CompletedTask;
        }
    }
}
