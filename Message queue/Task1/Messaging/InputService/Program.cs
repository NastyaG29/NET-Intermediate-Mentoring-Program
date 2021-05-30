using RabbitMQ.Client;
using System;

namespace InputService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory{ HostName = Common.Constants.Host };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(Common.Constants.ExchangeName, ExchangeType.Fanout);

            Console.WriteLine("Enter directory path.");
            var path = Console.ReadLine();
            using (var folderWatcher = new FolderWatcher(channel, path, Common.Constants.FileFormats))
            {
                Console.ReadLine();
            }
        }
    }
}