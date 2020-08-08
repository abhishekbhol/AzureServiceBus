using Common;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Text;
using System.Threading;

namespace Listener
{
    class Program
    {
        private static string topic = "test";
        private static string subscription = "listener";

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("This is Listener !");
            Console.WriteLine(" ");
            Console.WriteLine(" ");

            var serviceBusConnString = ConfigurationManager.AppSettings["serviceBus"];
            var subscriptionClient = new SubscriptionClient(serviceBusConnString, topic, subscription);
            subscriptionClient.RegisterMessageHandler(async (msg, CancellationToken) =>
            {
                var body = Encoding.UTF8.GetString(msg.Body);
                var model = JsonConvert.DeserializeObject<TestModel>(body);
                Print(model);

            },
            async exception =>
            {
                Console.WriteLine("Exception Occured ");
                }
            );

            Console.ReadLine();
        }

        private static void Print(TestModel model)
        {
            Console.WriteLine("New Message ");

            Console.WriteLine($"Name : {model?.Name}");
            Console.WriteLine($"Address : {model?.Address}");
            Console.WriteLine($"Age : {model?.Age}");
            Console.WriteLine($"Marks : {model?.Marks}");
            Console.WriteLine(" ");
        }
    }
}
