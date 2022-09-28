using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleShared.models;

namespace SampleWebSender.services
{
    public interface IAzureBusService
    {
        Task SendMessageAsync(Person personMessage, string queueName);
    }
}