using GeekBurger.Production.Contract;
using GeekBurger.StoreCatalog.Repository;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace GeekBurger.StoreCatalog.Service.GetProductionAreaChanged
{
    public class GetProductionAreaChangedService : IGetProductionAreaChangedService
    {
        private const string TopicName = "ProductionAreaChanged";
        private static IConfiguration _configuration;
        private static ServiceBusConfiguration serviceBusConfiguration;
        private const string SubscriptionName = "StoreCatalog";

        public GetProductionAreaChangedService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async void GetProductionAreaChanged()
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

            var storeID = _configuration.GetSection("Store:Id").Get<string>();
            await subscriptionClient.AddRuleAsync(new RuleDescription
            {
                Filter = new CorrelationFilter { Label = storeID },
                Name = "filter-store"
            });

            var mo = new MessageHandlerOptions(ExceptionHandle) { AutoComplete = true };

            subscriptionClient.RegisterMessageHandler(Handle, mo);
        }

        private static Task Handle(Message message, CancellationToken arg2)
        {
            var productionChangedString = Encoding.UTF8.GetString(message.Body);
            var productionChangedJson = JsonConvert.DeserializeObject<ProductionToGet>(productionChangedString);

            var optionsBuilder = new DbContextOptionsBuilder<StoreCatalogContext>();
            var options = optionsBuilder.UseInMemoryDatabase("geekburger-storecatalog").Options;

            using (var db = new StoreCatalogContext(options))
            {
                var sc = new StoreCatalogRepository(db, _configuration);
                sc.UpsertProduction(productionChangedJson);
            }

            return Task.CompletedTask;
        }

        private static Task ExceptionHandle(ExceptionReceivedEventArgs arg)
        {
            var context = arg.ExceptionReceivedContext;
            return Task.CompletedTask;
        }

    }
}
