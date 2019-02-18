using AutoMapper;
using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Model;
//using GeekBurger.StoreCatalog.Repository;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Service
{
    public class StoreCatalogReadyService : IStoreCatalogReadyService
    {
        private const string Topic = "StoreCatalogReady";
        private IConfiguration _configuration;
        private IMapper _mapper;
        private List<Message> _messages;
        private Task _lastTask;
        private IServiceBusNamespace _namespace;
        private ILogService _logService;

        public StoreCatalogReadyService(IMapper mapper, IConfiguration configuration, ILogService logService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _logService = logService;
            _messages = new List<Message>();
            _namespace = _configuration.GetServiceBusNamespace();
            EnsureTopicIsCreated();
        }

        public void EnsureTopicIsCreated()
        {
            if (!_namespace.Topics.List()
                .Any(topic => topic.Name
                    .Equals(Topic, StringComparison.InvariantCultureIgnoreCase)))
                _namespace.Topics.Define(Topic)
                    .WithSizeInMB(1024).Create();

        }

        public async void SendCatalogReady()
        {
            var storeId = _configuration.GetSection("Store:Id").Get<Guid>();

            var config = _configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
            var topicClient = new TopicClient(config.ConnectionString, Topic);

            var storeCatologReadySerialized = JsonConvert.SerializeObject(new StoreCatalogReadyMessage { StoreId = storeId, Ready = true });
            var storeCatologReadyByteArray = Encoding.UTF8.GetBytes(storeCatologReadySerialized);

            var message = new Message
            {
                Body = storeCatologReadyByteArray,
                MessageId = Guid.NewGuid().ToString(),
                Label = storeId.ToString()
            };

            await topicClient.SendAsync(message);

            //_logService.SendMessagesAsync("true");

            //SendAsync(topicClient);

            //await _lastTask;

            var closeTask = topicClient.CloseAsync();
            await closeTask;
            HandleException(closeTask);
        }      

        public bool HandleException(Task task)
        {
            if (task.Exception == null || task.Exception.InnerExceptions.Count == 0) return true;

            task.Exception.InnerExceptions.ToList().ForEach(innerException =>
            {
                Console.WriteLine($"Error in SendAsync task: {innerException.Message}. Details:{innerException.StackTrace} ");

                if (innerException is ServiceBusCommunicationException)
                    Console.WriteLine("Connection Problem with Host. Internet Connection can be down");
            });

            return false;
        }
    }
}