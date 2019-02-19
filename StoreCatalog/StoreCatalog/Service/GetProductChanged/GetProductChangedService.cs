using AutoMapper;
using GeekBurger.Products.Contract;
using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Model;
using GeekBurger.StoreCatalog.Repository;
//using GeekBurger.StoreCatalog.Repository;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace GeekBurger.StoreCatalog.Service.GetProductChanged
{
    public class GetProductChangedService : IGetProductChangedService
    {
        private const string TopicName = "ProductChanged";
        private static IConfiguration _configuration;
        private static ServiceBusConfiguration serviceBusConfiguration;
        private static string _storeId;
        private const string SubscriptionName = "Los Angeles - Beverly Hills";
        private static StoreCatalogContext _context;
        private static StoreCatalogRepository _repository;

        public GetProductChangedService(IConfiguration configuration, StoreCatalogContext context)
        {
            _configuration = configuration;
            _context = context;
            _repository = new StoreCatalogRepository(context, configuration);           
        }

        public async void SendCatalogReady()
        {
            _configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            serviceBusConfiguration = _configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();

            var serviceBusNamespace = _configuration.GetServiceBusNamespace();

            var topic = serviceBusNamespace.Topics.GetByName(TopicName);

            await topic.Subscriptions.DeleteByNameAsync(SubscriptionName);

            if (!topic.Subscriptions.List()
                   .Any(subscription => subscription.Name
                       .Equals(SubscriptionName, StringComparison.InvariantCultureIgnoreCase)))
                topic.Subscriptions
                    .Define(SubscriptionName)
                    .Create();

            ReceiveMessages();
        }

        private static async void ReceiveMessages()
        {
            var subscriptionClient = new SubscriptionClient(serviceBusConfiguration.ConnectionString, TopicName, SubscriptionName);

            //by default a 1=1 rule is added when subscription is created, so we need to remove it
            await subscriptionClient.RemoveRuleAsync("$Default");

            await subscriptionClient.AddRuleAsync(new RuleDescription
            {
                Filter = new CorrelationFilter { Label = _storeId },
                Name = "filter-store"
            });

            var mo = new MessageHandlerOptions(ExceptionHandle) { AutoComplete = true };

            subscriptionClient.RegisterMessageHandler(Handle, mo);            
        }

        private static Task Handle(Message message, CancellationToken arg2)
        {           
            var productChangesString = Encoding.UTF8.GetString(message.Body);
            var productChangesJson = JsonConvert.DeserializeObject<ProductToGet>(productChangesString);
            _repository.UpsertProduct(productChangesJson);

            return Task.CompletedTask;
        }

        private static Task ExceptionHandle(ExceptionReceivedEventArgs arg)
        {           
            var context = arg.ExceptionReceivedContext;            
            return Task.CompletedTask;
        }

    }
}
