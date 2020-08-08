using Common;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Reflection;
using System.Text;

namespace Publisher
{
    class Program
    {
        private static string topic = "test";
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("This is Publisher !");
            Console.WriteLine(" ");
            Console.WriteLine(" ");

            var serviceBusConnString = ConfigurationManager.AppSettings["serviceBus"];
            var topicClient = new TopicClient(serviceBusConnString, topic);

            while (true)
            {
                Console.WriteLine("Enter Name : ");
                var name = Console.ReadLine();

                if (name.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                    break;

                Message busMsg = GetBusMessage(name);

                topicClient.SendAsync(busMsg).ConfigureAwait(false).GetAwaiter();
            }

            Console.WriteLine("Publisher stopped ! ");
        }

        private static Message GetBusMessage(string name)
        {
            Random r = new Random();
            int age = r.Next(0, 100);
            int range = 100;
            double marks = r.NextDouble() * range;

            var model = new TestModel
            {
                Name = name,
                Address = "Address of " + name,
                Age = age,
                Marks = marks
            };

            var msg = JsonConvert.SerializeObject(model);

            var body = Encoding.UTF8.GetBytes(msg);
            var busMsg = new Message(body);
            return busMsg;
        }
    }
}
